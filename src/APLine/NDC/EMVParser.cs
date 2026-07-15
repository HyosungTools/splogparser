using System;
using System.Collections.Generic;
using System.Text;

namespace LogLineHandler
{
   /// <summary>
   /// Parses the EMV (chip) TLV data embedded in NDC transaction request
   /// messages and renders the common data elements in English.
   ///
   /// The ICC data block begins at the AIP (tag '82'), which follows the
   /// '5F34' framing. TLV is BER encoded: tag (1-2 bytes), length (1 byte),
   /// value (length bytes). DOL-holding tags such as CDOL1 ('8C') carry a
   /// tag+length list as their value; the walker consumes that as an opaque
   /// value and does not recurse into it.
   ///
   /// Value decodes verified against EMV 4.2 Book 3:
   ///   TVR ('95')   - Annex C5, Table 42
   ///   CID ('9F27') - Table 14
   ///   CVM ('9F34') - Annex C3, Table 39
   ///   Transaction Type ('9C') - ISO 8583 processing code (scheme-dependent)
   /// </summary>
   public static class EMVParser
   {
      public static string ParseToEnglish(string message)
      {
         if (string.IsNullOrEmpty(message))
         {
            return string.Empty;
         }

         int start = FindEmvStart(message);
         if (start < 0)
         {
            return string.Empty;
         }

         List<TlvObject> tags = ParseTlv(message, start);
         if (tags.Count == 0)
         {
            return string.Empty;
         }

         // Index tags by tag id for ordered emit. Last occurrence wins on the
         // rare duplicate (real value over a DOL-listed tag).
         Dictionary<string, string> v = new Dictionary<string, string>();
         int total = 0;
         foreach (TlvObject t in tags)
         {
            v[t.Tag] = t.Value;
            total = total + 1;
         }

         // Establish currency first so amounts label correctly.
         string currencyCode = "840";
         if (v.ContainsKey("5F2A"))
         {
            currencyCode = StripLeadingZero(v["5F2A"]);
         }

         List<string> parts = new List<string>();

         // Terse: only the fields that vary transaction-to-transaction on an
         // ATM fleet. Outcome (always ARQC), CVM (always online PIN), and the
         // always-on TVR flags are constant here and omitted; the decoders
         // remain in this class for the EMV workflow view / deeper lookup.

         // Transaction type
         if (v.ContainsKey("9C"))
         {
            parts.Add(DecodeTransactionType(v["9C"]));
         }

         // Amount + currency (only when EMV carries a non-zero value; the NDC
         // amount field is the primary source on withdrawals)
         if (v.ContainsKey("9F02") && !IsZeroAmount(v["9F02"]))
         {
            parts.Add("Amount " + FormatAmount(v["9F02"]) + " " + CurrencyName(currencyCode));
         }

         // Country
         if (v.ContainsKey("9F1A"))
         {
            parts.Add("Country " + CountryName(StripLeadingZero(v["9F1A"])));
         }

         // AID
         if (v.ContainsKey("9F06"))
         {
            parts.Add("AID " + v["9F06"]);
         }

         // Rare TVR problem flags only - the always-on ones (online PIN entered,
         // offline data auth not performed, exceeds floor limit) are filtered
         // out as constant for online-PIN ATM transactions; anything left is a
         // genuine anomaly worth surfacing in a dispute.
         if (v.ContainsKey("95"))
         {
            string tvr = DecodeTvrProblems(v["95"]);
            if (tvr.Length > 0)
            {
               parts.Add("TVR: " + tvr);
            }
         }

         if (parts.Count == 0)
         {
            return string.Empty;
         }

         StringBuilder result = new StringBuilder();
         result.Append("EMV [");
         result.Append(string.Join(", ", parts.ToArray()));
         result.Append("]");
         return result.ToString();
      }

      // 95 TVR - like DecodeTvrFlags but filters out the flags that are ALWAYS
      // set on an online-PIN ATM transaction (online PIN entered, offline data
      // auth not performed, exceeds floor limit), leaving only genuine
      // anomalies worth surfacing inline. Book 3 Table 42.
      private static string DecodeTvrProblems(string vv)
      {
         if (string.IsNullOrEmpty(vv) || vv.Length < 10 || !IsHex(vv))
         {
            return string.Empty;
         }

         int b1 = Convert.ToInt32(vv.Substring(0, 2), 16);
         int b2 = Convert.ToInt32(vv.Substring(2, 2), 16);
         int b3 = Convert.ToInt32(vv.Substring(4, 2), 16);
         int b4 = Convert.ToInt32(vv.Substring(6, 2), 16);
         int b5 = Convert.ToInt32(vv.Substring(8, 2), 16);

         List<string> notes = new List<string>();

         // Byte 1 - omit "offline data auth not performed" (b8, always on here)
         if ((b1 & 0x40) != 0) notes.Add("SDA failed");
         if ((b1 & 0x20) != 0) notes.Add("ICC data missing");
         if ((b1 & 0x10) != 0) notes.Add("card on exception file");
         if ((b1 & 0x08) != 0) notes.Add("DDA failed");
         if ((b1 & 0x04) != 0) notes.Add("CDA failed");

         // Byte 2
         if ((b2 & 0x80) != 0) notes.Add("app versions differ");
         if ((b2 & 0x40) != 0) notes.Add("expired application");
         if ((b2 & 0x20) != 0) notes.Add("application not yet effective");
         if ((b2 & 0x10) != 0) notes.Add("requested service not allowed");
         if ((b2 & 0x08) != 0) notes.Add("new card");

         // Byte 3 - omit "online PIN entered" (b3, always on here)
         if ((b3 & 0x80) != 0) notes.Add("cardholder verification failed");
         if ((b3 & 0x40) != 0) notes.Add("unrecognised CVM");
         if ((b3 & 0x20) != 0) notes.Add("PIN try limit exceeded");
         if ((b3 & 0x10) != 0) notes.Add("PIN pad missing/not working");
         if ((b3 & 0x08) != 0) notes.Add("PIN not entered - PIN pad present");

         // Byte 4 - omit "exceeds floor limit" (b8, always on here)
         if ((b4 & 0x40) != 0) notes.Add("lower consecutive offline limit exceeded");
         if ((b4 & 0x20) != 0) notes.Add("upper consecutive offline limit exceeded");
         if ((b4 & 0x10) != 0) notes.Add("randomly selected for online");
         if ((b4 & 0x08) != 0) notes.Add("merchant forced online");

         // Byte 5
         if ((b5 & 0x80) != 0) notes.Add("default TDOL used");
         if ((b5 & 0x40) != 0) notes.Add("issuer authentication failed");
         if ((b5 & 0x20) != 0) notes.Add("script processing failed before final GEN AC");
         if ((b5 & 0x10) != 0) notes.Add("script processing failed after final GEN AC");

         if (notes.Count == 0)
         {
            return string.Empty;
         }

         return string.Join("; ", notes.ToArray());
      }

      /// <summary>
      /// The ICC data block starts at the AIP (tag '82', length almost always
      /// '02'). Anchor on "8202" appearing after the '5F34' framing.
      /// </summary>
      private static int FindEmvStart(string h)
      {
         int f = h.IndexOf("5F3401");
         if (f >= 0)
         {
            int a = h.IndexOf("8202", f);
            if (a >= 0)
            {
               return a;
            }
         }

         int idx = h.IndexOf("8202");
         while (idx >= 0)
         {
            List<TlvObject> probe = ParseTlv(h, idx, 6);
            if (probe.Count >= 5)
            {
               return idx;
            }
            idx = h.IndexOf("8202", idx + 2);
         }

         return -1;
      }

      /// <summary>
      /// Walk BER-TLV from the given offset. Tolerates masked bytes inside
      /// values but stops cleanly on structural non-hex.
      /// </summary>
      private static List<TlvObject> ParseTlv(string h, int start, int limit = 100)
      {
         List<TlvObject> result = new List<TlvObject>();
         int i = start;

         while (i < h.Length && result.Count < limit)
         {
            if (i + 2 > h.Length || !IsHex(h.Substring(i, 2)))
            {
               break;
            }

            int b0 = Convert.ToInt32(h.Substring(i, 2), 16);
            string tag;
            int afterTag;

            if ((b0 & 0x1F) == 0x1F)
            {
               if (i + 4 > h.Length || !IsHex(h.Substring(i + 2, 2)))
               {
                  break;
               }
               tag = h.Substring(i, 4);
               afterTag = i + 4;
            }
            else
            {
               tag = h.Substring(i, 2);
               afterTag = i + 2;
            }

            if (afterTag + 2 > h.Length || !IsHex(h.Substring(afterTag, 2)))
            {
               break;
            }

            int len = Convert.ToInt32(h.Substring(afterTag, 2), 16);
            int valStart = afterTag + 2;

            if (valStart + (len * 2) > h.Length)
            {
               break;
            }

            string val = h.Substring(valStart, len * 2);

            TlvObject tlv = new TlvObject();
            tlv.Tag = tag;
            tlv.Length = len;
            tlv.Value = val;
            result.Add(tlv);

            i = valStart + (len * 2);
         }

         return result;
      }

      private static bool IsHex(string s)
      {
         foreach (char c in s)
         {
            bool ok = (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f');
            if (!ok)
            {
               return false;
            }
         }
         return true;
      }

      // Amount is minor units BCD. All fleet currencies are 2-exponent.
      private static string FormatAmount(string bcd)
      {
         if (string.IsNullOrEmpty(bcd) || !IsHex(bcd))
         {
            return "?";
         }

         decimal cents;
         if (!decimal.TryParse(bcd, out cents))
         {
            return "?";
         }

         return (cents / 100).ToString("0.00");
      }

      private static bool IsZeroAmount(string bcd)
      {
         if (string.IsNullOrEmpty(bcd) || !IsHex(bcd))
         {
            return true;
         }

         decimal amt;
         if (decimal.TryParse(bcd, out amt))
         {
            return amt == 0;
         }
         return true;
      }

      // Currency label - ISO 4217, only the codes the fleet sees.
      // VERIFY additions against ISO 4217 before adding.
      private static string CurrencyName(string code)
      {
         if (code == "840")
         {
            return "USD";
         }
         if (code == "214")
         {
            return "DOP";   // Dominican Peso - VERIFY 214 against ISO 4217
         }
         return code;
      }

      // Country label - ISO 3166 numeric, only the codes the fleet sees.
      // VERIFY additions against ISO 3166 before adding.
      private static string CountryName(string code)
      {
         if (code == "840")
         {
            return "USA";
         }
         if (code == "214")
         {
            return "DOM";   // Dominican Republic - VERIFY 214 against ISO 3166
         }
         return code;
      }

      // 9C - ISO 8583 processing code. Scheme-dependent; common conventions
      // labeled but noted as such.
      private static string DecodeTransactionType(string v)
      {
         if (v == "00")
         {
            return "00 (purchase)";
         }
         if (v == "01")
         {
            return "01 (cash withdrawal)";
         }
         if (v == "09")
         {
            return "09 (purchase with cashback)";
         }
         if (v == "20")
         {
            return "20 (refund)";
         }
         if (v == "30")
         {
            return "30 (balance inquiry)";
         }
         return v + " (scheme-defined)";
      }

      // 9F27 CID - Book 3 Table 14. Bits 8-7 give the cryptogram type.
      private static string DecodeCid(string v)
      {
         if (string.IsNullOrEmpty(v) || !IsHex(v))
         {
            return "CID " + v;
         }

         int b = Convert.ToInt32(v.Substring(0, 2), 16);
         int type = (b >> 6) & 0x03;

         if (type == 0)
         {
            return "CID " + v + " (AAC - declined/offline abort)";
         }
         if (type == 1)
         {
            return "CID " + v + " (TC - approved offline)";
         }
         if (type == 2)
         {
            return "CID " + v + " (ARQC - online authorization requested)";
         }
         return "CID " + v + " (RFU)";
      }

      // 9F34 CVM Results - Book 3 Table 39. Byte 1 low 6 bits = method.
      private static string DecodeCvm(string v)
      {
         if (string.IsNullOrEmpty(v) || v.Length < 2 || !IsHex(v.Substring(0, 2)))
         {
            return v;
         }

         int b1 = Convert.ToInt32(v.Substring(0, 2), 16);
         int method = b1 & 0x3F;
         string desc;

         switch (method)
         {
            case 0x00: desc = "fail CVM"; break;
            case 0x01: desc = "plaintext PIN by ICC"; break;
            case 0x02: desc = "enciphered PIN online"; break;
            case 0x03: desc = "plaintext PIN by ICC + signature"; break;
            case 0x04: desc = "enciphered PIN by ICC"; break;
            case 0x05: desc = "enciphered PIN by ICC + signature"; break;
            case 0x1E: desc = "signature"; break;
            case 0x1F: desc = "no CVM required"; break;
            default: desc = "method " + method.ToString(); break;
         }

         return v + " (" + desc + ")";
      }

      // 95 TVR - Book 3 Table 42. Returns problem-flag text only; empty string
      // when no flags are set (all-clear TVR is suppressed as noise).
      private static string DecodeTvrFlags(string vv)
      {
         if (string.IsNullOrEmpty(vv) || vv.Length < 10 || !IsHex(vv))
         {
            return string.Empty;
         }

         int b1 = Convert.ToInt32(vv.Substring(0, 2), 16);
         int b2 = Convert.ToInt32(vv.Substring(2, 2), 16);
         int b3 = Convert.ToInt32(vv.Substring(4, 2), 16);
         int b4 = Convert.ToInt32(vv.Substring(6, 2), 16);
         int b5 = Convert.ToInt32(vv.Substring(8, 2), 16);

         List<string> notes = new List<string>();

         // Byte 1
         if ((b1 & 0x80) != 0) notes.Add("offline data auth not performed");
         if ((b1 & 0x40) != 0) notes.Add("SDA failed");
         if ((b1 & 0x20) != 0) notes.Add("ICC data missing");
         if ((b1 & 0x10) != 0) notes.Add("card on exception file");
         if ((b1 & 0x08) != 0) notes.Add("DDA failed");
         if ((b1 & 0x04) != 0) notes.Add("CDA failed");

         // Byte 2
         if ((b2 & 0x80) != 0) notes.Add("app versions differ");
         if ((b2 & 0x40) != 0) notes.Add("expired application");
         if ((b2 & 0x20) != 0) notes.Add("application not yet effective");
         if ((b2 & 0x10) != 0) notes.Add("requested service not allowed");
         if ((b2 & 0x08) != 0) notes.Add("new card");

         // Byte 3
         if ((b3 & 0x80) != 0) notes.Add("cardholder verification failed");
         if ((b3 & 0x40) != 0) notes.Add("unrecognised CVM");
         if ((b3 & 0x20) != 0) notes.Add("PIN try limit exceeded");
         if ((b3 & 0x10) != 0) notes.Add("PIN pad missing/not working");
         if ((b3 & 0x08) != 0) notes.Add("PIN not entered - PIN pad present");
         if ((b3 & 0x04) != 0) notes.Add("online PIN entered");

         // Byte 4
         if ((b4 & 0x80) != 0) notes.Add("transaction exceeds floor limit");
         if ((b4 & 0x40) != 0) notes.Add("lower consecutive offline limit exceeded");
         if ((b4 & 0x20) != 0) notes.Add("upper consecutive offline limit exceeded");
         if ((b4 & 0x10) != 0) notes.Add("randomly selected for online");
         if ((b4 & 0x08) != 0) notes.Add("merchant forced online");

         // Byte 5
         if ((b5 & 0x80) != 0) notes.Add("default TDOL used");
         if ((b5 & 0x40) != 0) notes.Add("issuer authentication failed");
         if ((b5 & 0x20) != 0) notes.Add("script processing failed before final GEN AC");
         if ((b5 & 0x10) != 0) notes.Add("script processing failed after final GEN AC");

         if (notes.Count == 0)
         {
            return string.Empty;
         }

         return string.Join("; ", notes.ToArray());
      }

      // 9A date - YYMMDD BCD
      private static string DecodeDate(string v)
      {
         if (string.IsNullOrEmpty(v) || v.Length < 6)
         {
            return v;
         }
         return "20" + v.Substring(0, 2) + "-" + v.Substring(2, 2) + "-" + v.Substring(4, 2);
      }

      private static string StripLeadingZero(string code)
      {
         string c = code;
         while (c.Length > 3 && c.StartsWith("0"))
         {
            c = c.Substring(1);
         }
         return c;
      }

      private static string HexToInt(string v)
      {
         if (string.IsNullOrEmpty(v) || !IsHex(v))
         {
            return v;
         }
         return Convert.ToInt32(v, 16).ToString();
      }

      private class TlvObject
      {
         public string Tag = string.Empty;
         public int Length = 0;
         public string Value = string.Empty;
      }
   }
}
