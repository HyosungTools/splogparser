using System;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class MachineInfo : APLine
   {
      public string machineNumber;
      public string appVersion;
      public string sdkVersion;
      public string gsdkVersion;
      public string model;
      public string ipAddress;
      public string portNumber;
      public string timeZone;
      public string nextwareVersion;
      public string emvKernel;
      public string[,] deviceInfo;

      public string[,] installedPrograms;
      public string[,] installedPackages;

      public MachineInfo(ILogFileHandler parent, string logLine, APLogType apType = APLogType.APLOG_INSTALL) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize(); 

         installedPackages = InstalledPackages();
         installedPrograms = InstalledPrograms();

      }

      protected string[,] InstalledPackages()
      {
         string[,] packages = null;
         string installedPackages = "Installed Packages:\r\n";
         if (logLine.IndexOf(installedPackages) < 0)
         {
            return packages;
         }

         // isolated the installed packages part of the line
         installedPackages = logLine.Substring(logLine.IndexOf(installedPackages) + installedPackages.Length);

         string installedPackagesEnd = "=====";
         if (installedPackages.IndexOf(installedPackagesEnd) >= 0)
         {
            installedPackages = installedPackages.Substring(0, installedPackages.IndexOf(installedPackagesEnd));
         }

         string[] lines = Regex.Split(installedPackages, "\r\n");

         // storage will be 2D array, one for each line
         packages = new string[lines.Length, 3];

         int i = 0;
         foreach (string line in lines)
         {
            string[] elements = Regex.Split(line, ",");
            packages[i, 0] = (elements.Length > 0) ? elements[0].Replace(" - ", "").Trim() : string.Empty;
            packages[i, 1] = (elements.Length > 1) ? elements[1].Replace(" Installed: ", "").Trim() : string.Empty;
            packages[i, 2] = (elements.Length > 2) ? elements[2].Replace(" Status: ", "").Trim() : string.Empty;
            i++;
         }

         return packages;
      }

      protected string[,] InstalledPrograms()
      {
         string[,] packages = null;
         string installedPrograms = "Installed Programs:\r\n";
         if (logLine.IndexOf(installedPrograms) < 0)
         {
            return packages;
         }

         // isolated the installed programs part of the line
         installedPrograms = logLine.Substring(logLine.IndexOf(installedPrograms) + installedPrograms.Length);

         string installedProgramsEnd = "=====";
         if (installedPrograms.IndexOf(installedProgramsEnd) >= 0)
         {
            installedPrograms = installedPrograms.Substring(0, installedPrograms.IndexOf(installedProgramsEnd));
         }

         string[] lines = Regex.Split(installedPrograms, "\r\n");

         // storage will be 2D array, one for each line
         packages = new string[lines.Length, 3];

         int i = 0;
         foreach (string line in lines)
         {
            string[] elements = Regex.Split(line, ",");
            packages[i, 0] = (elements.Length > 0) ? elements[0].Replace(" - ", "").Trim() : string.Empty;
            packages[i, 1] = (elements.Length > 1) ? elements[1].Replace(" Installed: ", "").Trim() : string.Empty;
            packages[i, 2] = (elements.Length > 2) ? elements[2].Replace(" Version: ", "").Trim() : string.Empty;

            // For Installed Programs, the date format is MM/dd/yyyy. Normalize to yyyy-MM-dd
            string inputDateString = packages[i, 1];
            if (!string.IsNullOrEmpty(inputDateString))
            {
               DateTime inputDate = DateTime.ParseExact(inputDateString, "MM/dd/yyyy", null);
               packages[i, 1] = inputDate.ToString("yyyy-MM-dd");
            }
            i++;
         }

         return packages;
      }
   }
}
