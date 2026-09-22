# splogparser

C# / **.NET Framework 4.7.2** console app that parses ATM log archives and writes Excel
diagnostic workbooks for the software support team. Each worksheet is a **View** of the logs.
End users are support staff, not developers — clear labelling and readable output matter as
much as correctness.

`README.md` is the user-facing manual: every parse type, every view, every CLI flag, sample
commands. **Read it before answering questions about what the tool can do or how to invoke it.**
This file covers how the code is built and how to change it.

## Build & test

The solution is `src/splogparser.sln`. NuGet restore must run from `src/`, not the repo root.

```
cd src && nuget restore && cd ..
msbuild src\splogparser.sln /m:1 /t:Clean,Build /p:Configuration="Release" /p:Platform="Any CPU" /nodeReuse:false
msbuild src\splogparser.sln /t:RunUnitTests /p:Configuration="Release" /p:Platform="Any CPU"
```

These are exactly what `.github/workflows/build.yml` runs. `/m:1` and `/nodeReuse:false` are
deliberate — parallel builds and reused MSBuild nodes cause intermittent failures here.

`RunUnitTests` (defined in `src/Directory.Build.targets`) shells out to `vstest.console.exe`
for every project whose output ends in `Tests.dll`, so VSTest must be on PATH.

This is .NET **Framework**, not .NET Core / .NET 5+. `dotnet build` and `dotnet test` do not
apply. There is no `build-like-ci` script in this repo.

### Where output lands

Release builds of non-test, non-`Samples` projects output to `dist/` at the repo root
(`src/Directory.Build.props`), and `CopyViewDataFiles` in `Directory.Build.targets` copies each
project's `*.xml` and `*.xsd` there too. **A View's XML/XSD must sit in its project directory or
it silently won't reach `dist/`, and the View will fail at runtime.** `dist/` is gitignored.

### Project naming drives build behaviour

`Directory.Build.props` infers properties from the project name:

- ends in `Tests` → `IsTestProject=true` (excluded from `dist/`, included in `RunUnitTests`)
- ends in `Samples` → excluded from `dist/`
- ends in `splogparser` → `IsMainProject=true`

Name new projects accordingly. A test project not ending in `Tests` will never run.

## Repo layout

```
src/            ~90 projects — one per View, plus Line/LogLine libraries and tests
src/Contract/   IView, IOptions, IContext, ParseType — the interfaces everything implements
src/Impl/       Context, Logger, LogFind, LogTime, FileReaderFactory, DataTable helpers
src/splogparser/  Program.cs — the driver; VersionInfo.cs is CI-generated
src/BaseView/   BaseView, BaseTable
src/*Line/, src/*LogLine/   line-type classes + Factory methods per log family
.github/workflows/  build.yml (push/PR to main), release.yml (manual dispatch)
installer/      Inno Setup script
testdata/, *Samples/   sample logs for tests
docs/           design docs
```

## Architecture

MEF plugin system. Views are separate DLLs discovered at runtime via `[Export(typeof(IView))]`.

### Every View is four files

| File | Role |
|---|---|
| `XxxView.cs` | inherits `BaseView`, implements `IView`, MEF export attribute, overrides `CreateTableInstance` |
| `XxxTable.cs` | inherits `BaseTable`, holds the `ProcessRow()` parsing logic |
| `XxxView.xml` | seed data — empty DataTable rows plus lookup Messages |
| `XxxView.xsd` | schema defining the DataTable columns |

Minimal View, matching `src/CDMView/CDMView.cs`:

```csharp
[Export(typeof(IView))]
public class CDMView : BaseView, IView
{
   CDMView() : base(ParseType.SP, "CDMView") { }

   protected override BaseTable CreateTableInstance(IContext ctx)
   {
      CDMTable cdmTable = new CDMTable(ctx, viewName);
      cdmTable.ReadXmlFile();
      return cdmTable;
   }
}
```

The `base(...)` call is where a View declares its ParseType and its name — the name is what the
user types after the parse-type flag.

### Lifecycle (`Contract/IView.cs`, driven by `Program.cs`)

```
Initialize → PreProcess → Process → PostProcess
           → PreAnalyze → Analyze → PostAnalyze
           → WriteExcel → Cleanup
```

Each phase is a **complete pass over all selected views** before the next phase begins — not
nine calls per view in sequence. Parsing populates DataTables during Process; cross-referencing
and derived worksheets (WallClock charts, config analysis) happen during Analyze; Excel is
written once at the end.

### Exceptions are swallowed per phase — this is the #1 debugging trap

`Program.cs` wraps every lifecycle call in try/catch and logs:

```
EXCEPTION in view <Name> (<Phase>) - skipping this view: <message>
```

A broken View therefore produces a **missing or empty worksheet, not a crash**. When a View's
output is wrong or absent, grep the console output for `EXCEPTION in view` before assuming a
parsing problem.

### Parse types

`Contract/IOptions.cs`:

```
AP, AT, AE, AW, AV, SP, SF, RT, SS, BE, II, A2, TCR, WinCE, MV
```

Each maps to a log-file glob and a handler with its own `Factory()`. README.md has the full
flag/glob/view table.

### Line identification — the Factory pattern

`SPLine.Factory()` and its siblings are explicit if-chains, gated first by device number and
then matched against the literal protocol constant found in the log text:

```csharp
if (IsMyLine(logLine, "2"))      // 2 = IDC
{
   result = GenericMatch(WFS_INF_IDC_STATUS, logLine);
   if (result.success) return new WFSIDCSTATUS(logFileHandler, result.subLogLine);

   result = GenericMatch(WFS_CMD_IDC_CHIP_IO, logLine);
   if (result.success) return new WFSDEVSTATUS(logFileHandler, result.subLogLine,
                                               XFSType.WFS_CMD_IDC_CHIP_IO);
}
```

Two return shapes: a **dedicated structure class** (`WFSIDCSTATUS`, `WFSCIMCASHINFO`) when the
line carries structured fields worth parsing out, or the **generic `WFSDEVSTATUS`** with the
`XFSType` passed in when only the fact and timestamp of the event matter. Prefer the generic
one unless fields are genuinely needed.

Adding a new SP line type means: add the value to the `XFSType` enum in `SPLine.cs`, add a
`GenericMatch` branch inside the correct device block, and either reuse `WFSDEVSTATUS` or add a
`WFSxxx.cs` structure class alongside the others in `src/SPLogLine/`.

Enum values are the protocol constants as they appear in the logs (`WFS_INF_IDC_STATUS`,
`APLOG_CARD_PAN`). Class names are the XFS *structure* names (`WFSIDCSTATUS`) — they are related
but not mechanically derived, so don't assume a transformation rule. Follow the neighbours.

Device numbers in `*.nwlog`: 1=PTR, 2=IDC, 3=CDM, 4=PIN, 5=SIU, 7=CDM(alt), 11=CRD, 13=CIM,
14=CRD, 15=BCR, 16=IPM.

## Log formats

**APLog\*.log** — text, one line per event:
```
  INFO [2026-01-23 19:52:19-188] [v02.02.06.04] [ClassName.Method] [TID:27] payload
```
Notable payloads: `LogTransactionData [FLOWPOINT]` (transaction state as JSON *or*
pipe-delimited — format is auto-detected, don't assume), `WebServiceRequestFlowPoint` (the
banking-core conversation), `HybridFlowEngine.CreateNextState` (state transitions), device
status, NDC messages.

**\*.nwlog** — WFS/XFS traces:
```
tsTimestamp = [2025/12/16 09:35 48.496]
hResult = [0]
```

**JNL\*.dat** — binary transaction records, caret-delimited (`^`), EMV data in TLV.

**WinCE traces** — binary, character-by-character parsing. Legacy reference tools are
TraceViewer (C++ MFC) and EJViewer; new output must match them before adding enhancements.

**Config XML** — override chain, later folders win:
`Config → ConfigApplication → ConfigCustomer → ConfigModel → ConfigNetwork → ConfigRuntime`.
XmlParamView presents the merged result.

## Banking cores

`Core.Factory()` detects the core and dispatches to a `Core_*` subclass. Formats differ:

- **FiservDNA** — `"Total Amount:"`, `"Status Code: True/False"`
- **JackHenry** — `"Amount -"`, `"Status Code: Success"`, `"Account Number - x4361"`
- **SymXchange** — similar to FiservDNA
- Also CUAnswers, CMCFlex, KeyBridge, AccessAdvantage

## Conventions

1. **Follow the neighbours.** ~90 projects share these patterns. Consistency beats a cleaner
   one-off design. Read a comparable View/Table/Line before writing a new one.
2. **Code style is 3-space indent, Allman braces, `///` XML doc comments on members, UTF-8 BOM
   with CRLF in `.cs` files.** Match it; don't reformat surrounding code.
3. **Test-driven for parsing bugs.** A log line that fails to parse gets a unit test containing
   that exact line *first*, then the fix. Sample logs live in the `*Samples` projects and
   `testdata/`. Never fix a parser by inspection alone.
4. **Get it compiling, then make it complete.** Incremental over comprehensive.
5. **Analysis rules are data, not code.** Config checks, known-issue registries and lookup
   tables belong in external files the support team maintains, not hardcoded in C#.
6. **"At least as good as the legacy tool"** before adding enhancements.

## Gotchas

- **`src/splogparser/VersionInfo.cs` is generated by CI** on every build (`build.yml`,
  `release.yml`). Don't hand-edit it; edits get overwritten.
- **Versioning is GitVersion**, `GitVersion.yml` at repo root — `ContinuousDelivery` mode, `main`
  labelled `prerelease`, `Patch` increment, tag prefix `[vV]?`. Every push to `main` cuts a
  prerelease GitHub release; `release.yml` is manual dispatch with an explicit version. The
  `VERSION` file at the root is a separate marker. Don't hand-edit AssemblyInfo versions.
- **Excel output is COM interop**, driving real Excel — not a library. Interop objects must be
  released or orphaned `EXCEL.EXE` processes pile up.
- `Microsoft.Office.Interop.Excel.dll` is gitignored but referenced by projects.
- Loose `*View.xml` / `*View.xsd` and `APLog_*` files at the repo root are working artifacts
  from manual runs, not sources. The real ones live in `src/<View>/`.

## Git workflow

Feature branches off `main`, PRs. Descriptive commits explaining *what* and *why*, not a
restatement of the file list. `build.yml` runs on every push and PR to `main`.
