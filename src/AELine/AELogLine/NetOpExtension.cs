using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Contract;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LogLineHandler
{
   public class NetOpExtension : AELine
   {
      bool isRecognized = false;
      public string ModelName { get; set; }
      public string ConfigurationState { get; set; }
      public string RemoteDesktopServerState { get; set; }



      public NetOpExtension(ILogFileHandler parent, string logLine, AELogType aeType = AELogType.NetOpExtension) : base(parent, logLine, aeType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         /*
	         2023-11-17 03:00:22 [NetOpExtension] The 'NetOpExtension' extension is started.
	         2023-11-17 03:00:22 [NetOpExtension] Tried to set NetOp configurations but the model is null or empty.
            2023-11-13 03:01:05 [NetOpExtension] Tried to set NetOp configurations but the model was not provided.
	         2023-11-17 03:01:57 [NetOpExtension] Checking if NetOp configuration exists for 7800I
	         2023-11-17 03:01:57 [NetOpExtension] Attempting to get Asset Config XML for 7800I
            2023-11-13 03:02:58 [NetOpExtension] Attempting to update the netop.ini.
            2023-11-13 03:02:58 [NetOpExtension] Updated the netop.ini.
            2023-11-13 03:02:58 [NetOpExtension] The NetOp service was stopped.
            2023-11-13 03:02:58 [NetOpExtension] The NetOp service was started.
	         2023-11-17 03:01:57 [NetOpExtension] Configuring NetOp without video for model: 7800I
	         2023-11-17 03:01:57 [NetOpExtension] Attempting to get Asset Config XML for 7800I
	         2023-11-17 03:01:58 [NetOpExtension] The remote desktop server is already running
	         2023-11-17 09:24:30 [NetOpExtension] The remote desktop server is already running.
            2023-11-13 03:02:58 [NetOpExtension] Attempting to get "Standard" configuration revision 1 or earlier for model 7800I.
            2023-11-13 03:02:58 [NetOpExtension] Located "Standard" configuration revision 1 for model 7800I.
         */

         int idx = logLine.IndexOf("[NetOpExtension]");
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx + "[NetOpExtension]".Length + 1);

            //Tried to set NetOp configurations but the model is null or empty.
            if (subLogLine == "Tried to set NetOp configurations but the model is null or empty.")
            {
               isRecognized = true;
               ConfigurationState = "TRIED";
               ModelName = "null";
            }

            else if (subLogLine == "Tried to set NetOp configurations but the model was not provided.")
            {
               isRecognized = true;
               ConfigurationState = "TRIED";
               ModelName = "not provided";
            }

            else if (subLogLine.StartsWith("The remote desktop server is already running"))
            {
               isRecognized = true;
               RemoteDesktopServerState = "RUNNING";
            }

            else if (subLogLine.StartsWith("Attempting to update the netop.ini."))
            {
               isRecognized = true;
               ConfigurationState = "TRIED TO UPDATE netop.ini";
               ModelName = "null";
            }

            else if (subLogLine.StartsWith("Updated the netop.ini."))
            {
               isRecognized = true;
               ConfigurationState = "UPDATED netop.ini";
               ModelName = "null";
            }

            else if (subLogLine.StartsWith("The NetOp service was stopped."))
            {
               isRecognized = true;
               ConfigurationState = "STOPPED NetOp service";
               ModelName = "null";
            }

            else if (subLogLine.StartsWith("The NetOp service was started."))
            {
               isRecognized = true;
               ConfigurationState = "STARTED NetOp service";
               ModelName = "null";
            }

            else if (subLogLine.StartsWith("The located configuration is already in use."))
            {
               isRecognized = true;
               ConfigurationState = "Configuration already in use";
               ModelName = "null";
            }

            else if (subLogLine.StartsWith("Time out has expired and the operation has not been completed."))
            {
               isRecognized = true;
               ConfigurationState = "TIMED OUT, operation not completed";
               ModelName = "null";
            }

            else if (subLogLine.StartsWith("Trying to kill "))
            {
               isRecognized = true;
               ConfigurationState = subLogLine;
               ModelName = "null";
            }

            else
            {
               //Checking if NetOp configuration exists for 7800I
               Regex regex = new Regex("Checking if NetOp configuration exists for (?<machine>.*)$");
               Match m = regex.Match(subLogLine);
               if (m.Success)
               {
                  isRecognized = true;
                  ConfigurationState = "CHECKING";
                  ModelName = m.Groups["machine"].Value;
               }

               //Attempting to get "Standard" configuration revision 1 or earlier for model 7800I.
               regex = new Regex("Attempting to get \"(?<configname>.*)\" configuration revision (?<rev>[0-9]*) or earlier for model (?<machine>.*).$");
               m = regex.Match(subLogLine);
               if (m.Success)
               {
                  isRecognized = true;
                  ConfigurationState = $"GETTING CONFIGURAITON {m.Groups["configname"].Value} rev {m.Groups["rev"].Value}";
                  ModelName = m.Groups["machine"].Value;
               }

               //Located "Standard" configuration revision 1 for model 7800I.
               regex = new Regex("Located \"(?<configname>.*)\" configuration revision (?<rev>[0-9]*) for model (?<machine>.*).$");
               m = regex.Match(subLogLine);
               if (m.Success)
               {
                  isRecognized = true;
                  ConfigurationState = $"LOCATED CONFIGURAITON {m.Groups["configname"].Value} rev {m.Groups["rev"].Value}";
                  ModelName = m.Groups["machine"].Value;
               }

               //Attempting to get Asset Config XML for 7800I
               regex = new Regex("Attempting to get Asset Config XML for (?<machine>.*)$");
               m = regex.Match(subLogLine);
               if (m.Success)
               {
                  isRecognized = true;
                  ConfigurationState = "GETTING ASSET CONFIG";
                  ModelName = m.Groups["machine"].Value;
               }

               //Configuring NetOp without video for model: 7800I
               regex = new Regex("Configuring NetOp without video for model: (?<machine>.*)$");
               m = regex.Match(subLogLine);
               if (m.Success)
               {
                  isRecognized = true;
                  ConfigurationState = "CONFIGURING (NO VIDEO)";
                  ModelName = m.Groups["machine"].Value;
               }

               if (!isRecognized)
               {
                  throw new Exception($"AELogLine.NetOpExtension: did not recognize the log line '{logLine}'");
               }
            }
         }
      }
   }
}
