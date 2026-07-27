# TxnView — cash-in / cash-out transaction ledger (AP)

A new splogparser view that reconstructs transactions from the **AP log** into a
cash reconciliation ledger — one row per transaction, answering "where's my money":
requested vs dispensed vs taken, with the note breakdown.

It is an **AP-parseType** view (teller-controlled transactions live in the application log,
not the SP device trace), so it rides the existing `-a` handler — no `Program.cs` change.

## Install (same as CallView / SIUView)

1. Copy this `TxnView` folder into `src\` next to `OverView`.
2. Solution Explorer → right-click solution → **Add → Existing Project…** → `src\TxnView\TxnView.csproj`.
3. Build in **Release** (Release output goes straight to `dist` via `Directory.Build.props`;
   Debug does not). Confirm `TxnView.dll`, `TxnView.xsd`, `TxnView.xml` land in `dist`.
4. Run:  `splogparser.exe -a Txn -f <zip containing APLog*.log>`
   (add `-a *` to run every AP view.)

Output: a **TransactionLedger** worksheet.

## Columns

`time · type · requested · cashout · cashin · notes · taken · teller · outcome · fault · account`

- **requested** — `CurrentTransaction.Amount`.
- **cashout** — authoritative per-cassette `Last Dispensed Count A/B/C/D` × denomination.
- **notes** — generic cassette breakdown (e.g. `4x$20`), denominations read from the log's
  `Dispenser Unit Value` lines at run time — **not hardcoded** (per the PHYView convention).
- **cashin** — deposits, from `BillMixTotalAmount` + `TotalCheckAmount`.
- **outcome** — `Completed - taken` / `DISPENSED - not taken` / `RETRACTED` / `No dispense` / `Completed`.
- **fault** — the "where's my money" flags: `REQUESTED != DISPENSED`, `cash not taken`, `Host timeout`.

A `TxnView: N transactions (C clean, F flagged). Cassette denominations [A=$1, B=$5, C=$20, D=$100].`
summary line prints near the end.

## Verified

Reconstruction was validated against real teller-assisted withdrawals in `APLog20260710`:
FastCash $80 = 4×$20, WD $25 = 1×$5+1×$20, WD $400 = 4×$5+4×$20+3×$100, WD $1000 = 10×$100 —
requested = dispensed = taken in every case.

## Known limits (next iterations)

- **Deposit cash-in** is the counted bill/check total; the per-note deposit breakdown (CIM
  acceptor counts) is a later add, mirroring the dispense side.
- Denomination order assumes the first full set of `Dispenser Unit Value` lines is A,B,C,D in
  order (true on the sample). If a machine reports differently, the summary legend will show it.
