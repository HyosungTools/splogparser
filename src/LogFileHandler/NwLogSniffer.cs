using System;
using System.IO;
using System.Text.RegularExpressions;
using Contract;

namespace LogFileHandler
{
   /// <summary>
   /// Both SPLogHandler and ATNwLogHandler match the extension "*.nwlog", but the
   /// content is different:
   ///   - SP nwlogs are XFS/WFS device traces  ("tsTimestamp = [...]" / "hResult = [...]")
   ///   - ActiveTeller nwlogs are AP application logs ("  INFO [ts] [ver] [Class.Method] ...")
   /// A filename cannot tell them apart, and GetFiles() recurses the whole extraction tree,
   /// so without this both handlers would claim every *.nwlog. Classify() reads the first
   /// few real lines and decides by content, so each handler keeps only its own kind.
   /// </summary>
   public static class NwLogSniffer
   {
      public enum Kind { Unknown, Xfs, ApApplication }

      // "  INFO [2026-07-24 12:08:18-588] ..."  (also WARN / ERROR)
      private static readonly Regex ApHeader =
         new Regex(@"^\s*(INFO|WARN|ERROR)\s+\[\d{4}-\d{2}-\d{2} ", RegexOptions.Compiled);

      public static Kind Classify(string path, ICreateStreamReader readerFactory)
      {
         try
         {
            using (StreamReader reader = readerFactory.Create(path))
            {
               string line;
               int scanned = 0;
               while ((line = reader.ReadLine()) != null && scanned < 40)
               {
                  if (line.Trim().Length == 0) continue;
                  scanned++;

                  // XFS / WFS device trace -> belongs to SPLogHandler
                  if (line.Contains("tsTimestamp = [") || line.Contains("hResult = ["))
                     return Kind.Xfs;

                  // AP application framing -> belongs to ATNwLogHandler
                  if (ApHeader.IsMatch(line))
                     return Kind.ApApplication;
               }
            }
         }
         catch (Exception)
         {
            // If we can't read it, don't claim it either way.
         }

         return Kind.Unknown;
      }
   }
}
