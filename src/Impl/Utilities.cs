using Contract;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace Impl
{
   /// <summary>
   /// utility functions to assist with parsing
   /// </summary>
   public static class Utilities
   {
      public static string GetTimeFromLogLine(string logLine)
      {
         string logTime = "2022-01-01T00:00:00.000";
         // Example: P00102022/12/07001201:41 31.6120006Normal0024
         Regex timeRegEx = new Regex("(\\d{4})/(\\d{2})/(\\d{2}).*(\\d{2}:\\d{2}) (\\d{2}\\.\\d{3})");
         Match mtch = timeRegEx.Match(logLine);
         if (mtch.Success && mtch.Groups.Count >= 5)
         {
            logTime = mtch.Groups[1].Value + "-" + mtch.Groups[2].Value + "-" + mtch.Groups[3].Value + "T" +
                      mtch.Groups[4].Value + ":" + mtch.Groups[5].Value;
         }
         return logTime;
      }

      /// <summary>
      /// Search for a marker in a log line and return length characters after the marker
      /// For example, given {HSERVICE[6], HWND[0x00020314], REQUESTID[66744], dwCmdCode[801], dwTimeOut[0]}02184294967
      /// a search for 'dwTimeOut[' with a length of 1 return s '0'
      /// </summary>
      /// <param name="logLine">text string to search</param>
      /// <param name="searchStr">marker in text string</param>
      /// <param name="length">number of characters after the marker to return</param>
      /// <returns>tuple</returns>
      public static (bool found, string foundStr, string subLogLine) FindByMarker(string logLine, string searchStr, int length)
      {
         string subLogLine = logLine;
         int pos = subLogLine.IndexOf(searchStr);
         if (pos >= 0)
         {
            string foundStr = subLogLine.Substring(pos + searchStr.Length, length);
            subLogLine = subLogLine.Substring(pos + searchStr.Length + length);
            return (true, foundStr, subLogLine);
         }

         return (false, string.Empty, subLogLine);
      }

      /// <summary>
      /// Search for a substring in a string bounded by two markers. For example, given 
      /// lpQueryDetails = [0x008fec40] { fwPosition = [0] }
      /// a search bounded by "{" and "}" would return  fwPosition = [0] 
      /// </summary>
      /// <param name="logLine">text string to search</param>
      /// <param name="startStr">marker in text string</param>
      /// <param name="endStr">number of characters after the marker to return</param>
      /// <returns>tuple</returns>
      public static (bool found, string foundStr, string subLogLine) FindByMarker(string logLine, string startStr, string endStr)
      {
         string subLogLine = logLine;
         int startPos = subLogLine.IndexOf(startStr);
         if (startPos >= 0)
         {
            int endPos = subLogLine.IndexOf(endStr, startPos + 1);
            if (endPos > 0)
            {
               string foundStr = logLine.Substring(startPos, endPos - startPos);
               subLogLine = logLine.Substring(endPos + 1);
               return (true, foundStr, subLogLine);
            }
         }

         return (false, string.Empty, subLogLine);
      }
      /// <summary>
      /// Search for a substring in a string bounded by char two markers. The markers should be siblings, such as [] or {}
      /// </summary>
      /// <param name="logLine">text string to search</param>
      /// <param name="startStr">marker in text string</param>
      /// <param name="endStr">number of characters after the marker to return</param>
      /// <returns>tuple</returns>
      public static (bool found, string foundStr, string subLogLine) FindByBracket(string logLine, char cStart, char cEnd)
      {
         string subLogLine = logLine;

         // sanity test - is the start/end char in the logLine 
         if ((subLogLine.IndexOf(cStart) < 0) || (subLogLine.IndexOf(cEnd) < 0))
         {
            // either the start or end char are not present in the log line so fail
            return (false, string.Empty, logLine);
         }
         // sanity test - is the start/end char in the right order? 
         if (subLogLine.IndexOf(cStart) > subLogLine.IndexOf(cEnd))
         {
            // the start char is after the end char so fail
            return (false, string.Empty, logLine);
         }

         // try to find substring bounded by start/end char, account for nested brackets
         subLogLine = subLogLine.Substring(subLogLine.IndexOf(cStart) + 1);
         int startPos = 0;
         int endPos = 0;
         int bracketCount = 1;

         foreach (char c in subLogLine)
         {
            if (c.Equals(cStart))
            {
               bracketCount++;
            }
            else if (c.Equals(cEnd))
            {
               bracketCount--;

               if (bracketCount == 0)
               {
                  break;
               }
            }

            endPos++;
         }

         // mismatched brackets so fail
         if (bracketCount != 0)
         {
            // the start char is after the end char so fail
            return (false, string.Empty, logLine);
         }

         // found the substring based on matching brackets
         string foundStr = subLogLine.Substring(startPos, endPos - startPos);
         subLogLine = subLogLine.Substring(endPos + 1);

         return (true, foundStr, subLogLine);
      }

      /// <summary>
      /// Given a twlog log line (that can span multiple lines in the log file) read one line
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
      /// Given a twlog line (that can span mulitple lines), find theline that contains the marker. 
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
            nextLine = Utilities.ReadNextLine(nextLine.subLogLine);
         }
         if (nextLine.found)
         {
            return nextLine;
         }
         return (false, string.Empty, logLine);
      }


      /// <summary>
      /// Change a "1" to a "0" and vise-versa. A use case is where you isolate something like "Output Position Empty = [1]"
      /// and it makes more sense to report not empty. 
      /// </summary>
      /// <param name="flipMe"></param>
      /// <returns></returns>
      public static string Flip(string flipMe)
      {
         string returnMe = flipMe.Trim();
         if (string.IsNullOrEmpty(returnMe))
         {
            if (returnMe == "1")
            {
               returnMe = "0";
            }
            else if (returnMe == "0")
            {
               returnMe = "1";
            }
         }
         return returnMe;
      }

      public static bool ExtractZipFiles(IContext ctx, string currentFolder)
      {
         string[] zipFiles = ctx.ioProvider.GetFiles(currentFolder, "*.zip");
         foreach (string zipFile in zipFiles)
         {
            ctx.ConsoleWriteLogLine("ZipFile: " + zipFile);
            string newFolderName = ctx.ioProvider.GetFileNameWithoutExtension(zipFile);

            ctx.ConsoleWriteLogLine("Create directory:" + currentFolder + "\\" + newFolderName);
            if (!ctx.ioProvider.CreateDirectory(currentFolder + "\\" + newFolderName))
            {
               ctx.ConsoleWriteLogLine("Failed to create directory:" + zipFile);
               return false;
            }

            // Extract current zip file
            ctx.ioProvider.ExtractToDirectory(zipFile, currentFolder + "\\" + newFolderName);
         }

         foreach (var directory in Directory.GetDirectories(currentFolder))
         {
            ctx.ConsoleWriteLogLine("Iterating into: " + directory);
            ExtractZipFiles(ctx, directory);
         }

         return true;
      }

      public static bool FindAllTraceFiles(IContext ctx)
      {
         ctx.ConsoleWriteLogLine("Unzipping the archive...");
         if (!Utilities.ExtractZipFiles(ctx, ctx.WorkFolder))
         {
            return false;
         }
         ctx.ConsoleWriteLogLine("Unzip complete.");

         // find all nwlog files
         string[] tempFiles = ctx.ioProvider.GetFiles(ctx.WorkFolder, "*.nwlog");

         // reduce to nwlog files in the [SP] subfolder, if any
         List<string> tempList = new List<string>();
         for (int i = 0; i < tempFiles.Length; i++)
         {
            // get rid of nwlog files not in the [SP] subFolder
            if (tempFiles[i].Contains("[SP]"))
            {
               tempList.Add(tempFiles[i]);
            }
         }

         ctx.nwlogFiles = tempList.ToArray();
         return true;
      }
   }
}
