using System.Windows;

namespace hylogparser
{
   public partial class App : Application
   {
      /// <summary>
      /// Zip file path passed from the Windows context menu ("%1").
      /// </summary>
      public static string? InitialZipPath { get; private set; }

      protected override void OnStartup(StartupEventArgs e)
      {
         base.OnStartup(e);

         if (e.Args.Length > 0)
         {
            InitialZipPath = e.Args[0];
         }
      }
   }
}
