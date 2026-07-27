# CallView — integration guide (beta)

CallView is a new splogparser View that turns the Active Teller **Workstation** log into a
scannable **Call Ledger**: one row per teller call, with disposition (Clean / DROPPED) and, for
drops, the root cause. It also emits a **DroppedCalls** worksheet with a short correlated timeline
for each drop.

It reuses the existing `-w` (Workstation / AW) handler, so there is **no change to Program.cs** —
MEF discovers the DLL automatically.

## Files in this folder

| File | Purpose |
|---|---|
| `CallView.cs` | The View class. `[Export(typeof(IView))]`, `ParseType.AW`, name `"CallView"`. |
| `CallTable.cs` | Session reconstruction + emits the CallLedger / DroppedCalls tables. |
| `CallView.xsd` | Schema for the two worksheets (all string columns). |
| `CallView.xml` | Seed file (empty — no lookup Messages needed). |
| `CallView.csproj` | Project file, modeled on `AWView.csproj` (same references). |
| `Properties/AssemblyInfo.cs` | Standard assembly info. |

## Steps in Visual Studio 2022

1. Copy this `CallView` folder into `src\` next to `AWView`, i.e. `src\CallView\`.
2. In Solution Explorer, right-click the solution → **Add → Existing Project…** → select
   `src\CallView\CallView.csproj`.
3. Confirm the target framework is **.NET Framework 4.7.2** (it already is in the csproj) and that
   the five project references resolve (AWLogLine, BaseView, Contract, Impl, LogLineHandler).
4. Build the solution:
   `msbuild src\splogparser.sln /t:Build /p:Configuration=Release /p:Platform="Any CPU"`
5. Make sure `CallView.dll`, `CallView.xsd`, and `CallView.xml` land in the **dist** folder next to
   the other view DLLs (the same way AWView is distributed). If your dist copy is a post-build step
   on each project rather than the `Content`/`CopyToOutputDirectory` used here, add the equivalent
   copy lines from `docs/HowIAddedSIUView.md`.

## Running it

Point it at a zip (or the working folder) that contains a `Workstation*.log`:

```
splogparser.exe -w Call -f Workstation_TX005266_20260710.zip
```

- `-w Call` selects CallView (the `-w` verb runs the Workstation handler; `Call` = this view).
- `-w *` would run every Workstation view (AWView dump + CallView).
- Optional time window (Mike's existing feature) to focus on a reported drop:
  `splogparser.exe -w Call --timestart 202607101100 --timespan 30 -f <zip>`

Output: an Excel workbook with a **CallLedger** worksheet (one row per call) and, if any drops were
found, a **DroppedCalls** worksheet.

## What this beta covers (and what it doesn't, yet)

Covered now:
- One row per answered teller call: start, end, duration, asset, teller, assisted transaction,
  teller approvals, disposition, root cause.
- **Client-crash drops** detected via the VideoManager "file being used by another process"
  unhandled-exception signature (NHSWS-17180 family).
- A per-drop timeline on the DroppedCalls sheet.

Not in this beta (next iterations):
- **Other drop causes** (post-ACK race, RTCP/media degradation, socket errors). Those live in the
  `rvbeehd` (BE) log; a later version will fold BeeHD dispositions into the ledger.
- **Automatic ATM↔teller correlation.** This view is teller-side (Workstation log) only. The two
  clocks aren't synchronized, so cross-machine correlation needs auto time-alignment on the Call-id
  — a later step.
- The teller-assisted **WD/DEP transaction** view (AP log + FiservDNA + CheckProcessing).

## Feedback

This is a beta for the support team to try. If the build throws errors, or the ledger miscounts /
mislabels a call on a real zip, capture the command line + the console output (or the produced
workbook) and send it back — the parsing rules are easy to adjust from a failing example.
