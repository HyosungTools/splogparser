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
   //  Other parse-type definition — each has its own flag letter
   // ====================================================================
   public class OtherViewItem : ViewItem
   {
      public string Flag { get; set; } = "";
   }

   // ====================================================================
   //  Simple converter so GroupBox headers can span full width
   //  (subtracts padding so DockPanel fills the header area)
   // ====================================================================
   public class WidthMinusConverter : IValueConverter
   {
      public static readonly WidthMinusConverter Instance = new();

      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value is double width && parameter is string s && double.TryParse(s, out double subtract))
            return Math.Max(0, width - subtract);
         return value;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
         => throw new NotImplementedException();
   }

   // ====================================================================
   //  Main Window
   // ====================================================================
   public partial class MainWindow : Window
   {
      // ---------------------------------------------------------------
      //  View lists
      //  Adjust CLI names here if they differ from your Options class.
      // ---------------------------------------------------------------

      // -a flag: AP (Application Log) views
      private readonly List<ViewItem> _apViews = new()
      {
         new() { Display = "Over (Transaction Summary)",  CLI = "Over" },
         new() { Display = "Install (Installed Packages)", CLI = "Install" },
         new() { Display = "AddKey (Encryption Keys)",    CLI = "AddKey" },
         new() { Display = "XmlParam (Configuration)",    CLI = "XmlParam" },
         new() { Display = "WS (Web Service)",            CLI = "WS" },
         new() { Display = "EJ (Electronic Journal)",     CLI = "EJ" },
         new() { Display = "Disp (Cash Dispense)",        CLI = "Disp" },
         new() { Display = "DepositAnalysis",             CLI = "DepositAnalysis" },
      };

      // -s flag: SP (Service Provider / XFS) views
      private readonly List<ViewItem> _spViews = new()
      {
         new() { Display = "CDm (Cash Dispenser)",    CLI = "CDm" },
         new() { Display = "CIM (Cash-In Module)",    CLI = "CIM" },
         new() { Display = "IDC (Card Reader)",       CLI = "IDC" },
         new() { Display = "PIN (PIN Pad)",           CLI = "PIN" },
         new() { Display = "PTR (Printer)",           CLI = "PTR" },
         new() { Display = "SIU (Sensors/Indicators)", CLI = "SIU" },
         new() { Display = "BCR (Barcode Reader)",    CLI = "BCR" },
         new() { Display = "IPM (Image Processor)",   CLI = "IPM" },
         new() { Display = "CRD (Card Dispenser)",    CLI = "CRD" },
      };

      // Other parse types — each has its own CLI flag
      private readonly List<OtherViewItem> _otherViews = new()
      {
         new() { Display = "ActiveTeller",    CLI = "*", Flag = "-t" },
         new() { Display = "AT Extensions",   CLI = "*", Flag = "-e" },
         new() { Display = "Workstation",     CLI = "*", Flag = "-w" },
         new() { Display = "Server",          CLI = "*", Flag = "-v" },
         new() { Display = "SP Flat",         CLI = "*", Flag = "-p" },
         new() { Display = "RT",              CLI = "*", Flag = "-r" },
         new() { Display = "II",              CLI = "*", Flag = "-i" },
         new() { Display = "BE",              CLI = "*", Flag = "-b" },
         new() { Display = "SS",              CLI = "*", Flag = "-x" },
         new() { Display = "A2iA",            CLI = "*", Flag = "--a2" },
         new() { Display = "TCR",             CLI = "*", Flag = "--tcr" },
         new() { Display = "WinCE",           CLI = "*", Flag = "--wince" },
      };

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

         icOtherViews.ItemTemplate = MakeCheckBoxTemplate();
         icOtherViews.ItemsSource = _otherViews;

         // Pre-populate from context menu arg
         if (!string.IsNullOrEmpty(App.InitialZipPath) && File.Exists(App.InitialZipPath))
         {
            txtInputFile.Text = App.InitialZipPath;
         }

         // Show splogparser version
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
      //  Create a DataTemplate that binds CheckBox to ViewItem
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
      //  Locate splogparser.exe — same directory as hylogparser.exe
      // =================================================================
      private static string? FindSplogParser()
      {
         string dir = AppDomain.CurrentDomain.BaseDirectory;
         string candidate = Path.Combine(dir, "splogparser.exe");
         return File.Exists(candidate) ? candidate : null;
      }

      // =================================================================
      //  Browse
      // =================================================================
      private void btnBrowse_Click(object sender, RoutedEventArgs e)
      {
         var dlg = new Microsoft.Win32.OpenFileDialog
         {
            Title = "Select ATM log archive",
            Filter = "Zip Archives (*.zip)|*.zip|All Files (*.*)|*.*"
         };

         if (dlg.ShowDialog() == true)
            txtInputFile.Text = dlg.FileName;
      }

      // =================================================================
      //  Drag & Drop (whole window)
      // =================================================================
      private void Window_DragOver(object sender, DragEventArgs e)
      {
         e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop)
            ? DragDropEffects.Copy
            : DragDropEffects.None;
         e.Handled = true;
      }

      private void Window_Drop(object sender, DragEventArgs e)
      {
         if (e.Data.GetData(DataFormats.FileDrop) is string[] files && files.Length > 0)
         {
            txtInputFile.Text = files[0];
         }
      }

      // =================================================================
      //  All / None buttons
      // =================================================================
      private static void SetAll(IEnumerable<ViewItem> items, bool state)
      {
         foreach (var item in items) item.IsChecked = state;
      }

      private void RefreshAllLists()
      {
         icAPViews.Items.Refresh();
         icSPViews.Items.Refresh();
         icOtherViews.Items.Refresh();
      }

      private void btnAPAll_Click(object sender, RoutedEventArgs e)    { SetAll(_apViews, true);  RefreshAllLists(); }
      private void btnAPNone_Click(object sender, RoutedEventArgs e)   { SetAll(_apViews, false); RefreshAllLists(); }
      private void btnSPAll_Click(object sender, RoutedEventArgs e)    { SetAll(_spViews, true);  RefreshAllLists(); }
      private void btnSPNone_Click(object sender, RoutedEventArgs e)   { SetAll(_spViews, false); RefreshAllLists(); }
      private void btnOtherAll_Click(object sender, RoutedEventArgs e) { SetAll(_otherViews, true);  RefreshAllLists(); }
      private void btnOtherNone_Click(object sender, RoutedEventArgs e){ SetAll(_otherViews, false); RefreshAllLists(); }

      // =================================================================
      //  Build command-line arguments
      // =================================================================
      private string BuildArguments()
      {
         string inputFile = txtInputFile.Text.Trim();
         if (string.IsNullOrEmpty(inputFile))
            throw new InvalidOperationException("Please select a log archive or folder.");

         var args = new List<string>();

         // -f (input file) — always required
         args.Add("-f");
         args.Add($"\"{inputFile}\"");

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

         // Other parse-type flags
         foreach (var ov in _otherViews.Where(v => v.IsChecked))
         {
            args.Add(ov.Flag);
            args.Add(ov.CLI);
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
         bool anyView = apChecked.Count > 0 || spChecked.Count > 0 || _otherViews.Any(v => v.IsChecked);
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
               "Cannot find splogparser.exe.\n\nExpected location:\n" +
               Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "splogparser.exe"),
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

         // Working directory = folder containing the zip
         string workingDir = Path.GetDirectoryName(txtInputFile.Text.Trim()) ?? AppDomain.CurrentDomain.BaseDirectory;
         if (!Directory.Exists(workingDir))
            workingDir = AppDomain.CurrentDomain.BaseDirectory;

         // UI state: running
         SetRunningState(true);
         txtOutput.Clear();
         AppendOutput($"[hylogparser] {exePath}");
         AppendOutput($"[hylogparser] args: {arguments}");
         AppendOutput($"[hylogparser] cwd:  {workingDir}");
         AppendOutput("");

         try
         {
            var psi = new ProcessStartInfo
            {
               FileName = exePath,
               Arguments = arguments,
               WorkingDirectory = workingDir,
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

            // Wait asynchronously
            await Task.Run(() => _runningProcess.WaitForExit());

            int exitCode = _runningProcess.ExitCode;
            AppendOutput("");
            AppendOutput($"[hylogparser] splogparser exited with code {exitCode}");
            lblStatus.Text = exitCode == 0 ? "Completed successfully" : $"Exited with code {exitCode}";
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
         btnBrowse.IsEnabled = !running;
         txtInputFile.IsEnabled = !running;

         lblStatus.Text = running ? "Running…" : "Ready";
      }

      private void AppendOutput(string line)
      {
         txtOutput.AppendText(line + Environment.NewLine);
         txtOutput.ScrollToEnd();
      }
   }
}
