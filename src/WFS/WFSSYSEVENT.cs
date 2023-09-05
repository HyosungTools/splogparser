using System;
using System.Collections.Generic;
using Contract;

namespace Impl
{
   public class WFSSYSEVENT : WFS
   {
      public string logicalName = string.Empty;
      public string physicalName = string.Empty;
      public string lpbDescription = string.Empty;

      public WFSSYSEVENT(IContext ctx) : base(ctx)
      {
      }

      public string Initialize(string nwLogLine)
      {
         (bool success, string xfsMatch, string subLogLine) result;

         // fwDevice
         result = logicalNameFromSysEvent(nwLogLine);
         if (result.success) logicalName = result.xfsMatch.Trim();

         // fwSafeDoor
         result = physicalNameFromSysEvent(nwLogLine);
         if (result.success) physicalName = result.xfsMatch.Trim();

         // fwDispenser
         result = lpbDescriptionFromSysEvent(result.subLogLine);
         if (result.success) lpbDescription = result.xfsMatch.Trim();

         if (lpbDescription != string.Empty)
            lpbDescription = HexStringToAscii(lpbDescription);

         return result.subLogLine;
      }

      public static (bool success, string xfsMatch, string subLogLine) logicalNameFromSysEvent(string logLine)
      {
         return WFS.WFSMatchList(logLine, "lpszLogicalName = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) physicalNameFromSysEvent(string logLine)
      {
         return WFS.WFSMatchList(logLine, "lpszPhysicalName = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) lpbDescriptionFromSysEvent(string logLine)
      {
         return WFS.WFSMatchList(logLine, "lpbDescription = \\[(.*)\\]", "0");
      }

      static string HexStringToAscii(string hexString)
      {
         // Ensure the hex string has an even length (each byte is represented by 2 characters)
         if (hexString.Length % 2 != 0)
         {
            throw new ArgumentException("Hex string length must be even.");
         }

         // Create a byte array to store the converted bytes
         byte[] bytes = new byte[hexString.Length / 2];

         // Parse each pair of hex characters and convert them to bytes
         for (int i = 0; i < hexString.Length; i += 2)
         {
            string hexPair = hexString.Substring(i, 2);
            bytes[i / 2] = Convert.ToByte(hexPair, 16);
         }

         // Convert the byte array to an ASCII string
         string asciiString = System.Text.Encoding.ASCII.GetString(bytes);

         return asciiString;
      }
   }
}
