using Analogy.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Utils.Drawing.Helpers;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Analogy
{
    static class Program
    {
        public const int WM_COPYDATA = 0x004A;

        [DllImport("user32", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(IntPtr Hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        private static UserSettingsManager Settings => UserSettingsManager.UserSettings;
        private static string AssemblyLocation;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            AssemblyLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            WindowsFormsSettings.LoadApplicationSettings();
            WindowsFormsSettings.DefaultFont = Settings.FontSettings.UIFont;
            WindowsFormsSettings.DefaultMenuFont = Settings.FontSettings.MenuFont;
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            if (Settings.EnableFirstChanceException)
            {
                AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            }

            Settings.OnEnableFirstChanceExceptionChanged += (s, e) =>
            {
                if (e)
                {
                    AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
                }
                else
                {
                    AppDomain.CurrentDomain.FirstChanceException -= CurrentDomain_FirstChanceException;
                }
            };
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();
#if NETCOREAPP3_1
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
#endif
            Settings.IncreaseNumberOfLaunches();
            if (!string.IsNullOrEmpty(Settings.ApplicationSkinName))
            {
                if (!string.IsNullOrEmpty(Settings.ApplicationSvgPaletteName))
                {
                    UserLookAndFeel.Default.SetSkinStyle(Settings.ApplicationSkinName, Settings.ApplicationSvgPaletteName);
                }
                else
                {
                    UserLookAndFeel.Default.SetSkinStyle(Settings.ApplicationSkinName);
                }

                UserLookAndFeel.Default.Style = Settings.ApplicationStyle;
            }

            UserLookAndFeel.Default.StyleChanged += (s, e) =>
            {
                UserLookAndFeel laf = (UserLookAndFeel)s;
                Settings.ApplicationSkinName = laf.ActiveSkinName;
                Settings.ApplicationStyle = laf.Style;
                Settings.ApplicationSvgPaletteName = laf.ActiveSvgPaletteName;

            };

            if (UserSettingsManager.UserSettings.SingleInstance &&
                Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {

                if (Environment.GetCommandLineArgs().Length == 2)
                {
                    var otherAnalogy = GetAlreadyRunningInstance();
                    if (otherAnalogy != null)
                    {
                        SendDataMessage(otherAnalogy, Environment.GetCommandLineArgs()[1]);
                    }
                }
                else
                {
                    XtraMessageBox.Show("Single instance is on. Exiting this instance", "Analogy");
                }

                return;
            }

            if (Settings.IsFirstRun)
            {
                FirstTimeRunForm f = new FirstTimeRunForm();
                f.ShowDialog();
            }
            Application.Run(new MainForm());

        }

        public static void SendDataMessage(Process targetProcess, string msg)
        {
            //Copy the string message to a global memory area in unicode format
            IntPtr _stringMessageBuffer = Marshal.StringToHGlobalUni(msg);

            //Prepare copy data structure
            NativeMethods.COPYDATASTRUCT _copyData = new NativeMethods.COPYDATASTRUCT();
            _copyData.dwData = IntPtr.Zero;
            _copyData.lpData = _stringMessageBuffer;
            _copyData.cbData =
                msg.Length * 2; //Number of bytes required for marshalling this string as a series of unicode characters
            IntPtr _copyDataBuff = IntPtrAlloc(_copyData);

            //Send message to the other process
            SendMessage(targetProcess.MainWindowHandle, WM_COPYDATA, IntPtr.Zero, _copyDataBuff);

            Marshal.FreeHGlobal(_copyDataBuff);
            Marshal.FreeHGlobal(_stringMessageBuffer);
        }

        // Allocate a pointer to an arbitrary structure on the global heap.
        private static IntPtr IntPtrAlloc<T>(T param)
        {
            IntPtr retval = Marshal.AllocHGlobal(Marshal.SizeOf(param));
            Marshal.StructureToPtr(param, retval, false);
            return retval;
        }

        private static Process GetAlreadyRunningInstance()
        {
            var current = Process.GetCurrentProcess();
            return Process.GetProcessesByName(current.ProcessName).FirstOrDefault(p => p.Id != current.Id);
        }

        private static void CurrentDomain_FirstChanceException(object sender,
            System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            AnalogyLogger.Instance.LogWarning(e.Exception.ToString(), nameof(CurrentDomain_FirstChanceException));
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            AnalogyLogger.Instance.LogException("Error: " + e.ExceptionObject, e.ExceptionObject as Exception, nameof(CurrentDomain_UnhandledException));
            MessageBox.Show("Error: " + e.ExceptionObject, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            AnalogyLogger.Instance.LogException("Error: " + e.Exception, e.Exception, nameof(Application_ThreadException));
            MessageBox.Show("Error: " + e.Exception, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }


        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // missing resources are... missing
            if (args.Name.Contains(".resources"))
            {
                return null;
            }

            // check for assemblies already loaded
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == args.Name);
            if (assembly != null)
            {
                return assembly;
            }

            // Try to load by filename - split out the filename of the full assembly name
            // and append the base path of the original assembly (ie. look in the same dir)
            // NOTE: this doesn't account for special search paths but then that never
            //           worked before either.
            string filename = args.Name.Split(',')[0] + ".dll".ToLower();



            var paths = FactoriesManager.Instance.ProbingPaths.Select(Path.GetDirectoryName).Except(new List<string> { AssemblyLocation }).Distinct()
                .ToList();

            foreach (var path in paths)
            {
                string asmFile = FindFileInPath(filename, path);
                if (!string.IsNullOrEmpty(asmFile))
                {
                    try
                    {
                        string finalPath = Path.GetDirectoryName(asmFile);
                        if (!string.IsNullOrEmpty(finalPath) && paths.Count == 1)
                        {
                            Environment.CurrentDirectory = finalPath;
                        }

                        return Assembly.LoadFrom(asmFile);
                    }
                    catch
                    {
                        //ignore
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Looks for the first match in a file structure
        /// </summary>
        /// <param name="filename">The filename only to look for</param>
        /// <param name="path">Path to start with</param>
        /// <returns>Fully qualified path of the file found or NULL</returns>
        private static string FindFileInPath(string filename, string path)
        {

            foreach (var fullFile in Directory.GetFiles(path))
            {
                var file = Path.GetFileName(fullFile);
                if (file.Equals(filename, StringComparison.InvariantCultureIgnoreCase))
                {
                    return fullFile;
                }
            }

            foreach (var dir in Directory.GetDirectories(path))
            {
                var file = FindFileInPath(filename, dir);
                if (!string.IsNullOrEmpty(file))
                {
                    return file;
                }
            }

            return null;
        }


    }
}
