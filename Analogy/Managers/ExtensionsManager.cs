using Analogy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Analogy.Interfaces.DataTypes;

namespace Analogy
{
    public class ExtensionsManager : IExtensionsManager
    {
        private static readonly Lazy<ExtensionsManager> instance = new Lazy<ExtensionsManager>(() => new ExtensionsManager());
        public static ExtensionsManager Instance => instance.Value;
        private IAnalogyLogger Log { get; } = AnalogyLogger.Instance;
        private List<IAnalogyExtension> LoadedExtensions { get; } = new List<IAnalogyExtension>();
        private readonly List<IAnalogyExtension> registeredExtensions = new List<IAnalogyExtension>();
        public bool HasAny => RegisteredExtensions.Any();
        public IEnumerable<IAnalogyExtension> RegisteredExtensions => registeredExtensions.ToList();
        public bool HasAnyInPlace => InPlaceRegisteredExtensions.Any();
        public bool HasAnyUserControl => UserControlRegisteredExtensions.Any();

        public IEnumerable<IAnalogyExtension> InPlaceRegisteredExtensions =>
            registeredExtensions.Where(e => e.ExtensionType == AnalogyExtensionType.InPlace).ToList();
        public IEnumerable<IAnalogyExtension> UserControlRegisteredExtensions =>
            registeredExtensions.Where(e => e.ExtensionType == AnalogyExtensionType.UserControl).ToList();
        private int ColumnIndexes { get; set; } = 12;
        private readonly List<Tuple<IAnalogyExtension, AnalogyColumnInfo, int>> extensionsDataColumns =
            new List<Tuple<IAnalogyExtension, AnalogyColumnInfo, int>>();

        private ExtensionsManager()
        {

        }
        public int GetIndexForExtension(IAnalogyExtension extension)
            => extensionsDataColumns.Single(e => e.Item1 == extension).Item3;

        public void RegisterExtension(IAnalogyExtension extension)
        {
            if (extension == null)
            {
                return;
            }

            registeredExtensions.Add(extension);
            if (extension.ExtensionType == AnalogyExtensionType.InPlace)
            {
                var columns = extension.GetColumnsInfo();
                foreach (AnalogyColumnInfo column in columns)
                {
                    extensionsDataColumns.Add(
                        new Tuple<IAnalogyExtension, AnalogyColumnInfo, int>(extension, column, ColumnIndexes));
                    ColumnIndexes++;
                }
            }
        }

        public IEnumerable<IAnalogyExtension> GetExtensions() => FactoriesManager.Instance.GetAllExtensions();

    }
}
