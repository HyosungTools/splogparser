# CashTrailView — design spec + implementation notes

Driven by **NHSWS-18867** (F&M of VA, ATM 362004, settlement 2026-08-14 → 2026-08-21, "off $400 on
physical cash deposits"). Written to close the whole *"where's my money"* complaint class, not just
this ticket.

Status: **code written and verified against NHSWS-18867's own workbook.** The analyzer reproduces the
entire hand-written RCA — deposit variance $0.00, dispense variance $0.00, $400 dispenser-origin
reject cash, customer fully reversed — in one pass, unattended.

---

## 1. What went wrong for the analyst on this ticket

Nothing in the workbook was *incorrect*. The analyst was misled by three structural properties of the
output, and every one of them is fixable.

**a. The only place the $400 appears is on a CIM worksheet.** `CDMreject` has no denomination columns
at all — just `count`, `reject`, `dispensed`, `presented`, `retracted`. `CIMreject` has the full
`USD0…USD100` breakdown *and* a synthesized `Total $400` row. So the one sheet that names the number
is the deposit-module sheet. Seen there, $400 of dispenser cash reads as deposit money. The analyst's
note — *"I found this … however I do not see the $400.00 deposit"* — is the predictable result.

**b. `LCU01` is one physical bin reported by two devices, and the workbook never says so.** The only
hint is a string buried in the `comment` column of `CDMSummary`:
*"Reject cash unit. This type will also indicate a combined reject/retract cash unit."* Same for
`LCU00`/retract. Nothing marks `CDMreject` and `CIMreject` as the same box.

**c. `LCU02` means two different cassettes.** `CDM.LCU02` is the $50 dispense cassette;
`CIM.LCU02` is the cash-in cassette. Same label, unrelated hardware. A trap for anyone reading across
sheets — and, note, a trap for naive tooling too: matching shared units on **name alone** produces a
false positive here.

**d. The decisive number is not in any column.** What proves the $400 was dispenser cash is
`Δdispensed − Δpresented` per cassette: 6 fifties and 5 twenties left the cassettes and never reached
the shutter. Both columns are present; the subtraction is not. The analyst has to do it by eye, across
two sheets, at millisecond timestamps.

> **Design rule that falls out of this:** the tool already has every fact. What it does not have is
> *provenance* and *arithmetic*. That is what CashTrailView adds.

---

## 2. The invariants

These are the whole design. Each is checkable from data already in the workbook.

| # | Invariant | Meaning | Violation ⇒ |
|---|-----------|---------|-------------|
| **I1** | `Δdispensed = Δpresented + Δreject + Δretracted` (per cassette) | every note picked either reached the customer, went to reject, or was retracted | counter loss / missed update |
| **I2** | `Σ(Δdispensed − Δpresented − Δretracted) × denom` at time *T* = value of the reject-bin delta at *T* | notes in the bin that came from the dispenser are exactly the unpresented picks | leftover = deposit-origin |
| **I3** | `Σ AP CashDeposit.TotalCashAmount` = value in the cash-in cassette at period close | deposits balance | `DEPOSIT_VARIANCE` |
| **I4** | `Σ AP Withdrawal.AmountDispensed` = `Σ Δpresented × denom` | dispense balances | `DISPENSE_VARIANCE` |
| **I5** | every `Withdrawal` with `AmountDispensed < AmountRequested` has a `Withdrawal-Reversal` in the same session within 180 s | the customer was made whole | **`CUSTOMER_DEBITED_NO_CASH`** |
| **I6** | a unit is *shared* iff **name AND type AND capacity** all match across devices | one box, two reports | report once, with an origin |

**I5 is the highest-value line in the file.** This ticket was the benign case — session 3500 was
reversed one second later, so nobody lost money. The malignant case is identical in the logs except
for a missing reversal, and today nothing in the workbook flags it. That is the real *"where's my
money."*

**I2 is the fix for this ticket.** It attributes every note in a shared bin to the device that put it
there, so dispenser cash can never again read as deposit cash.

---

## 3. Architecture — no new plumbing

Analysis-only view, exactly the `DepositAnalysisView` shape. Parses no log files; runs in the
**Analyze** phase and reads the XML the other views wrote in `PostProcess`.

| Reads | From | Gives |
|-------|------|-------|
| `CDMView.xml` → `Summary` | SP | cash-unit topology: number, name, type, currency, denom, capacity |
| `CDMView.xml` → `CashUnit-N` | SP | per-unit cumulative counters — the raw material for I1/I2 |
| `CDMView.xml` → `Dispense` | SP | dispense/present/taken lifecycle, exchange markers |
| `CIMView.xml` → `Summary`, `CashIn-N`, `Deposit` | SP | the CIM's view of the shared bins (denominations!), cash-in cassette, exchange markers |
| `OverView.xml` → `Transaction` | AP | session, account, requested/dispensed/deposited, reversals |

### Table naming — the trap to avoid

Cash-unit tables are still named **`CashUnit-N`** (CDM) and **`CashIn-N`** (CIM) at Analyze time. The
rename to `USD50` / `reject` / `retract` happens in `WriteExcelFile`, which runs *after* Analyze.
`CashTrailTable` therefore resolves units **by number via the `Summary` table**, never by hardcoded
name.

> Worth fixing while you're in here: `DepositAnalysisTable` hardcodes `CASHIN_TABLE = "CashIn-2"` and
> `RETRACT_TABLE = "CashIn-1"`. On this machine the cash-in unit is `CashIn-3` and `CashIn-2` is the
> reject bin, so DepositAnalysis is reading the wrong cassette on 362004. Same `Summary`-lookup fix
> applies.

### Defensive rule for rows other analyses append

`CIMreject`/`cashin` in this workbook contain synthesized rows (`Exchange 1`, `$ per denomination`,
`Total $400`) with a parseable timestamp but **no `status`**. Differencing across them invents note
movement, and valuing a `$ per denomination` row as if it were note counts produces nonsense (it would
read `1120 × $20`). `ComputeDeltas` and `CashInSnapshot` skip any row with an empty `status`.
Device reports always carry one.

---

## 4. Output — three worksheets

### `CashBalance` — the answer sheet (read this first)

One block per settlement period, delimited by service visits (exchange markers clustered with a 60 s
gap). Every line is `item / amount / variance / flags / explain`.

```
DEPOSITS posted to host                 $1,650.00
DEPOSITS counted in cash-in unit        $1,650.00
DEPOSIT VARIANCE                            $0.00   Deposits balance. Every dollar the
                                                    customers put in was posted.
WITHDRAWALS posted to host              $4,470.00
WITHDRAWALS presented to customer       $4,470.00
DISPENSE VARIANCE                           $0.00   Dispense balances.
REJECT/RETRACT - dispenser origin         $400.00   DISPENSER_CASH_IN_BIN
REJECT/RETRACT - deposit origin             $0.00
VERDICT   Deposits balance to $0.00. Dispense balances to $0.00. $400 of dispenser cash is
          sitting in the reject/retract bin - that is the most likely source of a
          cash-deposit overage of the same amount.
```

Followed by two appended sections:

```
EXPOSURE  Failed dispense session 3500 x6999   $500.00   REVERSED_NO_CUSTOMER_LOSS
          $500 requested, $0 dispensed, REVERSED at 20:36:55. Customer was not debited -
          no claim owed.
SERVICE   CDM.LCU02 ($50) ran empty                      CASSETTE_EMPTY
          Empty from 2026-08-17 20:36 for 92.8 hours (never replenished in this log).
          Every dispense needing $50 notes in that window was at risk.
```

`DEPOSITS counted in cash-in unit` is a **running-total snapshot** at period close, not a sum of
deltas. Per NHSWS-17121: the device drops updates, and summing deltas silently loses those notes.

### `CashBins` — provenance (the fix for this ticket)

One row per increase in a reject/retract bin. Actual output for NHSWS-18867 — the whole ticket, one row:

| field | value |
|---|---|
| time | 2026-08-17 20:36:52.532 |
| bin | `CDM.LCU01 (reject)` |
| sharedWith | `CIM` |
| notes | 11 |
| USD20 / USD50 | 5 / 6 |
| amount | $400.00 |
| **origin** | **DISPENSER** |
| confidence | exact match |
| session / **card** / account | 3500 / **`UNK (AccountNumber x6999)`** / x6999 |
| evidence | `CDM.LCU02 ($50) dispensed +6, presented +0 -> 6 note(s) never reached the customer; CDM.LCU03 ($20) dispensed +5, presented +0 -> 5 note(s) never reached the customer` |
| flags | `DISPENSER_CASH_IN_SHARED_BIN;REVERSED_NO_CUSTOMER_LOSS` |
| explain | *$400 (5x$20 + 6x$50) is DISPENSER cash, not deposit money. 11 note(s) were picked from the cassettes for a dispense that failed and were diverted to the reject bin. session 3500, account x6999. Do NOT count this with deposits - credit the dispense cassettes. The customer was reversed ($500 requested, $0 dispensed) - no claim owed.* |

`origin` ∈ `DISPENSER` / `DEPOSIT` / `MIXED` / `UNKNOWN`. `MIXED` carries a `confidence` of
`"NN% dispenser"` and splits the amount. `UNKNOWN` means no corroborating cassette movement was in
the log — usually because the matching device was not parsed.

### `CashTrail` — the ledger

One row per money event, chronological: withdrawals, cash deposits, reversals, bin events, service
visits. Columns: `time · session · **card** · account · event · device · requested · toCustomer ·
fromCustomer · toBin · hostPosted · variance · outcome · flags · explain`.

For NHSWS-18867 the whole settlement week is **28 rows** — replacing 7,286 rows of `OverSummary` plus
six cash-unit sheets. The relevant fragment:

```
18:24:10  WITHDRAWAL           3497  412126XXXXXX0549           x7592  req=$300  cust=$300        OK
20:36:52  TO REJECT BIN        3500  UNK (AccountNumber x6999)  x6999            bin=$400  DISPENSER
20:36:54  WITHDRAWAL           3500  UNK (AccountNumber x6999)  x6999  req=$500  cust=$0    REVERSED
20:36:55  WITHDRAWAL-REVERSAL  3500  UNK (AccountNumber x6999)  x6999  req=$500             REVERSAL
20:38:23  WITHDRAWAL           3501  UNK (AccountNumber x6999)  x6999  req=$200  cust=$200        OK
```

Note the ordering: the money reaches the bin **two seconds before** the host transaction is written.
The SP layer moved the notes; the AP layer then recorded the failure. That ordering is itself a
diagnostic and it is invisible when the two layers live on different sheets.

### The `card` column — and why it says UNK

The card number is resolved from `OverSummary`'s `card` column, which carries masked PANs
(`479281XXXXXX3639`) mixed in with lifecycle keywords (`inserted`, `read`, `ejected`, `on-us`,
`timeout`); a PAN-shaped regex separates them. Resolution is **once per AP session**, from the
session's earliest transaction, so every row of a multi-transaction session names the same person.

**There is often no card to find, and that is not a parser bug.** On this fleet a customer can
identify with an account number plus an SSN and never present a card — 15 of the 25 customer rows in
this settlement are cardless, including session 3500, the one this ticket is about. So the column
degrades in two steps and is **never left blank**:

| what was found | column reads |
|---|---|
| masked PAN in the window | `479281XXXXXX3639` |
| no PAN, but a session identification | `UNK (AccountNumber x6999)` |
| nothing | `UNK` |

A blank cell reads as *"the tool didn't look."* `UNK` reads as *"there is no card number here"* — which
is the truth, and is itself a finding: a cardless session means a card-based dispute needs a different
line of enquiry. Where a cardless identification is known it is named alongside `UNK` rather than
thrown away.

Extraction can be improved (see open question 6) without changing this contract.

---

## 5. Flag catalogue

| Flag | Meaning | Action |
|------|---------|--------|
| `CUSTOMER_DEBITED_NO_CASH` | short/failed dispense with **no reversal** | escalate — real customer loss |
| `REVERSED_NO_CUSTOMER_LOSS` | short/failed dispense, reversal found | no claim owed; say so in the reply |
| `DISPENSER_CASH_IN_SHARED_BIN` | dispenser cash in a bin the CIM also reports | **do not count with deposits** |
| `DISPENSER_CASH_IN_BIN` | period-level roll-up of the above | reclassify at settlement |
| `DEPOSIT_CASH_IN_BIN` | customer notes that never reached cash-in | was it returned, or still in the bin? |
| `MIXED_ORIGIN_BIN` | one bin delta, both origins | split before balancing |
| `ORIGIN_UNKNOWN` | no corroborating cassette movement | re-run with both `-s CDM` and `-s CIM` |
| `DEPOSIT_VARIANCE` / `DISPENSE_VARIANCE` | I3 / I4 broken | investigate before touching the bins |
| `CASSETTE_EMPTY` | cassette empty, with duration | service + review low-cassette alerting |

---

## 6. What this would have changed on NHSWS-18867

| | Today | With CashTrailView |
|---|---|---|
| Find the $400 | manual, across `CIMreject` / `CDMreject` / `CDMUSD50` / `CDMUSD20` at ms timestamps | one row, `CashBins` |
| Know it isn't deposit money | requires knowing LCU01 is combined | `origin = DISPENSER`, `sharedWith = CIM` |
| Prove deposits balance | reconcile 8 deposits against a denomination roll-up by hand | `DEPOSIT VARIANCE $0.00` |
| Know the customer was made whole | spot the reversal one second later in a 169-row sheet | `REVERSED_NO_CUSTOMER_LOSS` |
| Answer the customer | hours | the `VERDICT` line, near enough verbatim |

---

## 7. Cheap fixes to the *existing* sheets

Independent of the new view, and worth doing regardless — these remove the traps at the source.

1. **Add `USD0…USD100` + `amount` to `CDMreject` / `CDMretract`.** Derivable from the simultaneous
   per-cassette `reject` increments. Today the only denominated view of the reject bin is on a CIM
   sheet, which is the root of this whole ticket.
2. **Add a `notPresented` column** (`Δdispensed − Δpresented − Δretracted`) and a `$value` column to
   every cash-unit sheet. One column, and the analyst never has to subtract across sheets again.
3. **Device-qualify unit labels** — `CDM.LCU02` / `CIM.LCU02` — everywhere. Today the same label means
   two different cassettes.
4. **Add a `sharedWith` column** to `CIMreject` / `CIMretract` when the unit is shared, with the
   comment *"combined reject/retract unit — contents may be dispenser cash."*
5. **Backfill the failed-dispense row in `Dispense`.** At 20:36:52 it reads `amount 0` with no note
   counts, because `WFS_CMD_CDM_DISPENSE` failed. The 11 notes are known from the cassette deltas —
   show them as `picked` alongside `amount 0`.
6. **Conditional formatting**: red on any non-zero variance, amber on `low`/`empty` cassette status.
   `BaseTable.WriteExcelFile` already does this for `Exception`; extend the same block.

Items 1–3 are the ones that would most likely have prevented the misread on their own.

---

## 8. Files

```
src/CashTrailView/CashTrailView.cs        [Export(typeof(IView))], ParseType.SP, analysis-only
src/CashTrailView/CashTrailTable.cs       the analyzer
src/CashTrailView/CashTrailView.xsd       three tables: CashBalance, CashTrail, CashBins
src/CashTrailView/CashTrailView.xml       empty seed
src/CashTrailView/CashTrailView.csproj    ProjectReference: BaseView, Contract, Impl
src/CashTrailView/Properties/AssemblyInfo.cs
```

`CashTrailTable` is deliberately **not** coupled to `LogLineHandler` or `SPLogLine` — it reads
`DataSet`s, not log lines — so the csproj has three project references instead of five.

Before adding to the solution: regenerate the `ProjectGuid` in the csproj and the `Guid` in
`AssemblyInfo.cs` (both currently placeholders), and confirm the `ProjectReference` GUIDs for
BaseView / Contract / Impl still match mainline.

### Verification performed

`CashTrailTable` was compiled (C# 7.2, `-langversion` capped by mcs; nothing above 7.2 is used) against
stub `Contract`/`Impl` assemblies and **executed** against `CDMView.xml` / `CIMView.xml` /
`OverView.xml` reconstructed from NHSWS-18867's own workbook in pre-rename table form. It produced the
`CashBalance`, `CashBins` and `CashTrail` output quoted verbatim above. Topology detection was checked
on the hard cases: `LCU00`/`LCU01` correctly identified as shared; `LCU02` correctly **rejected** as a
name collision.

Not yet exercised: Excel COM output (`WriteExcelFile`), MEF discovery, real `IContext`/`ioProvider`.

---

## 9. Wiring — decisions to lock

1. **ParseType.** Currently `ParseType.SP`, copying `DepositAnalysisView`, so it rides `-s`. But it
   needs `OverView.xml` too. Options: (a) leave as SP and degrade gracefully when Over wasn't run —
   what it does now, emitting `ORIGIN_UNKNOWN` and skipping the balance section; (b) give it its own
   flag, e.g. `--cash`. **Recommend (a)** for MVP; it produces useful output from `-s CDM,CIM` alone
   and full output from `-a Over -s CDM,CIM`.

2. **Analyze-phase ordering.** `Program.cs` iterates views in MEF discovery order. If a future CIM
   analysis pass appends rows to `CashIn-N` during Analyze, CashTrailView could see them or not
   depending on order. The empty-`status` guard (§3) covers the rows that exist today, but ordering
   between analysis views is worth making explicit rather than incidental.

3. **`Program.cs` per-view try/catch.** Flagged as HIGH #2 in `Core_Infra_Review.md`: one view throwing
   aborts the whole run. Adding an eighth analysis view raises the cost of that bug. Worth fixing in
   the same PR.

4. **Non-USD / multi-currency.** `DenomValues` is a fixed USD ladder, matching
   `DepositAnalysisTable`. Fine for the fleet today; the `Summary` table's `currency` column is the
   hook if that ever changes.

## 10. Open questions

1. **Retract-bin origin.** I2 is proven on the reject bin. Retract is the same shape (notes presented
   then taken back = `Δretracted`), but there is no retract event in this workbook to test against.
   Do you have a ticket with a real retract so the rule can be validated before it ships?

2. **Match window.** 5 s for bin↔cassette (the real events are 10 ms apart), 120 s for SP↔AP, 180 s for
   the reversal. The reversal window is the one that matters — too short and I5 raises false
   `CUSTOMER_DEBITED_NO_CASH` alarms. What is the real core timeout, and does it vary by banking core?

3. **Presented-but-not-taken.** `Δpresented` counts notes that reached the shutter. If the customer
   walked away, the notes are retracted and I4 still holds, but `toCustomer` overstates. Should
   `CashTrail` distinguish *presented* from *items taken*? The `Dispense` sheet has both.

4. **Grain of `CashTrail`.** One row per AP transaction + bin event + exchange (MVP). Or one row per
   session, collapsing the multi-transaction sessions like 3501? Sessions with several transactions
   are common enough here that it may read better.

6. **Card extraction.** The current resolver takes PAN-shaped values from `OverSummary`'s `card`
   column plus `Session.IdentificationNumberMasked`. There are almost certainly better sources —
   the `LogTransactionData [FLOWPOINT]` JSON, the NDC ATM2HOST messages, the EMV TLV in the journal.
   If you point me at a log with a card read that the current rule misses, the fix is one regex and
   one extra index. The `UNK` contract stays either way.

7. **Does the reject bin get counted with deposits?** The whole reclassification rests on that. If
   F&M's cash-handling procedure says otherwise, the finding stands but the *remedy* changes. Worth
   confirming with the customer as step 1 of the reply.
