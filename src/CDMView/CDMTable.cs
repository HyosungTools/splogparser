using Contract;
using Impl;
using System;
using System.Collections.Generic;
using System.Data;

namespace CDMView
{

   internal class CDMTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public CDMTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
         // for our view we want '0' to render as ' ' in the worksheet
         _zeroAsBlank = false;
      }

      /// <summary>
      /// Process one line from the merged log file. 
      /// </summary>
      /// <param name="logLine">logline from the file</param>
      public override void ProcessRow(string traceFile, string logLine)
      {
         try
         {
            (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);

            switch (result.xfsType)
            {
               case XFSType.WFS_INF_CDM_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_IN_CDM_STATUS(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_CDM_CASH_UNIT_INFO:
                  {
                     WFS_INF_CDM_CASH_UNIT_INFO(result.xfsLine);
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_INF_CDM_CASH_UNIT_INFO");
                     break;
                  }
               case XFSType.WFS_CMD_CDM_DISPENSE:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_CMD_CDM_DISPENSE");
                     break;
                  }
               case XFSType.WFS_CMD_CDM_PRESENT:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_CMD_CDM_PRESENT");
                     break;
                  }
               case XFSType.WFS_CMD_CDM_REJECT:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_CMD_CDM_REJECT");
                     break;
                  }
               case XFSType.WFS_CMD_CDM_RETRACT:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_CMD_CDM_RETRACT");
                     break;
                  }
               case XFSType.WFS_CMD_CDM_RESET:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_CMD_CDM_RESET");
                     break;
                  }
               case XFSType.WFS_SRVE_CDM_CASHUNITINFOCHANGED:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_SRVE_CDM_CASHUNITINFOCHANGED");
                     break;
                  }
               case XFSType.WFS_SRVE_CDM_ITEMSTAKEN:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_SRVE_CDM_ITEMSTAKEN");
                     break;
                  }
               case XFSType.WFS_USRE_CIM_CASHUNITTHRESHOLD:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_USRE_CIM_CASHUNITTHRESHOLD");
                     break;
                  }
               case XFSType.WFS_SRVE_CIM_CASHUNITINFOCHANGED:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_SRVE_CIM_CASHUNITINFOCHANGED");
                     break;
                  }
               case XFSType.WFS_SRVE_CIM_ITEMSTAKEN:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_SRVE_CIM_ITEMSTAKEN");
                     break;
                  }
               case XFSType.WFS_EXEE_CIM_INPUTREFUSE:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_EXEE_CIM_INPUTREFUSE");
                     break;
                  }
               default:
                  break;

            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("CDMTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      /// <summary>
      /// Prep the tables for Excel 
      /// </summary>
      /// <returns>true if the write was successful</returns>
      public override bool WriteExcelFile()
      {
         //STATUS TABLE

         // sort the table by time, visit every row and delete rows that are unchanged from their predecessor
         ctx.ConsoleWriteLogLine("Compress the Status Table: sort by time, visit every row and delete rows that are unchanged from their predecessor");
         ctx.ConsoleWriteLogLine(String.Format("Compress the Status Table start: rows before: {0}", dTableSet.Tables["Status"].Rows.Count));

         // the list of columns to compare
         string[] columns = new string[] { "error", "status", "dispenser", "intstack", "shutter", "posstatus", "transport", "transstat", "position" };
         (bool success, string message) result = DataTableOps.DeleteUnchangedRowsInTable(dTableSet.Tables["Status"], "time ASC", columns);
         if (!result.success)
         {
            ctx.ConsoleWriteLogLine("Unexpected error during table compression : " + result.message);
         }
         ctx.ConsoleWriteLogLine(String.Format("Compress the Status Table complete: rows after: {0}", dTableSet.Tables["Status"].Rows.Count));

         // add English
         string[,] colKeyMap = new string[8, 2]
         {
            {"status", "fwDevice" },
            {"dispenser", "fwDispenser"},
            {"intstack", "fwIntermediateStacker"},
            {"shutter", "fwShutter"},
            {"posstatus", "fwPositionStatus"},
            {"transport", "fwTransport"},
            {"transstat", "fwTransportStatus"},
            {"position", "wDevicePosition"}
         };

         for (int i = 0; i < 8; i++)
         {
            result = DataTableOps.AddEnglishToTable(dTableSet.Tables["Status"], dTableSet.Tables["Messages"], colKeyMap[i, 0], colKeyMap[i, 1]);
         }

         return base.WriteExcelFile();
      }

      protected (bool success, DataRow dataRow) FindMessages(string type, string code)
      {
         // Create an array for the key values to find.
         object[] findByKeys = new object[2];

         // Set the values of the keys to find.
         findByKeys[0] = type;
         findByKeys[1] = code;

         DataRow foundRow = dTableSet.Tables["Messages"].Rows.Find(findByKeys);
         if (foundRow != null)
         {
            return (true, foundRow);
         }
         else
         {
            return (false, null);
         }
      }
      protected void WFS_IN_CDM_STATUS(string xfsLine)
      {
         try
         {
            DataRow newRow = dTableSet.Tables["Status"].NewRow();

            newRow["file"] = _traceFile;
            newRow["time"] = lpResult.tsTimestamp(xfsLine);
            newRow["error"] = lpResult.hResult(xfsLine);

            (bool success, string xfsMatch, string subLogLine) result;

            // fwDevice
            result = _wfs_inf_cdm_status.fwDevice(xfsLine);
            if (result.success) newRow["status"] = result.xfsMatch.Trim();

            // fwDispenser
            result = _wfs_inf_cdm_status.fwDispenser(result.subLogLine);
            if (result.success) newRow["dispenser"] = result.xfsMatch.Trim();

            // fwIntermediateStacker
            result = _wfs_inf_cdm_status.fwIntermediateStacker(result.subLogLine);
            if (result.success) newRow["intstack"] = result.xfsMatch.Trim();

            // fwShutter
            result = _wfs_inf_cdm_status.fwShutter(result.subLogLine);
            if (result.success) newRow["shutter"] = result.xfsMatch.Trim();

            // fwPositionStatus
            result = _wfs_inf_cdm_status.fwPositionStatus(result.subLogLine);
            if (result.success) newRow["posstatus"] = result.xfsMatch.Trim();

            // fwTransport
            result = _wfs_inf_cdm_status.fwTransport(result.subLogLine);
            if (result.success) newRow["transport"] = result.xfsMatch.Trim();

            // fwTransportStatus
            result = _wfs_inf_cdm_status.fwTransportStatus(result.subLogLine);
            if (result.success) newRow["transstat"] = result.xfsMatch.Trim();

            // wDevicePosition
            result = _wfs_inf_cdm_status.wDevicePosition(result.subLogLine);
            if (result.success) newRow["position"] = result.xfsMatch.Trim();

            dTableSet.Tables["Status"].Rows.Add(newRow);
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("Exception : " + e.Message);
         }

         return;
      }

          //    <xs:element name = "Summary" >
          //< xs:complexType>
          //  <xs:sequence>
          //    <xs:element name = "file" type="xs:string" minOccurs="0" />
          //    <xs:element name = "time" type="xs:string" minOccurs="0" />
          //    <xs:element name = "error" type="xs:string" minOccurs="0" />
          //    <xs:element name = "number" type="xs:string" minOccurs="0" />
          //    <xs:element name = "type" type="xs:string" minOccurs="0" />
          //    <xs:element name = "name" type="xs:string" minOccurs="0" />
          //    <xs:element name = "currency" type="xs:string" minOccurs="0" />
          //    <xs:element name = "denom" type="xs:string" minOccurs="0" />
          //    <xs:element name = "initial" type="xs:string" minOccurs="0" />
          //    <xs:element name = "min" type="xs:string" minOccurs="0" />
          //    <xs:element name = "max" type="xs:string" minOccurs="0" />
          //  </xs:sequence>
          //</xs:complexType>

          //    <xs:element name = "CashUnit" >
          //< xs:complexType>
          //  <xs:sequence>
          //    <xs:element name = "file" type="xs:string" minOccurs="0" />
          //    <xs:element name = "time" type="xs:string" minOccurs="0" />
          //    <xs:element name = "error" type="xs:string" minOccurs="0" />
          //    <xs:element name = "number" type="xs:string" minOccurs="0" />
          //    <xs:element name = "count" type="xs:string" minOccurs="0" />
          //    <xs:element name = "reject" type="xs:string" minOccurs="0" />
          //    <xs:element name = "status" type="xs:string" minOccurs="0" />
          //    <xs:element name = "dispensed" type="xs:string" minOccurs="0" />
          //    <xs:element name = "presented" type="xs:string" minOccurs="0" />
          //    <xs:element name = "retracted" type="xs:string" minOccurs="0" />
          //    <xs:element name = "comment" type="xs:string" minOccurs="0" />
          //  </xs:sequence>
      protected void WFS_INF_CDM_CASH_UNIT_INFO(string xfsLine)
      {
         try
         {
            (bool success, string xfsMatch, string subLogLine) result;

            // there are two styles of log lines. If the log line contains 'lppList->' expect
            // a table of data. If the log lines contains 'lppList =' expect a list
            if (xfsLine.Contains("lppList->"))
            {
               // isolate count
               result = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine);
               if (!result.success)
               {
                  ctx.ConsoleWriteLogLine("Failed to isolate count from WFS_INF_CDM_CASH_UNIT_INFO message");
                  return;
               }

               int lUnitCount = int.Parse(result.xfsMatch.Trim());

               DataRow[] dataRowArr = new DataRow[lUnitCount];

               for (int i = 0; i < lUnitCount; i++)
               {
                  dataRowArr[i]["file"] = _traceFile;
                  dataRowArr[i]["time"] = lpResult.tsTimestamp(xfsLine);
                  dataRowArr[i]["error"] = lpResult.hResult(xfsLine);

                  dataRowArr[i]["number"] = i.ToString(); 
               }

               (bool success, string[] xfsMatch, string subLogLine) results;
               results = _wfs_inf_cdm_cash_unit_info.usType(xfsLine);


            }
               else if (xfsLine.Contains("lppList ="))
            {
               // isolate count
               result = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine);
               if (!result.success)
               {
                  ctx.ConsoleWriteLogLine("Failed to isolate count from WFS_INF_CDM_CASH_UNIT_INFO message");
                  return;
               }

               int lUnitCount = int.Parse(result.xfsMatch.Trim());

               for (int i = 0; i < lUnitCount; i++)
               {
                  DataRow newRow = dTableSet.Tables["Status"].NewRow();

                  newRow["file"] = _traceFile;
                  newRow["time"] = lpResult.tsTimestamp(xfsLine);
                  newRow["error"] = lpResult.hResult(xfsLine);

                  newRow["number"] = i.ToString();
               }
            }


         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("Exception : " + e.Message);
         }
      }
   }
}
