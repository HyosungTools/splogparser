using Contract;

namespace LogLineHandler
{
   /// <summary>
   /// Parses a [Pinpad.ExecuteDeviceCommand] line and extracts the PIN command name.
   /// Example:
   ///   INFO [...] [Pinpad.ExecuteDeviceCommand] [TID:3] [DEVICE] { "device": "PIN", "command": "ReadPIN", ... }
   /// Extracted field: "ReadPIN"
   /// </summary>
   public class APLinePinCommand : APLine
   {
      public string field = string.Empty;

      public APLinePinCommand(ILogFileHandler parent, string logLine) : base(parent, logLine, APLogType.APLOG_PIN_EXECUTECOMMAND)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // Extract the value of "command": "..." from the JSON payload
         string lookFor = "\"command\": \"";
         int idx = logLine.IndexOf(lookFor);
         if (idx != -1)
         {
            int start = idx + lookFor.Length;
            int end = logLine.IndexOf("\"", start);
            if (end != -1)
            {
               field = logLine.Substring(start, end - start);
            }
         }
      }
   }
}
