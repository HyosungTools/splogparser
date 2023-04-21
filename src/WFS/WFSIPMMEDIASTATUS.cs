using System.Collections.Generic;
using Contract;

namespace Impl
{
   // Commands where this structure is used: 


   public class WFSIPMMEDIASTATUS : WFS
   {
      public string usMediaID { get; set; }
      public string wMediaLocation { get; set; }
      public string usBinNumber { get; set; }
      public string ulCodelineDataLength { get; set; }
      public string lpbCodelineData { get; set; }
      public string wMagneticReadIndicator { get; set; }
      public string fwInsertOrientation { get; set; }
      public string lpMediaSize { get; set; }
      public string wMediaValidity { get; set; }
      public string wCustomerAccess { get; set; }


      public WFSIPMMEDIASTATUS(IContext ctx) : base(ctx)
      {

      }

      public string Initialize(string nwLogLine)
      {
         return nwLogLine;
      }
   }
}
