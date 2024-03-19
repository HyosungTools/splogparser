using Contract;
using CommandLine;
using System;

namespace splogparser
{
   public class Options : IOptions
   {
      /// <summary>
      /// Setting default to 'x' tells me if the option was even specified on the command line
      /// </summary>
      [Option('a', "ap", Default = "x", Required = false, HelpText = "Parse Application logs.")]
      public string APViews { get; set; }

      [Option('t', "atagent", Default = "x", Required = false, HelpText = "Parse Active Teller ITM logs.")]
      public string ATViews { get; set; }

      [Option('e', "atagentextensions", Default = "x", Required = false, HelpText = "Parse Active Teller Extensions ITM logs.")]
      public string AEViews { get; set; }

      [Option('w', "atworkstation", Default = "x", Required = false, HelpText = "Parse Active Teller Workstation logs.")]
      public string AWViews { get; set; }

      [Option('v', "atserver", Default = "x", Required = false, HelpText = "Parse Active Teller Server logs.")]
      public string AVViews { get; set; }

      [Option('b', "be", Default = "x", Required = false, HelpText = "Parse BeeHD logs.")]
      public string BEViews { get; set; }
	  
      [Option('s', "sp", Default = "x", Required = false, HelpText = "Parse Service Provider logs.")]
      public string SPViews { get; set; }

      [Option('r', "rt", Default = "x", Required = false, HelpText = "Parse Retail logs.")]
      public string RTViews { get; set; }

      [Option('i', "ii", Default = "x", Required = false, HelpText = "Parse IIS logs.")]
      public string IIViews { get; set; }

      [Option('f', "file", Required = true, HelpText = "Input files to be processed.")]
      public string InputFile { get; set; }

      [Option("timestart", Default = "x", Required = false, HelpText = "Start time to consider (yyyymmddhhmm).  See timespan.")]
      public string TimeStart { get; set; }

      [Option("timespan", Default = "x", Required = false, HelpText = "Add to timestart to get the End time to consider (minutes).")]
      public string TimeSpanMinutes { get; set; }

      [Option("rawlogline", Default = false, Required = false, HelpText = "Include raw log line in the payload column.")]
      public bool RawLogLine { get; set; }


      // default time range includes-all
      public DateTime StartTime { get; set; } = DateTime.MinValue;
      public DateTime EndTime { get; set; } = DateTime.MaxValue;


      // Omitting long name, defaults to name of property, ie "--ssrv"
      [Option("ss", Default = "x", Required = false, HelpText = "Parse Settlement Server API logs.")]
      public string SSViews { get; set; }

      public bool IsAP { get { return APViews != "x"; } }
      public bool IsAT { get { return ATViews != "x"; } }
      public bool IsAE { get { return AEViews != "x"; } }
      public bool IsAW { get { return AWViews != "x"; } }
      public bool IsAV { get { return AVViews != "x"; } }
      public bool IsBE { get { return BEViews != "x"; } }
      public bool IsSP { get { return SPViews != "x"; } }
      public bool IsRT { get { return RTViews != "x"; } }
      public bool IsII { get { return IIViews != "x"; } }
      public bool IsSS { get { return SSViews != "x"; } }

      public bool RunView(ParseType parseType, string viewName)
      {
         viewName = viewName.Replace("View", "");
         return
            ( ( (IsAP && parseType == ParseType.AP) && (APViews.Contains(viewName) || APViews.Contains("*"))) ||
              ( (IsSP && parseType == ParseType.SP) && (SPViews.Contains(viewName) || SPViews.Contains("*"))) ||
              ( (IsRT && parseType == ParseType.RT) && (RTViews.Contains(viewName) || RTViews.Contains("*"))) ||
              ( (IsII && parseType == ParseType.II) && (IIViews.Contains(viewName) || IIViews.Contains("*"))) ||
              ( (IsAE && parseType == ParseType.AE) && (AEViews.Contains(viewName) || AEViews.Contains("*"))) ||
              ( (IsAT && parseType == ParseType.AT) && (ATViews.Contains(viewName) || ATViews.Contains("*"))) ||
              ( (IsAW && parseType == ParseType.AW) && (AWViews.Contains(viewName) || AWViews.Contains("*"))) ||
              ( (IsBE && parseType == ParseType.BE) && (BEViews.Contains(viewName) || BEViews.Contains("*"))) ||
              ( (IsAV && parseType == ParseType.AV) && (AVViews.Contains(viewName) || AVViews.Contains("*"))) ||
              ( (IsSS && parseType == ParseType.SS) && (SSViews.Contains(viewName) || SSViews.Contains("*")))
            );
      }

      protected string _Suffix(string parseType, string arguments)
      {
         return (arguments == "*") ? parseType : parseType + "_" + arguments.Replace(',', '_');
      }
      public string Suffix()
      {
         string suffix = string.Empty;

         if (IsAP) suffix += _Suffix("__AP", APViews);
         if (IsAT) suffix += _Suffix("__AT", ATViews);
         if (IsAE) suffix += _Suffix("__AE", AEViews);
         if (IsAW) suffix += _Suffix("__AW", AWViews);
         if (IsBE) suffix += _Suffix("__BE", BEViews);
         if (IsAV) suffix += _Suffix("__AV", AVViews);
         if (IsSP) suffix += _Suffix("__SP", SPViews);
         if (IsRT) suffix += _Suffix("__RT", RTViews);
         if (IsII) suffix += _Suffix("__II", IIViews);
         if (IsSS) suffix += _Suffix("__SS", SSViews);

         return suffix; 
      }
   }

}
