using System.Drawing;

namespace Analogy.DataTypes
{
    public class DownloadEntry
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Image  Icon { get; set; }
        public string URL { get; set; }
    }
}
