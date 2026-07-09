using System;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   /// <summary>
   /// Parses per-property CIM logical unit lines (FI-style flat logs), one field per line:
   ///
   ///    PROPERTY Ctrl::GetLogicalUnit.Type  LogicalUnit[1].Type[CDMSPECIFIC]
   ///    PROPERTY Ctrl::GetLogicalUnit.InitialCount  LogicalUnit[1].InitialCount[3000]
   ///
   /// DieboldNixdorf flat logs use the consolidated CLogicalUnit::TraceCIMCashUnitInfo
   /// format instead - see CIMCashUnitTrace. The SPFlatLine Factory routes each shape
   /// to the right class.
   /// </summary>
   public class CIMLogicalUnit : SPFlatLine
   {
      /// <summary>The logical unit index n from LogicalUnit[n].*, or empty if not found.</summary>
      public string LogicalUnit { get; private set; }

      /// <summary>The extracted field value, or empty if not found.</summary>
      public string LogicalValue { get; private set; }

      public CIMLogicalUnit(ILogFileHandler handler, string line, SPFlatType flatType) : base(handler, line, flatType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         LogicalUnit = string.Empty;
         LogicalValue = string.Empty;

         string fieldName = string.Empty;

         switch (flatType)
         {
            case SPFlatType.CIM_LogicalUnit_InitialCount:
               fieldName = "InitialCount";
               break;

            case SPFlatType.CIM_LogicalUnit_RejectCount:
               fieldName = "RejectCount";
               break;

            case SPFlatType.CIM_LogicalUnit_RetractedCount:
               fieldName = "RetractedCount";
               break;

            case SPFlatType.CIM_LogicalUnit_DispensedCount:
               fieldName = "DispensedCount";
               break;

            case SPFlatType.CIM_LogicalUnit_PresentedCount:
               fieldName = "PresentedCount";
               break;

            case SPFlatType.CIM_LogicalUnit_TotalCount:
               fieldName = "TotalCount";
               break;

            case SPFlatType.CIM_LogicalUnit_MaximumCount:
               fieldName = "MaximumCount";
               break;

            case SPFlatType.CIM_LogicalUnit_CashInCount:
               fieldName = "CashInCount";
               break;

            case SPFlatType.CIM_LogicalUnit_Type:
               fieldName = "Type";
               break;

            case SPFlatType.CIM_LogicalUnit_Status:
               fieldName = "Status";
               break;

            case SPFlatType.CIM_LogicalUnit_Number:
               fieldName = "Number";
               break;

            case SPFlatType.CIM_LogicalUnit_UnitID:
               fieldName = "UnitID";
               break;

            case SPFlatType.CIM_LogicalUnit_CurrencyID:
               fieldName = "CurrencyID";
               break;

            case SPFlatType.CIM_LogicalUnit_NumberOfItems:
               fieldName = "NumberOfItems";
               break;

            case SPFlatType.CIM_LogicalUnit_NumberOfPCU:
               fieldName = "NumberOfPCU";
               break;

            default:
               break;
         }

         if (fieldName.Length == 0)
         {
            return;
         }

         Match match = Regex.Match(logLine, @"LogicalUnit\[(\d+)\]\." + fieldName + @"\[([^\]]*)\]");
         if (match.Success)
         {
            LogicalUnit = match.Groups[1].Value;
            LogicalValue = match.Groups[2].Value;
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
