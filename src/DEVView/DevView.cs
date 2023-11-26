using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace DeviceView
{
   [Export(typeof(IView))]
   public class DevView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      DevView() : base(ParseType.SP, "DEVView") { }

      /// <summary>
      /// Creates an CDM Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new CDM table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         DEVTable devTable = new DEVTable(ctx, viewName);
         devTable.ReadXmlFile();
         return devTable;
      }
   }
}
