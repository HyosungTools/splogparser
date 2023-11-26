using Contract;
using CommandLine;

namespace splogparser
{
   public class Options : IOptions
   {
      /// <summary>
      /// Setting default to 'x' tells me if the option was even specified on the command line
      /// </summary>
      [Option('a', "ap", Default = "x", Required = false, HelpText = "Parse Application logs.")]
      public string APViews { get; set; }

      [Option('t', "at", Default = "x", Required = false, HelpText = "Parse Active Teller ITM logs.")]
      public string ATViews { get; set; }

      [Option('w', "aw", Default = "x", Required = false, HelpText = "Parse Active Teller Workstation logs.")]
      public string AWViews { get; set; }

      [Option('s', "sp", Default = "x", Required = false, HelpText = "Parse Service Provider logs.")]
      public string SPViews { get; set; }

      [Option('r', "rt", Default = "x", Required = false, HelpText = "Parse Retail logs.")]
      public string RTViews { get; set; }

      [Option('f', "file", Required = true, HelpText = "Input files to be processed.")]
      public string InputFile { get; set; }

      public bool IsAP { get { return APViews != "x"; } }
      public bool IsAT { get { return ATViews != "x"; } }
      public bool IsAW { get { return AWViews != "x"; } }
      public bool IsSP { get { return SPViews != "x"; } }
      public bool IsRT { get { return RTViews != "x"; } }

      public bool RunView(ParseType parseType, string viewName)
      {
         viewName = viewName.Replace("View", "");
         return (((parseType == ParseType.AP && IsAP) && APViews.Contains(viewName) || APViews == "*") ||
                 ((parseType == ParseType.AT && IsAT) && ATViews.Contains(viewName) || ATViews == "*") ||
                 ((parseType == ParseType.AW && IsAW) && AWViews.Contains(viewName) || AWViews == "*") ||
                 ((parseType == ParseType.SP && IsSP) && SPViews.Contains(viewName) || SPViews == "*") ||
                 ((parseType == ParseType.RT && IsRT) && RTViews.Contains(viewName) || RTViews == "*"));
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
         if (IsAW) suffix += _Suffix("__AW", AWViews);
         if (IsSP) suffix += _Suffix("__SP", SPViews);
         if (IsRT) suffix += _Suffix("__RT", RTViews);

         return suffix; 
      }
   }

}
