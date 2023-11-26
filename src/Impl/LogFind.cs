namespace Impl
{
   public static class LogFind
   {
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
            startPos = startPos + startStr.Length;
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
   }
}
