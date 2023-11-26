﻿using Contract;
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
      CDMView() : base(ParseType.SP, "CDMView") { }

      /// <summary>
      /// Creates an CDM Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new CDM table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         CDMTable cdmTable = new CDMTable(ctx, viewName);
         cdmTable.ReadXmlFile();
         return cdmTable;
      }
   }
}
