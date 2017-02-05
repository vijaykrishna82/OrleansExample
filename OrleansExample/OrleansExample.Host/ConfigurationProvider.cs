using Orleans.Runtime.Configuration;
using OrleansExample.FileStorageProvider;
using OrleansExample.GrainImplementations;

namespace OrleansExample.Host
{
    public class ConfigurationProvider
    {
        public static ClusterConfiguration ConfigureCluster()
        {
            var config = ClusterConfiguration.LocalhostPrimarySilo();
            //config.AddMemoryStorageProvider();
            config.AddFileStorageProvider<DeviceGrainState>(providerName: "DeviceGrainFileProvider");
            return config;
        }
    }
}