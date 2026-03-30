# How I Added SIUView

I'm going to describe how I added SIUView to the application. The application has a really simple architecture. If you follow along here you should be able to add any other view as they all look the same.

My philosophy about software - it has to look like 1 person wrote the whole thing. If you are going to add to this, you must write code that looks like all the other code. Also your change set has to be the absolute minimum change to get the job done. There are absolutely better ways to write this but that's not the goal. The goal is to spend a few hours and get a lot more hours back on the Software Support side.

With that said...

## Create the Project

First step, create the csproj project. From within Visual Studio, select the solution, right-click and select `New Project...` to lauch the dialog. Select the option to create a Class Library (.NET Framework). At some point we can convert to .NET CORE but not today. I called the project SIUView because its a view of the SIU over time. You should use a similar naming convention. Once the project is created check the properties. Make sure the target framework is the same as all the other View projects (e.g. .NET Framework 4.7.2) and also define these Build Events/Post-build events:

``` text
copy "$(MSBuildProjectDirectory)\bin\$(Configuration)\*.dll" $(SolutionDir)\..\dist
copy "$(MSBuildProjectDirectory)\SIUView.xsd" $(SolutionDir)\..\dist
copy "$(MSBuildProjectDirectory)\SIUView.xml" $(SolutionDir)\..\dist
```

Note: for each view we distrubute a DLL, an XSD and an XML file.

## Add References to the Project

You need to add References to the project. In Visual Studio, open the project (i.e. SIUView), select `References`, right-click and select `Add Reference...` to show the Add Reference dialog.

- Under Assemblies add `Microsoft.ComponentModel.Composition`
- Under Projects add `Contract`, `Impl`, and `WFS`

## Add Files to the Project

All the View Projects look the same. They all have the same 4 files. For SIUView I need to created the following files:

- SIUTable.cs
- SIUView.cs
- SIUView.xml
- SIUView.xsd

Note: there is a lot of scope for refactoring in this tool but hours spent refactoring will not help Support Engineers get to root cause faster today so I see that as a low priority task. We should only spend hours on tasks that will generate an immediate return on investment. If we can spend 20-40 hours on the tool now to save 100 hours over the next few months we should do it. That's my mindset.

### SIUTable.cs

This is where most of the work happens.

Again, all the Views have an XXXTable.cs file and the all look the same, so you can probably copy one (e.g. CIMTable.cs) as a start point. In fact I think that's what I did. Again there is scope for refactoring but for now I want minimum spend for maximum return.

Looking at SIUTable.cs there are 5 methods:

- constructor
- WriteExcel
- FindMessages
- ProcessRow
- WFS_INF_SIU_STATUS

#### Constructor

I don't have much to say about the Constructor. You will see `IContext` everywhere. It's a class that holds pointers to utility functions like FileSystem and Logging. One hole in the application is that I haven't written a mock for this class. That's on my TODO list.

#### WriteExcel

This is where we manipulate the in-memory tables before they become Excel Worksheets. SIUView is perhaps a bad example because currently there is only 1 Table (Status). Something like CIMView has more tables so more work happens. There are common tables across Views - Summary, Status - and the same work happens on each.

I compress all Status tables. I only want to report new information. If two rows in the table are more or less identical, I delete the bottom row. I do this by calling `_datatable_ops.DeleteUnchangedRowsInTable`.

I add English to the Status table. I do this by calling `_datatable_ops.AddEnglishToTable`. This translates numbers to words using the Messages in the xml file.

Finally I call `base.WriteExcelFile` to generate the worksheet.

SIUTable is a bad example. If you look at other files (CDMTable.cs, CIMTable.cs) you will see more work.

#### FindMessages

This is a method that can move to the BaseTable.cs file. Its identical in all the views. It does the look up for messages. It relies on the `type` and `code` field of a `Message` record being a unique id. Make sure your xsd has this:

``` text
    <xs:unique name="Constraint1" msdata:PrimaryKey="true">
      <xs:selector xpath=".//Messages" />
      <xs:field xpath="type" />
      <xs:field xpath="code" />
    </xs:unique>
```

#### ProcessRow

ProcessRow is a jump table. The parser parses all the nwlog files, each line from each file is tested to see if its a known XFS line (more on that later). Each XFS line is passed to each View to see if they want to process it. SIUView is not that interesting because it tests for one XFS line (WFS_INF_SIU_STATUS) and if found, jumps to the method WFS_INF_SIU_STATUS.

#### WFS_INF_SIU_STATUS

This is where a WFS_INF_SIU_STATUS XFS line from an nwlog file is handled. If you want to see an example log line look at SampleTests\sample_siu.cs. In fact the SampleTests project has classes full of `const string` unit test strings.

These XFS log line handling methods across all Views look the same. This section of code: 

``` text
            WFSSIUSTATUS siuStatus = new WFSSIUSTATUS(ctx);

            try
            {
               siuStatus.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_INF_SIU_STATUS Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }
```

Parses the log line and loads up a `WFSSIUSTATUS` instance. This section of code:

``` text
            DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            dataRow["safe"] = siuStatus.fwSafeDoor;

            dTableSet.Tables["Status"].AcceptChanges();
```

Takes that data and stores it in the in-memory tables, which will become an Excel worksheet. If you look you will see blocks of code like this everywhere. Its a simple architecture.

There are some outstanding questions:

- How are XFS lines identified?
- What is this `WFSSIUSTATUS` class?

I also want to mention unit testing. I will answer these questions below.

### SIUView.cs

This is the easiest file to create. Copy any other `XXXView.cs` file - e.g. CIMView.cs - and make the obvious changes - e.g. change CIM to SIU everywhere. Its that simple.

### SIUView.xml

The xml file has two jobs. It holds start up data when the tool first starts, and then it holds parse data as the tool runs. The data in the xml file is the data in the resulting Excel worksheets.

Let's talk a bit more about start up data. On start up the xml file holds message records. The message records map numbers in the nwlog lines to words and sentences. For example, from CIMView.xml.

``` text
  <Messages>
    <type>fwDevice</type>
    <code>2</code>
    <brief>pwoff</brief>
    <description>Powered off or physically not connected</description>
  </Messages>
```

If the CIM Status message has `fwDevice = [2]`, that gets translated to `pwoff` (i.e. power off) in the worksheet and the comment is `Powered off or physically not connected`. This is one of the main uses of the tool - you don't have to look things up, everything is in plain English.

So on start up the xml file should have Message records. But another use is to simplify the code. Again if you look in CIMView.xml you will see a number of Summary records, each with a unique `number`. The Summary table lists the logical units and attributes (logical name, demonination, initial count etc). When I start parsing logs, I don't know how many logical units there are for the Summary worksheet, so instead of trying to figure that out as I parse, I add 10 dummy records to the xml. That means on startup the Summary Table has 10 rows pre-populated (mostly empty). As I parse I just update these records. Its a lot simpler to update an exiting record rather than try to figure out if I need to create one, then create it. At the end I delete unused records. This breaks if there are more than 10 logical units but so far so good.

### SIUView.xsd

The xsd file defines the internal tables maintained during the running of the tool and in most cases the worksheets created (each View has a Messages table but there is no Messages worksheet for example).

You can probably look at any other View to see how this file works.

## How are XFS Log Lines Identified?

Log file matching happens in the project\file `Impl\IdentifyLines.cs`. There is an enum of XFS log lines we are interested in. They are organized by XFS, so for example you can see I added this for SIU:

``` text
      /* 8 - SIU */
      WFS_INF_SIU_STATUS,
```

Add new enums in the correct place please.

The method `XFSLine` does the regex matching. For SIU I added this:

``` text
         /* 8 - SIU */
         if (logLine.Contains("GETINFO[8") || logLine.Contains("EXECUTE[8") || logLine.Contains("SERVICE_EVENT[8"))
         {
            /* INFO */
            Regex wfs_inf_siu_status = new Regex("GETINFO.801.[0-9]+WFS_GETINFO_COMPLETE");

            /* Test for INFO */
            result = GenericMatch(wfs_inf_siu_status, logLine);
            if (result.success) return (XFSType.WFS_INF_SIU_STATUS, result.xfsLine);
         }
```

Again, all very basic. Performance in general is pretty good. Performance around `WFPOpen/WFPClose` is noticably slow but for now leave it.

## What is WFSSIUSTATUS?

There is an entire project - `WFS` - that's full of very similar classes. For any XFS log line we are interested in that has a payload, create a class to hold that payload.

What you will notice is that these classes are less like `class` and more like `struct` because all the members are `public` and all the methods (except Initialize) are `static`. I did this because I dont have a mock for the `context`. Its a bit ugly but it works.

Don't think about refactoring these classes. It's very readable.

The class `WFSSIUSTATUS` does need to be extended. Currently it only parses out safe door (you can see this in the xsd). First I added a member variable:

``` text
      public string fwSafeDoor { get; set; }
```

Then I added a parsing method:

``` text
      public static (bool success, string xfsMatch, string subLogLine) fwSafeDoorFromSIUStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwDoors\\[WFS_SIU_SAFE\\] = \\[(.*)\\]", "0");
      }
```

This allows me to pull out whatever safe door value is reported in the logs. Back in SIUTable.cs that value gets stored in the in-memory table.

## What about Unit Testing?

There's an entire project - `WFSTests` - that's full of unit tests. Unit testing is part of my "definition of done".
