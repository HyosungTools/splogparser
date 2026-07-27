using System;
using System.Linq;
using Contract;
using LogLineHandler;

namespace LogFileHandler
{
   /// <summary>
   /// Reads ActiveTeller network logs (*.nwlog).
   ///
   /// These are AP application logs: identical framing and content to APLog
   /// ("  INFO [ts] [ver] [Class.Method] [TID:n] payload") — FlowPoints, LogTransactionData,
   /// NDC/host comms, EMV, note counting (BNACountHandler), screens, SendStringToAgent.
   /// They are NOT XFS/WFS device traces. So this handler is simply APLogHandler pointed at
   /// "*.nwlog" with the AP line factory (APLine.Factory) injected — the lines flow straight
   /// into the existing AP views (Over, Txn, WS, Install, ...) with no new view code.
   ///
   /// The ".nwlog" extension collides with SPLogHandler (XFS traces), and GetFiles() recurses
   /// the whole extraction tree, so both handlers see every *.nwlog. Initialize() filters
   /// FilesFound by content (NwLogSniffer) so this handler keeps only the application-framed
   /// nwlogs and SPLogHandler keeps the XFS ones. Extension decides candidacy; content decides
   /// ownership.
   /// </summary>
   public class ATNwLogHandler : APLogHandler, ILogFileHandler
   {
      public ATNwLogHandler(ICreateStreamReader createReader, ParseType parseType = ParseType.AP, Func<ILogFileHandler, string, ILogLine> Factory = null)
         : base(createReader, parseType, Factory)
      {
         LogExpression = "*.nwlog";
         Name = "ATNwLogFileHandler";
      }

      /// <summary>
      /// Base (LogHandler) finds candidate files via GetFiles (recursive). Then keep only the
      /// application-framed nwlogs so we never fight SPLogHandler over an XFS device trace.
      /// </summary>
      public override bool Initialize(IContext ctx)
      {
         base.Initialize(ctx);
         FilesFound = FilesFound
            .Where(f => NwLogSniffer.Classify(f, createReader) == NwLogSniffer.Kind.ApApplication)
            .ToArray();
         return FilesFound.Length > 0;
      }
   }
}
