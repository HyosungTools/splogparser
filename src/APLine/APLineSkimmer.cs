using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   /// <summary>
   /// Anti-skimming events from the APLog.
   ///
   /// APLOG_CARD_SKIMMER_CHANGED
   ///   [CardReader.RaiseDeviceUnSolEvent] FireDeviceUnsolEvent(SkimmerDetectChanged, SKIMMER_DETECTED)
   ///   detail = event parameter, e.g. "SKIMMER_DETECTED"
   ///
   /// APLOG_CARD_SKIMMER_OUTOFSERVICE
   ///   [CardReadState.CommonInitCardRead] Skimmer is detected while Init Card Read. Change Mode to OutOfService.
   ///   detail = "out of service"
   ///
   /// APLOG_CARD_SKIMMER_RESET
   ///   [AntiSkimmingProcessHelper.RunSankyoAntiSkimmingAutoResetTimer] Change to previous mode : None
   ///   detail = previous mode, e.g. "None"
   /// </summary>
   public class APLineSkimmer : APLine
   {
      public string detail { get; set; } = string.Empty;

      public APLineSkimmer(ILogFileHandler parent, string logLine, APLogType apType) : base(parent, logLine, apType)
      {
         switch (apType)
         {
            case APLogType.APLOG_CARD_SKIMMER_CHANGED:
               {
                  Match m = Regex.Match(logLine, @"SkimmerDetectChanged,\s*(\w+)\s*\)");
                  detail = m.Success ? m.Groups[1].Value : "unknown";
                  break;
               }

            case APLogType.APLOG_CARD_SKIMMER_OUTOFSERVICE:
               {
                  detail = "out of service";
                  break;
               }

            case APLogType.APLOG_CARD_SKIMMER_RESET:
               {
                  Match m = Regex.Match(logLine, @"Change to previous mode\s*:\s*([^\r\n]*)");
                  detail = m.Success ? m.Groups[1].Value.Trim() : "unknown";
                  break;
               }

            default:
               break;
         }
      }
   }
}
