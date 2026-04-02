using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace hylogparser
{
   // ====================================================================
   //  View definition — each item becomes a CheckBox in the UI
   // ====================================================================
   public class ViewItem
   {
      public string Display { get; set; } = "";
      public string CLI { get; set; } = "";
      public bool IsChecked { get; set; }
   }

   // ====================================================================
   //  Main Window
   // ====================================================================
   public partial class MainWindow : Window
   {
      // ---------------------------------------------------------------
      //  View lists — adjust CLI names to match your Options class
      // ---------------------------------------------------------------

      // -a flag: AP (Application Log) views
      private readonly List<ViewItem> _apViews = new()
      {
         new() { Display = "Over (Transaction Summary)",   CLI = "Over" },
         new() { Display = "Install (Installed Packages)", CLI = "Install" },
         new() { Display = "AddKey (Encryption Keys)",     CLI = "AddKey" },
         new() { Display = "XmlParam (Configuration)",     CLI = "XmlParam" },
         new() { Display = "WS (Web Service)",             CLI = "WS" },
         new() { Display = "EJ (Electronic Journal)",      CLI = "EJ" },
         new() { Display = "Disp (Cash Dispense)",         CLI = "Disp" },
         new() { Display = "DepositAnalysis",              CLI = "DepositAnalysis" },
      };

      // -s flag: SP (Service Provider / XFS) views
      private readonly List<ViewItem> _spViews = new()
      {
         new() { Display = "CDM (Cash Dispenser)",     CLI = "CDm" },
         new() { Display = "CIM (Cash-In Module)",     CLI = "CIM" },
         new() { Display = "IDC (Card Reader)",        CLI = "IDC" },
         new() { Display = "PIN (PIN Pad)",            CLI = "PIN" },
         new() { Display = "PTR (Printer)",            CLI = "PTR" },
         new() { Display = "SIU (Sensors/Indicators)", CLI = "SIU" },
         new() { Display = "BCR (Barcode Reader)",     CLI = "BCR" },
         new() { Display = "IPM (Image Processor)",    CLI = "IPM" },
         new() { Display = "CRD (Card Dispenser)",     CLI = "CRD" },
      };

      // The -f argument passed to splogparser (zip filename or folder name)
      private string _inputArg = "";
      // The working directory for splogparser
      private string _workingDirectory = "";

      private Process? _runningProcess;

      // =================================================================
      //  Constructor
      // =================================================================
      public MainWindow()
      {
         InitializeComponent();

         // Bind view lists to ItemsControls as CheckBoxes
         icAPViews.ItemTemplate = MakeCheckBoxTemplate();
         icAPViews.ItemsSource = _apViews;

         icSPViews.ItemTemplate = MakeCheckBoxTemplate();
         icSPViews.ItemsSource = _spViews;

         // Resolve the input path
         string inputPath = ResolveInputPath();

         if (string.IsNullOrEmpty(inputPath))
         {
            // User cancelled — close the app
            Application.Current.Shutdown();
            return;
         }

         // Determine if it's a zip or folder and set up paths accordingly
         if (File.Exists(inputPath) && inputPath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
         {
            // Zip file: -f gets the filename, cwd is the zip's folder
            _inputArg = Path.GetFileName(inputPath);
            _workingDirectory = Path.GetDirectoryName(inputPath) ?? AppDomain.CurrentDomain.BaseDirectory;
            lblInputType.Text = "Log Archive";
         }
         else if (Directory.Exists(inputPath))
         {
            // Folder: -f gets the folder name, cwd is the folder's parent
            _inputArg = Path.GetFileName(inputPath);
            _workingDirectory = Path.GetDirectoryName(inputPath) ?? AppDomain.CurrentDomain.BaseDirectory;
            lblInputType.Text = "Log Folder";
         }
         else
         {
            MessageBox.Show($"Path not found:\n{inputPath}", "hylogparser",
               MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current.Shutdown();
            return;
         }

         // Display the full path
         lblZipPath.Text = inputPath;
         lblZipPath.ToolTip = inputPath;

         // Show the name in the title bar
         Title = $"hylogparser — {_inputArg}";

         // Show splogparser version if we can find it
         string? exePath = FindSplogParser();
         if (exePath != null)
         {
            try
            {
               var vi = FileVersionInfo.GetVersionInfo(exePath);
               lblVersion.Text = $"splogparser {vi.ProductVersion ?? vi.FileVersion}";
            }
            catch { /* swallow */ }
         }
      }

      // =================================================================
      //  Get the input path: from command-line arg or startup dialog
      // =================================================================
      private static string ResolveInputPath()
      {
         // If launched from context menu, the zip path is in App.InitialZipPath
         if (!string.IsNullOrEmpty(App.InitialZipPath) && File.Exists(App.InitialZipPath))
            return App.InitialZipPath;

         // Otherwise, show the startup dialog
         var dlg = new StartupDialog();
         if (dlg.ShowDialog() == true)
            return dlg.SelectedPath;

         return "";
      }

      // =================================================================
      //  Locate splogparser.exe
      //  Production: same folder as hylogparser.exe
      //  Development: walk up to repo root and check dist\
      // =================================================================
      private static string? FindSplogParser()
      {
         // 1. Same directory as hylogparser.exe (production/installed)
         string dir = AppDomain.CurrentDomain.BaseDirectory;
         string candidate = Path.Combine(dir, "splogparser.exe");
         if (File.Exists(candidate)) return candidate;

         // 2. Walk up to repo root and check dist\ (development)
         DirectoryInfo? d = new DirectoryInfo(dir);
         while (d != null)
         {
            string distCandidate = Path.Combine(d.FullName, "dist", "splogparser.exe");
            if (File.Exists(distCandidate)) return distCandidate;
            d = d.Parent;
         }

         return null;
      }

      // =================================================================
      //  CheckBox data template
      // =================================================================
      private static DataTemplate MakeCheckBoxTemplate()
      {
         var template = new DataTemplate(typeof(ViewItem));
         var factory = new FrameworkElementFactory(typeof(CheckBox));
         factory.SetBinding(CheckBox.IsCheckedProperty, new Binding("IsChecked") { Mode = BindingMode.TwoWay });
         factory.SetBinding(CheckBox.ContentProperty, new Binding("Display"));
         factory.SetValue(CheckBox.MarginProperty, new Thickness(0, 2, 0, 2));
         template.VisualTree = factory;
         return template;
      }

      // =================================================================
      //  All / None buttons
      // =================================================================
      private static void SetAll(IEnumerable<ViewItem> items, bool state)
      {
         foreach (var item in items) item.IsChecked = state;
      }

      private void btnAPAll_Click(object sender, RoutedEventArgs e)  { SetAll(_apViews, true);  icAPViews.Items.Refresh(); }
      private void btnAPNone_Click(object sender, RoutedEventArgs e) { SetAll(_apViews, false); icAPViews.Items.Refresh(); }
      private void btnSPAll_Click(object sender, RoutedEventArgs e)  { SetAll(_spViews, true);  icSPViews.Items.Refresh(); }
      private void btnSPNone_Click(object sender, RoutedEventArgs e) { SetAll(_spViews, false); icSPViews.Items.Refresh(); }

      // =================================================================
      //  Build command-line arguments
      // =================================================================
      private string BuildArguments()
      {
         var args = new List<string>();

         // -f (input) — zip filename or folder name, cwd is already set
         args.Add("-f");
         args.Add($"\"{_inputArg}\"");

         // -a (AP views)
         var apChecked = _apViews.Where(v => v.IsChecked).Select(v => v.CLI).ToList();
         if (apChecked.Count > 0)
         {
            args.Add("-a");
            args.Add(string.Join(",", apChecked));
         }

         // -s (SP views)
         var spChecked = _spViews.Where(v => v.IsChecked).Select(v => v.CLI).ToList();
         if (spChecked.Count > 0)
         {
            args.Add("-s");
            args.Add(string.Join(",", spChecked));
         }

         // -p (SP Flat)
         if (chkSPFlatAll.IsChecked == true)
         {
            args.Add("-p");
            args.Add("*");
         }

         // --wince (WinCE)
         if (chkWinCEAll.IsChecked == true)
         {
            args.Add("--wince");
            args.Add("*");
         }

         // Time range
         string timeStart = txtTimeStart.Text.Trim();
         string timeSpan = txtTimeSpan.Text.Trim();
         if (!string.IsNullOrEmpty(timeStart))
         {
            args.Add("--timestart");
            args.Add(timeStart);

            if (!string.IsNullOrEmpty(timeSpan))
            {
               args.Add("--timespan");
               args.Add(timeSpan);
            }
         }

         // Raw log line
         if (chkRawLogLine.IsChecked == true)
            args.Add("--rawlogline");

         // Validate at least one view selected
         bool anyView = apChecked.Count > 0
                     || spChecked.Count > 0
                     || chkSPFlatAll.IsChecked == true
                     || chkWinCEAll.IsChecked == true;

         if (!anyView)
            throw new InvalidOperationException("Please select at least one view.");

         return string.Join(" ", args);
      }

      // =================================================================
      //  Run
      // =================================================================
      private async void btnRun_Click(object sender, RoutedEventArgs e)
      {
         string? exePath = FindSplogParser();
         if (exePath == null)
         {
            MessageBox.Show(
               "Cannot find splogparser.exe.\n\nLooked in:\n" +
               "• Same folder as hylogparser.exe\n" +
               "• dist\\ folder up from the repo root",
               "hylogparser", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
         }

         string arguments;
         try
         {
            arguments = BuildArguments();
         }
         catch (InvalidOperationException ex)
         {
            MessageBox.Show(ex.Message, "hylogparser", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
         }

         // UI state: running
         SetRunningState(true);
         txtOutput.Clear();
         AppendOutput($"[hylogparser] {exePath}");
         AppendOutput($"[hylogparser] args: {arguments}");
         AppendOutput($"[hylogparser] cwd:  {_workingDirectory}");
         AppendOutput("");

         try
         {
            var psi = new ProcessStartInfo
            {
               FileName = exePath,
               Arguments = arguments,
               WorkingDirectory = _workingDirectory,
               UseShellExecute = false,
               CreateNoWindow = true,
               RedirectStandardOutput = true,
               RedirectStandardError = true,
            };

            _runningProcess = new Process { StartInfo = psi, EnableRaisingEvents = true };

            _runningProcess.OutputDataReceived += (_, ev) =>
            {
               if (ev.Data != null)
                  Dispatcher.BeginInvoke(() => AppendOutput(ev.Data));
            };

            _runningProcess.ErrorDataReceived += (_, ev) =>
            {
               if (ev.Data != null)
                  Dispatcher.BeginInvoke(() => AppendOutput($"[ERR] {ev.Data}"));
            };

            _runningProcess.Start();
            _runningProcess.BeginOutputReadLine();
            _runningProcess.BeginErrorReadLine();

            await Task.Run(() => _runningProcess.WaitForExit());

            int exitCode = _runningProcess.ExitCode;
            AppendOutput("");
            AppendOutput($"[hylogparser] splogparser exited with code {exitCode}");
            lblStatus.Text = exitCode == 0 ? "Completed successfully" : $"Exited with code {exitCode}";

            // Open Excel and close the app on success
            if (exitCode == 0 && chkOpenExcel.IsChecked == true)
            {
               var xlsx = new DirectoryInfo(_workingDirectory)
                  .GetFiles("*.xlsx")
                  .OrderByDescending(f => f.LastWriteTime)
                  .FirstOrDefault();

               if (xlsx != null)
               {
                  Process.Start(new ProcessStartInfo(xlsx.FullName) { UseShellExecute = true });
                  Close();
                  return;
               }
            }
         }
         catch (Exception ex)
         {
            AppendOutput($"[hylogparser] ERROR: {ex.Message}");
            lblStatus.Text = "Error";
         }
         finally
         {
            _runningProcess = null;
            SetRunningState(false);
         }
      }

      // =================================================================
      //  Cancel
      // =================================================================
      private void btnCancel_Click(object sender, RoutedEventArgs e)
      {
         if (_runningProcess is { HasExited: false })
         {
            try
            {
               _runningProcess.Kill(entireProcessTree: true);
               AppendOutput("[hylogparser] Process killed.");
            }
            catch { /* may have already exited */ }
         }
      }

      // =================================================================
      //  Helpers
      // =================================================================
      private void SetRunningState(bool running)
      {
         btnRun.IsEnabled = !running;
         btnCancel.IsEnabled = running;
         lblStatus.Text = running ? "Running…" : "Ready";
      }

      private void AppendOutput(string line)
      {
         txtOutput.AppendText(line + Environment.NewLine);
         txtOutput.ScrollToEnd();
      }
   }
}
