using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace WinCEJournalView
{
   /// <summary>
   /// View for WinCE electronic journal files (JNL*.dat).
   /// Displays parsed journal entries with KindCode descriptions from XML lookup.
   /// </summary>
   [Export(typeof(IView))]
   public class WinCEJournalView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      WinCEJournalView() : base(ParseType.WinCE, "WinCEJournalView") { }

      /// <summary>
      /// Creates a WinCEJournalTable instance.
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <returns>New WinCEJournalTable</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         WinCEJournalTable journalTable = new WinCEJournalTable(ctx, viewName);
         journalTable.ReadXmlFile();
         return journalTable;
      }
   }
}
