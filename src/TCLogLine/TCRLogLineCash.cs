using Contract;

namespace LogLineHandler
{
   public class TCRLogLineCash : TCRLogLine
   {
      public string USD0 = string.Empty;
      public string USD1 = string.Empty;
      public string USD2 = string.Empty;
      public string USD5 = string.Empty;
      public string USD10 = string.Empty;
      public string USD20 = string.Empty;
      public string USD50 = string.Empty;
      public string USD100 = string.Empty;

      public TCRLogLineCash(ILogFileHandler parent, string logLine, TCRLogType tcrType) : base(parent, logLine, tcrType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         string lookFor = string.Empty;
         int idx;
         char[] trimChars = { '[', ']', '.', '-' };
         char[] splitChars = { ',' };

         switch (this.tcrType)
         {
            case TCRLogType.TCR_DEP_CASHDEPOSITED:
               {
                  // e.g. CashDeposited=[USD,(1,0)][USD,(2,0)][USD,(5,0)][USD,(10,0)][USD,(20,0)][USD,(50,0)][USD,(100,160)][UNKNOWN,(0,0)]
                  lookFor = "CashDeposited=";
                  idx = logLine.IndexOf(lookFor);
                  if (idx != -1)
                  {
                     string cash = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                     cash = cash.Replace(",(", "");
                     cash = cash.Replace("][", "");
                     cash = cash.Replace("[", "");
                     cash = cash.Replace("]", "");
                     cash = cash.Replace(")", ",");

                     // e.g. USD1,0,USD2,0,USD5,0,USD10,0,USD20,0,USD50,0,USD100,160,UNKNOWN0,0,

                     string[] currencies = cash.Split(splitChars, 100);
                     for (int i = 0; i < currencies.Length; i = i + 2)
                     {
                        switch (currencies[i])
                        {
                           case "UNKNOWN0": { USD0 = currencies[i + 1]; break; }
                           case "USD1": { USD1 = currencies[i + 1]; break; }
                           case "USD2": { USD2 = currencies[i + 1]; break; }
                           case "USD5": { USD5 = currencies[i + 1]; break; }
                           case "USD10": { USD10 = currencies[i + 1]; break; }
                           case "USD20": { USD20 = currencies[i + 1]; break; }
                           case "USD50": { USD50 = currencies[i + 1]; break; }
                           case "USD100": { USD100 = currencies[i + 1]; break; }
                        }
                     }
                  }
                  break;
               }
            case TCRLogType.TCR_WD_CASHDISPENSED:
               {
                  // e.g. CashDispensed=[USD,(100,1)][USD,(5,10)][USD,(10,7)]

                  lookFor = "CashDispensed=";
                  idx = logLine.IndexOf(lookFor);
                  if (idx != -1)
                  {
                     string cash = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                     cash = cash.Replace(",(", "");
                     cash = cash.Replace("][", "");
                     cash = cash.Replace("[", "");
                     cash = cash.Replace("]", "");
                     cash = cash.Replace(")", ",");

                     // e.g. USD100,1,USD5,10,USD10,7

                     string[] currencies = cash.Split(splitChars, 100);
                     for (int i = 0; i < currencies.Length; i = i + 2)
                     {
                        switch (currencies[i])
                        {
                           case "UNKNOWN0": { USD0 = currencies[i + 1]; break; }
                           case "USD1": { USD1 = currencies[i + 1]; break; }
                           case "USD2": { USD2 = currencies[i + 1]; break; }
                           case "USD5": { USD5 = currencies[i + 1]; break; }
                           case "USD10": { USD10 = currencies[i + 1]; break; }
                           case "USD20": { USD20 = currencies[i + 1]; break; }
                           case "USD50": { USD50 = currencies[i + 1]; break; }
                           case "USD100": { USD100 = currencies[i + 1]; break; }
                        }
                     }
                  }
                  break;
               }
         }
      }
   }
}
