# splogparser
[![splogparser build](https://github.com/HyosungTools/splogparser/actions/workflows/build.yml/badge.svg)](https://github.com/HyosungTools/splogparser/actions/workflows/build.yml)

Utility to unzip, merge and present ATM logs as as number of worksheets in an Excel file.

[toc]

## Quick Start Guide

### Prerequsites
You *must* have Excel 2016 installed on the workstation you plan to run splogparser. 

### Accessing the Release
In a web browser (e.g. chrome) naviagate to the splogparser open-source project on [GitHub]( https://github.com/HyosungTools/splogparser). 

On the right hand side you will see the link to [Releases](https://github.com/HyosungTools/splogparser/releases). Select the link to navigate to this page. 

For the latest release, scroll down to view the Assets of the Release; select the release.zip to download the release. 

### Install and Run
The utility is a console app. There is no install. Its enough to unzip the files into a pre-defined folder (e.g. C:\\Work_Tools\\splogparser).

In a cmd.exe, navigate to a folder that holds an ATM zip file you want to parse - for example C:\\Work_Bugs\\ATMD4555. First you need to make the cmd.exe aware of the location of splogparser. If it was in C:\\Work_tools\\splogparser, in the cmd.exe you would type: 

```
C:\Work_Bugs\ATMD4555> set path =%path%;C:\Work_Tools\splogparser
```
You can test if this is correct by typing 'where splogparser': 
```
C:\ATMD4555> where sp-log-parser
C:\ATMD4555> C:\Work_Tools\splogparser\splogparser.exe
```
and Windows should be able to find it for you. 

Now that Windows knows where to find the exe you can now run it. Suppose in this folder there is a zip file called 20221116175903.zip. you can run splogparser by typing: 

```
C:\ATMD4555> splogparser 20221116175903.zip
```
The application will run and generate two files: a log file for the run called 20221116175903.log and an Excel spreadsheet called 20221116175903.xls.

