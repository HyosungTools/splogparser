using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impl
{
   public static class _wfs_inf_cim_status 
   {
      public static (bool success, string xfsMatch, string subLogLine) fwDevice(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "fwDevice = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwSafeDoor(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "fwSafeDoor = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwAcceptor(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "fwAcceptor = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwIntermediateStacker(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "fwIntermediateStacker = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwStackerItems(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "fwStackerItems = \\[(.*)\\]", "0");
      }
   }
}
