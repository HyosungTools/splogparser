using System;
using Contract;

namespace LogLineHandler
{
   public class Atm2Host23 : Atm2Host
   {
      public Atm2Host23(ILogFileHandler parent, string logLine, APLogType apType = APLogType.NDC_ATM2HOST23) : base(parent, logLine, apType)
      {
         msgclass = "2";
         msgsubclass = "3";
      }

      protected override void Initialize()
      {
         base.Initialize();

         english = string.Empty;

         try
         {
            // Field b and c - Message Class / Sub-Class - 23
            (bool success, string field, string subMessage) result = GetNextFieldBySeparator(ndcmsg);
            if (!result.success)
               return;

            english = "Solicited Status to Host (Encryptor Initialisation Data), ";

            // Field d - LUNO
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return;

            english = english + String.Format("LUNO '{0}', ", result.field);

            // Field - none - two FS back to back
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return;

            // Field e - Information Identifier
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success || result.field.Length == 0)
               return;

            string infoId = result.field.Substring(0, 1);
            english = english + String.Format("Information Identifier : {0}, ", InfoIdentifierInEnglish(infoId));

            // Field f - Encryptor Information (format depends on the identifier).
            // Note: GetNextFieldBySeparator returns success=false for the LAST
            // field (no trailing separator) but still returns the field text,
            // so guard on field content, not success.
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (result.field.Length > 0)
            {
               if (infoId == "6")
               {
                  // Key Entry Mode - a single meaningful character
                  english = english + String.Format("Key Entry Mode : {0}, ", KeyEntryModeInEnglish(result.field.Substring(0, 1)));
               }
               else if (infoId == "1" || infoId == "2")
               {
                  // EPP serial/public key signatures - long base-94 blobs, not useful in a summary
                  english = english + "Signature data present, ";
               }
               else
               {
                  // KVVs, key status, capabilities, etc. - short and diagnostically useful
                  english = english + String.Format("Data : {0}, ", result.field);
               }
            }
         }
         catch (Exception e)
         {
            this.parentHandler.ctx.ConsoleWriteLogLine(String.Format("{0} Unexpected parse in message : {1}, {2}", myName, ndcmsg, e.Message));
         }

         return;
      }

      // see p 9-66 - Encryptor Initialisation Data, Information Identifier (field e)
      private string InfoIdentifierInEnglish(string infoId)
      {
         if (infoId == "1")
         {
            return "1 (EPP Serial Number and Signature)";
         }
         if (infoId == "2")
         {
            return "2 (EPP Public Key and Signature)";
         }
         if (infoId == "3")
         {
            return "3 (New Key Verification Value for key just loaded or reactivated)";
         }
         if (infoId == "4")
         {
            return "4 (Keys Status)";
         }
         if (infoId == "5")
         {
            return "5 (Key Loaded)";
         }
         if (infoId == "6")
         {
            return "6 (Key Entry Mode)";
         }
         if (infoId == "7")
         {
            return "7 (RSA encryption KVV)";
         }
         if (infoId == "9")
         {
            return "9 (ATM random number)";
         }
         if (infoId == "B")
         {
            return "B (Encryptor capabilities and state)";
         }
         if (infoId == "C")
         {
            return "C (Key deleted)";
         }
         return infoId;
      }

      // see p 9-67 - Key Entry Mode, valid when Information Identifier = '6'
      private string KeyEntryModeInEnglish(string mode)
      {
         if (mode == "1")
         {
            return "1 (Single length without XOR)";
         }
         if (mode == "2")
         {
            return "2 (Single length with XOR)";
         }
         if (mode == "3")
         {
            return "3 (Double length with XOR)";
         }
         if (mode == "4")
         {
            return "4 (Double length, restricted)";
         }
         return mode;
      }
   }
}
