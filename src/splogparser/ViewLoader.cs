using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Contract;

namespace splogparser
{
   public class ViewLoader
   {
      /// <summary>
      /// Accessor to iterate over the Views
      /// </summary>
      [ImportMany]
      public IEnumerable<IView> Views { get; set; }

      /// <summary>
      /// Discovered Parts 
      /// </summary>
      public CompositionContainer Container { get; }

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="path">path on hard drive where to look for View DLLs</param>
      public ViewLoader(string path)
      {
         DirectoryCatalog directoryCatalog = new DirectoryCatalog(path, "*.dll");
         IReadOnlyCollection<string> loadedFiles = directoryCatalog.LoadedFiles;
         foreach (string loadedName in loadedFiles)
         {
            Console.WriteLine(String.Format("Found '{0}' DLLs", loadedName));
         }

         AggregateCatalog aggregateCatalog = new AggregateCatalog(directoryCatalog);

         // Create the CompositionContainer with all parts in the catalog (links Exports and Imports) 
         Container = new CompositionContainer(aggregateCatalog);

         //Fill the imports of this object 
         Container.ComposeParts(this);
      }
   }
}
