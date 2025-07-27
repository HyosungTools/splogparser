using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class CDMDispenseLine : SPFlatLine
   {
      private const string V = ")(";

      public int? MixAlgorithm { get; private set; }
      public string Currency { get; private set; }
      public int? Amount { get; private set; }
      public int[] NoteCounts { get; private set; }
      public int? Present { get; private set; }
      public int? Timeout { get; private set; }

      public CDMDispenseLine(ILogFileHandler handler, string line, SPFlatType flatType = SPFlatType.CDM_DispenseInvoked) : base(handler, line, flatType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // {MixAlgorithm[0], Currency[USD], Amount[0], NoteCounts[(0)(0)(2)(0)(0)], Present[0], Timeout[60000]}

         var match = Regex.Match(logLine, @"Invoked\s*{([^}]+)}");
         if (!match.Success) return;

         string payload = match.Groups[1].Value;

         foreach (Match kv in Regex.Matches(payload, @"(\w+)\[([^\]]+)\]"))
         {
            string key = kv.Groups[1].Value;
            string val = kv.Groups[2].Value;

            switch (key)
            {
               case "MixAlgorithm":
                  MixAlgorithm = int.Parse(val);
                  break;
               case "Currency":
                  Currency = val;
                  break;
               case "Amount":
                  Amount = int.Parse(val);
                  break;
               case "Present":
                  Present = int.Parse(val);
                  break;
               case "Timeout":
                  Timeout = int.Parse(val);
                  break;
               case "NoteCounts":
                  {
                     // Secondary regex to extract numbers in (num) format
                     string numPattern = @"\((\d+)\)";
                     MatchCollection nums = Regex.Matches(val, numPattern);
                     var tempList = new List<int>();
                     foreach (Match numMatch in nums)
                     {
                        tempList.Add(int.Parse(numMatch.Groups[1].Value));
                     }
                     NoteCounts = tempList.ToArray();
                     break;
                  }
               default:
                  break;
            }
         }
      }
   }
}
