using System.IO;
using System.Text;
using Contract;

namespace AELogLineTests
{
   class CreateTextStreamReaderMock : ICreateStreamReader
   {
      public StreamReader Create(string fileInfo)
      {
         return new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(fileInfo)));
      }
   }
}
