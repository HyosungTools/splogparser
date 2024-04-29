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
            string installed = ", Installed:";
            string version = ", Version:";

            packages[i, 0] = string.Empty;
            packages[i, 1] = string.Empty;
            packages[i, 2] = string.Empty;

            int idx = line.IndexOf(installed);
            if (idx > 0)
            {
               // packages[i, 0] - the package(s) installed
               packages[i, 0] = line.Substring(0, idx).Replace(" - ", "").Trim();
               string subLine = line.Substring(idx + installed.Length);

               idx = subLine.IndexOf(version);
               if (idx > 0)
               {
                  // only try to parse the install date if it's the correct length
                  if (subLine.Substring(0, idx).Length > 10)
                  {
                     packages[i, 1] = subLine.Substring(0, idx).Trim();
                     try
                     {
                        DateTime inputDate = DateTime.ParseExact(packages[i, 1], "MM/dd/yyyy", null);
                        packages[i, 1] = inputDate.ToString("yyyy-MM-dd");
                     }
                     catch (Exception e)
                     {
                        parentHandler.ctx.ConsoleWriteLogLine(String.Format("Exception - Failed to parse time from {0}, {1}, {2}", subLine, packages[i, 1], e.Message));
                     }
                  }
                  packages[i, 2] = line.Substring(idx + version.Length).Trim();

               }
            }

            i++;
         }

         return packages;
      }
   }
}
