using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogLineHandler;
using Samples;
using System;

namespace SPLogLineTests
{
   [TestClass]
   public class WFSCIMCASHUNFOTests
   {
      private static readonly Type WFSCIMCUINFOType = typeof(WFSCIMCASHINFO);

      private static string GetLogicalSubLogLineTable(string logLine)
      {
         int indexOfLogical = logLine.IndexOf("lppList");
         int indexOfPhysical = logLine.IndexOf("lppPhysical");
         Assert.IsTrue(indexOfLogical > 0, "lppList not found in log line.");
         Assert.IsTrue(indexOfPhysical > 0, "lppPhysical not found in log line.");
         string logicalSubLogLine = logLine.Substring(indexOfLogical);
         if (indexOfPhysical > 0)
         {
            logicalSubLogLine = logLine.Substring(indexOfLogical, indexOfPhysical - indexOfLogical);
         }
         return logicalSubLogLine; 
      }

      // Helper method to extract lppPhysical substring
      private static string GetPhysicalSubLogLineTable(string logLine)
      {
         int indexOfTable = logLine.IndexOf("lppList");
         int indexOfPhysical = logLine.IndexOf("lppPhysical");
         Assert.IsTrue(indexOfTable > 0, "lppList not found in log line.");
         Assert.IsTrue(indexOfPhysical > 0, "lppPhysical not found in log line.");
         return logLine.Substring(indexOfPhysical);
      }

      private static string GetLogicalSubLogLineList(string logLine)
      {
         int indexOfLogical = logLine.IndexOf("lppList");
         Assert.IsTrue(indexOfLogical > 0, "lppList not found in log line.");
         string logicalSubLogLine = logLine.Substring(indexOfLogical);
         return logicalSubLogLine;
      }

      // L O G I C A L   T A B L E
      #region LogicalTable


      #endregion

      // P H Y S I C A L   T A B L E
      #region PhysicalTable


      #endregion

   }
}

