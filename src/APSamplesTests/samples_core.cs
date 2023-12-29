using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APSamples
{
   public class samples_core
   {
      /* CORE */


      /* FiservDNA */

      public const string Core_ProcessWithdrawalTransaction_Amount_1 =
      @"[2023-06-07 12:18:15-179][3][][FiservDNAWebServiceRequestFlowPoint][ProcessWithdrawalTransaction][NORMAL]Total Amount: 500
";
      public const string Core_ProcessWithdrawalTransaction_Account_1 =
      @"[2023-11-16 11:09:19-900][3][][FiservDNAWebServiceRequestFlowPoint][ProcessWithdrawalTransaction][NORMAL]Account Number: XXX5754
";
      public const string Core_RequiredBillMixList_1 =
      @"[2023-06-07 12:18:15-596][3][][FiservDNAWebServiceRequestFlowPoint][DispenseCurrency    ][NORMAL]RequiredBillMixList: 100~0|20~6|5~76
";
      public const string Core_DispensedAmount_1 =
      @"[2023-06-07 12:18:33-379][3][][FiservDNAWebServiceRequestFlowPoint][DispenseCurrency    ][NORMAL]dispensedAmount= 500
";

      /* JackHenry */

      public const string Core_ProcesWithdrawalTransaction_Amount_2 =
      @"[2023-10-05 16:16:11-896][3][][JackHenryWebServiceRequestFlowPoint][ProcessWithdrawalTransaction][NORMAL]Amount - 60
";
      public const string Core_ProcessWithdrawalTransaction_Account_2 =
      @"[2023-10-05 14:46:27-344][3][][JackHenryWebServiceRequestFlowPoint][ProcessWithdrawalTransaction][NORMAL]Account Number - x4361, Account Type - D, Transaction Code - 58
";
      public const string Core_RequiredBillMixList_2 =
      @"[2023-11-21 12:13:05-982][3][][JackHenryWebServiceRequestFlowPoint][DispenseCurrency    ][NORMAL]RequiredBillMixList: 100~0|20~4|5~4|1~0
";

      public const string Core_DispensedAmount_2 =
      @"[2023-11-21 12:13:14-418][3][][JackHenryWebServiceRequestFlowPoint][DispenseCurrency    ][NORMAL]dispensedAmount= 100
";

      /* SymXchange */

      public const string Core_ProcessWithdrawalTransaction_Amount_3 =
      @"[2023-11-13 10:37:45-919][3][][SymXchangeWebServiceRequestFlowPoint][ProcessWithdrawalTransaction][NORMAL]Total Amount: 800
";
      public const string Core_ProcessWithdrawalTransaction_Account_3 =
      @"[2023-11-13 10:37:45-914][3][][SymXchangeWebServiceRequestFlowPoint][ProcessWithdrawalTransaction][NORMAL]Account Number: XXXXXX8451
";
      public const string Core_RequiredBillMixList_3 =
      @"[2023-11-22 14:53:08-481][3][][SymXchangeWebServiceRequestFlowPoint][DispenseCurrency    ][NORMAL]RequiredBillMixList: 100~0|20~25|5~0|1~0
";
      public const string Core_DispensedAmount_3 =
      @"[2023-11-22 14:53:20-513][3][][SymXchangeWebServiceRequestFlowPoint][DispenseCurrency    ][NORMAL]dispensedAmount= 500
";

      /* CUAnswers */

      public const string Core_ProcessWithdrawalTransaction_Amount_4 =
      @"[2023-11-06 14:59:17-355][3][][CUAnswersWebServiceRequestFlowPoint][ProcessWithdrawalTransaction][NORMAL]Total Amount: 60
";
      public const string Core_ProcessWithdrawalTransaction_Account_4 =
      @"[2023-11-06 14:59:17-350][3][][CUAnswersWebServiceRequestFlowPoint][ProcessWithdrawalTransaction][NORMAL]Account Number: XX9725
";
      public const string Core_RequiredBillMixList_4 =   
      @"[2023-11-06 14:59:23-484][3][][CUAnswersWebServiceRequestFlowPoint][DispenseCurrency    ][NORMAL]RequiredBillMixList: 100~0|20~2|5~4|1~0
";
      public const string Core_DispensedAmount_4 =
      @"[2023-11-06 14:59:31-859][3][][CUAnswersWebServiceRequestFlowPoint][DispenseCurrency    ][NORMAL]dispensedAmount= 60
";

      /* CMCFlex */

      public const string Core_ProcessWithdrawalTransaction_Amount_5 =
      @"[2023-12-04 10:32:45-152][3][][CMCFlexWebServiceRequestFlowPoint][ProcessWithdrawalTransaction][NORMAL]Amount: 1
";
      public const string Core_ProcessWithdrawalTransaction_Account_5 =
      @"[2023-12-04 10:32:45-144][3][][CMCFlexWebServiceRequestFlowPoint][ProcessWithdrawalTransaction][NORMAL]Account Number: x2754
";
      public const string Core_RequiredBillMixList_5 =
      @"[2023-12-04 10:33:03-975][3][][CMCFlexWebServiceRequestFlowPoint][DispenseCurrency    ][NORMAL]RequiredBillMixList: 100~0|20~0|5~0|1~1
";
      public const string Core_DispensedAmount_5 =
      @"[2023-12-04 10:33:12-687][3][][CMCFlexWebServiceRequestFlowPoint][DispenseCurrency    ][NORMAL]dispensedAmount= 1
";

      /* KeyBridge */

      public const string Core_ProcessWithdrawalTransaction_Amount_6 =
      @"[2023-12-12 10:39:36-985][3][][KeyBridgeWebServiceRequestFlowPoint][ProcessWithdrawalTransaction][NORMAL]Total Amount: 100
";
      public const string Core_ProcessWithdrawalTransaction_Account_6 =
      @"[2023-12-12 10:39:36-984][3][][KeyBridgeWebServiceRequestFlowPoint][ProcessWithdrawalTransaction][NORMAL]Account Number: XXX3160
";
      public const string Core_RequiredBillMixList_6 =
      @"[2023-12-12 02:02:49-495][3][][KeyBridgeWebServiceRequestFlowPoint][DispenseCurrency    ][NORMAL]RequiredBillMixList: 20~1
";
      public const string Core_DispensedAmount_6 =
      @"[2023-12-12 02:02:57-167][3][][KeyBridgeWebServiceRequestFlowPoint][DispenseCurrency    ][NORMAL]dispensedAmount= 20
";

      /* AccessAdvantage */

      public const string Core_ProcessWithdrawalTransaction_Amount_7 =
      @"[2023-12-04 09:03:47-232][3][][AccessAdvantageWebServiceRequestFlowPoint][ProcessWithdrawalTransaction][NORMAL]Total Amount: 25
";
      public const string Core_ProcessWithdrawalTransaction_Account_7 =
      @"[2023-12-04 09:03:47-232][3][][AccessAdvantageWebServiceRequestFlowPoint][ProcessWithdrawalTransaction][NORMAL]Account Number: X4050
";
      public const string Core_RequiredBillMixList_7 =
      @"[2023-12-04 09:03:48-015][3][][AccessAdvantageWebServiceRequestFlowPoint][DispenseCurrency    ][NORMAL]RequiredBillMixList: 100~0|20~1|5~1|1~0
";
      public const string Core_DispensedAmount_7 =
      @"[2023-12-04 09:03:55-474][3][][AccessAdvantageWebServiceRequestFlowPoint][DispenseCurrency    ][NORMAL]dispensedAmount= 25
";

   }
}
