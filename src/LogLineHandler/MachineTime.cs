using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogLineHandler
{
   /// <summary>
   /// A class that derives time information from the timestamp of a log line which comes from a source machine,
   /// and the timestamp within a message contained in that log line.  The message may originate from a different
   /// source machine.
   /// </summary>
   public class MachineTime
   {
      /// <summary>
      /// Whether the time objects in this MachineTime object are valid.
      /// </summary>
      public bool IsValid { get; private set; } = false;

      /// <summary>
      /// The type of message in the SourcePayload.
      /// </summary>
      public string PayloadType { get; private set; } = string.Empty;

      /// <summary>
      /// The payload message containing the timestamp.
      /// </summary>
      public string SourcePayload { get; private set; } = string.Empty;

      /// <summary>
      /// The asset name of the machine which is the source of the log file.
      /// </summary>
      public string LogSourceMachine { get; private set; } = string.Empty;

      /// <summary>
      /// The asset name of the machine which is the source of the message that contained the timestamp.
      /// </summary>
      public string TimeSourceMachine { get; private set; } = string.Empty;

      /// <summary>
      /// A raw timestamp string in the form 2023-11-04T09:48:07.4570066-07:00.
      /// </summary>
      public string InputRawTimestamp { get; private set; } = string.Empty;

      /// <summary>
      /// The timestamp of the log line which contained the InputRawTimestamp value.
      /// </summary>
      public DateTime LogLineTime { get; private set; } = DateTime.MinValue;

      /// <summary>
      /// The local time on the source machine.
      /// </summary>
      public DateTime SourceMachineLocalTime { get; private set; } = DateTime.MinValue;

      /// <summary>
      /// The timezone offset relative to UTC on the source machine.  Format [-/+]hh:mm
      /// </summary>
      public string SourceMachineUtcOffset { get; private set; } = string.Empty;

      /// <summary>
      /// The UTC time on the source machine.
      /// </summary>
      public DateTime SourceMachineUtcTime { get; private set; } = DateTime.MinValue;

      /// <summary>
      /// The time difference between the LogLineTime and SourceMachineLocalTime.  If the
      /// source machine has an earlier time, this value will be negative.
      /// </summary>
      public TimeSpan InputTimeDifference { get; private set; } = TimeSpan.Zero;


      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="logTimestamp">Timestamp of the log line containing the messagePayload.</param>
      /// <param name="sourceMachine">The AssetName of the machine that generated the logline that contains the messagePayload.</param>
      /// <param name="timeSourceMachine">The AssetName of the machine that generated the messagePayload.</param>
      /// <param name="payloadType">The type of request described by the messagePayload.</param>
      /// <param name="messagePayload">A string that will be examined for a timestamp string in the form 2023-11-04T09:48:07.4570066-07:00.</param>
      /// <param name="inputTimeDifference">The difference between the input timestamp and the current logline timestamp.</param>
      /// <returns>UTC time equivalent to the input timestamp.</returns>
      public MachineTime(DateTime logTimestamp, string sourceMachine, string timeSourceMachine, string payloadType, string messagePayload)
      {
         SourcePayload = messagePayload;
         PayloadType = payloadType;
         LogLineTime = logTimestamp;
         LogSourceMachine = sourceMachine;
         TimeSourceMachine = timeSourceMachine;

         // if conversion to DateTime sees "ANY" timezone offset in the string .NET adjusts the result to LocalTime
         // FOR THE MACHINE ON WHICH THIS PARSER IS RUNNING, not the source machine of the log file

         // validate the input timestamp
         // 2023-11-04T09:48:07.4570066-07:00
         Regex regex = new Regex(@"(?<datetime>[0-9\-]*T[0-9\:\.]*)(?<sign>[+-])(?<utcoffset>[0-9\:]*)");
         Match m = regex.Match(SourcePayload);
         if (m.Success)
         {
            InputRawTimestamp = $"{m.Groups["datetime"].Value}{m.Groups["sign"].Value}{m.Groups["utcoffset"].Value}";
            SourceMachineLocalTime = DateTime.Parse(m.Groups["datetime"].Value);
            SourceMachineUtcOffset = $"{m.Groups["sign"].Value}{m.Groups["utcoffset"].Value}";
            SourceMachineUtcTime = DateTime.Parse(InputRawTimestamp).ToUniversalTime();
            InputTimeDifference = SourceMachineLocalTime - LogLineTime;
            IsValid = true;
         }

         else
         {
            regex = new Regex(@"\""(?<datetime>[0-9\-]*T[0-9\:\.]*)\""");
            m = regex.Match(SourcePayload);
            if (m.Success)
            {
               InputRawTimestamp = $"{m.Groups["datetime"].Value}";
               SourceMachineLocalTime = DateTime.Parse(m.Groups["datetime"].Value);
               InputTimeDifference = SourceMachineLocalTime - LogLineTime;
               IsValid = true;
            }
         }

         if (!IsValid)
         {
            throw new ArgumentException("The parameter is not in the expected format yyyy-MM-ddThh:mm:ss.fff[+/-]-hh:mm or yyyy-MM-ddThh:mm:ss.fff", SourcePayload);
         }
      }
   }

}
