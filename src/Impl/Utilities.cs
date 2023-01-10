using Contract;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace Impl
{
   /// <summary>
   /// utility functions to assist with parsing
   /// </summary>
   public static class Utilities
   {



      /// <summary>
      /// Change a "1" to a "0" and vise-versa. A use case is where you isolate something like "Output Position Empty = [1]"
      /// and it makes more sense to report not empty. 
      /// </summary>
      /// <param name="flipMe"></param>
      /// <returns></returns>
      public static string Flip(string flipMe)
      {
         string returnMe = flipMe.Trim();
         if (string.IsNullOrEmpty(returnMe))
         {
            if (returnMe == "1")
            {
               returnMe = "0";
            }
            else if (returnMe == "0")
            {
               returnMe = "1";
            }
         }
         return returnMe;
      }

      private static bool _ExtractZipFiles(IContext ctx, string currentFolder)
      {
         string[] zipFiles = ctx.ioProvider.GetFiles(currentFolder, "*.zip");
         foreach (string zipFile in zipFiles)
         {
            ctx.ConsoleWriteLogLine("ZipFile: " + zipFile);
            string newFolderName = ctx.ioProvider.GetFileNameWithoutExtension(zipFile);

            ctx.ConsoleWriteLogLine("Create directory:" + currentFolder + "\\" + newFolderName);
            if (!ctx.ioProvider.CreateDirectory(currentFolder + "\\" + newFolderName))
            {
               ctx.ConsoleWriteLogLine("Failed to create directory:" + zipFile);
               return false;
            }

            // Extract current zip file
            ctx.ioProvider.ExtractToDirectory(zipFile, currentFolder + "\\" + newFolderName);
         }

         foreach (var directory in Directory.GetDirectories(currentFolder))
         {
            ctx.ConsoleWriteLogLine("Iterating into: " + directory);
            _ExtractZipFiles(ctx, directory);
         }

         return true;
      }

      public static bool ExtractZipFiles(IContext ctx)
      {
         // Extract current zip file
         ctx.ioProvider.ExtractToDirectory(ctx.ZipFileName, ctx.WorkFolder + "\\" + ctx.SubFolder);
         return _ExtractZipFiles(ctx, ctx.WorkFolder + "\\" + ctx.SubFolder);
      }

      public static bool FindAllTraceFiles(IContext ctx)
      {
         ctx.ConsoleWriteLogLine("Unzipping the archive...");
         if (!Utilities.ExtractZipFiles(ctx))
         {
            return false;
         }
         ctx.ConsoleWriteLogLine("Unzip complete.");

         // find all nwlog files
         string[] tempFiles = ctx.ioProvider.GetFiles(ctx.WorkFolder, "*.nwlog");

         // reduce to nwlog files in the [SP] subfolder, if any
         List<string> tempList = new List<string>();
         for (int i = 0; i < tempFiles.Length; i++)
         {
            // get rid of nwlog files not in the [SP] subFolder
            if (tempFiles[i].Contains("[SP]"))
            {
               tempList.Add(tempFiles[i]);
            }
         }

         ctx.nwlogFiles = tempList.ToArray();
         return true;
      }

      /// <summary>
      /// Convert an fwType to string based on this mapping
      /// 
      /// /* values of WFSCIMCASHIN.fwType */
      /// #define WFS_CIM_TYPERECYCLING (1)
      /// #define WFS_CIM_TYPECASHIN (2)
      /// #define WFS_CIM_TYPEREPCONTAINER (3)
      /// #define WFS_CIM_TYPERETRACTCASSETTE (4)
      /// #define WFS_CIM_TYPEREJECT (5)
      /// #define WFS_CIM_TYPECDMSPECIFIC (6)
      /// </summary>
      /// <param name="fwType"></param>
      /// <returns></returns>


      public static string CashInTypeName(int fwType)
      {
         string typeName = "UNKNW";
         switch (fwType)
         {
            case 1: // WFS_CIM_TYPERECYCLING
               typeName = "RECYC";
               break;
            case 2: // WFS_CIM_TYPECASHIN
               typeName = "CASHIN";
               break;
            case 3: // WFS_CIM_TYPEREPCONTAINER
               typeName = "REPCON";
               break;
            case 4: // WFS_CIM_TYPERETRACTCASSETTE
               typeName = "RETRAC";
               break;
            case 5: // WFS_CIM_TYPEREJECT
               typeName = "REJECT";
               break;
            case 6: // WFS_CIM_TYPECDMSPECIFIC
               typeName = "CMDSPC";
               break;
            default:
               break;
         }
         return typeName; 
      }

      public static bool ListsAreEqual(List<string> firstList, List<string> secondList)
      {
         bool listsAreEqual = firstList.Count == secondList.Count;

         if (listsAreEqual)
         {
            string[] firstArray = firstList.ToArray();
            string[] secondArray = secondList.ToArray(); 

            for (int i = 0; i < firstArray.Length && listsAreEqual; i++)
            {
               listsAreEqual = listsAreEqual && firstArray[i] == secondList[i];
            }
         }

         return listsAreEqual;
      }
   }
}
