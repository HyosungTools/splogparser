
using System;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public enum AVLogType
   {
      /* Not a log line we are interested in */
      None,

      Startup,
      CMCFlexCustomerDataExtension,
      CorelationKeyBridgeCustomerDataExtension,
      CSIBridgeCustomerDataExtension,
      CUAnswersCustomerDataExtension,
      CUProdigyCustomerDataExtension,
      DemoCustomerDataExtension,
      FiservAccessAdvantageCustomerDataExtension,
      FiservDNACustomerDataExtension,
      FiservESFCustomerDataExtension,
      FiservSpectrumCustomerDataExtension,
      FISHorizonCustomerDataExtension,
      FISIBSCustomerDataExtension,
      FISMiserCustomerDataExtension,
      JXchangeCustomerDataExtension,
      SymXchangeCustomerDataExtension
   }


   public class AVLine : LogLine, ILogLine
   {

      /// <summary>
      /// Gets the log type for the customer data extensionn corresponding to the input name.
      /// </summary>
      /// <param name="name"></param>
      /// <returns></returns>
      public static AVLogType GetCoreBankingAVLogType(string name)
      {
         int idx = name.IndexOf("CustomerDataExtension");

         if (idx > 0)
         {
            // remove it
            name = name.Substring(0, idx);
         }

         foreach (int i in Enum.GetValues(typeof(AVLogType)))
         {
            var eName = Enum.GetName(typeof(AVLogType), i).ToLower();

            if (eName.StartsWith(name.ToLower()) && eName.EndsWith("customerdataextension"))
            {
               return (AVLogType)i;
            }
         }

         return AVLogType.None;
      }

      // implementations of the ILogLine interface
      public string Timestamp { get; set; }
      public string HResult { get; set; }

      public AVLogType avType { get; set; }

      public AVLine(ILogFileHandler parent, string logLine, AVLogType avType) : base(parent, logLine)
      {
         this.avType = avType;
         Initialize();
      }

      protected virtual void Initialize()
      {
         Timestamp = tsTimestamp();
         IsValidTimestamp = bCheckValidTimestamp(Timestamp);
         HResult = hResult();
      }

      protected override string hResult()
      {
         return "";
      }

      protected override string tsTimestamp()
      {
         // set timeStamp to a default time
         string timestamp = LogLine.DefaultTimestamp;

         //2023-10-16 01:00:01 ActiveTeller Server version 1.3.1.0 is starting
         //2023-10-16 01:00:04 [SymXchangeCustomerDataExtension] The 'SymXchangeCustomerDataExtension' extension is started.

         // search for timestamp in the log line
         string regExp = @"\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}";
         Regex timeRegex = new Regex(regExp);
         Match m = timeRegex.Match(logLine);
         if (m.Success)
         {
            timestamp = m.Groups[0].Value;
         }

         return timestamp;
      }
   }
}
