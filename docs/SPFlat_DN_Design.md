# Parsing Diebold-Nixdorf "flat" nwlogs (`--sf`)

Design + reference for the `--sf` (SPFlat) subsystem: how splogparser turns a competitor
(Diebold-Nixdorf / Nextware) "flat" `*.nwlog` trace into the same device worksheets we get from a
structured Hyosung `[SP]` nwlog. Written from the reverse-engineering of the `NHSWS-18804-DN` ticket
(`.../[SP]/Nextware/BSTrace.nwlog`).

Audience: developers extending the flat parser (a new device view, a new competitor format, or naming
more XFS command codes).

---

## 1. Why this exists

A Hyosung machine's SP nwlog is structured WFS/XFS text that the `-s`/`--sp` path
(`SPLogHandler` → `SPLine.Factory` → `WFS_*`) parses richly. A DN machine's SP nwlog is a **flat**
binary-ish trace in a completely different shape, produced by the Nextware ActiveX service providers.
Pointing `-s` at it produces an **empty workbook** (the structured parser can't read a single record —
you'll see `0 in time range` and `1/1/0001` timestamps). The DN format is handled by the separate
`--sf` verb.

> **Support gotcha:** `-s`/`--sp` on a DN machine silently yields an empty workbook (no error). The fix
> is `--sf *`. A future "this looks like a flat/DN nwlog — try `--sf`" hint when the SP handler lands
> zero rows would save a support round-trip.

---

## 2. The flat record format (length-prefixed TLV)

`BSTrace.nwlog` is **not** line-oriented text. It is a stream of records, each a run of length-prefixed
ASCII fields: `NNNN<value>` where `NNNN` is a **4-digit decimal length** and `<value>` is exactly that
many characters. Little binary framing sits between records (the handler renders non-printable bytes as
spaces).

A device record's fields, in order:

```
0003 <DEV>     device tag: CDM, CIM, IPM, IDC, PIN, SPR, VDM, COD, DEP, JPR, ...
0007 ACTIVEX   source (the ActiveX SP)
0010 <date>    yyyy/MM/dd
0012 <time>    "HH:MM SS.mmm"   (note: a SPACE where the second ':' would be)
NNNN <CATEGORY> PROPERTY | INFORMATION | METHOD | EVENT | XFSAPI | ERROR
NNNN <METHOD>   a Ctrl::Xxx property/method, or a service-handler symbol
NNNN <PAYLOAD>  Name[value] | Name[(v1)(v2)(v3)] arrays | "Invoked {k[v], ...}"
NNNN <trailer>  hResult sentinel (4294967295 = 0xFFFFFFFF = void) + sequence
```

Example (CDM device-status record), raw:

```
0003CDM0007ACTIVEX00102026/07/10001210:15 01.4600008PROPERTY0021Ctrl::GetDeviceStatus0023DeviceStatus[DEVONLINE]0122...
```

decodes to: DEVICE=CDM, DATE=2026/07/10, TIME=10:15 01.460, CATEGORY=PROPERTY,
METHOD=`Ctrl::GetDeviceStatus`, PAYLOAD=`DeviceStatus[DEVONLINE]`.

### The timestamp
The record date is `yyyy/MM/dd` (10 chars, prefix `0010`) followed immediately by the time field
(prefix `0012`, value `HH:MM SS.mmm`). Concatenated in the raw stream they read
`2026/07/10001210:15 01.460`. Both the handler and `SPFlatRecord` recognise the record by this
signature. Normal form is `yyyy-MM-dd HH:MM:SS.mmm`.

---

## 3. Framing & device attribution (the critical subtlety)

**Only ~70% of records carry the `0003<DEV>0007ACTIVEX` device envelope.** The other ~30% are
framework / SP-level records (`CMsgWnd::DefWindowProc`, `CContextMgr::MgrWndProc`, `CService::*`) with a
different or absent envelope. On the sample: 60,450 timestamps vs 42,266 device envelopes.

Consequences:
- **The timestamp is the only reliable per-record delimiter.** Framing on the device envelope would
  swallow the ~30% enveloped-less records. So `SPFlatLogHandler` frames on the **timestamp**.
- In a plain timestamp-framed line the device tag sits at the *end of the previous record* (envelope
  precedes the date, so `line[i]`'s date is preceded by `...0003<DEV>0007ACTIVEX0010`). So a naive
  timestamp-framed line does not contain its own device tag.

**Fix (`SPFlatLogHandler.OpenLogFile`):** frame on the timestamp, but when a device envelope
(`0003<DEV>0007ACTIVEX0010`, exactly 22 chars) sits immediately before a record's date, **extend that
line's start back 22 chars** to include it. Device records then carry their own device tag; framework
records are untouched; no record is dropped (still one line per timestamp). This is what makes
`Record.Device` reliable — and it's the prerequisite for the per-device views and the (pending) CDM/CIM
Status sheets. Verified: every device record (CIM/IPM/IDC/CDM/PIN/...) attributes correctly; the "?"
bucket is only framework chatter.

---

## 4. The generic decoder — `SPFlatRecord`

`SPFlatLogLines/SPFlatRecord.cs` — standalone, dependency-free, unit-tested against real DN **and** FI
records (`SPFlatRecordTests`). `Decode(record)` →
`{ Device, Source, Date, Time, Category, Method, Payload, Fields[] }`.

Algorithm: find the record's own timestamp; split the matched text into Date + Time; TLV-walk the fields
*after* the timestamp for Category / Method / Payload; read Device from a `0003<DEV>0007ACTIVEX`
envelope **immediately before** the date (else `"?"`). Robust to both framings, so it was safe to
introduce before the handler was reframed.

---

## 5. Routing — `SPFlatLine.Factory`

Decodes once via `SPFlatRecord.Decode`, then routes on **method + payload** (not device — device
attribution is only reliable after the handler fix, and most methods are device-unique anyway: only 26
of ~1328 method names are shared across devices, and those are the status methods). Order:

1. `_COMPLETE(... hResult=-)` → `Error`.
2. **CDM** method routes → CDM-specific line types.
3. **CIM** method routes → CIM-specific line types.
4. **Per-device** (`rec.Device == "IPM" || "IDC" || "PIN"`) → `SPFlatDeviceLine` (generic; the device's
   table filters by `Record.Device` and routes on method/payload).

Two `SPFlatType` enums exist in the codebase (`Impl` and `LogLineHandler`) — the flat path uses
`LogLineHandler.SPFlatType`; qualify when both are in scope.

---

## 6. DN dialect vs FI/Hyosung-flat

The pre-existing `--sf` code was written for an **FI/Hyosung-flat** dialect and matched ~nothing on DN.
Key differences:

| Concept | FI/Hyosung-flat | Diebold-Nixdorf |
|---|---|---|
| CDM cash units | per-property `Ctrl::GetUnitID/Type/Value...` | one consolidated `Ctrl::TraceCDMCashUnitInfo` (parallel arrays) |
| CIM cash units | per-property | one line per unit `CLogicalUnit::TraceCIMCashUnitInfo` |
| CDM/CIM unit model | Unit* | CIM Bin*/LogicalUnit; IPM Bin* |
| Cash dispense | `Ctrl::Dispense` / `HandleDispense` | **XFS command-code events** (see §8); `Ctrl::Dispense` = 0 |
| Cash-in start | `Ctrl::StartCashInEx` | `Ctrl::StartCashIn` (no `Ex`) |
| Currency method | `Ctrl::GetUnitCurrency` | `Ctrl::GetUnitCurrencyID` |
| Status value form | `Name[VALUE]` | mostly `Name[VALUE]`; **IDC uses `Name = VALUE`** |

---

## 7. Per-device views

All are `ParseType.SF`, selected by `--sf *` (view names end `_Flat`; `--sf CDM` won't match — use `*`).
`Status` tables auto-rename per view (`CDM_FlatStatus`, `IPM_FlatStatus`, ...). `Messages` is a lookup,
not written to Excel.

- **CDMView_Flat** — `Summary` (cash units, incl. `UnitValue` = per-cassette denomination, from
  `TraceCDMCashUnitInfo`); `Dispense` (denominate rows + XFS command-code operations, see §8).
- **CIMView_Flat** — `Summary` (from `TraceCIMCashUnitInfo`; keyed on the unit **Number**, not the
  0-based index — see the off-by-one note); `Deposit` (cash-in lifecycle).
- **IPMView_Flat** — `IPM_FlatStatus` (device/media/acceptor/stacker/ink/toner/shutter timeline);
  `Bins` (media bins: number/type/status/count from `GetBinType/GetBinStatus/GetBinCount` arrays).
- **IDCView_Flat** — `IDC_FlatStatus` (device/media/type; handles the `Name = VALUE` form); `Cards`
  (card operations from `HandleXFSResult` command codes + `ChipIO`).
- **PINView_Flat** — `PIN_FlatStatus`; **`Keys`** (loaded encryption keys: name/use/loaded from
  `GetKeyName/GetKeyUse/GetKeyLoaded` arrays — the crypto-diagnostics gem); `Ops` (PIN operations).

`SPFlatDeviceLine` (generic decoded record) feeds IPM/IDC/PIN; each table filters `Record.Device` and
routes on method. Adding a 6th device = new view project (clone IPM) + add its tag to the Factory
per-device route; no other shared change.

---

## 8. The XFS command-code channel

DN does **not** log dispense/present/reject/card-ops/pin-ops as named handlers. It logs the completion
of each XFS command as an event:

```
EVENT  C<DEV>Service::HandleXFSResult  ->  FireXFSEvent [uiMsg=408, dwCommandCode=<N>, hResult=<M>]
```

`uiMsg=408` = `WFS_EXECUTE_COMPLETE`; `dwCommandCode` is the `WFS_CMD_<DEV>_*` command; `hResult` 0 =
success, negative = fault. This is where the real operations live (the current DN sample had 12 CDM
dispenses + 25 rejects here, invisible until we parsed this channel).

**Naming is data-driven** via each view's `Messages` lookup, `type = "dwCommandCode"`, `brief` = English
name. Add a code = one `<Messages>` XML row, no recompile. Codes derived from the DN log by correlating
each `dwCommandCode` with the `Ctrl::` method that issued it, cross-checked against the XFS enums:

| Device | Code → name (seeded) |
|---|---|
| CDM | 301 denominate · 302 dispense · 303 present · 304 reject · 311 start exchange · 312 end exchange · 321 reset |
| IDC | 203 eject card · 204 retain card · 207 read card · 209 chip io · 210 reset · 212 parse data · 215 power save |
| PIN | 401 generate random · 405 read pin · 407 build pin block · 408 read data · 432 get certificate · 439 generate kcv · 446 import key block |

Lookup uses `Tables["Messages"].Select("type='dwCommandCode' AND code='N'")` (IDC/PIN) — no primary key
needed. To keep the sheet readable, the CDM/IDC/PIN operation sheets show **named** operations plus **any
faulted** operation (even an un-named code shows on failure, as "command N" + hResult).

---

## 9. Known gaps / future work

- **CDM/CIM Status sheets** — those views have empty `Status` tables; now unblocked by device
  attribution. Populate from `GetDeviceStatus/GetDispenserStatus/GetAcceptorStatus/GetShutterStatus`.
- **Un-named command codes** — e.g. CDM 319/323 had no clean trigger in the sample; add to the
  `dwCommandCode` Messages when confirmed.
- **Per-bin / per-unit time series** — the IPM `Bins` and CDM/CIM summaries show final state; a
  time-series (like the structured views' `MediaBin-*` / `Unit-*` sheets) is a later enhancement.
- **Payload decode for encrypted content** — out of scope; not present in the SP flat trace.

## 10. Test-first note
Per project rule, parsing changes start from a failing unit test built from a real log line.
`SPFlatRecordTests` covers the decoder against both DN (device-anchored) and FI (timestamp-framed)
records — the FI cases are the regression guard that caught the decoder breaking the FI dialect during
the generic-decoder rework.
