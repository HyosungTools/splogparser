using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
   public interface ILogReader
   {
      /// <summary>
      /// EOF test
      /// </summary>
      /// <returns>true if read to EOF; false otherwise</returns>
      bool EOF();

      /// <summary>
      /// Read one log line from a file. 
      /// </summary>
      /// <returns></returns>
      string ReadLine(); 
   }
}
