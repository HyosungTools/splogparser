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
      protected override string hResult()
      {
         return "";
      }

      protected override string tsTimestamp()
      {
         return "";
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
            packages[i, 0] = string.Empty;
            packages[i, 1] = string.Empty;
            packages[i, 2] = string.Empty;

            // e.g.: - a2ia-v9.0, Installed: 2024-04-17 12:36:25, Status: Good

            Regex regex = new Regex("^ - (?<package>.*), Installed:(?<installDate>.*), Status:(?<status>.*)?");
            Match m = regex.Match(line);
            if (!m.Success)
            {
               continue;
            }

            if (m.Groups["package"].Value.Length > 0)
               packages[i, 0] = m.Groups["package"].Value.Trim();

            if (m.Groups["installDate"].Value.Length > 0)
               packages[i, 1] = m.Groups["installDate"].Value.Trim();

            if (m.Groups["status"].Value.Length > 0)
               packages[i, 2] = m.Groups["status"].Value.Trim();

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
            packages[i, 0] = string.Empty;
            packages[i, 1] = string.Empty;
            packages[i, 2] = string.Empty;

            // e.g.: - A2iA CheckReader V9.0, Installed: 04/17/2024, Version: 9.0

            Regex regex = new Regex("^ - (?<program>.*), Installed:(?<installDate>.*), Version:(?<version>.*)?");
            Match m = regex.Match(line);
            if (!m.Success)
            {
               continue;
            }

            if (m.Groups["program"].Value.Length > 0)
               packages[i, 0] = m.Groups["program"].Value.Trim();

            if (m.Groups["installDate"].Value.Length > 0)
               packages[i, 1] = m.Groups["installDate"].Value.Trim();

            if (m.Groups["version"].Value.Length > 0)
               packages[i, 2] = m.Groups["version"].Value.Trim();

            i++;
         }

         return packages;
      }
   }
}
