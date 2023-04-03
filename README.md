# splogparser
[![splogparser build](https://github.com/HyosungTools/splogparser/actions/workflows/build.yml/badge.svg)](https://github.com/HyosungTools/splogparser/actions/workflows/build.yml)

Utility to unzip, merge and present ATM logs as as number of worksheets in an Excel file.

## Quick Start Guide

### Prerequsites
You *must* have Excel 2016 installed on the workstation you plan to run splogparser. 

### Accessing the Release
On the right hand side you will see the link to [Releases](https://github.com/HyosungTools/splogparser/releases). Select the link to navigate to this page. 

For the latest release, scroll down to view the Assets of the Release; select the release.zip to download the release. 

### Install and Run
The utility is a console app; there is no install. Its enough to unzip the files into a folder (e.g. C:\\Work_Tools\\splogparser).
You do need to make the cmd.exe aware of the location. For example, if you unzipped to C:\\Work_Tools\\splogparser, in the cmd.exe you would type: 

```
C:\Work_Bugs\ATMD4555> set path=%path%;C:\Work_Tools\splogparser
```

You can test if this is correct by typing 'where splogparser': 

```
C:\ATMD4555> where splogparser
C:\ATMD4555> C:\Work_Tools\splogparser\splogparser.exe
```

and Windows should be able to find it for you.

With all that done, in the cmd.exe, navigate to a folder that holds an ATM zip file you want to parse - for example C:\\Work_Bugs\\ATMD4555. Suppose in this folder there is a zip file called 20221116175903.zip. you can run splogparser by typing: 

```
C:\ATMD4555> splogparser 20221116175903.zip
```
The application will run and generate two files: a log file for the run called 20221116175903.log and an Excel spreadsheet called 20221116175903.xls.

## Known Issues

Its a really dumb install. If you are upgrading, unzip to a clean folder (or clear out the existing folder) so you don't accidentally pick up stuff from the previous release. 

Talking about installs, when I download the release.zip from GitHub I'm prompted for a virus scan. If that doesnt happen for you the DLLs of the parser could be blocked by Win10. You have this problem with the parser finishes qucikly, doesnt produce an Excel file, and in the log file generated you see 'Number of Views : 0'. The work around is to 'unblock' each DLL using the property page of each DLL.  

It doesn't work well for machines with Cash and Coin. It doesn't understand the difference. 

I've seen the unzip part fail twice. I don't know why. The workaround is to unzip the log file manually, zip up the [SP] subfolder and use that zip. You can throw any zip file at the parser as long as there's an SP folder inside the zip it should work. 

