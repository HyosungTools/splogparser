using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class CIMLogicalUnitValues : SPFlatLine
   {
      string[] logicalUnitValues;


      public CIMLogicalUnitValues(ILogFileHandler handler, string line, SPFlatType flatType) : base(handler, line, flatType)
      {
      }

      protected override void Initialize()
      {
         try
         {
            base.Initialize();

            // Regular expression to match content inside parentheses
            string pattern = @"\(([^)]*)\)";
            MatchCollection matches = Regex.Matches(logLine, pattern);

            // Extract content and convert whitespace-only to empty string
            logicalUnitValues = matches
                .Cast<Match>()
                .Select(m => string.IsNullOrWhiteSpace(m.Groups[1].Value) ? "" : m.Groups[1].Value)
                .ToArray();
         }
         catch (Exception e)
         {
            Console.WriteLine("Exception in Initialize()");
         }
      }
   }
}

