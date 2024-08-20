using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class NDC : APLine
   {
      public string msgclass = string.Empty;
      public string msgsubclass = string.Empty;
      public string luno = string.Empty;
      public string msn = string.Empty;
      public string ndcmsg = string.Empty;
      public string english = "Unknown ";

      public NDC(ILogFileHandler parent, string logLine, APLogType apType) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         (bool success, string subLine) result = NDC.IsolateNdcMessage(logLine);
         if (result.success)
         {
            ndcmsg = result.subLine; 
         }
      }

      protected string DeviceIdInEnglish(string deviceId)
      {
         if (deviceId == "E")
         {
            return deviceId + " (Cash Handler) ";
         }
         if (deviceId == "d")
         {
            return deviceId + " (Cash Handler 0) ";
         }
         if (deviceId == "e")
         {
            return deviceId + " (Cash Handler 1) ";
         }
         if (deviceId == "c")
         {
            return deviceId + " (emv card reader) ";
         }
         if (deviceId == "F")
         {
            return deviceId + " (PPD depository) ";
         }
         if (deviceId == "q")
         {
            return deviceId + " (check processing module) ";
         }

         return deviceId; 
      }

      public virtual bool ParseToEnglishBrief()
      {
         return false;
      }

      public virtual bool ParseToEnglish()
      {
         return false;
      }

      public static (bool success, string xfsMatch, string subLogLine) NDCMatch(string logLine, string regStr, string def = "0")
      {
         Regex timeRegex = new Regex(regStr);
         Match m = timeRegex.Match(logLine);
         if (m.Success)
         {
            return (true, m.Groups[1].Value, logLine.Substring(m.Index));
         }

         return (false, def, logLine);
      }

      public static string[] NDCMatchTable(string logLine, string regStr)
      {
         List<string> values = new List<string>();
         Regex regEx = new Regex(regStr);
         Match m = regEx.Match(logLine);
         if (m.Success)
         {
            values = m.Groups[0].Value.Split('\t').ToList();
            values.RemoveAll(s => s == "");
         }

         return values.ToArray();
      }

      /// <summary>
      /// Similar to the above except here we are dealing with a list type log line, not a table, 
      /// so we are returning distinct single values not a list. 
      /// This is identical to the GenericMatch in _wft_inf_cdm_status so there's a case to abstract into a
      /// separate class all the different regex functions
      /// </summary>
      /// <param name="regEx"></param>
      /// <param name="logLine"></param>
      /// <returns></returns>
      public static (bool success, string xfsMatch, string subLogLine) NDCMatchList(string logLine, string regStr, string def = "0")
      {
         Regex regEx = new Regex(regStr);
         Match m = regEx.Match(logLine);
         if (m.Success)
         {
            return (true, m.Groups[1].Value, logLine.Substring(m.Index));
         }

         return (false, def, logLine);
      }


      // e.g. lpulValues = [0, 0, 0, 4, 4, 0],
      public static (bool success, string[] xfsMatch, string subLogLine) NDCMatchListToArray(string logLine, string regStr)
      {
         Regex typeRegex = new Regex(regStr);
         Match m = typeRegex.Match(logLine);
         if (m.Success)
         {
            List<string> usTypes = m.Groups[0].Value.Split(',').ToList();
            usTypes.RemoveAll(s => s == "");
            string[] usTypesArray = usTypes.ToArray();
            for (int i = 0; i < usTypesArray.Length; i++)
               usTypesArray[i] = usTypesArray[i].Trim();
            return (true, usTypesArray, logLine.Substring(m.Index));
         }

         return (false, null, logLine);
      }

      /// <summary>
      /// Generic pull out usCount. There are two forms 'usCount=5' and 'usCount = [5]'
      /// </summary>
      /// <param name="logLine">xfs log line </param>
      /// <param name="regStr">regular expression identifying usCount</param>
      /// <param name="def">default return value</param>
      /// <returns></returns>
      public static int NDCMatchInt(string logLine, string regStr, int def = 0)
      {
         Regex countRegex = new Regex(regStr);
         Match m = countRegex.Match(logLine);
         if (m.Success)
         {
            return int.Parse(m.Groups[1].Value);
         }

         return def;
      }

      public static string[] Resize(string[] values, int ulCount, string defValue = "0")
      {
         int oldSize = values.Length;
         Array.Resize(ref values, ulCount);
         for (int i = oldSize; i < ulCount; i++)
         {
            values[i] = defValue;
         }
         return values;
      }

      public static string[] TrimAll(string[] values)
      {
         return values.Select(value => value.Trim()).ToArray();
      }

      /// <summary>
      /// Given a list format pull off in strings the next logical unit for processing
      /// </summary>
      /// <param name="logLine"></param>
      /// <returns></returns>
      public static (string thisLogicalUnit, string nextLogicalUnits) NextLogicalUnit(string logLine)
      {
         int indexOfOpenBracket = logLine.IndexOf('{');
         if (indexOfOpenBracket < 0)
         {
            return (string.Empty, logLine);
         }

         string subLogLine = logLine.Substring(indexOfOpenBracket);
         int endPos = -1;
         int bracketCount = 0;

         foreach (char c in subLogLine)
         {
            // endPos is the index of 'c'
            endPos++;
            if (c.Equals('{'))
            {
               bracketCount++;
            }
            else if (c.Equals('}'))
            {
               bracketCount--;
            }
            if (bracketCount == 0)
            {
               break;
            }
         }

         return (subLogLine.Substring(0, endPos), subLogLine.Substring(endPos + 1));
      }

      public static (bool, string) IsolateNdcMessage(string logLine)
      {
         string atm2Host = "ATM2HOST: ";
         string host2Atm = "HOST2ATM: ";

         int idx = logLine.IndexOf(atm2Host);
         if (idx >= 0)
            return (true, logLine.Substring(idx + atm2Host.Length));

         idx = logLine.IndexOf(host2Atm);
         if (idx >= 0)
            return (true, logLine.Substring(idx + host2Atm.Length));

         return (false, logLine);
      }

      public static (bool, string, string) GetNextFieldBySeparator(string ndcMessage, string separatorHex = "1c")
      {
         char separator = (char)Convert.ToInt32(separatorHex, 16);

         int idx = ndcMessage.IndexOf(separator);
         if (idx < 0)
         {
            // error
            return (false, string.Empty, ndcMessage);
         }
         if (idx == 0)
         {
            return (true, string.Empty, ndcMessage.Substring(idx + 1));
         }

         return (true, ndcMessage.Substring(0, idx), ndcMessage.Substring(idx + 1));
      }

      public (bool, string, string) GetNextFieldBySize(string ndcMessage, int size)
      {
         return (true, ndcMessage.Substring(0, size), ndcMessage.Substring(size));
      }

      public static string[] SplitMessage(string logLine)
      {
         string separatorHex = "1c";
         char separator = (char)Convert.ToInt32(separatorHex, 16);

         return Regex.Split(logLine, separator.ToString());
      }
   }
}
