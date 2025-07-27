using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class CDMDenominateLine : SPFlatLine
   {
      private const string V = ")(";

      public int? MixAlgorithm { get; private set; }
      public string Currency { get; private set; }
      public int? Amount { get; private set; }

      public CDMDenominateLine(ILogFileHandler handler, string line, SPFlatType flatType = SPFlatType.CDM_DenominateInvoked) : base(handler, line, flatType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // {MixAlgorithm[0], Currency[USD], Amount[0]}

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
               default:
                  break;
            }
         }
      }
   }
}
