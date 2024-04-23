using System;
using System.Data;
using Contract;
using Impl;
using LogLineHandler;

namespace OverView
{
   class OverTable : BaseTable
   {
      string FITSwitchOnUsNextStateNumbers = string.Empty;
      string FITSwitchForeignNextStateNumbers = string.Empty;
      string FITSwitchOtherNextStateNumbers = string.Empty;

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public OverTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      /// <summary>
      /// Process one line from the merged log file. 
      /// </summary>
      /// <param name="logLine">logline from the file</param>
      public override void ProcessRow(ILogLine logLine)
      {
         try
         {
            if (logLine is APLine apLogLine)
            {
               switch (apLogLine.apType)
               {
                  /* mode */
                  case APLogType.APLOG_CURRENTMODE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField apLineField = (APLineField)apLogLine;
                           APLINE(apLineField, "mode", apLineField.field);
                        }
                        break;
                     }

                  /* host */
                  case APLogType.APLOG_HOST:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField apLineField = (APLineField)apLogLine;
                           APLINE(apLineField, "host", apLineField.field);
                        }
                        break;
                     }

                  /* headset */

                  /* card */
                  case APLogType.APLOG_CARD_OPEN:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "card", "open");
                        }
                        break;
                     }
                  case APLogType.APLOG_CARD_CLOSE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "card", "close");
                        }
                        break;
                     }
                  case APLogType.APLOG_CARD_ONMEDIAINSERTED:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "card", "inserted");
                        }
                        break;
                     }
                  case APLogType.APLOG_CARD_ONREADCOMPLETE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "card", "read");
                        }
                        break;
                     }
                  case APLogType.APLOG_CARD_ONEJECTCOMPLETE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "card", "ejected");
                        }
                        break;
                     }
                  case APLogType.APLOG_CARD_PAN:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE(lineField, "card", lineField.field);
                        }
                        break;
                     }

                  /* pin */
                  case APLogType.APLOG_PIN_OPEN:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "pin", "open");
                        }
                        break;
                     }
                  case APLogType.APLOG_PIN_CLOSE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "pin", "close");
                        }
                        break;
                     }
                  case APLogType.APLOG_PIN_ISPCI:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE(lineField, "pin", lineField.field);
                        }
                        break;
                     }
                  case APLogType.APLOG_PIN_ISTR31:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE(lineField, "pin", lineField.field);
                        }
                        break;
                     }
                  case APLogType.APLOG_PIN_ISTR34:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE(lineField, "pin", lineField.field);
                        }
                        break;
                     }
                  case APLogType.APLOG_PIN_KEYIMPORTED:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "pin", "key_imported");
                        }
                        break;
                     }
                  case APLogType.APLOG_PIN_RAND:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "pin", "rand_generated");
                        }
                        break;
                     }
                  case APLogType.APLOG_PIN_PINBLOCK:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "pin", "pinblock");
                        }
                        break;
                     }
                  case APLogType.APLOG_PIN_PINBLOCK_FAILED:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "pin", "pinblock_failed");
                        }
                        break;
                     }
                  case APLogType.APLOG_PIN_TIMEOUT:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "pin", "timeout");
                        }
                        break;
                     }
                  case APLogType.APLOG_PIN_READCOMPLETE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "pin", "read_complete");
                        }
                        break;
                     }

                  /* screen */
                  case APLogType.APLOG_DISPLAYLOAD:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE(lineField, "screen", lineField.field);
                        }
                        break;
                     }

                  case APLogType.APLOG_ADDKEY:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is AddKey)
                        {
                           AddKey addKey = (AddKey)apLogLine;
                           APLOG_ADDKEY(addKey);
                        }
                        break;
                     }

                  case APLogType.APLOG_FLW_SWITCH_FIT:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE_FLW_SWITCH_FIT(lineField);
                        }
                        break;
                     }

                  case APLogType.NDC_ATM2HOST11:
                  case APLogType.NDC_ATM2HOST12:
                  case APLogType.NDC_ATM2HOST22:
                  case APLogType.NDC_ATM2HOST23:
                  case APLogType.NDC_ATM2HOST51:
                  case APLogType.NDC_ATM2HOST61:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is Atm2Host)
                        {
                           Atm2Host atm2Host = (Atm2Host)apLogLine;
                           APLINE2(atm2Host, "state", "ATM2HOST", "comment", atm2Host.english);
                        }
                        break;
                     }

                  case APLogType.NDC_HOST2ATM1:
                  case APLogType.NDC_HOST2ATM3:
                  case APLogType.NDC_HOST2ATM4:
                  case APLogType.NDC_HOST2ATM6:
                  case APLogType.NDC_HOST2ATM7:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is Host2Atm)
                        {
                           Host2Atm host2Atm = (Host2Atm)apLogLine;
                           APLINE2(host2Atm, "state", "HOST2ATM", "comment", host2Atm.english);
                        }
                        break;
                     }

                  case APLogType.Core_DispensedAmount:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is Core_DispensedAmount)
                        {
                           Core_DispensedAmount dispenseAmount = (Core_DispensedAmount)apLogLine;
                           APLINE2(dispenseAmount, "state", dispenseAmount.name, "comment", String.Format("Dispense Amount : ${0}", dispenseAmount.amount));
                        }
                        break;
                     }

                  case APLogType.Core_ProcessWithdrawalTransaction_Account:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is Core_ProcessWithdrawalTransaction_Account)
                        {
                           Core_ProcessWithdrawalTransaction_Account transAccount = (Core_ProcessWithdrawalTransaction_Account)apLogLine;
                           APLINE2(transAccount, "state", transAccount.name, "comment", String.Format("Process WD Transaction Account : {0}", transAccount.account));
                        }
                        break;
                     }

                  case APLogType.Core_ProcessWithdrawalTransaction_Amount:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is Core_ProcessWithdrawalTransaction_Amount)
                        {
                           Core_ProcessWithdrawalTransaction_Amount transAccount = (Core_ProcessWithdrawalTransaction_Amount)apLogLine;
                           APLINE2(transAccount, "state", transAccount.name, "comment", String.Format("Process WD Transaction Amount : ${0}", transAccount.amount));
                        }
                        break;
                     }

                  /* function key */
                  case APLogType.APLOG_FUNCTIONKEY_SELECTED:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE(lineField, "functionkey", lineField.field);
                        }
                        break;
                     }
                  case APLogType.APLOG_FUNCTIONKEY_SELECTED2:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE(lineField, "functionkey", lineField.field);
                        }
                        break;
                     }


                  /* dispense */
                  case APLogType.CashDispenser_UpdateTypeInfoToDispense:
                     {
                        base.ProcessRow(apLogLine);
                        if (apLogLine is CashDispenser_UpdateTypeInfoToDispense typeInfoToDispense)
                        {
                           APLINE(apLogLine, "dispensed", String.Format("${0}", typeInfoToDispense.dispenseAmount));
                        }
                        break;
                     }

                  /* deposit */

                  default:
                     break;
               };
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("OveTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      protected void APLINE(APLine lineField, string columnName, string columnValue)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Summary"].Rows.Add();

            dataRow["file"] = lineField.LogFile;
            dataRow["time"] = lineField.Timestamp;
            dataRow["error"] = lineField.HResult;

            dataRow["mode"] = string.Empty;
            dataRow["host"] = string.Empty;
            dataRow["headset"] = string.Empty;
            dataRow["card"] = string.Empty;
            dataRow["pin"] = string.Empty;
            dataRow["screen"] = string.Empty;
            dataRow["state"] = string.Empty;
            dataRow["functionkey"] = string.Empty;
            dataRow["comment"] = string.Empty;

            dataRow[columnName] = columnValue;

            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("APLINE Exception : " + e.Message);
         }
      }

      protected void APLINE2(APLine lineField, string columnName, string columnValue, string columnName2, string columnValue2)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Summary"].Rows.Add();

            dataRow["file"] = lineField.LogFile;
            dataRow["time"] = lineField.Timestamp;
            dataRow["error"] = lineField.HResult;

            dataRow["mode"] = string.Empty;
            dataRow["host"] = string.Empty;
            dataRow["headset"] = string.Empty;
            dataRow["card"] = string.Empty;
            dataRow["pin"] = string.Empty;
            dataRow["screen"] = string.Empty;
            dataRow["state"] = string.Empty;
            dataRow["functionkey"] = string.Empty;
            dataRow["comment"] = string.Empty;

            dataRow[columnName] = columnValue;
            dataRow[columnName2] = columnValue2;

            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("APLINE2 Exception : " + e.Message);
         }
      }

      protected void APLOG_ADDKEY(AddKey addKey)
      {
         try
         {
            if (addKey.keyName == "FITSwitchOnUsNextStateNumbers")
            {
               FITSwitchOnUsNextStateNumbers = addKey.value; 
            }
            else if (addKey.keyName == "FITSwitchForeignNextStateNumbers")
            {
               FITSwitchForeignNextStateNumbers = addKey.value; 
            }
            else if (addKey.keyName == "FITSwitchOtherNextStateNumbers")
            {
               FITSwitchOtherNextStateNumbers = addKey.value; 
            }

            return;
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("APLOG_ADDKEY Exception : " + e.Message);
         }
      }

      protected void APLINE_FLW_SWITCH_FIT(APLineField lineField)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Summary"].Rows.Add();

            dataRow["file"] = lineField.LogFile;
            dataRow["time"] = lineField.Timestamp;
            dataRow["error"] = lineField.HResult;

            dataRow["mode"] = string.Empty;
            dataRow["host"] = string.Empty;
            dataRow["headset"] = string.Empty;
            dataRow["card"] = string.Empty;
            dataRow["pin"] = string.Empty;
            dataRow["screen"] = string.Empty;
            dataRow["state"] = string.Empty;
            dataRow["functionkey"] = string.Empty;
            dataRow["comment"] = string.Empty;

            if (FITSwitchForeignNextStateNumbers.Contains(lineField.field))
            {
               dataRow["card"] = "foreign";
            }
            else if (FITSwitchOnUsNextStateNumbers.Contains(lineField.field))
            {
               dataRow["card"] = "on-us";
            }
            else if (FITSwitchOtherNextStateNumbers.Contains(lineField.field))
            {
               dataRow["card"] = "other";
            }
            else 
            {
               dataRow["card"] = lineField.field;
            }

            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("APLINE2 Exception : " + e.Message);
         }
      }
   }
}
