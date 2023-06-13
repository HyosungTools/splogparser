# splogparser
[![splogparser build](https://github.com/HyosungTools/splogparser/actions/workflows/build.yml/badge.svg)](https://github.com/HyosungTools/splogparser/actions/workflows/build.yml)

Utility to unzip, merge and present ATM logs as as number of worksheets in an Excel file.

## Quick Start Guide

### Prerequsites
You *must* have Excel 2016 installed on the workstation you plan to run splogparser. 

### Accessing the Release
On the right hand side you will see the link to [Releases](https://github.com/HyosungTools/splogparser/releases). Select the link to navigate to this page. 

For the latest release, scroll down to view the Assets of the Release; select the `release.zip` to download the release. 

### Install and Run
The utility is a console app; there is no install. Its enough to unzip the files into a folder (e.g. `C:\Work_Tools\splogparser`).

Open a cmd.exe in the folder of your log zip file (e.g. `C:\Work_Bugs\ATMD4555`).

You need to make this cmd.exe aware of the splogparser location. For example, if you unzipped to `C:\Work_Tools\splogparser`, in the cmd.exe you would type: 

```
C:\Work_Bugs\ATMD4555> set path=%path%;C:\Work_Tools\splogparser
```

You can test if this is correct by typing `where splogparser` like this: 

```
C:\Work_Bugs\ATMD4555> where splogparser
C:\Work_Bugs\ATMD4555> C:\Work_Tools\splogparser\splogparser.exe
```

and Windows should answer where it found splogparser.exe.

With all that done, you can now run the parser. Suppose in the folder `C:\Work_Bugs\ATMD4555` there is a zip file called 20221116175903.zip. you can run splogparser by typing: 

```
C:\Work_Bugs\ATMD4555> splogparser 20221116175903.zip
```
The application will run and generate two files: a log file for the run called 20221116175903.log and an Excel spreadsheet called 20221116175903.xls.

## Known Issues

It's a really dumb install. If you are upgrading, unzip to a clean folder, or clear out the existing folder, so you don't accidentally pick up stuff from the previous release. 

Talking about installs, when I download the release.zip from GitHub I'm prompted for a virus scan. If that doesnt happen for you the DLLs of the parser could be blocked by Win10. You have this problem when the parser finishes quickly, doesnt produce an Excel file, and in the log file generated you see `'Number of Views : 0'`. The work around is to 'unblock' each DLL using the property page of each DLL. 

SIU takes a long time to run. If you dont care about SIU delete the SIUView from your install folder. The parser only runs View DLLs it can find. 

It doesn't work well for machines with Cash and Coin. It doesn't understand the difference. 

I've seen the unzip part fail twice. I don't know why. The workaround is to unzip the log file manually, zip up the [SP] subfolder and use that zip. You can throw any zip file at the parser as long as there's an SP folder inside the zip it should work. 

# How to Contribute

Anyone can contribute. Please read the notes on: 

- [Development Environment](https://github.com/HyosungTools/docs/blob/main/DevelopmentEnvironment.md)
- [Making a Change](https://github.com/HyosungTools/docs/blob/main/MakingChanges.md)

After that you might want to read up on: 

- [General Design](https://github.com/HyosungTools/splogparser/blob/main/docs/GeneralDesign.md)
- [How to Build](https://github.com/HyosungTools/splogparser/blob/main/docs/HowToBuild.md)



