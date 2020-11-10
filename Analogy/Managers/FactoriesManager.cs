using Analogy.DataProviders;
using Analogy.DataTypes;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using Analogy.Managers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Analogy
{
    public class FactoriesManager
    {

        private static readonly Lazy<FactoriesManager>
            _instance = new Lazy<FactoriesManager>(() => new FactoriesManager());

        public List<string> ProbingPaths { get; set; } = new List<string>();
        public static readonly FactoriesManager Instance = _instance.Value;
        public List<FactoryContainer> BuiltInFactories { get; }
        public List<FactoryContainer> Factories { get; }

        public FactoriesManager()
        {
            Factories = new List<FactoryContainer>();
            BuiltInFactories = new List<FactoryContainer>();
            var analogyFactory = new AnalogyBuiltInFactory();
            var currentAssembly = Assembly.GetExecutingAssembly();
            var analogyFactorySetting = UserSettingsManager.UserSettings.GetOrAddFactorySetting(analogyFactory);
            analogyFactorySetting.FactoryName = analogyFactory.Title;
            FactoryContainer fc = new FactoryContainer(currentAssembly, Environment.CurrentDirectory, analogyFactory,
                analogyFactorySetting);
            fc.AddDataProviderFactory(new AnalogyOfflineDataProviderFactory());
            fc.AddCustomActionFactory(new AnalogyCustomActionFactory());
            BuiltInFactories.Add(fc);
        }

        public async Task InitializeBuiltInFactories()
        {

            var dataProviders = BuiltInFactories
                .Where(f => f.FactorySetting.Status != DataProviderFactoryStatus.Disabled)
                .SelectMany(fc => fc.DataProvidersFactories.SelectMany(d => d.DataProviders)).ToList();
            var initTasks = dataProviders.Select(d => d.InitializeDataProviderAsync(AnalogyLogger.Instance))
                .ToList();
            var completion = Task.WhenAll(initTasks);
            try
            {
                await completion;
            }
            catch (AggregateException ex)
            {
                AnalogyLogger.Instance.LogException("Error during Initialization", ex, "InitializeBuiltInFactories");
            }

            foreach (var t in initTasks)
            {
                if (t.Status != TaskStatus.RanToCompletion)
                {
                    AnalogyLogger.Instance.LogException("Error during Initialization", t.Exception, "AddExternalDataSources");
                }
            }
        }

        public async Task AddExternalDataSources()
        {
            ExternalDataProviders result = await ExternalDataProviders.GetExternalDataProviders();
            var factoryContainers = result.Factories.Where(f => !Factories.Contains(f)).ToList();
            Factories.AddRange(factoryContainers);
            Factories.RemoveAll(f => !f.AssemblyExist);

            var dataProviders = factoryContainers.Where(f =>
                    f.FactorySetting.Status != DataProviderFactoryStatus.Disabled)
                .SelectMany(fc => fc.DataProvidersFactories.SelectMany(d => d.DataProviders)).ToList();
            var initTasks = new List<Task>();
            foreach (var provider in dataProviders)
            {
                try
                {
                    initTasks.Add(provider.InitializeDataProviderAsync(AnalogyLogger.Instance));

                }
                catch (Exception e)
                {
                    AnalogyLogger.Instance.LogException($"Error during Initialization of {provider.OptionalTitle}", e, "AddExternalDataSources");

                }
            }

            try
            {
                await Task.WhenAll(initTasks);
            }
            catch (AggregateException ex)
            {
                AnalogyLogger.Instance.LogException("Error during Initialization", ex, "AddExternalDataSources");
            }

            foreach (var t in initTasks)
            {
                if (t.Status != TaskStatus.RanToCompletion)
                {
                    AnalogyLogger.Instance.LogException("Error during Initialization", t.Exception, "AddExternalDataSources");
                }
            }
        }

        public IEnumerable<(IAnalogyOfflineDataProvider DataProvider, Guid FactoryID)> GetSupportedOfflineDataSources(
            string[] fileNames)
        {
            foreach (var factory in Factories.Where(f => f.FactorySetting.Status != DataProviderFactoryStatus.Disabled))
            {
                foreach (var dataProvidersFactory in factory.DataProvidersFactories)
                {
                    var supported = dataProvidersFactory.DataProviders.Where(i =>
                        i is IAnalogyOfflineDataProvider offline && offline.CanOpenAllFiles(fileNames));
                    foreach (IAnalogyDataProvider dataSource in supported)
                    {
                        yield return (dataSource as IAnalogyOfflineDataProvider,
                            dataProvidersFactory.FactoryId);
                    }
                }
            }
        }

        public IEnumerable<IAnalogyOfflineDataProvider> GetSupportedOfflineDataSourcesFromFactory(Guid factoryId,
            string[] fileNames)
        {
            return GetSupportedOfflineDataSources(fileNames).Where(res => res.FactoryID == factoryId)
                .Select(res => res.DataProvider);
        }

        public IEnumerable<(string Name, Guid ID, Image Image, string Description)> GetRealTimeDataSourcesNamesAndIds()
        {
            foreach (var fc in Factories)
            {
                foreach (var dpf in fc.DataProvidersFactories)
                {
                    IEnumerable<IAnalogyDataProvider> supported =
                        dpf.DataProviders.Where(d => d is IAnalogyRealTimeDataProvider);
                    foreach (var analogyDataSource in supported)
                    {
                        var dataSource = (IAnalogyRealTimeDataProvider)analogyDataSource;
                        yield return (dpf.Title, dataSource.Id, GetLargeImage(dataSource.Id), dpf.Title);
                    }
                }
            }
        }

        public Assembly GetAssemblyOfFactory(IAnalogyFactory factory)
            => BuiltInFactories.Exists(f => f.Factory == factory)
                ? BuiltInFactories.First(f => f.Factory == factory).Assembly
                : Factories.Single(f => f.Factory == factory).Assembly;


        public FactoryContainer GetBuiltInFactoryContainer(Guid id) =>
            BuiltInFactories.Single(f => f.Factory.FactoryId == id);

        public bool IsBuiltInFactory(IAnalogyFactory factory) => IsBuiltInFactory(factory.FactoryId);

        public bool IsBuiltInFactory(Guid factoryId) =>
            BuiltInFactories.Exists(fc => fc.Factory.FactoryId == factoryId);

        public List<IAnalogyDataProviderSettings> GetProvidersSettings() => Factories
            .Where(f => f.FactorySetting.Status != DataProviderFactoryStatus.Disabled)
            .SelectMany(f => f.DataProvidersSettings)
            .ToList();

        public Image GetLargeImage(Guid componentId)
        {
            foreach (var factoryContainer in Factories.Where(f => f.ContainsDataProviderOrDataFactory(componentId)))
            {
                if (factoryContainer.Factory.LargeImage != null)
                {
                    return factoryContainer.Factory.LargeImage;
                }
            }

            return null;
        }

        public FactoryContainer GetFactoryContainer(Guid componentId)
            => Factories.SingleOrDefault(f => f.ContainsDataProviderOrDataFactory(componentId));

        public Image GetSmallImage(Guid componentId)
        {
            foreach (var factoryContainer in Factories.Where(f => f.ContainsDataProviderOrDataFactory(componentId)))
            {
                if (factoryContainer.Factory.SmallImage != null)
                {
                    return factoryContainer.Factory.SmallImage;
                }
            }

            return null;
        }

        public IEnumerable<IAnalogyExtension> GetExtensions(IAnalogyDataProvider dataProvider)
            => GetAllExtensions().Where(e => e.TargetProviderId == dataProvider.Id);

        public IEnumerable<IAnalogyExtension> GetAllExtensions()
        {
            foreach (var factory in Factories)
            {
                if (factory.FactorySetting.Status == DataProviderFactoryStatus.Disabled)
                {
                    continue;
                }

                foreach (var extensionFactory in factory.ExtensionsFactories)
                {
                    foreach (IAnalogyExtension extension in extensionFactory.Extensions)
                    {
                        yield return extension;
                    }
                }
            }
        }

        public FactoryContainer FactoryContainer(Guid componentId)
            => IsBuiltInFactory(componentId)
                ? BuiltInFactories.FirstOrDefault(f => f.Factory.FactoryId == componentId ||
                                                       f.DataProvidersFactories.Any(dpf =>
                                                           dpf.DataProviders.Any(dp => dp.Id == componentId)))
                : Factories.FirstOrDefault(f => f.Factory.FactoryId == componentId ||
                                                f.DataProvidersFactories.Any(dpf =>
                                                    dpf.DataProviders.Any(dp => dp.Id == componentId)));

    }
}