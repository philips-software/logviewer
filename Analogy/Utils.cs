using Analogy.Interfaces;
using DevExpress.LookAndFeel;
using DevExpress.XtraBars.Ribbon;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Analogy.DataTypes;
using Analogy.Interfaces.DataTypes;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace Analogy
{
    public static class Utils
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct LASTINPUTINFO
        {
            public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dwTime;
        }
        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        /// <summary>
        /// No filter. All allowed
        /// </summary>
        internal const string DateFilterNone = "All";
        /// <summary>
        /// From Today 
        /// </summary>
        internal const string DateFilterToday = "Today";
        /// <summary>
        /// From last 2 days 
        /// </summary>
        internal const string DateFilterLast2Days = "Last 2 days";
        /// <summary>
        /// From last 3 days
        /// </summary>
        internal const string DateFilterLast3Days = "Last 3 days";
        /// <summary>
        /// From last week
        /// </summary>
        internal const string DateFilterLastWeek = "Last one week";
        /// <summary>
        /// From last 2 weeks
        /// </summary>
        internal const string DateFilterLast2Weeks = "Last 2 weeks";
        /// <summary>
        /// From last month
        /// </summary>
        internal const string DateFilterLastMonth = "Last one month";

        private static Regex HasQuestionMarkRegEx = new Regex(@"\?", RegexOptions.Compiled);
        private static Regex IllegalCharactersRegex = new Regex("[" + @"\/:<>|" + "\"]", RegexOptions.Compiled);
        private static Regex CatchExtentionRegex = new Regex(@"^\s*.+\.([^\.]+)\s*$", RegexOptions.Compiled);
        private static string NonDotCharacters = @"[^.]*";
        public static List<string> LogLevels { get; } = Enum.GetValues(typeof(AnalogyLogLevel)).Cast<AnalogyLogLevel>().Select(e => e.ToString()).ToList();



        //
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="filename"></param>
        public static void SerializeToBinaryFile<T>(T item, string filename)
        {
            var formatter = new BinaryFormatter();
            var directoryName = Path.GetDirectoryName(filename);
            try
            {
                if (!string.IsNullOrEmpty(directoryName) && !(Directory.Exists(directoryName)))
                {
                    Directory.CreateDirectory(directoryName);
                }

                using (Stream myWriter = File.Open(filename, FileMode.Create, FileAccess.ReadWrite))
                {
                    formatter.Serialize(myWriter, item);
                }
            }
            catch (SerializationException ex)
            {
                throw new Exception("GeneralDataUtils: Error in SerializeToBinaryFile", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static T DeSerializeBinaryFile<T>(string filename) where T : class, new()
        {
            var formatter = new BinaryFormatter();
            if (File.Exists(filename))
            {
                try
                {
                    using (Stream myReader = File.Open(filename, FileMode.Open, FileAccess.Read))
                    {
                        return (T)formatter.Deserialize(myReader);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("GeneralDataUtils: Error in DeSerializeBinaryFile", ex);
                }
            }

            throw new FileNotFoundException("GeneralDataUtils: File does not exist: " + filename, filename);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DataTable DataTableConstructor()
        {

            DataTable dtb = new DataTable();
            dtb.Columns.Add(new DataColumn("Date", typeof(DateTime)));
            dtb.Columns.Add(new DataColumn("TimeDiff", typeof(string)));
            dtb.Columns.Add(new DataColumn("Text", typeof(string)));
            dtb.Columns.Add(new DataColumn("Source", typeof(string)));
            dtb.Columns.Add(new DataColumn("Level", typeof(string)));
            dtb.Columns.Add(new DataColumn("Class", typeof(string)));
            dtb.Columns.Add(new DataColumn("Category", typeof(string)));
            dtb.Columns.Add(new DataColumn("User", typeof(string)));
            dtb.Columns.Add(new DataColumn("Module", typeof(string)));
            dtb.Columns.Add(new DataColumn("Object", typeof(object)));
            dtb.Columns.Add(new DataColumn("ProcessID", typeof(int)));
            dtb.Columns.Add(new DataColumn("ThreadID", typeof(int)));
            dtb.Columns.Add(new DataColumn("DataProvider", typeof(string)));
            dtb.Columns.Add(new DataColumn("MachineName", typeof(string)));
            var manager = ExtensionsManager.Instance;
            foreach (IAnalogyExtension extension in manager.InPlaceRegisteredExtensions)
            {
                var columns = extension.GetColumnsInfo();
                foreach (AnalogyColumnInfo column in columns)
                {
                    dtb.Columns.Add(new DataColumn(column.ColumnName, column.ColumnType));
                }
            }
            dtb.DefaultView.AllowNew = false;
            dtb.DefaultView.RowStateFilter = DataViewRowState.Unchanged;
            dtb.DefaultView.Sort = UserSettingsManager.UserSettings.DefaultDescendOrder ?
                "Date DESC" : "Date ASC";

            return dtb;
        }

        public static void SetSkin(Control control, string skinName)
        {
            if (control is RibbonControl ribbonControl)
            {
                SetLookAndFellSkin(ribbonControl.GetController().LookAndFeel, skinName);
                return;
            }
            if (control is ISupportLookAndFeel feel)
            {
                SetLookAndFellSkin(feel.LookAndFeel, skinName);
            }
            foreach (Control c in control.Controls)
            {
                SetSkin(c, skinName);
            }
        }
        private static void SetLookAndFellSkin(UserLookAndFeel lookAndFeel, string skinName)
        {
            lookAndFeel.UseDefaultLookAndFeel = false;
            lookAndFeel.SkinName = skinName;
        }

        public static string GetFileNameAsDataSource(string fileName)
        {
            string file = Path.GetFileName(fileName);
            return fileName != null && fileName.Equals(file) ? fileName : $"{file} ({fileName})";
        }

        static long GetLastInputTime()
        {
            uint idleTime = 0;
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;

            uint envTicks = (uint)Environment.TickCount;

            if (GetLastInputInfo(ref lastInputInfo))
            {
                uint lastInputTick = lastInputInfo.dwTime;

                idleTime = envTicks - lastInputTick;
            }

            return ((idleTime > 0) ? (idleTime / 1000) : 0);
        }
        public static TimeSpan IdleTime() => TimeSpan.FromSeconds(GetLastInputTime());

        public static bool MatchedAll(string pattern, IEnumerable<string> files)
        {
            Regex reg = Convert(pattern);
            return files.All(f => reg.IsMatch(f));
        }
        public static Regex Convert(string pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException();
            }
            pattern = pattern.Trim();
            if (pattern.Length == 0)
            {
                throw new ArgumentException("Pattern is empty.");
            }
            if (IllegalCharactersRegex.IsMatch(pattern))
            {
                throw new ArgumentException("Pattern contains illegal characters.");
            }
            bool hasExtension = CatchExtentionRegex.IsMatch(pattern);
            bool matchExact = false;
            if (HasQuestionMarkRegEx.IsMatch(pattern))
            {
                matchExact = true;
            }
            else if (hasExtension)
            {
                matchExact = CatchExtentionRegex.Match(pattern).Groups[1].Length != 3;
            }
            string regexString = Regex.Escape(pattern);
            regexString = "^" + Regex.Replace(regexString, @"\\\*", ".*");
            regexString = Regex.Replace(regexString, @"\\\?", ".");
            if (!matchExact && hasExtension)
            {
                regexString += NonDotCharacters;
            }
            regexString += "$";
            Regex regex = new Regex(regexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return regex;
        }
        //public static async Task<(bool newData, T result)> GetAsync<T>(string uri, string token, DateTime lastModified)
        //{
        //    try
        //    {
        //        Uri myUri = new Uri(uri);
        //        HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
        //        myHttpWebRequest.Accept = "application/json";
        //        myHttpWebRequest.UserAgent = "Analogy";
        //        if (!string.IsNullOrEmpty(token))
        //            myHttpWebRequest.Headers.Add(HttpRequestHeader.Authorization, $"Token {token}");

        //        myHttpWebRequest.IfModifiedSince = lastModified;

        //        HttpWebResponse myHttpWebResponse = (HttpWebResponse)await myHttpWebRequest.GetResponseAsync();
        //        if (myHttpWebResponse.StatusCode == HttpStatusCode.NotModified)
        //            return (false, default)!;

        //        using (var reader = new System.IO.StreamReader(myHttpWebResponse.GetResponseStream()))
        //        {
        //            string responseText = await reader.ReadToEndAsync();
        //            return (true, JsonConvert.DeserializeObject<T>(responseText));
        //        }
        //    }
        //    catch (WebException e) when (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.NotModified)
        //    {
        //        return (false, default)!;
        //    }
        //    catch (Exception)
        //    {
        //        return (false, default)!;
        //    }
        //}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DataRow CreateRow(DataTable table, AnalogyLogMessage message, string dataSource, bool checkAdditionalInformation)
        {
            var dtr = table.NewRow();
            dtr.BeginEdit();
            dtr["Date"] = message.Date;
            dtr["Text"] = message.Text ?? "";
            dtr["Source"] = message.Source ?? "";
            dtr["Level"] = string.Intern(message.Level.ToString());
            dtr["Class"] = string.Intern(message.Class.ToString());
            dtr["Category"] = message.Category ?? "";
            dtr["User"] = message.User ?? "";
            dtr["Module"] = message.Module ?? "";
            dtr["Object"] = message;
            dtr["ProcessID"] = message.ProcessId;
            dtr["ThreadID"] = message.ThreadId;
            dtr["DataProvider"] = dataSource ?? string.Empty;
            dtr["MachineName"] = message.MachineName ?? string.Empty;
            if (checkAdditionalInformation && message.AdditionalInformation != null && message.AdditionalInformation.Any())
            {
                foreach (KeyValuePair<string, string> info in message.AdditionalInformation)
                {
                    if (dtr.Table.Columns.Contains(info.Key))
                    {
                        dtr[info.Key] = info.Value;
                    }
                    else
                    {
                        AnalogyLogger.Instance.LogError("",
                            $"key {info.Key} does not exist in table {table.TableName}");
                    }

                }

            }
            return dtr;
        }
        public static bool IsCompressedArchive(string filename)
        {
            return filename.EndsWith(".gz", StringComparison.InvariantCultureIgnoreCase) ||
                   filename.EndsWith(".zip", StringComparison.InvariantCultureIgnoreCase);
        }

        public static void SetLogLevel(CheckedListBoxControl chkLstLogLevel)
        {
            chkLstLogLevel.Items.Clear();
            switch (UserSettingsManager.UserSettings.LogLevelSelection)
            {
                case LogLevelSelectionType.Single:
                    chkLstLogLevel.CheckMode = CheckMode.Single;
                    chkLstLogLevel.CheckStyle = CheckStyles.Radio;
                    CheckedListBoxItem[] radioLevels = {
                        new CheckedListBoxItem("Trace"),
                        new CheckedListBoxItem("Error + Critical"),
                        new CheckedListBoxItem("Warning"),
                        new CheckedListBoxItem("Debug"),
                        new CheckedListBoxItem("Verbose")
                    };
                    chkLstLogLevel.Items.AddRange(radioLevels);
                    break;
                case LogLevelSelectionType.Multiple:
                    chkLstLogLevel.CheckMode = CheckMode.Multiple;
                    chkLstLogLevel.CheckStyle = CheckStyles.Standard;
                    chkLstLogLevel.Items.AddRange(LogLevels.Select(l => new CheckedListBoxItem(l, false)).ToArray());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void OpenLink(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo(url)
                {
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
            catch (Exception exception)
            {
                AnalogyLogger.Instance.LogException($"Error: {exception.Message}",exception, "");
            }
        }
     
        public static string CurrentDirectory()
        {
            string location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(location);
            return directory;
        }
    }
    
}