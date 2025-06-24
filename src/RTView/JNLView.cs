using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace JNLView
{
   [Export(typeof(IView))]
   public class JNLView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      JNLView() : base(ParseType.RT, "JNLView") { }

      /// <summary>
      /// Creates an AddKey Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>AddKeyTable</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         JNLTable installTable = new JNLTable(ctx, viewName);
         installTable.ReadXmlFile();
         return installTable;
      }
   }
}
