using System.Windows;
using Microsoft.Win32;

namespace hylogparser
{
   public partial class StartupDialog : Window
   {
      /// <summary>
      /// The path the user selected (zip file or folder). Empty if cancelled.
      /// </summary>
      public string SelectedPath { get; private set; } = "";

      public StartupDialog()
      {
         InitializeComponent();
      }

      private void btnOpenZip_Click(object sender, RoutedEventArgs e)
      {
         var dlg = new OpenFileDialog
         {
            Title = "Select ATM log archive",
            Filter = "Zip Archives (*.zip)|*.zip|All Files (*.*)|*.*"
         };

         if (dlg.ShowDialog() == true)
         {
            SelectedPath = dlg.FileName;
            DialogResult = true;
            Close();
         }
      }

      private void btnOpenFolder_Click(object sender, RoutedEventArgs e)
      {
         var dlg = new OpenFolderDialog
         {
            Title = "Select unzipped log folder"
         };

         if (dlg.ShowDialog() == true)
         {
            SelectedPath = dlg.FolderName;
            DialogResult = true;
            Close();
         }
      }
   }
}
