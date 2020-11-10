using Analogy.Interfaces.Factories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;

namespace Analogy.Managers
{
    public class FactoryContainer
    {
        public bool AssemblyExist => File.Exists(AssemblyFullPath);
        public string AssemblyFullPath { get; }
        public Assembly Assembly { get; }
        public IAnalogyFactory Factory { get; }
        public FactorySettings FactorySetting { get; }
        public IAnalogyDownloadInformation? DownloadInformation { get; set; }
        public List<IAnalogyCustomActionsFactory> CustomActionsFactories { get; }
        public List<IAnalogyDataProvidersFactory> DataProvidersFactories { get; }
        public List<IAnalogyDataProviderSettings> DataProvidersSettings { get; }
        public List<IAnalogyShareableFactory> ShareableFactories { get; }
        public List<IAnalogyExtensionsFactory> ExtensionsFactories { get; }
        public List<IAnalogyImages> Images { get; private set; }
        public FactoryContainer(Assembly assembly, string assemblyFullPath, IAnalogyFactory factory, FactorySettings factorySetting)
        {
            Assembly = assembly;
            AssemblyFullPath = assemblyFullPath;
            Factory = factory;
            FactorySetting = factorySetting;
            CustomActionsFactories = new List<IAnalogyCustomActionsFactory>();
            DataProvidersFactories = new List<IAnalogyDataProvidersFactory>();
            DataProvidersSettings = new List<IAnalogyDataProviderSettings>();
            ShareableFactories = new List<IAnalogyShareableFactory>();
            ExtensionsFactories = new List<IAnalogyExtensionsFactory>();
            Images=new List<IAnalogyImages>();
        }


        public void AddDataProviderFactory(IAnalogyDataProvidersFactory dataProvidersFactory) =>
            DataProvidersFactories.Add(dataProvidersFactory);

        public void AddDataProvidersSettings(IAnalogyDataProviderSettings settings) =>
            DataProvidersSettings.Add(settings);

        public void AddCustomActionFactory(IAnalogyCustomActionsFactory action) => CustomActionsFactories.Add(action);

        public void AddShareableFactory(IAnalogyShareableFactory shareableFactory) =>
            ShareableFactories.Add(shareableFactory);

        public void AddExtensionFactory(IAnalogyExtensionsFactory extensionFactory) =>
            ExtensionsFactories.Add(extensionFactory);
        public void AddImages(IAnalogyImages images) => Images.Add(images);

        public void AddDownloadInformation(IAnalogyDownloadInformation downloadInformation)
            => DownloadInformation = downloadInformation;
        public override string ToString() => $"{nameof(Factory)}: {Factory}, {nameof(Assembly)}: {Assembly}";

        public bool ContainsDataProviderOrDataFactory(Guid componentId)
        {
            var contains =
            DataProvidersFactories.Any(d =>
                d.FactoryId == componentId ||
                d.DataProviders.Any(dp => dp.Id == componentId));
            return contains;
        }
    }
}
