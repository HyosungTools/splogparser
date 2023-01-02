using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Contract;

namespace sp_logparser
{
   public class ViewLoader
   {
      /// <summary>
      /// Accessor to iterate over the Views
      /// </summary>
      [ImportMany]
      public IEnumerable<IView> Views { get; set; }

      /// <summary>
      /// Necessary part of implementing MEF. I dont think this is used. 
      /// </summary>
      public CompositionContainer Container { get; }

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="path">path on hard drive where to look for View DLLs</param>
      public ViewLoader(string path)
      {
         DirectoryCatalog directoryCatalog = new DirectoryCatalog(path);

         //An aggregate catalog that combines multiple catalogs 
         var catalog = new AggregateCatalog(directoryCatalog);

         // Create the CompositionContainer with all parts in the catalog (links Exports and Imports) 
         Container = new CompositionContainer(catalog);

         //Fill the imports of this object 
         Container.ComposeParts(this);
      }
   }
}
