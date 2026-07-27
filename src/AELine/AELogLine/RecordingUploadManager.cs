using System;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   /// <summary>
   /// ActiveTeller Agent Extensions lines emitted by the video-recording upload subsystem,
   /// tagged [RecordingUploadManager]. This component was added to ActiveTeller after the
   /// original AE parser was written (which only knew [MoniPlus2sExtension] / [NetOpExtension]
   /// / [NextwareExtension]), so these lines used to fall through to None and get dropped.
   ///
   /// It NEVER throws on an unrecognized message: an unmatched line is still captured with its
   /// raw text (IsRecognized = false) so nothing is lost.
   ///
   /// Sample lines:
   ///   2026-07-24 00:30:35 [RecordingUploadManager] Pausing video recording uploads.
   ///   2026-07-24 00:30:35 [RecordingUploadManager] Resuming video recording uploads.
   ///   2026-07-24 00:30:35 [RecordingUploadManager] Queueing recording for ID:33536
   ///   2026-07-24 00:30:35 [RecordingUploadManager] Missing customer audio file.
   ///   2026-07-24 00:30:35 [RecordingUploadManager] GetLastEnqueuedTellerSessionId: Returning Teller Session Id '0' from path '0'.
   ///   2026-07-24 00:30:36 [RecordingUploadManager] MoveNext: Failed with exception System.IO.DirectoryNotFoundException: Could not find a part of the path 'D:\MP2S_VideoRecordings\33536'.
   /// (Raw stack-trace frames on the following physical lines have no tag, so the handler
   ///  leaves them as None; the exception type + message above is what lands here.)
   /// </summary>
   public class RecordingUploadManager : AELine
   {
      public string Action { get; set; } = string.Empty;
      public string TellerSessionId { get; set; } = string.Empty;
      public string Error { get; set; } = string.Empty;

      public RecordingUploadManager(ILogFileHandler parent, string logLine, AELogType aeType = AELogType.RecordingUploadManager)
         : base(parent, logLine, aeType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         const string tag = "[RecordingUploadManager]";
         int idx = logLine.IndexOf(tag, StringComparison.Ordinal);
         string msg = (idx != -1) ? logLine.Substring(idx + tag.Length).Trim() : logLine.Trim();

         // Teller session id, wherever it appears:
         //   "Queueing recording for ID:33536"
         //   "Returning Teller Session Id '0' from path '0'."
         //   "...path 'D:\MP2S_VideoRecordings\33536'."
         Match m = Regex.Match(msg, @"ID:(?<id>\d+)");
         if (!m.Success) m = Regex.Match(msg, @"Teller Session Id '(?<id>\d+)'");
         if (!m.Success) m = Regex.Match(msg, @"VideoRecordings[\\/](?<id>\d+)");
         if (m.Success) TellerSessionId = m.Groups["id"].Value;

         if (msg.StartsWith("Pausing", StringComparison.OrdinalIgnoreCase))
         {
            Action = "PAUSE UPLOADS";
            IsRecognized = true;
         }
         else if (msg.StartsWith("Resuming", StringComparison.OrdinalIgnoreCase))
         {
            Action = "RESUME UPLOADS";
            IsRecognized = true;
         }
         else if (msg.StartsWith("Queueing recording", StringComparison.OrdinalIgnoreCase))
         {
            Action = "QUEUE RECORDING";
            IsRecognized = true;
         }
         else if (msg.IndexOf("Missing customer audio", StringComparison.OrdinalIgnoreCase) >= 0)
         {
            Action = "MISSING AUDIO";
            IsRecognized = true;
         }
         else if (msg.StartsWith("GetLastEnqueuedTellerSessionId", StringComparison.OrdinalIgnoreCase))
         {
            Action = "GET LAST SESSION";
            IsRecognized = true;
         }
         else if (msg.IndexOf("Failed with exception", StringComparison.OrdinalIgnoreCase) >= 0
                  || msg.IndexOf("Exception", StringComparison.Ordinal) >= 0)
         {
            Action = "ERROR";
            IsRecognized = true;

            // Pull "<Exception type>: <message>" if present.
            Match em = Regex.Match(msg, @"(?<err>System\.[^\s:]+: .+)$");
            Error = em.Success ? em.Groups["err"].Value : msg;
         }
         else
         {
            // Unknown [RecordingUploadManager] message — keep it, do not throw.
            Action = msg;
            IsRecognized = false;
         }
      }
   }
}
