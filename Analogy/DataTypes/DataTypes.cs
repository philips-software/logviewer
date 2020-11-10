using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Analogy.DataTypes
{
    public struct FactoryCheckItem
    {
        public string Name { get; }
        public Guid ID { get; }
        public string Description { get; }
        public Image Image { get; }

        public FactoryCheckItem(string name, Guid id, string description, Image image)
        {
            Name = name;
            ID = id;
            Description = description;
            Image = image;
        }

        public override string ToString() => $"{Name} ({ID})";
    }
    [Serializable]
    public class AnalogyPositionState
    {
        public FormWindowState WindowState { get; set; }
        public Point Location { get; set; }
        public Size Size { get; set; }
        public bool RememberLastPosition { get; set; }
        public AnalogyPositionState()
        {
            WindowState = FormWindowState.Maximized;
            RememberLastPosition = true;
        }

        public AnalogyPositionState(FormWindowState formWindowState, Point location, Size size)
        {
            WindowState = formWindowState;
            Location = location;
            Size = size;
        }

    }
    [Serializable]
    public class FilterCriteriaUIOption
    {
        public string DisplayMember { get; set; }
        public string ValueMember { get; set; }
        public bool CheckMember { get; set; }


        public FilterCriteriaUIOption(string displayMember, string valueMember, bool checkMember)
        {
            DisplayMember = displayMember;
            ValueMember = valueMember;
            CheckMember = checkMember;
        }
    }
    public enum DataSourceType
    {
        Client,
        Server,
        NetworkPath,
        LocalPath
    }
    [Serializable]
    public class DataSource
    {
        public string HostNameOrIP { get; set; }
        public string Path { get; set; }
        public DataSourceType Type { get; set; }
        public string DisplayName { get; set; }
        public DataSource(string hostNameOrIp, string path, string displayName, DataSourceType type)
        {
            HostNameOrIP = hostNameOrIp ?? throw new ArgumentNullException(nameof(hostNameOrIp));
            Path = path ?? throw new ArgumentNullException(nameof(path));
            Type = type;
            DisplayName = !string.IsNullOrEmpty(displayName) ? displayName : hostNameOrIp;
        }

        public override string ToString()
        {
            return $"{DisplayName} ({Type})";
        }
    }
    

    [Serializable]
    public class FontSettings
    {
        public FontSelectionType FontSelectionType { get; set; }
        public FontSelectionType MenuFontSelectionType { get; set; }
        public float GridFontSize { get; set; }
        public string FontName { get; set; }
        public float FontSize { get; set; }
        [JsonIgnore] public Font UIFont => new Font(FontName, FontSize,FontStyle.Regular,GraphicsUnit.Point);
        public string MenuFontName { get; set; }
        public float MenuFontSize { get; set; }
        [JsonIgnore] public Font MenuFont => new Font(MenuFontName, MenuFontSize, FontStyle.Regular, GraphicsUnit.Point);
        public FontSettings()
        {
            FontSelectionType = FontSelectionType.Normal;
            MenuFontSelectionType = FontSelectionType.Normal;
            GridFontSize = 8.5f;
            FontName = "Tahoma";
            FontSize = 8.25f;
            MenuFontName = "Segoe UI";
            MenuFontSize = 12f;
        }

        public void SetFontSelectionType(FontSelectionType mode)
        {
            FontSelectionType = mode;
            switch (mode)
            {
                case FontSelectionType.Large:
                    FontName = "Tahoma";
                    FontSize =10f;
                    break;
                case FontSelectionType.VeryLarge:
                    FontName = "Tahoma";
                    FontSize = 14f;
                    break;
                case FontSelectionType.Normal:
                case FontSelectionType.Manual:
                default:
                    FontName = "Tahoma";
                    FontSize = 8.25f;
                    break;
            }
        }
        public void SetMenuFontSelectionType(FontSelectionType mode)
        {
            MenuFontSelectionType = mode;
            switch (mode)
            {
                case FontSelectionType.Large:
                    MenuFontName = "Segoe UI";
                    MenuFontSize = 14f;
                    break;
                case FontSelectionType.VeryLarge:
                    MenuFontName = "Segoe UI";
                    MenuFontSize = 16f;
                    break;
                case FontSelectionType.Normal:
                case FontSelectionType.Manual:
                default:
                    MenuFontName = "Segoe UI";
                    MenuFontSize = 12f;
                    break;
            }
        }
    }
    public class MyWebClient : WebClient
    {
        /// <summary>
        ///     Response Uri after any redirects.
        /// </summary>
        public Uri ResponseUri;

        /// <inheritdoc />
        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            WebResponse webResponse = base.GetWebResponse(request, result);
            ResponseUri = webResponse.ResponseUri;
            return webResponse;
        }
    }
}
