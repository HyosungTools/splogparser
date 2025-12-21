using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class CIMLogicalUnit : SPFlatLine
   {
      string logicalUnit;
      string logicalValue;


      public CIMLogicalUnit(ILogFileHandler handler, string line, SPFlatType flatType) : base(handler, line, flatType)
      {
      }

      protected override void Initialize()
      {
         /*
CIM_LogicalUnit,
CIM_LogicalUnit_InitialCount,
CIM_LogicalUnit_RejectCount,
CIM_LogicalUnit_RetractedCount,
CIM_LogicalUnit_DispensedCount,
CIM_LogicalUnit_PresentedCount,
CIM_LogicalUnit_TotalCount,
CIM_LogicalUnit_MaximumCount,
CIM_LogicalUnit_CashInCount,

CIM_LogicalUnit_Type,
CIM_LogicalUnit_Status,
CIM_LogicalUnit_Number,
CIM_LogicalUnit_UnitID,
CIM_LogicalUnit_CurrencyID,
CIM_LogicalUnit_NumberOfItems,
CIM_LogicalUnit_NumberOfPCU,
*/
         try
         {
            base.Initialize();
            string pattern = string.Empty;

            switch (flatType)
            {
               case SPFlatType.CIM_LogicalUnit_InitialCount:
                  pattern = @"LogicalUnit\[(\d+)\]\.InitialCount\[([^\]]*)\]";
                  break;

               case SPFlatType.CIM_LogicalUnit_RejectCount:
                  pattern = @"LogicalUnit\[(\d+)\]\.RejectCount\[([^\]]*)\]";
                  break;

               case SPFlatType.CIM_LogicalUnit_RetractedCount:
                  pattern = @"LogicalUnit\[(\d+)\]\.RetractedCount\[([^\]]*)\]";
                  break;

               case SPFlatType.CIM_LogicalUnit_DispensedCount:
                  pattern = @"LogicalUnit\[(\d+)\]\.DispensedCount\[([^\]]*)\]";
                  break;

               case SPFlatType.CIM_LogicalUnit_PresentedCount:
                  pattern = @"LogicalUnit\[(\d+)\]\.PresentedCount\[([^\]]*)\]";
                  break;

               case SPFlatType.CIM_LogicalUnit_TotalCount:
                  pattern = @"LogicalUnit\[(\d+)\]\.TotalCount\[([^\]]*)\]";
                  break;

               case SPFlatType.CIM_LogicalUnit_MaximumCount:
                  pattern = @"LogicalUnit\[(\d+)\]\.MaximumCount\[([^\]]*)\]";
                  break;

               case SPFlatType.CIM_LogicalUnit_CashInCount:
                  pattern = @"LogicalUnit\[(\d+)\]\.CashInCount\[([^\]]*)\]";
                  break; 

               // 8/20/2025 6:04:26 PM : 2025/07/04001200:13 58.7660008PROPERTY0025Ctrl::GetLogicalUnit.Type0032LogicalUnit[1].Type[CDMSPECIFIC]01354294967295013001584828210003CIM0007ACTIVEX0010
               case SPFlatType.CIM_LogicalUnit_Type:
                  pattern = @"LogicalUnit\[(\d+)\]\.Type\[([^\]]*)\]";
                  break;

               // 
               case SPFlatType.CIM_LogicalUnit_Status:
                  pattern = @"LogicalUnit\[(\d+)\]\.Status\[([^\]]*)\]";
                  break;

               case SPFlatType.CIM_LogicalUnit_Number:
                  pattern = @"LogicalUnit\[(\d+)\]\.Number\[([^\]]*)\]";
                  break;

               case SPFlatType.CIM_LogicalUnit_UnitID:
                  pattern = @"LogicalUnit\[(\d+)\]\.UnitID\[([^\]]*)\]";
                  break;

               case SPFlatType.CIM_LogicalUnit_CurrencyID:
                  pattern = @"LogicalUnit\[(\d+)\]\.CurrencyID\[([^\]]*)\]";
                  break;

               case SPFlatType.CIM_LogicalUnit_NumberOfItems:
                  pattern = @"LogicalUnit\[(\d+)\]\.NumberOfItems\[([^\]]*)\]";
                  break;

               case SPFlatType.CIM_LogicalUnit_NumberOfPCU:
                  pattern = @"LogicalUnit\[(\d+)\]\.NumberOfPCU\[([^\]]*)\]";
                  break;

               default:
                  break;
            }

            Match match = Regex.Match(logLine, pattern);
            if (match.Success)
            {
               logicalUnit = match.Groups[1].Value;
               logicalValue = match.Groups[2].Value;
               Console.WriteLine($"Logical Unit Number: {logicalUnit}");
               Console.WriteLine($"Type: {logicalValue}");
            }
            else
            {
               Console.WriteLine("No match found.");
            }

         }
         catch (Exception e)
         {
            Console.WriteLine("Exception in Initialize()");
         }
      }

      public static ILogLine LUFactory(ILogFileHandler handler, string logLine)
      {

         if (logLine.Contains("].InitialCount["))
            return new CIMLogicalUnit(handler, logLine, SPFlatType.CIM_LogicalUnit_InitialCount);

         if (logLine.Contains("].RejectCount["))
            return new CIMLogicalUnit(handler, logLine, SPFlatType.CIM_LogicalUnit_RejectCount);

         if (logLine.Contains("].RetractedCount["))
            return new CIMLogicalUnit(handler, logLine, SPFlatType.CIM_LogicalUnit_RetractedCount);

         if (logLine.Contains("].DispensedCount["))
            return new CIMLogicalUnit(handler, logLine, SPFlatType.CIM_LogicalUnit_DispensedCount);

         if (logLine.Contains("].PresentedCount["))
            return new CIMLogicalUnit(handler, logLine, SPFlatType.CIM_LogicalUnit_PresentedCount);

         if (logLine.Contains("].TotalCount["))
            return new CIMLogicalUnit(handler, logLine, SPFlatType.CIM_LogicalUnit_TotalCount);

         if (logLine.Contains("].MaximumCount["))
            return new CIMLogicalUnit(handler, logLine, SPFlatType.CIM_LogicalUnit_MaximumCount);

         if (logLine.Contains("].CashInCount["))
            return new CIMLogicalUnit(handler, logLine, SPFlatType.CIM_LogicalUnit_CashInCount);

         if (logLine.Contains("].Type["))
            return new CIMLogicalUnit(handler, logLine, SPFlatType.CIM_LogicalUnit_Type);

         if (logLine.Contains("].Status["))
            return new CIMLogicalUnit(handler, logLine, SPFlatType.CIM_LogicalUnit_Status);

         if (logLine.Contains("].Number["))
            return new CIMLogicalUnit(handler, logLine, SPFlatType.CIM_LogicalUnit_Number);

         if (logLine.Contains("].UnitID["))
            return new CIMLogicalUnit(handler, logLine, SPFlatType.CIM_LogicalUnit_UnitID);

         if (logLine.Contains("].CurrencyID["))
            return new CIMLogicalUnit(handler, logLine, SPFlatType.CIM_LogicalUnit_CurrencyID);

         if (logLine.Contains("].NumberOfItems["))
            return new CIMLogicalUnit(handler, logLine, SPFlatType.CIM_LogicalUnit_NumberOfItems);

         if (logLine.Contains("].NumberOfPCU["))
            return new CIMLogicalUnit(handler, logLine, SPFlatType.CIM_LogicalUnit_NumberOfPCU);


         return null;
      }
   }
}

