using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace XmlParamsView
{
   [Export(typeof(IView))]
   public class XmlParamView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      XmlParamView() : base(ParseType.AP, "XmlParamView") { }

      /// <summary>
      /// Creates an CIM Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new HCDU table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         XmlParamTable installTable = new XmlParamTable(ctx, viewName);
         installTable.ReadXmlFile();
         return installTable;
      }

      public override void PreProcess(IContext ctx)
      {
         bTable.PreProcess(ctx);
      }
   }
}
