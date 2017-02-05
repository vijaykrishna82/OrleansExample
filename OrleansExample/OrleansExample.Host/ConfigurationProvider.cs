using Orleans.Runtime.Configuration;

namespace OrleansExample.Host
{
    public class ConfigurationProvider
    {
        public static ClusterConfiguration ConfigureCluster()
        {
            var config = ClusterConfiguration.LocalhostPrimarySilo();
            config.AddMemoryStorageProvider();
            return config;
        }
    }
}