using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class AddKey : APLine
   {
      public string tableName { get; set; }
      public string keyName { get; set; }
      public string value { get; set; }

      public AddKey(ILogFileHandler parent, string logLine, APLogType apType = APLogType.APLOG_ADDKEY) : base(parent, logLine, apType)
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
