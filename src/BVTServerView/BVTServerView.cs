using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace BVTServerView
{
   /// <summary>
   /// BVTServerView summarises the BlueVerse Teller (ActiveTeller) server logs (AV parse type).
   ///
   /// Where AVView dumps one categorized row per line, BVTServerView reconstructs two stories:
   ///   Allocation   - one row per teller session request (asset, routing rule, assigned?,
   ///                  which teller, wait-to-assignment) - the server-side "was a teller available".
   ///   ServerFaults - server exceptions grouped by signature with a count and first/last seen,
   ///                  so a multi-thousand-line exception storm reads as a handful of lines.
   ///
   /// Selected on the command line with:  -v BVTServer  (or -v *).
   /// </summary>
   [Export(typeof(IView))]
   public class BVTServerView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      BVTServerView() : base(ParseType.AV, "BVTServerView") { }

      /// <summary>
      /// Creates a BVTServerTable instance.
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <returns>BVTServerTable</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         BVTServerTable serverTable = new BVTServerTable(ctx, viewName);
         serverTable.ReadXmlFile();
         return serverTable;
      }
   }
}
