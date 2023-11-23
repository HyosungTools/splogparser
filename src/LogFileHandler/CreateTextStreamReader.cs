using System.IO;
using Contract;

namespace LogFileHandler
{
   public class CreateTextStreamReader : ICreateStreamReader
   {
      public CreateTextStreamReader()
      {
      }
      public StreamReader Create(string fileInfo)
      {
         return new StreamReader(fileInfo);
      }
   }
}
