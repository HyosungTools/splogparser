using System;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Contract;
using LogLineHandler;

namespace LogFileHandler
{
   /// <summary>
   /// Reads IIS logs 
   /// </summary>
   public class IILogHandler : LogHandler, ILogFileHandler
   {
      public IILogHandler(ICreateStreamReader createReader) : base(ParseType.II, createReader)
      {
         LogExpression = "u_ex*.log";    // u_ex231114.log
         Name = "IILogFileHandler";
      }



      /// <summary>
      /// EOF test
      /// </summary>
      /// <returns>true if read to EOF; false otherwise</returns>
      public bool EOF()
      {
         return traceFilePos >= logFile.Length;
      }

      /// <summary>
      /// Read one log line from a twlog file. 
      /// </summary>
      /// <returns></returns>
      public string ReadLine()
      {
         // builder will hold the line
         StringBuilder builder = new StringBuilder();

         bool endOfLine = false;

         // while not EOL or EOF
         while (!endOfLine && !EOF())
         {
            char c = logFile[traceFilePos];
            traceFilePos++;

            // check for end of line or end of file
            if (c == '\n' || EOF())
            {
               endOfLine = true;

               if (c == '\n')
               {
                  break;
               }
            }

            // ignore nulls and non-printing ASCII characters
            if (c > 0 && c < 128 && c != '\r')
            {
               builder.Append(c);
            }
         }

         return builder.ToString();
      }

      public ILogLine IdentifyLine(string logLine)
      {
         /* u_ex*.log
         
         // once at start of file, and within the file (each time IIS is restarted?)
         #Software: Microsoft Internet Information Services 10.0
         #Version: 1.0
         #Date: 2023-11-14 00:00:00
         #Fields: date time s-ip cs-method cs-uri-stem cs-uri-query s-port cs-username c-ip cs(User-Agent) cs(Referer) sc-status sc-substatus sc-win32-status time-taken

         2023-11-14 00:00:00 10.201.33.12 PUT /ActiveTeller/api/applicationstates/6 - 80 - 10.50.230.10 - - 200 0 0 110
         2023-11-14 00:04:26 10.201.33.12 GET /ActiveTeller/api/operatingmodes AssetName=A060314 80 - 10.50.209.10 - - 200 0 0 79
         #Software: Microsoft Internet Information Services 10.0
         #Version: 1.0
         #Date: 2023-11-14 07:00:00
         #Fields: date time s-ip cs-method cs-uri-stem cs-uri-query s-port cs-username c-ip cs(User-Agent) cs(Referer) sc-status sc-substatus sc-win32-status time-taken
         2023-11-14 06:03:29 10.201.33.12 POST /ActiveTeller/signalr/send clientProtocol=1.4&transport=serverSentEvents&connectionData=[%7B%22Name%22:%22clientHub%22%7D]&connectionToken=u7E8uiZjg1phc%2BMJVogr7zAFpMO%2FoMcvfr9INhtUCr0ljZnPrECpx%2FJLQ1NP9Objku%2Bmns91Y1OnKnsKRrC5osoNorBT3O7A%2FQ8rucfW68KbCbHOqb%2FmBxC2U8TUkoxZ 80 - 10.50.230.10 SignalR.Client.NET45/2.2.2.0+(Microsoft+Windows+NT+6.2.9200.0) - 200 0 0 146
         2023-11-14 06:03:29 10.201.33.12 POST /ActiveTeller/api/assets - 80 - 10.50.230.10 - - 201 0 0 263
         2023-11-14 06:03:29 10.201.33.12 PUT /ActiveTeller/api/applicationstates/6 - 80 - 10.50.230.10 - - 200 0 0 82
         2023-11-14 06:03:29 10.201.33.12 GET /ActiveTeller/api/operatingmodes AssetName=A060302 80 - 10.50.230.10 - - 200 0 0 90
         2023-11-14 06:03:29 10.201.33.12 POST /ActiveTeller/api/assetconfigurations - 80 - 10.50.230.10 - - 201 0 0 134
         2023-11-14 06:03:31 10.201.33.12 GET /ActiveTeller/signalr/connect clientProtocol=1.4&transport=serverSentEvents&connectionData=[%7B%22Name%22:%22clientHub%22%7D]&connectionToken=u7E8uiZjg1phc%2BMJVogr7zAFpMO%2FoMcvfr9INhtUCr0ljZnPrECpx%2FJLQ1NP9Objku%2Bmns91Y1OnKnsKRrC5osoNorBT3O7A%2FQ8rucfW68KbCbHOqb%2FmBxC2U8TUkoxZ 80 - 10.50.230.10 SignalR.Client.NET45/2.2.2.0+(Microsoft+Windows+NT+6.2.9200.0) - 200 0 64 56813827
         #Software: Microsoft Internet Information Services 10.0
         #Version: 1.0
         #Date: 2023-11-14 07:00:00
         #Fields: date time s-ip cs-method cs-uri-stem cs-uri-query s-port cs-username c-ip cs(User-Agent) cs(Referer) sc-status sc-substatus sc-win32-status time-taken
         2023-11-14 07:00:00 10.201.33.12 GET /ActiveTeller/signalr/reconnect clientProtocol=1.4&transport=serverSentEvents&connectionData=[%7B%22Name%22:%22clientHub%22%7D]&connectionToken=u7E8uiZjg1phc%2BMJVogr7zAFpMO%2FoMcvfr9INhtUCr0ljZnPrECpx%2FJLQ1NP9Objku%2Bmns91Y1OnKnsKRrC5osoNorBT3O7A%2FQ8rucfW68KbCbHOqb%2FmBxC2U8TUkoxZ&messageId=d-FE1C586E-B%2C0%7CBI%2C8CF%7CBJ%2C1 80 - 10.50.230.10 SignalR.Client.NET45/2.2.2.0+(Microsoft+Windows+NT+6.2.9200.0) - 200 0 0 3391224
         2023-11-14 07:00:00 10.201.33.12 GET /ActiveTeller/signalr/connect clientProtocol=1.4&transport=serverSentEvents&connectionData=[%7B%22Name%22:%22clientHub%22%7D]&connectionToken=bPxh2xx2ir4vdgw6bX31IO2rWvZUtBZ%2FyI12sXeLg45Er7ZjuWNdl29IEgpkysRM342w8nwYIqE%2BuU3UUtGfOipqrnhgGLoqLl%2FxrkzbgMHAbbDOqdgTM7w7M%2Fen1s5c 80 - 10.50.209.10 SignalR.Client.NET45/2.2.2.0+(Microsoft+Windows+NT+6.2.9200.0) - 200 0 0 50966640
         2023-11-14 07:00:00 10.201.33.12 GET /activeteller/SignalR/signalr/connect clientProtocol=1.4&transport=serverSentEvents&connectionData=[%7B%22Name%22:%22ClientHub%22%7D]&connectionToken=Nnk26q9ZnN%2Bg6L7bXA6lZkqxwHlTZSi%2Bnoo22b2lIY19duK8TvgzGza7yvaUjk%2B5k12lkz3b9gbCQpz5ZWZ4OsjqZ%2FqfoC5cqlYm3GqQjDZaYLLhWuyEczzID1mH46xi 80 - 10.20.10.84 SignalR.Client.NET45/2.2.2.0+(Microsoft+Windows+NT+6.2.9200.0) - 200 0 0 33019337
         2023-11-14 07:00:00 10.201.33.12 GET /ActiveTeller/signalr/connect clientProtocol=1.4&transport=serverSentEvents&connectionData=[%7B%22Name%22:%22clientHub%22%7D]&connectionToken=ruCTBPZ8GcVOjK1i1DBY%2F3ebXFwPZyC%2B76kWbT%2FDrkmX5WMaEeDnOPKzJCHqqtgK0cGxKzefjyTXVgB8mnvuplZKjFEI7ql%2BuPfIBQaeU15sxPHhtin45fgBPzAqCT0q 80 - 10.50.240.10 SignalR.Client.NET45/2.2.2.0+(Microsoft+Windows+NT+6.2.9200.0) - 200 0 0 79118488

          */

         /*
         //2023-10-16 01:01:08 [SymXchangeCustomerDataExtension]
         Regex regex = new Regex("\\[(?<extension>.*)CustomerDataExtension\\] (?<sublogline>.*)");
         Match m = regex.Match(logLine.Substring(20));
         if (m.Success)
         {
            // process ActiveTellerServerExtension log

            string coreBankingExtension = m.Groups["extension"].Value;
            string subLogLine = m.Groups["sublogline"].Value;

            // expected that all log lines in the ActiveTellerServerExtension log file are for the same extension type
            return new CustomerDataExtension(this, logLine, IILine.GetCoreBankingIILogType(coreBankingExtension));
         }

         // process ActiveTellerServer log

         return new Startup(this, logLine, IILogType.Startup);
         */

         return new IILine(this, logLine, IILogType.None);
      }
   }
}
