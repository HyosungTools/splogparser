using System;
using System.Dynamic;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Contract;


namespace LogLineHandler
{
   public class IISRequest : IILine
   {
      private string className = "IISRequest";

      private static string prev_IISRequestState = string.Empty;
      private static long prev_request_count = 0;

      public bool IgnoreThis { get; private set; } = false;
      public string IISRequestState { get; private set; } = string.Empty;
      public string RepeatCount { get; private set; } = string.Empty;
      public string Server_IpAddress { get; private set; } = string.Empty;
      public string Method { get; private set; } = string.Empty;
      public string Uri { get; private set; } = string.Empty;
      public string Query { get; private set; } = string.Empty;
      public string Port { get; private set; } = string.Empty;
      public string Username { get; private set; } = string.Empty;
      public string Client_IpAddress { get; private set; } = string.Empty;
      public string UserAgent { get; private set; } = string.Empty;
      public string Referer { get; private set; } = string.Empty;
      public string HttpStatusCode { get; private set; } = string.Empty;
      public string SubStatusCode { get; private set; } = string.Empty;
      public string HttpError { get; private set; } = string.Empty;
      public string Win32StatusCode { get; private set; } = string.Empty;
      public string Win32Error { get; private set; } = string.Empty;
      public string TimeTakenMsec { get; private set; } = string.Empty;
      public string Exception { get; private set; } = string.Empty;

      public IISRequest(ILogFileHandler parent, string logLine, IILogType awType = IILogType.IISRequest) : base(parent, logLine, awType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         if (string.IsNullOrEmpty(logLine))
         {
            return;
         }

         //#Fields: date time s-ip cs-method cs-uri-stem cs-uri-query s-port cs-username c-ip cs(User-Agent) cs(Referer) sc-status sc-substatus sc-win32-status time-taken
         //2023-11-14 07:00:00 10.201.33.12 GET /ActiveTeller/signalr/reconnect clientProtocol=1.4&transport=serverSentEvents&connectionData=[%7B%22Name%22:%22clientHub%22%7D]&connectionToken=u7E8uiZjg1phc%2BMJVogr7zAFpMO%2FoMcvfr9INhtUCr0ljZnPrECpx%2FJLQ1NP9Objku%2Bmns91Y1OnKnsKRrC5osoNorBT3O7A%2FQ8rucfW68KbCbHOqb%2FmBxC2U8TUkoxZ&messageId=d-FE1C586E-B%2C0%7CBI%2C8CF%7CBJ%2C1 80 - 10.50.230.10 SignalR.Client.NET45/2.2.2.0+(Microsoft+Windows+NT+6.2.9200.0) - 200 0 0 3391224
         //2023-11-14 07:00:00 10.201.33.12 GET /ActiveTeller/signalr/connect clientProtocol=1.4&transport=serverSentEvents&connectionData=[%7B%22Name%22:%22clientHub%22%7D]&connectionToken=bPxh2xx2ir4vdgw6bX31IO2rWvZUtBZ%2FyI12sXeLg45Er7ZjuWNdl29IEgpkysRM342w8nwYIqE%2BuU3UUtGfOipqrnhgGLoqLl%2FxrkzbgMHAbbDOqdgTM7w7M%2Fen1s5c 80 - 10.50.209.10 SignalR.Client.NET45/2.2.2.0+(Microsoft+Windows+NT+6.2.9200.0) - 200 0 0 50966640
         //2023-11-14 07:00:00 10.201.33.12 GET /activeteller/SignalR/signalr/connect clientProtocol=1.4&transport=serverSentEvents&connectionData=[%7B%22Name%22:%22ClientHub%22%7D]&connectionToken=Nnk26q9ZnN%2Bg6L7bXA6lZkqxwHlTZSi%2Bnoo22b2lIY19duK8TvgzGza7yvaUjk%2B5k12lkz3b9gbCQpz5ZWZ4OsjqZ%2FqfoC5cqlYm3GqQjDZaYLLhWuyEczzID1mH46xi 80 - 10.20.10.84 SignalR.Client.NET45/2.2.2.0+(Microsoft+Windows+NT+6.2.9200.0) - 200 0 0 33019337
         //2023-11-14 07:00:00 10.201.33.12 GET /ActiveTeller/signalr/connect clientProtocol=1.4&transport=serverSentEvents&connectionData=[%7B%22Name%22:%22clientHub%22%7D]&connectionToken=ruCTBPZ8GcVOjK1i1DBY%2F3ebXFwPZyC%2B76kWbT%2FDrkmX5WMaEeDnOPKzJCHqqtgK0cGxKzefjyTXVgB8mnvuplZKjFEI7ql%2BuPfIBQaeU15sxPHhtin45fgBPzAqCT0q 80 - 10.50.240.10 SignalR.Client.NET45/2.2.2.0+(Microsoft+Windows+NT+6.2.9200.0) - 200 0 0 79118488

         Regex regex = new Regex("#Fields: (?<s_ip>.*) (?<cs_method>.*) (?<cs_uri_stem>.*) (?<cs_uri_query>.*) (?<s_port>.*) (?<cs_username>.*) (?<c_ip>.*) (?<cs_User_Agent>.*) (?<cs_Referer>.*) (?<sc_status>.*) (?<sc_substatus>.*) (?<sc_win32_status>.*) (?<time_taken>.*)");
         Match m = regex.Match(logLine);
         if (m.Success)
         {
            IISRequestState = logLine;
            IsRecognized = true;
            IgnoreThis = true;

            prev_IISRequestState = string.Empty;
            prev_request_count = 0;

            return;
         }

         // remove date and time
         string subLogLine = logLine.Substring( (logLine.Length > 20) ? 20 : logLine.Length);

         regex = new Regex("^(?<s_ip>.*) (?<cs_method>.*) (?<cs_uri_stem>.*) (?<cs_uri_query>.*) (?<s_port>.*) (?<cs_username>.*) (?<c_ip>.*) (?<cs_User_Agent>.*) (?<cs_Referer>.*) (?<sc_status>.*) (?<sc_substatus>.*) (?<sc_win32_status>.*) (?<time_taken>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Server_IpAddress = m.Groups["s_ip"].Value;
            Method = m.Groups["cs_method"].Value;
            Uri = m.Groups["cs_uri_stem"].Value;
            Query = m.Groups["cs_uri_query"].Value;
            Port = m.Groups["s_port"].Value;
            Username = m.Groups["cs_username"].Value;
            Client_IpAddress = m.Groups["c_ip"].Value;
            UserAgent = m.Groups["cs_User_Agent"].Value;
            Referer = m.Groups["cs_Referer"].Value;
            HttpStatusCode = m.Groups["sc_status"].Value;
            SubStatusCode = m.Groups["sc_substatus"].Value;
            Win32StatusCode = m.Groups["sc_win32_status"].Value;
            TimeTakenMsec = m.Groups["time_taken"].Value;

            string separator = (Query != string.Empty ? "?" : string.Empty);
            string response = (HttpStatusCode != string.Empty ? "ResponseCode=" + HttpStatusCode : string.Empty);

            IISRequestState = $"{Method} {Uri}{separator}{Query} client={Server_IpAddress}:{Port} {response}".Trim().Replace("  ", " ");

            IsRecognized = true;

            // ADVICE TO USER:
            // Everytime IIS receives a request, if IIS logging in enabled, IIS logs the request into a Log file. In IIS 6 logging in enabled by default however in IIS 7 it's a choice.
            // In IIS 6 logs are stored at C:\windows\system32\LogFiles\
            // In IIS 7 the location would be C:\inetpub\logs\logfiles\
            // Once you get to this location you would be seeing entries like this W3SVC1, W3SVC87257621.
            // W3SVC stands for website and 1 or 87257621 stands for the unique identifier that is associated with a particular website.
            // How do you know which identifier is associated with which website?
            //   Goto, Inetmgr->click on Web Sites and take a look at the right hand side screen... And there you have it the Identifier column. This identifier is unique and no 2 websites have the same identifier.Even if you delete and recreate the same website you will notice that the website identifier is different.
            // Once you go into the log folder you will see logs depending on the format you have chosen. By default in IIS 6, they are daily.And the log filenames are in the format exYYMMDD.log... (ROVASTAR, thanks for the inputs)

               //https://learn.microsoft.com/en-us/troubleshoot/developer/webapps/iis/www-administration-management/http-status-code
               //https://blogs.iis.net/ma_khan/troubleshooting-iis-6-status-and-substatus-codes

               bool isStatusRecognized = false;

            if (HttpStatusCode.StartsWith("1"))
            {
               HttpError = "INFORMATION";

               switch (HttpStatusCode)
               {
                  case "100": HttpError += ", CONTINUE"; isStatusRecognized = true; break;
                  case "101": HttpError += ", SWITCHING PROTOCOLS"; isStatusRecognized = true; break;
                  default: HttpError += ", UNHANDLED STATUS CODE"; break;
               }
            }

            else if (HttpStatusCode.StartsWith("2"))
            {
               HttpError = " SUCCESSFUL";

               switch (HttpStatusCode)
               {
                  case "200": HttpError += ", SUCCESS"; isStatusRecognized = true; break;
                  case "201": HttpError += ", CREATED"; isStatusRecognized = true; break;
                  default: HttpError += ", UNHANDLED STATUS CODE"; break;
               }
            }

            else if (HttpStatusCode.StartsWith("3"))
            {
               HttpError = "REDIRECTION";

               switch (HttpStatusCode)
               {
                  case "301": HttpError += ", MOVED PERMANENTLY"; isStatusRecognized = true; break;
                  case "302": HttpError += ", MOVED TEMPORARILY"; isStatusRecognized = true; break;
                  case "304": HttpError += ", REQUEST FAILED A CONDITION"; isStatusRecognized = true; break;
                  case "307": HttpError += ", TEMPORARY REDIRECT - USING CACHED COPY"; isStatusRecognized = true; break;
                  default: HttpError += ", UNHANDLED STATUS CODE"; break;
                  }
            }

            else if (HttpStatusCode.StartsWith("4"))
            {
               HttpError = "CLIENT ERROR";

               switch (HttpStatusCode)
               {
                  case "400": HttpError += ", BAD REQUEST"; isStatusRecognized = true; break;

                  case "401":
                     HttpError += ", ACCESS DENIED";
                     isStatusRecognized = true;

                     switch (SubStatusCode)
                     {
                        case "x": HttpError += ", UNAUTHORIZED"; break;
                        case "1": HttpError += ", ACCESS DENIED INVALID CREDENTIALS"; break;
                        case "2": HttpError += ", ACCESS DENIED USE ALTERNATE AUTHENTICATION METHOD"; break;
                        case "3": HttpError += ", ACCESS DENIED DUE TO ACL SETTING"; break;
                        case "4": HttpError += ", AUTHORIZATION FAILED IN FILTER"; break;
                        case "5": HttpError += ", AUTHORIZATION FAILED BY ISAPI/CGI APPLICATION"; break;
                        case "7": HttpError += ", ACCESS DENIED DUE TO URL AUTHORIZATION POLICY"; break;

                        default:
                           HttpError += ", UNHANDLED SUBSTATUS CODE";
                           break;
                     }
                     break;

                  case "403": 
                     HttpError += ", FORBIDDEN";
                     isStatusRecognized = true;

                     switch (SubStatusCode)
                     {
                        case "x": HttpError += ", Access is denied"; break;
                        case "1": HttpError += ", Execute access is denied"; break;
                        case "2": HttpError += ", Read access is denied"; break;
                        case "3": HttpError += ", Write access is denied"; break;
                        case "4": HttpError += ", SSL is required to view this resource"; break;
                        case "5": HttpError += ", SSL 128 is required to view this resource"; break;
                        case "6": HttpError += ", IP address of the client has been rejected"; break;
                        case "7": HttpError += ", SSL client certificate is required"; break;
                        case "8": HttpError += ", DNS name of the client is rejected"; break;
                        case "9": HttpError += ", Too many clients are trying to connect to the Web server"; break;
                        case "10": HttpError += ", Web server is configured to deny Execute access"; break;
                        case "11": HttpError += ", Password has been changed"; break;
                        case "12": HttpError += ", Client certificate is denied access by the server certificate mapper"; break;
                        case "13": HttpError += ", Client certificate has been revoked on the Web server"; break;
                        case "14": HttpError += ", Directory listing is denied on the Web server"; break;
                        case "15": HttpError += ", Client access licenses have exceeded limits on the Web server"; break;
                        case "16": HttpError += ", Client certificate is ill-formed or is not trusted by the Web server"; break;
                        case "17": HttpError += ", Client certificate has expired or is not yet valid"; break;
                        case "18": HttpError += ", Cannot execute requested URL in the current application pool"; break;
                        case "19": HttpError += ", Cannot execute CGIs for the client in this application pool"; break;
                        case "20": HttpError += ", Passport logon failed"; break;
                        default: HttpError += ", UNHANDLED SUBSTATUS CODE"; break;
                     }

                     break;

                  case "404":
                     HttpError += ", TARGET NOT FOUND";
                     isStatusRecognized = true;

                     switch (SubStatusCode)
                     {
                        case "x": HttpError += ", File or directory not found"; break;
                        case "1": HttpError += ", Web site not accessible on the requested port"; break;
                        case "2": HttpError += ", Web service extension lockdown policy prevents this request"; break;
                        case "3": HttpError += ", MIME map policy prevents this request"; break;
                        case "4": HttpError += ", No handler was found to serve the request"; break;
                        case "5": HttpError += ", The Request Filtering Module rejected an URL sequence in the request"; break;
                        case "6": HttpError += ", The Request Filtering Module denied the HTTP verb of the request"; break;
                        case "7": HttpError += ", The Request Filtering module rejected the file extension of the request"; break;
                        case "8": HttpError += ", The Request Filtering module rejected a particular URL segment (characters between two slashes)"; break;
                        case "9": HttpError += ", IIS rejected to serve a hidden file"; break;
                        case "10": HttpError += ", The Request Filtering module rejected a header that was too long"; break;
                        case "11": HttpError += ", The Request Filtering module rejected a request that was double escaped"; break;
                        case "12": HttpError += ", The Request Filtering module rejected a request that contained high bit characters"; break;
                        case "13": HttpError += ", The Request Filtering module rejected a request that was too long (request + entity body)"; break;
                        case "14": HttpError += ", The Request Filtering module rejected a request with a URL that was too long"; break;
                        case "15": HttpError += ", The Request Filtering module rejected a request with a too long query string"; break;
                        default: HttpError += ", UNHANDLED SUBSTATUS CODE"; break;
                     }

                     break;

                  case "405": HttpError += ", METHOD NOT ALLOWED"; isStatusRecognized = true; break;
                  case "406": HttpError += ", MIME TYPE NOT ACCEPTED"; isStatusRecognized = true; break;
                  case "408": HttpError += ", TIMED OUT BEFORE REQUEST RECEIVED"; isStatusRecognized = true; break;
                  case "412": HttpError += ", REQUEST PRECONDITION FAILED"; isStatusRecognized = true; break;
                  default: HttpError += "CLIENT ERROR-BAD REQUEST, UNHANDLED STATUS CODE"; break;
               }
            }

            else if (HttpStatusCode.StartsWith("5"))
            {
               HttpError = "SERVER ERROR";

               switch (HttpStatusCode)
               {
                  case "500":
                     HttpError += ", INTERNAL SERVER ERROR";
                     isStatusRecognized = true;

                     switch (SubStatusCode)
                     {
                        case "11": HttpError += ", Application is shutting down on the Web server"; break;
                        case "12": HttpError += ", Application is busy restarting on the Web server"; break;
                        case "13": HttpError += ", Web server is too busy"; break;
                        case "14": HttpError += ", Invalid application configuration on the server"; break;
                        case "15": HttpError += ", Direct requests for Global.asa are not allowed"; break;
                        case "16": HttpError += ", UNC authorization credentials are incorrect"; break;
                        case "17": HttpError += ", URL authorization store cannot be found"; break;
                        case "18": HttpError += ", URL authorization store cannot be opened"; break;
                        case "100": HttpError += ", Internal ASP error"; break;
                        default: HttpError += ", UNHANDLED SUBSTATUS CODE"; break;
                     }

                     break;

                  case "501": HttpError += ", FUNCTION NOT IMPLEMENTED"; isStatusRecognized = true; break;
                  case "502": HttpError += ", GATEWAY OR PROXY ERROR"; isStatusRecognized = true; break;
                  case "503": HttpError += ", SERVICE UNAVAILABLE"; isStatusRecognized = true; break;
                  default: HttpError += ", UNHANDLED STATUS CODE"; break;
               }
            }

            else
            {
               throw new Exception($"Invalid server response code {HttpStatusCode} substatus code {SubStatusCode}");
            }

            if (Win32StatusCode != "0")
            {
               Win32Error = "WIN32 ERROR";

               switch (Win32StatusCode)
               {
                  case "2": Win32Error += ", FILE NOT FOUND"; isStatusRecognized = true; break;

                  // errors associated with persistent SignalR channel failures
                  case "64":  Win32Error += ", NO ACK RESPONSE FROM CLIENT, TIMEOUT";  isStatusRecognized = true; break;
                  case "121": Win32Error += ", SEMAPHORE EXPIRED, TIMEOUT"; isStatusRecognized = true; break;
                  case "1236": Win32Error += ", CONNECTION ABORTED"; isStatusRecognized = true; break;
                  default: Win32Error += ", UNHANDLED WIN32 STATUS CODE"; break;
               }
            }

            if (!isStatusRecognized)
            {
               // breakpoint here
            }

            IgnoreThis = (IISRequestState == prev_IISRequestState);

            if (IgnoreThis)
            {
               prev_request_count++;

               RepeatCount = prev_request_count.ToString();
            }
            else
            {
               /*
               if (prev_request_count > 1)
               {
                  // TODO  - actually want to defer output until the last duplicate, show the # of occurrences and the min/avg/max time duration

                  Console.WriteLine($"{prev_IISRequestState} ({prev_request_count} occurrences)");
               }
               */

               prev_IISRequestState = IISRequestState;
               prev_request_count = 0;
            }
         }

         if (!IsRecognized)
         {
           throw new Exception($"IILogLine.{className}: did not recognize the log line '{logLine}'");
         }
      }
   }
}
