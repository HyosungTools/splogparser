using Contract;

namespace LogLineHandler
{
   public class APLineEmvCurrencyType : APLine
   {
      public string field = string.Empty;

      public APLineEmvCurrencyType(ILogFileHandler parent, string logLine, APLogType apType, string field = "") : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // https://en.wikipedia.org/wiki/ISO_4217
         // e.g. 5F2A02 0840 5F3601 00

         string[,] currencyCodes = new string[,]
         {
             { "840", "USD" },
             { "978", "EUR" },
             { "826", "GBP" },
             { "826", "GBP" },
             { "052", "BBD" },
             { "060", "BMD" },
             { "124", "CAD" },
             { "484", "MXN" }
         };

         if (field != string.Empty)
            return;

         field = "UNK";

         string lookFor = "5F2A02";
         int idx = logLine.IndexOf(lookFor);
         if (idx != -1)
         {
            string subLine = logLine.Substring(idx + lookFor.Length).Trim();

            // iterate over rows
            for (int i = 0; i < currencyCodes.GetLength(0); i++)
            {
               if (subLine.Contains(currencyCodes[i,0]))
               {
                  field = currencyCodes[i, 1];
               }
            }
         }
      }
   }
}
