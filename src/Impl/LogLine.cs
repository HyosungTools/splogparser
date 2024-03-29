﻿using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Impl
{
   public static class LogLine
   {
      public static string TimeFormatStringMsec = "yyyy-mm-dd hh:mm:ss.ffff";

      /// <summary>
      /// Given a nwlog log line (that can span multiple lines in the log file) read one line
      /// Returns a tuple where: 
      /// found - indicates whether a line was found
      /// oneLine - is the line, 
      /// subLogLine - is the rest of the logLine
      /// </summary>
      /// <param name="logLine">the twlog line (that can span multiple lines in the log file)</param>
      /// <returns>tuple</returns>
      public static (bool found, string oneLine, string subLogLine) ReadNextLine(string logLine)
      {
         string subLogLine = logLine;
         StreamReader streamReader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(subLogLine)));
         string oneLine = streamReader.ReadLine();
         if (!(string.IsNullOrEmpty(oneLine) || string.IsNullOrWhiteSpace(oneLine)))
         {
            // we were able to read one line - for the next line trim any leftover \r\n from the found line
            subLogLine = subLogLine.Substring(oneLine.Length).TrimStart();
            return (true, oneLine, subLogLine);
         }
         // we were not able to read a line
         return (false, string.Empty, subLogLine);
      }

      /// <summary>
      /// Given a nwlog line (that can span mulitple lines), find theline that contains the marker. 
      /// Return a tuple where: 
      /// found - indicates a line with the marker was found
      /// oneLine - the line containing the marker
      /// subLogLine - the rest of the twlog line
      /// </summary>
      /// <param name="logLine">twlog line</param>
      /// <param name="marker">marker to search for</param>
      /// <returns></returns>
      public static (bool found, string oneLine, string subLogLine) FindLine(string logLine, string marker)
      {
         (bool found, string oneLine, string subLogLine) nextLine;
         nextLine.found = true;
         nextLine.oneLine = string.Empty;
         nextLine.subLogLine = logLine;
         while (nextLine.found == true && !nextLine.oneLine.Contains(marker))
         {
            nextLine = LogLine.ReadNextLine(nextLine.subLogLine);
         }
         if (nextLine.found)
         {
            return nextLine;
         }
         return (false, string.Empty, logLine);
      }

      /// <summary>
      /// Converts a list to a CSV string
      /// </summary>
      /// <param name="separator">custom separator</param>
      /// <param name="list"></param>
      /// <returns>a string with values separated by the separator character</returns>
      public static string ListToString(char separator, List<string> list)
      {
         if (list == null)
         {
            return string.Empty;
         }

         StringBuilder sb = new StringBuilder();

         foreach (string s in list)
         {
            if (s != null)
            {
               sb.Append(s);
            }
            else
            {
               sb.Append(string.Empty);
            }

            sb.Append(separator);
         }

         return sb.ToString();
      }

      /// <summary>
      /// Converts a list to a CSV string
      /// </summary>
      /// <param name="separator">custom separator</param>
      /// <param name="list"></param>
      /// <returns>a string with values separated by the separator character</returns>
      public static string ListToString(char separator, List<long> list)
      {
         if (list == null)
         {
            return string.Empty;
         }

         StringBuilder sb = new StringBuilder();

         foreach (long v in list)
         {
            sb.Append(v.ToString());
            sb.Append(separator);
         }

         return sb.ToString();
      }
   }
}
