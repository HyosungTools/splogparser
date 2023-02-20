using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace CDMView
{
   [Export(typeof(IView))]
   public class CDMView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      CDMView() : base("CDM", "CDM") { }

      /// <summary>
      /// Creates an CDM Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new CDM table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         return new CDMTable(ctx, viewName, "CDMViewSchema.xsd");
      }
   }
}
