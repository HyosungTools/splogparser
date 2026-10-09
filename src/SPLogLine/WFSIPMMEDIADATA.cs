using System.Collections.Generic;
using System.Text.RegularExpressions;
using Contract;
using RegEx;

namespace LogLineHandler
{
   // Events where this structure is used:
   //
   // WFS_EXEE_IPM_MEDIADATA (1613) - sent during WFS_CMD_IPM_MEDIA_IN once per item, with the
   // code line and images the device captured. This is the device's verdict on the item: if it
   // shows a code line and wMediaValidity = ITEMOK but the deposit is then rolled back, the
   // decision to reject the item was made by the application or host, not the hardware.
   //
   // The code line itself is account data and is deliberately NOT captured - only its length.

   public class WFSIPMMEDIADATA : SPLine
   {
      public string usMediaID { get; set; } = string.Empty;
      public string ulCodelineDataLength { get; set; } = string.Empty;
      public string wMagneticReadIndicator { get; set; } = string.Empty;
      public string fwInsertOrientation { get; set; } = string.Empty;
      public string ulSizeX { get; set; } = string.Empty;
      public string ulSizeY { get; set; } = string.Empty;
      public string wMediaValidity { get; set; } = string.Empty;

      // one entry per image returned (front, back, ...)
      public List<string> wImageSources { get; set; } = new List<string>();
      public List<string> wImageStatuses { get; set; } = new List<string>();

      // wMediaValidity is prefixed so it can share the Deposit 'reason' column with
      // MEDIABINERROR (10x), MEDIAREFUSED (20x) and MEDIAREJECTED (30x) - see IPMView.xml (40x)
      private const string prefix = "40";

      public WFSIPMMEDIADATA(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_EXEE_IPM_MEDIADATA) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         // e.g. usMediaID = [1],
         result = Util.MatchList(logLine, @"(?<=usMediaID = \[)(\d+)");
         if (result.success) usMediaID = result.xfsMatch.Trim();

         // e.g. ulCodelineDataLength = [30],
         result = Util.MatchList(logLine, @"(?<=ulCodelineDataLength = \[)(\d+)");
         if (result.success) ulCodelineDataLength = result.xfsMatch.Trim();

         // e.g. wMagneticReadIndicator = [1],
         result = Util.MatchList(logLine, @"(?<=wMagneticReadIndicator = \[)(\d+)");
         if (result.success) wMagneticReadIndicator = result.xfsMatch.Trim();

         // e.g. fwInsertOrientation = [0x0012],
         result = Util.MatchList(logLine, @"(?<=fwInsertOrientation = \[)(0x[0-9A-Fa-f]+|\d+)");
         if (result.success) fwInsertOrientation = result.xfsMatch.Trim();

         // e.g. ulSizeX = [69],   ulSizeY = [190]
         result = Util.MatchList(logLine, @"(?<=ulSizeX = \[)(\d+)");
         if (result.success) ulSizeX = result.xfsMatch.Trim();

         result = Util.MatchList(logLine, @"(?<=ulSizeY = \[)(\d+)");
         if (result.success) ulSizeY = result.xfsMatch.Trim();

         // e.g. wMediaValidity = [0]
         result = Util.MatchList(logLine, @"(?<=wMediaValidity = \[)(\d+)");
         if (result.success) wMediaValidity = prefix + result.xfsMatch.Trim();

         // e.g. wImageSource = [0x0001], ... wImageStatus = [0],  (repeated per image)
         foreach (Match m in Regex.Matches(logLine, @"wImageSource = \[(0x[0-9A-Fa-f]+|\d+)\]"))
         {
            wImageSources.Add(m.Groups[1].Value);
         }
         foreach (Match m in Regex.Matches(logLine, @"wImageStatus = \[(\d+)\]"))
         {
            wImageStatuses.Add(m.Groups[1].Value);
         }
      }

      // H U M A N   R E A D A B L E   H E L P E R S

      /// <summary>WFS_IPM_MRI_* : how the code line was read.</summary>
      public string MagneticReadText()
      {
         switch (wMagneticReadIndicator)
         {
            case "0": return "MICR";
            case "1": return "not MICR (image/OCR)";
            case "2": return "MICR, no magnetic chars";
            case "3": return "unknown";
            case "4": return "not MICR format";
            case "5": return "not read";
            default: return wMagneticReadIndicator;
         }
      }

      /// <summary>WFS_IPM_INS* bit flags : how the item was inserted.</summary>
      public string InsertOrientationText()
      {
         int flags;
         string hex = fwInsertOrientation.StartsWith("0x") ? fwInsertOrientation.Substring(2) : fwInsertOrientation;
         if (!int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out flags))
            return fwInsertOrientation;
         if (flags == 0) return "unknown";

         List<string> parts = new List<string>();
         if ((flags & 0x0001) != 0) parts.Add("codeline right");
         if ((flags & 0x0002) != 0) parts.Add("codeline left");
         if ((flags & 0x0004) != 0) parts.Add("codeline bottom");
         if ((flags & 0x0008) != 0) parts.Add("codeline top");
         if ((flags & 0x0010) != 0) parts.Add("face up");
         if ((flags & 0x0020) != 0) parts.Add("face down");
         return string.Join(" ", parts);
      }

      /// <summary>WFS_IPM_DATAOK / DATASRCNOTSUPP / DATASRCMISSING per image.</summary>
      public string ImageStatusText()
      {
         if (wImageStatuses.Count == 0) return "none";
         List<string> parts = new List<string>();
         foreach (string st in wImageStatuses)
         {
            parts.Add(st == "0" ? "ok" : st == "1" ? "not supp" : st == "2" ? "missing" : st);
         }
         return string.Join("/", parts);
      }
   }
}
