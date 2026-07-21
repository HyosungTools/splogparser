using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace MVRosterView
{
   [Export(typeof(IView))]
   public class MVRosterView : BaseView, IView
   {
      /// <summary>
      /// Constructor. ParseType.MV routes this view to the MoniView server-log handler.
      /// </summary>
      MVRosterView() : base(ParseType.MV, "MVRosterView") { }

      /// <summary>
      /// Creates an MVRosterTable instance and loads its seed XML/XSD.
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <returns>MVRosterTable</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         MVRosterTable rosterTable = new MVRosterTable(ctx, viewName);
         rosterTable.ReadXmlFile();
         return rosterTable;
      }
   }
}
