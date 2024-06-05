using System;
using Contract;

namespace LogLineHandler
{
   public class Atm2Host11 : Atm2Host
   {
      public string amount = string.Empty;

      public Atm2Host11(ILogFileHandler parent, string logLine, APLogType apType = APLogType.NDC_ATM2HOST11) : base(parent, logLine, apType)
      {
         msgclass = "1";
         msgsubclass = "1";
      }

      protected override void Initialize()
      {
         base.Initialize();

         english = string.Empty;

         try
         {
            // Field Size  Man/Opt  Description
            // a     Var   M        Header - protocol dependent
            // b     1     M        Message Class : '1' - Unsolicited message
            // c     1     M        Message Sub-Class : '1' -Transaction Request
            (bool success, string field, string subMessage) result = NDC.GetNextFieldBySeparator(ndcmsg);
            if (!result.success)
               return;

            english = "Transaction Request to Host, ";

            // d     3/9      M     Logical Unit Number - 
            // FS    1        M
            // FS    1        M
            // e     8        ?     Time Variant - only present if Data Security selected
            // FS    1        M
            // f     1        M     Top of Receipt Trans Flag '0' - will not print '1' will print
            // g     1        M     Message Coord Num
            // FS    1        M

            // some fields may not be present
            // continue to read fields until we have one that is exactly 2 wide - this will be 'f' and 'g'
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            while (result.field.Length != 2)
            {
               result = NDC.GetNextFieldBySeparator(result.subMessage);
               if (!result.success)
                  return;
            }

            int value = result.field[1] - '0';
            english = english + String.Format(" co-ord number '{0}', ", result.field[1] - '0');

            // fields ‘h’ to ‘n’ are optional

            // h     Var(39)  M     Track 2 Data
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return;

            // i     Var(106) M     Track 3 Data
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return;

            // j     8        M     Operation Code Data
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return;

            // interpret opcode - Optional
            if (!string.IsNullOrEmpty(result.field))
               english = english + String.Format(" opcode '{0}', ", result.field);

            // k     8/12     ?     Amount Entry
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return;

            if (!string.IsNullOrEmpty(result.field))
            {
               if (int.Parse(result.field) == 0)
               {
                  //English = English + String.Format(" amount $0.00, ");
               }
               else
               {
                  amount = (Convert.ToDecimal(result.field) / 100).ToString();
                  english = english + String.Format(" amount ${0}, ", amount);
               }
            }

            // l     Var(32)  ?     PIN Buffer (Buffer A)
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return;

            // m     Var(32)  ?     General Purpose Buffer B
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return;

            // n     Var(32)  ?     General Purpose Buffer C
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return;

            // o     1        O     Track 1 Identifier
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return;

            // p     Var(78)  O     Track 1 Data
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return;

            // Fields ‘q’ and ‘r’ and the preceding field separator are
            // present only if the download option selects them

            // q     1        ?     Transaction State Data Identifier
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return;

            // if the field is '2' field 'r' is Last Transaction Status Data
            if (result.field.Length != 1 && result.field[0] != '2')
               return;

            // r     Var(71)  ?     Last Transaction Status Data
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return;

            english = english + String.Format("TODO Last Transaction Status Data: '{0}", result.field);

         }
         catch (Exception e)
         {
            this.parentHandler.ctx.ConsoleWriteLogLine(String.Format("{0} Unexpected parse in message : {1}, {2}", myName, ndcmsg, e.Message));
         }

         return;
      }
   }
}
