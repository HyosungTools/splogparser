# StateWallClock Integration Guide

## Overview

This feature adds ATM state duration analysis to OverView, generating pie charts that visualize machine availability across 12-hour periods (AM/PM).

## Files

1. **StateWallClockAnalyzer.cs** - New file
   - Contains analysis logic
   - Calculates state durations per day/period
   - Handles Unknown state inference (start of day, end of day, PowerDown gaps)

2. **OverView.cs** - Replacement
   - Overrides `PreAnalyze()`, `Analyze()`, `PostAnalyze()`, and `WriteExcel()`
   - Coordinates the analysis workflow

3. **OverTable_Additions.cs** - Methods to add to OverTable.cs
   - `GetSummaryTable()` - Returns the Summary DataTable for analysis
   - `PopulateStateWallClockTable()` - Creates results table from analyzer
   - `WriteStateWallClockExcel()` - Creates worksheet with pie charts
   - `AddSummaryTable()` - Adds percentage summary table to worksheet

## Integration Steps

### 1. Add StateWallClockAnalyzer.cs
Copy `StateWallClockAnalyzer.cs` to the OverView project folder.

### 2. Replace OverView.cs
Replace the existing `OverView.cs` with the new version.

### 3. Update OverTable.cs
Add the methods from `OverTable_Additions.cs` to the existing OverTable class:
- Add `using System.Linq;` and Excel interop usings to the top of the file
- Add the `#region StateWallClock Analysis Support` section to the class

### 4. Verify COM References
Ensure the project has references to both:
- `Microsoft.Office.Interop.Excel`
- `Microsoft Office 16.0 Object Library` (or your installed version)

The Office Object Library is required because chart formatting uses Office core types.

In Visual Studio: 
1. Right-click project → Add → Reference
2. Go to COM tab
3. Check "Microsoft Office 16.0 Object Library"
4. Click OK

## Data Flow

```
PostProcess (existing)
    │
    └── Writes OverView.xml with Summary table
            │
            ▼
PreAnalyze
    │
    ├── Loads OverView.xml
    └── Creates StateWallClockAnalyzer
            │
            ▼
Analyze
    │
    ├── Gets Summary DataTable
    ├── Calculates state durations
    └── Logs results
            │
            ▼
PostAnalyze
    │
    └── Populates StateWallClock table
            │
            ▼
WriteExcel
    │
    ├── Writes Summary worksheet (existing)
    └── Writes StateWallClock worksheet with pie charts
```

## State Duration Rules

| Scenario | State Assigned |
|----------|---------------|
| Midnight to first log entry | Unknown |
| Last log entry to midnight | Unknown |
| Last log entry before PowerUp to PowerUp | Unknown |
| Mode value in log | That mode, until next mode change |

## Chart Output

- Two pie charts per day (AM: 00:00-12:00, PM: 12:00-24:00)
- Color-coded by state:
  - InService: Green
  - OutOfService: Orange
  - OffLine: Red
  - PowerUp: Blue
  - Supervisor: Purple
  - Unknown: Gray
- Summary table showing percentages for all periods

## Testing

Use the Python validation script to verify algorithm correctness:
```bash
python validate_algorithm.py
```

Expected output shows each period totaling exactly 12:00:00.
