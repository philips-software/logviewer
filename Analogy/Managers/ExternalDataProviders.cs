using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Analogy.DataTypes;
using Analogy.Interfaces.DataTypes;

namespace Analogy.Managers
{
    public class ExternalDataProviders
    {
        private static readonly AsyncLazy<ExternalDataProviders> _instance =
            new AsyncLazy<ExternalDataProviders>(() => new ExternalDataProviders());

        public static async Task<ExternalDataProviders> GetExternalDataProviders() => await _instance.Start();
        public List<FactoryContainer> Factories { get; private set; }
        public List<(Guid id, IAnalogyImages images)> DataProvidersImages { get; set; }

        private ExternalDataProviders()
        {
            DataProvidersImages = new List<(Guid id, IAnalogyImages images)>();
            Factories = new List<FactoryContainer>();
            var analogyAssemblies = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory,
                @"*Analogy.LogViewer.*.dll", SearchOption.AllDirectories).ToList();
            if (UserSettingsManager.UserSettings.AdditionalProbingLocations != null)
            {
                foreach (string folder in UserSettingsManager.UserSettings.AdditionalProbingLocations)
                {
                    try
                    {
                        if (Directory.Exists(folder))
                        {
                            analogyAssemblies.AddRange(Directory.EnumerateFiles(folder, @"*Analogy.LogViewer.*.dll",
                                SearchOption.TopDirectoryOnly).ToList());
                        }
                    }
                    catch (Exception e)
                    {
                        AnalogyLogger.Instance.LogException($"Error probing folder {folder}. Error: {e.Message}", e,
                            nameof(ExternalDataProviders));
                    }
                }


            }

            var typesToLoad = new List<(Assembly assembly, string fileName, List<Type> types)>();
            foreach (string aFile in analogyAssemblies)
            {
                if (aFile.Contains("Analogy.LogViewer.Template"))
                {
                    continue;
                }

                try
                {
                    string fileName = Path.GetFullPath(aFile);
                    string path = Path.GetDirectoryName(aFile);
                    Assembly assembly = Assembly.LoadFrom(fileName);
                    if (!FactoriesManager.Instance.ProbingPaths.Contains(path))
                    {
                        FactoriesManager.Instance.ProbingPaths.Add(path);
                    }

                    var types = assembly.GetTypes().Where(t => !t.IsAbstract).ToList();
                    typesToLoad.Add((assembly, aFile, types));
                }
                catch (Exception e)
                {
                    AnalogyLogManager.Instance.LogError($"{aFile}: Error during data providers: {e} ({e.InnerException}. {aFile})", nameof(FactoriesManager));
                }
            }

            foreach ((Assembly assembly, string fileName, List<Type> types) in typesToLoad)
            {
                foreach (var f in types.Where(aType => aType.GetInterface(nameof(IAnalogyFactory)) != null))
                {
                    try
                    {
                        var factory = (Activator.CreateInstance(f) as IAnalogyFactory)!;
                        var setting = UserSettingsManager.UserSettings.GetOrAddFactorySetting(factory);
                        setting.FactoryName = factory.Title;
                        FactoryContainer fc = new FactoryContainer(assembly, fileName, factory, setting);
                        if (Factories.Exists(fa => fa.Factory.FactoryId == factory.FactoryId))
                        {
                            Factories.Remove(Factories.FirstOrDefault(fa =>
                                fa.Factory.FactoryId == factory.FactoryId));
                        }
                        Factories.Add(fc);
                    }
                    catch (Exception e)
                    {
                        AnalogyLogManager.Instance.LogError($"{fileName}: Error during data providers: {e} ({e.InnerException}. {fileName})", nameof(FactoriesManager));
                    }
                }
            }
            foreach ((Assembly assembly, string fileName, List<Type> types) in typesToLoad)
            {
                foreach (var img in types.Where(aType => aType.GetInterface(nameof(IAnalogyImages)) != null))
                {
                    try
                    {
                        var images = (Activator.CreateInstance(img) as IAnalogyImages)!;
                        var factory = Factories.FirstOrDefault(f => f.Assembly == assembly);
                        factory?.AddImages(images);

                    }
                    catch (Exception e)
                    {
                        AnalogyLogManager.Instance.LogError($"{fileName}: Error during data providers: {e} ({e.InnerException}. {fileName})", nameof(FactoriesManager));
                    }
                }

                foreach (Type dpf in types.Where(aType =>
                    aType.GetInterface(nameof(IAnalogyDataProvidersFactory)) != null))
                {
                    try
                    {
                        var dataProviderFactory = (Activator.CreateInstance(dpf) as IAnalogyDataProvidersFactory)!;
                        var factory = Factories.First(f => f.Factory.FactoryId == dataProviderFactory?.FactoryId);
                        factory.AddDataProviderFactory(dataProviderFactory);
                    }
                    catch (Exception e)
                    {
                        AnalogyLogManager.Instance.LogError($"{fileName}: Error during data providers: {e} ({e.InnerException}. {fileName})", nameof(FactoriesManager));
                    }

                    foreach (Type isettings in types.Where(aType =>
                        aType.GetInterface(nameof(IAnalogyDataProviderSettings)) != null))
                    {
                        try
                        {
                            var settings = (Activator.CreateInstance(isettings) as IAnalogyDataProviderSettings)!;
                            var factory = Factories.First(f => f.Factory.FactoryId == settings?.FactoryId);
                            factory.AddDataProvidersSettings(settings);
                        }
                        catch (Exception e)
                        {
                            AnalogyLogManager.Instance.LogError($"{fileName}: Error during data providers: {e} ({e.InnerException}. {fileName})", nameof(FactoriesManager));
                        }
                    }

                    foreach (Type iaction in types.Where(aType => aType.GetInterface(nameof(IAnalogyCustomActionsFactory)) != null))
                    {
                        try
                        {
                            var custom = (Activator.CreateInstance(iaction) as IAnalogyCustomActionsFactory)!;
                            var factory = Factories.First(f => f.Factory.FactoryId == custom?.FactoryId);
                            factory.AddCustomActionFactory(custom);
                        }
                        catch (Exception e)
                        {
                            AnalogyLogManager.Instance.LogError($"{fileName}: Error during data providers: {e} ({e.InnerException}. {fileName})", nameof(FactoriesManager));
                        }
                    }

                    foreach (Type ishare in types.Where(aType => aType.GetInterface(nameof(IAnalogyShareableFactory)) != null))
                    {
                        try
                        {
                            var share = (Activator.CreateInstance(ishare) as IAnalogyShareableFactory)!;
                            var factory = Factories.First(f => f.Factory.FactoryId == share?.FactoryId);
                            factory.AddShareableFactory(share);
                        }
                        catch (Exception e)
                        {
                            AnalogyLogManager.Instance.LogError($"{fileName}: Error during data providers: {e} ({e.InnerException}. {fileName})", nameof(FactoriesManager));
                        }
                    }

                    foreach (Type aType in types.Where(aType => aType.GetInterface(nameof(IAnalogyExtensionsFactory)) != null))
                    {
                        try
                        {
                            var extension = (Activator.CreateInstance(aType) as IAnalogyExtensionsFactory)!;
                            var factory = Factories.First(f => f.Factory.FactoryId == extension?.FactoryId);
                            factory.AddExtensionFactory(extension);
                        }
                        catch (Exception e)
                        {
                            AnalogyLogManager.Instance.LogError($"{fileName}: Error during data providers: {e} ({e.InnerException}. {fileName})", nameof(FactoriesManager));
                        }
                    }

                    foreach (Type aType in types.Where(aType => aType.GetInterface(nameof(IAnalogyDownloadInformation)) != null))
                    {
                        try
                        {
                            var downloadInfo = (Activator.CreateInstance(aType) as IAnalogyDownloadInformation)!;
                            var factory = Factories.First(f => f.Factory.FactoryId == downloadInfo?.FactoryId);
                            factory.AddDownloadInformation(downloadInfo);
                        }
                        catch (Exception e)
                        {
                            AnalogyLogManager.Instance.LogError($"{fileName}: Error during data providers: {e} ({e.InnerException}. {fileName})", nameof(FactoriesManager));
                        }
                    }
                }
            }
            Factories.RemoveAll(f => f.FactorySetting.Status == DataProviderFactoryStatus.Disabled);

        }
    }
}
