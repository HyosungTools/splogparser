using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace NHCDMView
{
   [Export(typeof(IView))]
   public class NHCDMView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      NHCDMView() : base(ParseType.SP, "NHCDMView") { }

      /// <summary>
      /// Creates an CDM Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new CDM table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         NHCDMTable cdmTable = new NHCDMTable(ctx, viewName);
         cdmTable.ReadXmlFile();
         return cdmTable;
      }
   }
}
