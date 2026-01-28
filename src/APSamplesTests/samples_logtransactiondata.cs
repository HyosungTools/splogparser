using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APSamples
{
   public class samples_logtransactiondata
   {
      /* LogTransactionData - Pipe Delimited Format (WebService cores) */

      /* CMCFlex - PinAuthentication (minimal payload) */
      public const string LogTransactionData_PipeDelimited_CMCFlex_1 =
      @"[2026-01-20 21:39:17-677] [v02.02.05.01] [CMCFlexWebServiceRequestFlowPoint.LogTransactionData] [TID:35] CurrentSession.OnUs - True | CurrentTransaction.Message - PinAuthentication | CurrentTransaction.Type - CustomerIdentification | CurrentSession.CoreAvailable - True | CurrentSession.NDCAvailable - True | TransactionInfo.Track2 - ;************5774=****************0000? | CurrentSession.IdentificationNumber - ************5774 | TransactionFlag.IsContactlessTransaction - False | CurrentSession.Language - English
";

      /* CMCFlex - Withdrawal (full payload) */
      public const string LogTransactionData_PipeDelimited_CMCFlex_2 =
      @"[2026-01-20 21:39:48-843] [v02.02.05.01] [CMCFlexWebServiceRequestFlowPoint.LogTransactionData] [TID:35] CurrentSession.OnUs - True | CurrentTransaction.AccountKeySelect - 16 | CurrentTransaction.AccountNumber - *****9856 | CurrentTransaction.AccountType - SD | CurrentTransaction.Amount - 1000 | CurrentTransaction.BillMixTotalAmount - 1000 | CurrentTransaction.Message - Withdrawal | CurrentTransaction.Type - Withdrawal | CurrentSession.CoreAvailable - True | CurrentSession.NDCAvailable - True | TransactionInfo.Track2 - ;************5774=****************0000? | CurrentSession.IdentificationNumber - ************5774 | TransactionFlag.IsContactlessTransaction - False | CurrentTransaction.CustomBillMixBuffer - 002000000000 | CurrentSession.Language - English
";

      /* SymXchange - No TID */
      public const string LogTransactionData_PipeDelimited_SymXchange_1 =
      @"[2025-06-24 17:43:42-370] [SymXchangeWebServiceRequestFlowPoint.LogTransactionData] CurrentSession.OnUs - True | CurrentTransaction.Message - BalanceInquiry | CurrentTransaction.Type - BalanceInquiry | CurrentSession.CoreAvailable - True | CurrentSession.NDCAvailable - True | CurrentSession.IdentificationNumber - XXXXXX2603 | TransactionFlag.IsContactlessTransaction - False | CurrentSession.Language - English
";

      /* LogTransactionData - JSON Format (non-WebService FlowPoints - FlowPoint should be empty) */

      /* StateWrapper - Idle (minimal JSON) */
      public const string LogTransactionData_Json_StateWrapper_Idle =
      @"[2026-01-02 02:59:58-034] [v02.02.06.03] [StateWrapperFlowPoint.LogTransactionData] [TID:28] [FLOWPOINT] { ""fp"": ""Common-Idle"", ""TransactionFlag"": { ""IsContactlessTransaction"": false }, ""CurrentSession"": { ""Language"": ""English"" } }
";

      /* Standard - PreBeginICCInitForDip */
      public const string LogTransactionData_Json_Standard_PreBeginICCInit =
      @"[2026-01-02 09:13:09-278] [v02.02.06.03] [StandardFlowPoint.LogTransactionData] [TID:33] [FLOWPOINT] { ""fp"": ""Common-PreBeginICCInitForDip"", ""TransactionFlag"": { ""IsContactlessTransaction"": false }, ""CurrentSession"": { ""Language"": ""English"" } }
";

      /* StateWrapper - CompleteICCAppSel with Track2 */
      public const string LogTransactionData_Json_StateWrapper_CompleteICCAppSel =
      @"[2026-01-02 09:13:13-516] [v02.02.06.03] [StateWrapperFlowPoint.LogTransactionData] [TID:33] [FLOWPOINT] { ""fp"": ""Common-CompleteICCAppSel"", ""TransactionInfo"": { ""Track2"": "";************5548=****************0000?"" }, ""TransactionFlag"": { ""IsContactlessTransaction"": false }, ""CurrentSession"": { ""Language"": ""English"" } }
";

      /* Standard - PLACEHOLDER with session info */
      public const string LogTransactionData_Json_Standard_Placeholder =
      @"[2026-01-02 09:13:13-548] [v02.02.06.03] [StateWrapperFlowPoint.LogTransactionData] [TID:33] [FLOWPOINT] { ""fp"": ""PLACEHOLDER-PostInsertCardFITMatch"", ""CurrentSession"": { ""OnUs"": false, ""Language"": ""English"" }, ""TransactionInfo"": { ""Track2"": "";************5548=****************0000?"" }, ""TransactionFlag"": { ""IsContactlessTransaction"": false } }
";

      /* Standard - DetermineAuthentication with full session */
      public const string LogTransactionData_Json_Standard_DetermineAuth =
      @"[2026-01-02 09:13:13-604] [v02.02.06.03] [StandardFlowPoint.LogTransactionData] [TID:33] [FLOWPOINT] { ""fp"": ""Common-DetermineAuthentication"", ""CurrentSession"": { ""OnUs"": false, ""CoreAvailable"": false, ""NDCAvailable"": false, ""IdentificationNumber"": ""************5548"", ""Language"": ""English"" }, ""TransactionInfo"": { ""Track2"": "";************5548=****************0000?"" }, ""TransactionFlag"": { ""IsContactlessTransaction"": false } }
";

      /* StandardKeyEntry - AccountSelection with CurrentTransaction */
      public const string LogTransactionData_Json_StandardKeyEntry_AccountSelection =
      @"[2026-01-02 09:13:32-224] [v02.02.06.03] [StandardKeyEntryFlowPoint.LogTransactionData] [TID:33] [FLOWPOINT] { ""fp"": ""Withdrawal-AccountSelection"", ""CurrentSession"": { ""OnUs"": false, ""CoreAvailable"": false, ""NDCAvailable"": true, ""IdentificationNumber"": ""************5548"", ""Language"": ""English"" }, ""CurrentTransaction"": { ""AccountKeySelect"": ""A"", ""AccountType"": ""Checking"", ""Type"": ""Withdrawal"", ""MultipleAccountList"": ""Checking~Checking~~A~~~"", ""SelectedAccount"": ""Checking~Checking~~A~~~"" }, ""TransactionInfo"": { ""Track2"": "";************5548=****************0000?"" }, ""TransactionFlag"": { ""IsContactlessTransaction"": false } }
";

      /* StandardKeyEntry - EnterAmount with Amount */
      public const string LogTransactionData_Json_StandardKeyEntry_EnterAmount =
      @"[2026-01-02 09:13:56-535] [v02.02.06.03] [StandardKeyEntryFlowPoint.LogTransactionData] [TID:33] [FLOWPOINT] { ""fp"": ""Withdrawal-EnterAmount"", ""CurrentSession"": { ""OnUs"": false, ""CoreAvailable"": false, ""NDCAvailable"": true, ""IdentificationNumber"": ""************5548"", ""Language"": ""English"" }, ""CurrentTransaction"": { ""AccountKeySelect"": ""A"", ""AccountType"": ""Checking"", ""Amount"": 50000, ""Type"": ""Withdrawal"", ""MultipleAccountList"": ""Checking~Checking~~A~~~"", ""SelectedAccount"": ""Checking~Checking~~A~~~"" }, ""TransactionInfo"": { ""Track2"": "";************5548=****************0000?"" }, ""TransactionFlag"": { ""IsContactlessTransaction"": false } }
";

      /* BeginICCAppSelectionLocal - BeginICCAppSel */
      public const string LogTransactionData_Json_BeginICCAppSelectionLocal =
      @"[2026-01-02 09:13:11-094] [v02.02.06.03] [BeginICCAppSelectionLocalFlowPoint.LogTransactionData] [TID:33] [FLOWPOINT] { ""fp"": ""Common-BeginICCAppSel"", ""TransactionFlag"": { ""IsContactlessTransaction"": false }, ""CurrentSession"": { ""Language"": ""English"" } }
";

      /* ValidateTransactionData - PreTransactionRequest */
      public const string LogTransactionData_Json_ValidateTransactionData =
      @"[2026-01-02 09:14:02-248] [v02.02.06.03] [ValidateTransactionDataFlowPoint.LogTransactionData] [TID:33] [FLOWPOINT] { ""fp"": ""Common-PreTransactionRequest"", ""CurrentSession"": { ""OnUs"": false, ""CoreAvailable"": false, ""NDCAvailable"": true, ""IdentificationNumber"": ""************5548"", ""Language"": ""English"" }, ""CurrentTransaction"": { ""AccountKeySelect"": ""A"", ""AccountType"": ""Checking"", ""Amount"": 50000, ""BillMixTotalAmount"": 50000, ""Type"": ""Withdrawal"", ""CustomBillMixBuffer"": 4004008, ""MultipleAccountList"": ""Checking~Checking~~A~~~"", ""SelectedAccount"": ""Checking~Checking~~A~~~"" }, ""TransactionInfo"": { ""Track2"": "";************5548=****************0000?"" }, ""TransactionFlag"": { ""IsContactlessTransaction"": false } }
";
   }
}
