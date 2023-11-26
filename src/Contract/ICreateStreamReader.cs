using System.IO;

namespace Contract
{
   public interface ICreateStreamReader
   {
      StreamReader Create(string fileInfo);
   }
}
