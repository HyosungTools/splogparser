# splogparser

[![splogparser build](https://github.com/HyosungTools/splogparser/actions/workflows/build.yml/badge.svg)](https://github.com/HyosungTools/splogparser/actions/workflows/build.yml)

Utility to unzip, merge and present ATM / video-teller logs as a set of worksheets in an Excel file.
Point it at a log zip (or folder), tell it which log families and which views you want, and it produces
one `.xlsx` where each worksheet is a focused view of what the machine did — turning an
hours-long log dig into a few minutes of reading.

## What it can do

splogparser understands many different log families produced by a Hyosung ATM / BlueVerse Teller and
cross-references them into readable worksheets:

- **Application (AP) logs** — transaction workflow with an ATM-state **WallClock** pie chart per
  period, a cash-in/cash-out reconciliation ledger, per-flowpoint transaction data, EMV/card/EJ
  detail, installed package versions, merged configuration parameters, and loaded encryption keys.
- **Web-service conversations** — the ATM's dialog with the banking **core** (host authorization,
  account lookups, dispense approvals). Multiple cores are supported (FiservDNA, JackHenry,
  SymXchange, CU\*Answers, and more).
- **Service-Provider (SP / `*.nwlog`) WFS/XFS traces** — device-level detail for the cash dispenser
  (CDM), **coin dispenser**, cash-in module (CIM), check/item processor (IPM), card reader (IDC),
  PIN pad, sensors (SIU), plus physical cassette configuration and serial-number history.
- **Active Teller / BlueVerse Teller** — agent, agent-extension, workstation and server logs,
  including a one-row-per-transaction **teller transaction** summary and a call/session summary.
- **Other sources** — BeeHD video, WinCE journal & trace, IIS, Settlement Server API, A2iA check
  recognition, MoniView server logs, and TCP wire traces.

See the [Parse Types](#parse-types) and per-family view tables below for the full menu.

## Quick Start Guide

### Prerequisites

You *must* have Excel installed on the workstation you run splogparser on (it drives Excel through COM
automation to build the workbook).

### Accessing the release

On the right-hand side of the GitHub repo you'll see [Releases](https://github.com/HyosungTools/splogparser/releases).
For the latest release, scroll to the Assets and download `release.zip`.

### Install

It's a console app — there is no installer. Unzip the files into a folder (e.g. `C:\Work_Tools\splogparser`).

Open a `cmd.exe` in the folder of your log zip (e.g. `C:\Work_Bugs\ATMD4555`) and make it aware of the
splogparser location:

```text
C:\Work_Bugs\ATMD4555> set path=%path%;C:\Work_Tools\splogparser
```

Confirm with `where splogparser`:

```text
C:\Work_Bugs\ATMD4555> where splogparser
C:\Work_Tools\splogparser\splogparser.exe
```

### Run

Every run needs a **target** (`-f`, a `.zip` or a folder) plus one or more **Parse Types** and one or
more **Views**. The target alone does nothing — you have to say what you want.

```text
splogparser -s * -f 20221116175903.zip
```

That parses every SP view; the output is `20221116175903__SP.xlsx`. The `*` means "all views for this
parse type" (the command-line parser needs the explicit `*`). To limit to specific views, list them:

```text
splogparser -s CDM,CIM -f 20221116175903.zip
```

which writes `20221116175903__SP_CDM_CIM.xlsx`.

## Parse Types

Each Parse Type selects a log family by filename pattern. Combine one or more Parse Types with one or
more Views.

| Short | Long | Log files | Parses |
|-------|------|-----------|--------|
| `-a` | `--ap` | `APLog*.*` | Application (AP) logs |
| `-t` | `--atagent` | `ActiveTellerAgent_*.*` | Active Teller agent (ITM) logs |
| `-e` | `--atagentextensions` | `ActiveTellerAgentExtensions_*.*` | Active Teller agent-extension logs |
| `-w` | `--atworkstation` | `Workstation*.*` | Active Teller workstation logs |
| `-v` | `--atserver` | `ActiveTellerServer*.*` | Active Teller / BlueVerse Teller server logs |
| `-b` | `--be` | `rvbeehd*.*` | BeeHD video logs |
| `-s` | `--sp` | `*.nwlog` | Service-Provider WFS/XFS traces |
|      | `--sf` | `*.nwlog` | SP "flat" parsing (alternate nwlog reader) |
| `-r` | `--rt` | `JNL*.dat` | Retail journal (JNL) records |
| `-c` | `--ce` | `*_????????.log`, `JNL*.dat` | WinCE trace & journal (use `*` only) |
| `-i` | `--ii` | `u_ex*.log` | IIS web logs |
|      | `--ss` | `settlement-api-all-*.log` | Settlement Server API logs |
|      | `--a2` | `A2iaResults*.log` | A2iA check-recognition results |
|      | `--tcr` | TCR AP logs | Teller Cash Recycler AP logs |
|      | `--mv` | `MoniViewServerLog*.txt`, `TcpTrace_*.txt` | MoniView server logs & TCP wire traces |

## Views by Parse Type

### `-a` / `--ap` — Application logs

| View | Description |
|------|-------------|
| Over | Transaction workflow overview + ATM-state **WallClock** pie charts (per-period state segmentation) |
| Txn | Cash-in / cash-out reconciliation ledger built from the AP log |
| TransData | Per-flowpoint transaction data (account, amounts, type) |
| WS | **Web-service conversations** with the banking core (auth, lookups, dispense approvals); multi-core |
| Disp | Cash dispense (application view) |
| EJ | Electronic-journal insert commands |
| Emv | EMV chip / contactless tag data |
| Card | Card reader / card events |
| Install | Installed programs and package versions |
| XmlParam | Merged configuration parameters (the Config → … → ConfigRuntime override chain) |
| AddKey | Encryption keys loaded at start-up |
| `*` | All of the above |

### `-s` / `--sp` — Service-Provider (`*.nwlog`) logs

| View | Description |
|------|-------------|
| CDM | Cash dispenser: status, dispense operations and counts |
| Coin | **Coin dispenser** (separate device, rare): status, dispense, not-dispensable, coin-unit counts. **Opt-in** — name it explicitly (`-s Coin`); it is *not* pulled in by `-s CDM` or `-s *` |
| CIM | Cash-in module: status, deposit operations and counts |
| IPM | Check / item processor: status, deposits and counts |
| IDC | Card reader: status and inserts |
| PIN | PIN pad status |
| SIU | Sensors & indicators (safe open/close, enter/exit supervisor, etc.) |
| DEV | Generic device status over time |
| Extra | The `lpszExtra` values from device status — good for flagging error codes |
| PHY | Physical cash-dispenser cassettes: configuration (position, unit ID, currency, denomination) + per-cassette time-series status/counts |
| NHCDM | NH CDM physical cassette serial numbers, note revision, calibration, missing-check — use to track cassette swaps (requires NH CDM hardware) |
| `*` | All of the above |

### `-e` / `--atagentextensions` — Active Teller agent extensions

| View | Description |
|------|-------------|
| AE | MoniPlus2s / Nextware / NetOp events, device sessions and faults, recording uploads |
| TellerTxn | One row per teller-assisted transaction: type, amount, teller approval, device fault, outcome |
| `*` | All of the above |

### `-t` / `--atagent` — Active Teller agent

| View | Description |
|------|-------------|
| AT | Active Teller agent (ITM) events |
| `*` | All of the above |

### `-w` / `--atworkstation` — Active Teller workstation

| View | Description |
|------|-------------|
| AW | Workstation log detail |
| Call | A *summary* view of the workstation log (call / session) |
| `*` | All of the above |

### `-v` / `--atserver` — Active Teller / BlueVerse Teller server

| View | Description |
|------|-------------|
| AV | Active Teller server events |
| BVTServer | BlueVerse Teller (ActiveTeller) server summary |
| `*` | All of the above |

### `-b` / `--be` — BeeHD video

| View | Description |
|------|-------------|
| BHD | BeeHD video-session detail (large files: use time filtering to limit lines) |
| `*` | All of the above |

### `-r` / `--rt` — Retail journal

| View | Description |
|------|-------------|
| JNL | Journal (`JNL*.dat`) records |
| `*` | All of the above |

### `-c` / `--ce` — WinCE (use `*` only)

| View | Description |
|------|-------------|
| WinCEJournal | WinCE journal records |
| WinCETrace | WinCE binary service-provider trace |
| `*` | All of the above |

### `--mv` — MoniView server

| View | Description |
|------|-------------|
| MVErrors | MoniView server errors |
| MVOutbound | MoniView outbound messages |
| MVRoster | MoniView roster |
| WireTrace | TCP wire traces (`TcpTrace_*.txt`) |
| `*` | All of the above |

### `-i` / `--ii`, `--ss`, `--a2`, `--tcr`

| Parse Type | Views | Description |
|-----------|-------|-------------|
| `--ii` | `*` | IIS (`u_ex*.log`) web logs |
| `--ss` | `*` | Settlement Server API events (uploaded / created / discovered / imported) |
| `--a2` | `*` | A2iA check-recognition results |
| `--tcr` | `*` (TCR, Trans) | Teller Cash Recycler AP logs |

## Global Options

### Time filtering

Specify both a start time and a span (in minutes) to skip log lines outside the window — this can make
a scan much faster on large multi-day archives.

```text
--timestart 202311040600 --timespan 1440
```

Start time is `yyyyMMddHHmm` (24-hour clock; e.g. `1400` = 2:00 PM). Timespan is in minutes and is
**required** when `--timestart` is used.

### Include the raw log line

Each worksheet has a payload column that can optionally carry the raw log line. It's excluded by
default; to include it:

```text
--rawlogline
```

## Sample commands

Parse the SP logs and show all dispense and deposit operations:

```text
splogparser -s CDM,CIM -f 20221116175903.zip
```

Look at the coin dispenser (opt-in — it's a separate device and rare on ATMs):

```text
splogparser -s Coin -f 20221116175903.zip
```

*(Coin is produced only when named explicitly. `-s CDM` and `-s *` do **not** include it, so cash-only
machines never get empty coin sheets. Use `-s CDM,Coin` if you want both.)*

Physical cassette configuration and serial-number history:

```text
splogparser -s PHY,NHCDM -f 20221116175903.zip
```

Configuration settings in table form:

```text
splogparser -a XmlParam -f 20221116175903.zip
```

Reconstruct teller-assisted transactions, with cash dispense and check disposition:

```text
splogparser -a Over -e * -s CDM,IPM -f APLog_xxx.zip
```

All AP views plus the SP dispense view:

```text
splogparser -a * -s CDM -f 20221116175903.zip
```

Settlement server events:

```text
splogparser --ss * -f settlementserverlogs.zip
```

## Known Issues

It's a really dumb install. If you're upgrading, unzip to a clean folder (or clear the existing one)
so you don't accidentally pick up DLLs from a previous release.

When you download `release.zip` from GitHub you may be prompted for a virus scan. If that doesn't
happen, Windows may have **blocked** the parser DLLs — you'll see the parser finish quickly, produce
no Excel file, and log `Number of Views : 0`. The fix is to *unblock* each DLL from its file
Properties page.

The SIU view can take a long time to run. If you don't need it, don't use `*` — list the individual
views you want instead.

Occasionally the unzip step fails (cause unknown). Workaround: unzip the log manually, re-zip the
`[SP]` subfolder, and point splogparser at that zip. Any zip works as long as it contains an SP
folder.

## How to Contribute

Anyone can contribute. Please read:

- [Development Environment](https://github.com/HyosungTools/docs/blob/main/DevelopmentEnvironment.md)
- [Making a Change](https://github.com/HyosungTools/docs/blob/main/MakingChanges.md)

Then:

- [General Design](https://github.com/HyosungTools/splogparser/blob/main/docs/GeneralDesign.md)
- [How to Build](https://github.com/HyosungTools/splogparser/blob/main/docs/HowToBuild.md)
