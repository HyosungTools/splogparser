using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace IPMView
{
   [Export(typeof(IView))]
   public class IPMView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      IPMView() : base(ParseType.SP, "IPMView") { }

      /// <summary>
      /// Creates an IPM Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new IPM table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         IPMTable ipmTable = new IPMTable(ctx, viewName);
         ipmTable.ReadXmlFile();
         return ipmTable;
      }
   }
}
