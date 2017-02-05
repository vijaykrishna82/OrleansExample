using Orleans.Runtime.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace OrleansExample.FileStorageProvider
{
    public static class Extensions
    {
        public static void AddFileStorageProvider<TDataType>(this ClusterConfiguration config, string directory = null,  string providerName = "FileStorageProvider", int numStorageGrains = 10)
        {
            if (string.IsNullOrWhiteSpace(providerName))
                throw new ArgumentNullException("providerName");

            directory = directory ?? Directory.GetCurrentDirectory();

            string type = typeof (TDataType).AssemblyQualifiedName;
            var dictionary = new Dictionary<string, string>{{"directory",directory}, {"type", type} };
            
            config.Globals.RegisterStorageProvider<OrleansFileStorageProvider>(providerName, (IDictionary<string, string>)dictionary);
        }
    }
}