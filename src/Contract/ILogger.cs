namespace Contract
{
   /// <summary>
   /// Interface for a logging object. 
   /// </summary>
   public interface ILogger
   {
      /// <summary>
      /// Write a log line to the log file. 
      /// </summary>
      /// <param name="message"></param>
      void WriteLog(string message);
   }
}
