using LogFileHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contract;
using TCRSamples;
using System;
using LogLineHandler;

namespace TCRLogLineTests
{
   class TCR_Read_EntireFile
   {
      [TestMethod]
      public void ReadEntireFile()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.TCR, TCRLogLine.Factory);
         logFileHandler.OpenLogFile(tcr_samples_entirefile.tcr_entirefile_1);

         _ = logFileHandler.ReadLine();
         while (!logFileHandler.EOF())
         {
            try
            {
               _ = logFileHandler.ReadLine();
            }
            catch (Exception)
            {
               Assert.IsTrue(false);
               return;
            }
         }

         Assert.IsTrue(true);
         return;
      }
   }
}
