﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples
{
   public static class samples_ndc
   {
      /* Transaction Requests*/
      public const string ATM2HOST11_1 =
         @"[2023-02-21 19:59:51-367][0][][Log                 ][Send                ][SEND]ATM2HOST: 110501?;XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX?BA    A 00000350XXXXXXXXXXXXXXXX0180000000005CAM00048407A00000000422035710511829XXXXXXXXXXXXXXXXXXXXXX07805A08511829XXXXXX50935F2A0208405F3401**820218008C279F02069F03069F1A0295055F2A029A039C019F37049F35019F45029F4C089F34039F21039F7C14950580000480009A032302219C01019F0607A00000000422039F10120110A00001220000000000000000000000FF9F1A0208409F2608609064DCF34C2C359F2701809F33036040009F360203089F3704********5F2403******500544656269749F34034203009F02060000000003509F03060000000000009F3501149F2103195900";

      public const string ATM2HOST11_2 =
         @"[2023-07-03 14:32:54-184][0][][Log                 ][Send                ][SEND]ATM2HOST: 1100019;XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX?DBA    A00000000XXXXXXXXXXXXXXXX253991000000000000000000000Z02560355010700955CAM00048C279F02069F03069F1A0295055F2A029A039C019F37049F35019F45029F4C089F34039F21039F7C148407A0000000042203500855532044656269745713523446XXXXXXXXXXXXXXXXXXXXXXXXXXXX000F5A08523446XXXXXX64435F3401**9F26082C9AF530864F6C439F2701809F10120110A00001220000000000000000000000FF820218009F3602016B5F2403******9F33036040009F3901059F5301009F34034203009F1A0208409F02060000000000009F0306000000000000950580800480005F2A0208409A032307039C01309F3704********9F3501149F2103143245";

      public const string ATM2HOST11_3 =
         @"[2023-07-03 09:52:49-798][0][][Log                 ][Send                ][SEND]ATM2HOST: 1100014C";

      public const string ATM2HOST11_4 =
         @"[2023-07-03 13:27:38-844][0][][Log                 ][Send                ][SEND]ATM2HOST: 1100016;XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX?A A    A00050000XXXXXXXXXXXXXXXX006008003002253971000000000000000000000Z02500347010400935CAM00048C279F02069F03069F1A0295055F2A029A039C019F37049F35019F45029F4C089F34039F21039F7C148407A0000000042203500544656269745713533248XXXXXXXXXXXXXXXXXXXXXXXXXXXX000F5A08533248XXXXXX13575F3401**9F2608CBA3FA11D73592919F2701809F10120110A00001220000000000000000000000FF820218009F360200EE5F2403******9F33036040009F3901059F5301009F34034203009F1A0208409F02060000000500009F0306000000000000950580800480005F2A0208409A032307039C01019F3704********9F3501149F2103132656";

      public const string ATM2HOST11_5 =
         @"[2023-07-05 17:45:21-838][0][][Log                 ][Send                ][SEND]ATM2HOST: 1100013;XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX?DBA    A00000000XXXXXXXXXXXXXXXX254201000000000000000000000Z02750402012501085CAM00048C279F02069F03069F1A0295055F2A029A039C019F37049F35019F45029F4C089F34039F21039F7C148407A0000000042203500855532044656269745713523446XXXXXXXXXXXXXXXXXXXXXXXXXXXX000F5A08523446XXXXXX03835F3401**9F2608C79A19FD0DCE31299F2701809F10120110A00001220000000000000000000000FF820218009F360200D45F2403******9F33036040009F3901059F5301009F34034203009F1A0208409F02060000000000009F0306000000000000950580800480005F2A0208409A032307059C01309F3704********9F3501149F2103174520";

      public const string ATM2HOST11_6 =
         @"[2023-04-19 14:05:40-663][0][][Log                 ][Send                ][SEND]ATM2HOST: 1105018;XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX?BB    A 00020000XXXXXXXXXXXXXXXX0100000000005CAM00048407A00000009808405713466013XXXXXXXXXXXXXXXXXXXXXXXXXXXX931F5A08466013XXXXXX14625F2A0208405F3401**820218008C159F02069F03069F1A0295055F2A029A039C019F3704950580000480009A032304199C01019F0607A00000009808409F100706011203A020009F1A0208409F2608775E060EA2FA4CA39F2701809F33036040009F3602000C9F3704********5F2403******500855532044454249549F34030201009F02060000000200009F0306000000000000";

      public const string ATM2HOST11_7 =
         @"[2023-04-19 14:05:40-663][0][][Log                 ][Send                ][SEND]ATM2HOST: 1105018;XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX?BB    A 00020000XXXXXXXXXXXXXXXX0100000000005CAM00048407A00000009808405713466013XXXXXXXXXXXXXXXXXXXXXXXXXXXX931F5A08466013XXXXXX14625F2A0208405F3401**820218008C159F02069F03069F1A0295055F2A029A039C019F3704950580000480009A032304199C01019F0607A00000009808409F100706011203A020009F1A0208409F2608775E060EA2FA4CA39F2701809F33036040009F3602000C9F3704********5F2403******500855532044454249549F34030201009F02060000000200009F0306000000000000";

      public const string ATM2HOST11_8 =
         @"[2023-04-19 14:09:08-312][0][][Log                 ][Send                ][SEND]ATM2HOST: 1105019;XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX?BA    A 00020000XXXXXXXXXXXXXXXX0100000000005CAM00048407A00000009808405713466013XXXXXXXXXXXXXXXXXXXXXXXXXXXX728F5A08466013XXXXXX85015F2A0208405F3401**820218008C159F02069F03069F1A0295055F2A029A039C019F3704950580000480009A032304199C01019F0607A00000009808409F100706011203A020009F1A0208409F2608FB80C9554A2428A79F2701809F33036040009F360201759F3704********5F2403******500855532044454249549F34030201009F02060000000200009F0306000000000000";

      public const string ATM2HOST11_9 =
         @"[2023-04-19 14:10:22-590][0][][Log                 ][Send                ][SEND]ATM2HOST: 110501:;XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX?BB    A 00004000XXXXXXXXXXXXXXXX0020000000005CAM00048407A00000009808405713466013XXXXXXXXXXXXXXXXXXXXXXXXXXXX728F5A08466013XXXXXX85015F2A0208405F3401**820218008C159F02069F03069F1A0295055F2A029A039C019F3704950580000480009A032304199C01019F0607A00000009808409F100706011203A020009F1A0208409F260854B11AB35EE995CA9F2701809F33036040009F360201769F3704********5F2403******500855532044454249549F34030201009F02060000000040009F0306000000000000";

      public const string ATM2HOST11_10 =
         @"[2023-04-19 14:33:37-937][0][][Log                 ][Send                ][SEND]ATM2HOST: 110501;;XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX?BA    A 00004000XXXXXXXXXXXXXXXX0020000000005CAM00048407A00000009808405711434256XXXXXXXXXXXXXXXXXXXXXXXX02595A08434256XXXXXX45245F2A0208405F3401**820218008C159F02069F03069F1A0295055F2A029A039C019F3704950580000480009A032304199C01019F0607A00000009808409F100706061203A020009F1A0208409F2608C88F7383856B94229F2701809F33036040009F360200E19F3704********5F2403******500855532044454249549F34030201009F02060000000040009F0306000000000000";

      public const string ATM2HOST11_CASHDEPOT_1 =
         @"  INFO [2023-07-24 12:51:29-846] [Log.Send] ATM2HOST: 110001=;XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX?WAA     00030000XXXXXXXXXXXXXXXX0000000050040000000005CAM00045712400022XXXXXXXXXXXXXXXXXXXXXXXXXX000F5A08400022XXXXXX23555F2A0208405F3401**820218008C159F02069F03069F1A0295055F2A029A039C019F3704950580000480009A032307249C01019F0607A00000009808409F100706021203A0A0009F1A0208409F2608D2CC7F3A2C6942B59F2701809F33036040009F360200959F3704********5F2403******500855532044454249549F02060000000300009F0306000000000000";

      public const string ATM2HOST11_CASHDEPOT_2 =
         @"  INFO [2023-07-24 12:51:34-695] [Log.Send] ATM2HOST: 110001>A";

      public const string ATM2HOST12_1 =
         @"[2023-07-03 03:02:46-073][0][][Log                 ][Send                ][SEND]ATM2HOST: 12000B8300";

      public const string ATM2HOST12_2 =
         @"[2023-07-03 03:39:47-581][0][][Log                 ][Send                ][SEND]ATM2HOST: 12000G02002111";

      public const string ATM2HOST12_3 =
         @"[2023-07-06 12:05:04-254][0][][Log                 ][Send                ][SEND]ATM2HOST: 12000P21";

      public const string ATM2HOST12_4 =
         @"[2023-07-05 14:01:43-925][0][][Log                 ][Send                ][SEND]ATM2HOST: 12000E000000000000000012111";

      public const string ATM2HOST12_5 =
         @"[2023-07-04 11:11:13-722][0][][Log                 ][Send                ][SEND]ATM2HOST: 12000D04000";

      public const string ATM2HOST22_1 =
         @"[2023-07-03 14:32:55-537][0][][Log                 ][Send                ][SEND]ATM2HOST: 220009CAM5A0852344600003964438C279F02069F03069F1A0295055F2A029A039C019F37049F35019F45029F4C089F34039F21039F7C149F2608F5A7E94F13F844E09F2701009F10120110201001220000000000000000000000FF950580800480009B026800";

      public const string ATM2HOST22_2 =
         @"[2023-07-03 03:02:46-474][0][][Log                 ][Send                ][SEND]ATM2HOST: 22000FLA060000BG530-0205";

      public const string ATM2HOST22_3 =
         @"[2023-07-03 03:02:46-600][0][][Log                 ][Send                ][SEND]ATM2HOST: 22000F68300";

      public const string ATM2HOST22_4 =
         @"[2023-07-03 03:02:48-837][0][][Log                 ][Send                ][SEND]ATM2HOST: 220009";

      public const string ATM2HOST22_5 =
         @"[2023-07-03 14:12:15-270][0][][Log                 ][Send                ][SEND]ATM2HOST: 22000F183000000004000000000000000167F001D01020580000000C7000000010200007F7F0000001131000000011110000000001011111";

      public const string ATM2HOST22_6 =
         @"[2023-10-02 15:31:05-288][0][][Log                 ][Send                ][SEND]ATM2HOST: 223828E200000000000000910000";

      public const string ATM2HOST23_1 =
         @"[2023-03-17 01:33:16-455][0][][Log                 ][Send                ][SEND]ATM2HOST: 230003A3B801";

      public const string ATM2HOST23_2 =
         @"[2023-03-17 09:29:02-602][0][][Log                 ][Send                ][SEND]ATM2HOST: 230003345C69";

      public const string ATM2HOST23_3 =
         @"[2022-12-23 13:48:20-659][0][][Log                 ][Send                ][SEND]ATM2HOST: 230001TA078859|vh#1K$^RLCbu1;ayzX7?/$orZ>MOdL5B%qs99GSXWD.`#hF1C^ij#NmV//C*mo6'$M.Bh+E25Mv|IGK{K8:\$_\0]C{pSG{K.RaS{+QG&}8R]iN+<aE<3-Y+B;tQ)QOmd2.Mqkn2Up34h*vHG#-oN|a8U'`V'K8O(}dc2(+qQ^J,&>a5gORa=uGpL!4aF&>]a`+6UocN8vWmz='Ga6/jOp4B;$z#5,F:#_,v1IIq2\RhID$&a[$FC]=t>7QNm-KfcN+8>b:B0]HT TpI10Ta=`Ais^xLOYdoICm4|7+yG3AL7)(T%F'2G{#JnT";

      public const string ATM2HOST23_4 =
         @"[2022-12-23 13:48:24-475][0][][Log                 ][Send                ][SEND]ATM2HOST: 230005";

      public const string ATM2HOST23_5 =
         @"[2023-03-30 03:08:19-477][0][][Log                 ][Send                ][SEND]ATM2HOST: 23050401DFDD058B2B000000000000";

      public const string ATM2HOST51_1 =
         @"[2023-03-17 01:34:18-209][0][][Log                 ][Send                ][SEND]ATM2HOST: 61015920230317013418130566130604038";

      public const string ATM2HOST61_1 =
         @"[2023-03-17 08:56:27-338][0][][Log                 ][Send                ][SEND]ATM2HOST: 61015920230317085627132404132604200T TYPE:D";

      public const string ATM2HOST61_2 =
         @"[2023-03-17 08:56:28-726][0][][Log                 ][Send                ][SEND]ATM2HOST: 610159202303170856281350041352042000]";



      public const string HOST2ATM1_1 =
         @"[2023-03-17 01:33:08-488][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 12";

      public const string HOST2ATM1_2 =
         @"[2023-03-17 01:33:16-518][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 171";

      //public const string HOST2ATM1_3 =
      //   @"[2023-03-17 08:39:27-936][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 1BB530400128A023030";

      public const string HOST2ATM1_4 =
         @"[2023-04-28 03:04:20-954][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 10192";

      public const string HOST2ATM1_5 =
         @"[2023-04-28 03:04:27-730][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 101975";


      public const string HOST2ATM3_1 =
         @"[2023-03-17 01:33:16-041][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 342030028155193241073169238029006074042075204015221164";

      public const string HOST2ATM3_2 =
         @"[2023-04-28 03:04:27-847][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 301912022X100067068023700032231000023W608609610255255613614615024X101067068025701032000000025W255255255255255255255255026D022144000000000000000000028Y103067006029703012231000029W041041041255255041041041030X102067006031702032008000031W255255255041255255255255032Y104067006033704012199000033W040040040255255255040040034Y105067006035705012255000035W637637637036637637637637036Y106067006037706012137000037W637255255034255255255637038D034145000000000000000000039C032000000000000000000000040F053067006M3I040M3I040010041F054067006045041045041010";

      public const string HOST2ATM3_3 =
         @"[2023-04-28 03:04:32-404][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 301911070126802800810AHACCOUNT BALANCECF&XXXXXXXXXXXXXXXXXXFDPAYOFFF0#XXXXXXXX.XXGDAMOUNT DUEG0*XXXXXXXX.XX1070126802800810AHSALDO DE CUENTACF&XXXXXXXXXXXXXXXXXXF@SALDO PAGO TOTALF0#XXXXXXXX.XXG@CANTIDAD DEBIDAG0*XXXXXXXX.XX071126802800810AHACCOUNT BALANCECF&XXXXXXXXXXXXXXXXXXF@BALANCE AMOUNTF0#XXXXXXXX.XXG@AMOUNT  DUEG0*XXXXXXXX.XX1071126802800810AHSALDO DE CUENTACF&XXXXXXXXXXXXXXXXXXF@SALDOF0#XXXXXXXX.XXG@CANTIDAD DEBIDAG0*XXXXXXXX.XX";

      public const string HOST2ATM3_4 =
         @"[2023-03-31 03:04:55-488][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 34H";

      public const string HOST2ATM3_5 =
         @"";


      /* Transaction Reply */
      public const string HOST2ATM4_1 =
         @"[2023-03-17 08:39:27-914][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 41010000000007765000148$4308.34102";

      public const string HOST2ATM4_2 =
         @"[2022-12-17 06:48:56-361][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 409000000000635950328015287250200281529   ";

      public const string HOST2ATM4_3 =
         @"[2022-12-17 09:03:31-302][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 40900000000063765032:015287250552963021   ";

      public const string HOST2ATM4_4 =
         @"[2023-04-19 14:05:41-837][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 4152102422A111150B@       SAVINGS WITHDRAWAL       C@166M@176801 CARD NO: 466013XXXXXX1462";

      public const string HOST2ATM4_5 =
         @"[2023-04-19 14:09:09-397][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 4152102423A111150B@      CHECKING WITHDRAWAL       C@166M@176901 CARD NO: 466013XXXXXX8501";

      public const string HOST2ATM4_6 =
         @"[2023-04-19 14:10:24-036][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 4152022424A111150B@       SAVINGS WITHDRAWAL       C@166M@176:01 CARD NO: 466013XXXXXX8501";

      public const string HOST2ATM4_7 =
         @"[2023-04-19 14:33:42-169][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 4152022425A111150B@      CHECKING WITHDRAWAL       C@166M@176<01 CARD NO: 434256XXXXXX4524";

      public const string HOST2ATM4_8 =
         @"[2023-04-19 16:10:33-692][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 4152142426A111150B@      CHECKING WITHDRAWAL       C@166M@176=01 CARD NO: 431751XXXXXX4418";

      public const string HOST2ATM4_9 =
         @"[2023-04-19 16:11:30-586][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 415224275100150A@       SAVINGS WITHDRAWAL       BA     EXCEEDS AMOUNT LIMIT     D@165M@175>01 CARD NO: 431751XXXXXX4418";

      public const string HOST2ATM4_10 =
         @"[2023-03-18 08:49:39-996][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 415205048636A111150B@      CHECKING WITHDRAWAL       C@166M@176701 CARD NO: 519880XXXXXX9172";

      public const string HOST2ATM4_11 =
         @"[2023-09-16 11:02:42-347][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 43360050000000021555026:0309/16/23  11:023WI001336";

      public const string HOST2ATM6_1 =
         @"[2023-02-27 17:14:40-177][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 61387153";

      public const string HOST2ATM8_1 =
         @"[2023-03-30 03:08:28-790][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 850607A000000004306007MAESTROCAM0000000204009800001584575A5F2A5F34828C959A9C9F069F109F1A9F269F279F339F369F375F24509F34059F279F269F10959B0207A000000004101007A0000000046000000201";

      public const string HOST2ATM8_2 =
         @"[2023-03-30 03:08:30-408][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 850F08A00000033301010807USCUPICCAM0000002000100000001384575A5F2A5F34828C959A9C9F069F109F1A9F269F279F339F369F379F34059F279F269F10959B00000201";
   }
}
