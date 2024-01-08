# splogparser

[![splogparser build](https://github.com/HyosungTools/splogparser/actions/workflows/build.yml/badge.svg)](https://github.com/HyosungTools/splogparser/actions/workflows/build.yml)

Utility to unzip, merge and present ATM logs as as number of worksheets in an Excel file.

## Quick Start Guide

### Prerequsites

You *must* have Excel 2016 installed on the workstation you plan to run splogparser. 

### Accessing the Release

On the right hand side you will see the link to [Releases](https://github.com/HyosungTools/splogparser/releases). Select the link to navigate to this page. 

For the latest release, scroll down to view the Assets of the Release; select the `release.zip` to download the release. 

### Install

The utility is a console app; there is no install. Its enough to unzip the files into a folder (e.g. `C:\Work_Tools\splogparser`).

Open a cmd.exe in the folder of your log zip file (e.g. `C:\Work_Bugs\ATMD4555`).

You need to make this cmd.exe aware of the splogparser location. For example, if you unzipped to `C:\Work_Tools\splogparser`, in the cmd.exe you would type: 

```text
C:\Work_Bugs\ATMD4555> set path=%path%;C:\Work_Tools\splogparser
```

You can test if this is correct by typing `where splogparser` like this: 

```text
C:\Work_Bugs\ATMD4555> where splogparser
C:\Work_Bugs\ATMD4555> C:\Work_Tools\splogparser\splogparser.exe
```

and Windows should answer where it found splogparser.exe.

### Run

With all that done, you can now run the parser by typing:

```text
C:\Work_Bugs\ATMD4555> splogparser
```

This will error reporting you need to enter command line options.

#### CommandLine Options

First, you always need to specify the target (e.g. zip) file. This command runs without errors:

```text
splogparser -f 20221116175903.zip
```

But this will *do nothing* because you havent said what you want it to do. You need to add (one or more) *Parse Types* and (one or more) *Views* to generate. For example, to get a complete parse of the SP logs you can type:

```text
splogparser -s * -f 20221116175903.zip
```

This is equivalent to early revisions of the tool. The output is what early users are used to. The output file name would be `20221116175903__SP.xlsx`. Note - the `*` is important. I used an off-the-shelf CommandLine Parser tool and it has its limits. If you want all Views use the `*`. But you can also limit the parse to only certain views of the SP logs. So if you were only interested in dispense and deposits, you could enter this instead:

```text
splogparser -s CDM,CIM -f 20221116175903.zip
```

In this case the output file would be `20221116175903__SP_CDM_CIM.xlsx`.

### Parse Types

The list of Parse Types supported is:

| Short | Long | Description |
|-------|------|----------------|
| -a    | --ap | Parse the [AP] logs (e.g. APLog*.*) in the target file |
| -s    | --sp | Parse the [SP] logs (e.g. *.nwlog) in the target file |
|       | --ss | Parse the Settlement Server (e.g. settlement-api-all-*.log) in the target file |
| -w    | --aw | TBD            |
| -r    | --ar | TBD            |

Combine one or more *Parse Type* with one or more *View* to tell the parse what to do. 

### -a (--ap) View Option Meaning

| View     | Description |
|----------|----------------------|
| Disp     | Cash Dispense |
| EJ       | EJ Insert commands            |
| XmlParam | Config files and their parameters in table form |
| AddKey   | Keys loaded on start-up |
| *        | All of the above |

### -s (--sp) View Option Meaning

| View     | Description |
|----------|----------------------|
| CDM      | Cash Dispense Module Status, Dispense operation and Counts |
| CIM      | Cash-In Module Status, Deposits operations and Counts|
| DEV      | Device Status over time |
| Extra    | The lpszExtra values from device status. Good for flagging error codes |
| IDC      | Card Reader Status and inserts |
| IPM      | Check Depositor Status, Deposits and Counts |
| PIN      | PinPad Status |
| SIU      | Status & Indicator Status (e.g. Safe Open/Close, Enter/Exit Supervisor) |
| *        | All of the above |

### --ss View Option Meaning

| View     | Description |
|----------|----------------------|
| *        | All views |

### Samples Commands and their Meaning

Parse the [SP] logs and show me all Dispense and Deposit operations:

```text
splogparser -s CDM,CIM -f 20221116175903.zip
```

Parse the [AP] logs and show me the configuration settings in table form:

```text
splogparser -a XmlParam -f 20221116175903.zip
```

Show me all [AP] views and from the [SP] logs the Dispense operations. 

```text
splogparser -a * -s CDM -f 20221116175903.zip
```

Show me all uploaded, created, discovered and imported events in the settlement server logs

```text
splogparser --ss * -f settlementserverlogs.zip
```

## Known Issues

It's a really dumb install. If you are upgrading, unzip to a clean folder, or clear out the existing folder, so you don't accidentally pick up stuff from the previous release. 

Talking about installs, when I download the release.zip from GitHub I'm prompted for a virus scan. If that doesnt happen for you the DLLs of the parser could be blocked by Win10. You have this problem when the parser finishes quickly, doesnt produce an Excel file, and in the log file generated you see `'Number of Views : 0'`. The work around is to 'unblock' each DLL using the property page of each DLL. 

Sometimes the SIU takes a long time to run. If you don't care about SIU don't use `*` use a select list of individual views instead.

It doesn't work well for machines with Cash and Coin. It doesn't understand the difference.

I've seen the unzip part fail twice. I don't know why. The workaround is to unzip the log file manually, zip up the [SP] subfolder and use that zip. You can throw any zip file at the parser as long as there's an SP folder inside the zip it should work.

## How to Contribute

Anyone can contribute. Please read the notes on: 

- [Development Environment](https://github.com/HyosungTools/docs/blob/main/DevelopmentEnvironment.md)
- [Making a Change](https://github.com/HyosungTools/docs/blob/main/MakingChanges.md)

After that you might want to read up on: 

- [General Design](https://github.com/HyosungTools/splogparser/blob/main/docs/GeneralDesign.md)
- [How to Build](https://github.com/HyosungTools/splogparser/blob/main/docs/HowToBuild.md)
