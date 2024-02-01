using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class AWAddKey : AWLine
   {
      public string tableName { get; set; }
      public string keyName { get; set; }
      public string value { get; set; }

      public AWAddKey(ILogFileHandler parent, string logLine, AWLogType awType = AWLogType.AddKey) : base(parent, logLine, awType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         int idx = logLine.LastIndexOf(']');
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx + 1);

            // e.g. TcpIp Add Key=SSLCertifcationFilePath, Value=C:\Program Files\MoniPlus2s\Config\Internal\client.pem

            Regex regex = new Regex("^(?<tableName>.*?) Add Key=(?<key>.*?), Value=(?<value>.*?)$");
            Match m = regex.Match(subLogLine);
            if (!m.Success)
            {
               return;
            }

            tableName = m.Groups["tableName"].Value;
            keyName = m.Groups["key"].Value;
            value = m.Groups["value"].Value.Trim();
         }
      }
   }
}
