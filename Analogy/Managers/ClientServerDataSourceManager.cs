using System;
using System.Collections.Generic;
using System.IO;
using Analogy.DataTypes;

namespace Analogy
{
    public class ClientServerDataSourceManager
    {
        private string FileName { get; } = "ExternalLocations.dat";
        private static readonly Lazy<ClientServerDataSourceManager> _instance = new Lazy<ClientServerDataSourceManager>(() => new ClientServerDataSourceManager());
        public static ClientServerDataSourceManager Instance { get; } = _instance.Value;
        public List<DataSource> DataSources { get; } = new List<DataSource>();

        public ClientServerDataSourceManager()
        {
            if (File.Exists(FileName))
            {
                try
                {
                    DataSources = Utils.DeSerializeBinaryFile<List<DataSource>>(FileName);
                }
                catch (Exception)
                {
                    //do nothing. log later
                }

            }
        }



        public void Remove(DataSource data)
        {
            DataSources.Remove(data);
            Save();

        }

        private void Save()
        {
            Utils.SerializeToBinaryFile(DataSources, FileName);
        }

        public void Add(DataSource data)
        {
            DataSources.Add(data);
            Save();
        }
    }
}
