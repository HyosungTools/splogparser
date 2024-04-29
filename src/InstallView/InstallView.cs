using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace InstallView
{
   [Export(typeof(IView))]
   public class InstallView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      InstallView() : base(ParseType.AP, "Install") { }

      /// <summary>
      /// Creates an CIM Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new HCDU table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         InstallTable installTable = new InstallTable(ctx, viewName);
         installTable.ReadXmlFile();
         return installTable;
      }
   }
}
