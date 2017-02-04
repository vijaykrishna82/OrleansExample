using System;
using System.Threading.Tasks;

using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

namespace OrleansExample.DevTestHost
{
    internal class OrleansHostWrapper : IDisposable
    {
        public bool Debug
        {
            get { return SiloHost != null && SiloHost.Debug; }
            set { SiloHost.Debug = value; }
        }

        private SiloHost SiloHost;

        public OrleansHostWrapper(string[] args)
        {
            var parameters =   SiloParameters.ParseArguments(args);
            Start(parameters);
            Init();
        }

        public bool Run()
        {
            bool ok = false;

            try
            {
                SiloHost.InitializeOrleansSilo();

                ok = SiloHost.StartOrleansSilo();

                if (ok)
                {
                    Console.WriteLine(string.Format("Successfully started Orleans silo '{0}' as a {1} node.", SiloHost.Name, SiloHost.Type));
                }
                else
                {
                    throw new SystemException(string.Format("Failed to start Orleans silo '{0}' as a {1} node.", SiloHost.Name, SiloHost.Type));
                }
            }
            catch (Exception exc)
            {
                SiloHost.ReportStartupError(exc);
                var msg = string.Format("{0}:\n{1}\n{2}", exc.GetType().FullName, exc.Message, exc.StackTrace);
                Console.WriteLine(msg);
            }

            return ok;
        }

        public bool Stop()
        {
            bool ok = false;

            try
            {
                SiloHost.StopOrleansSilo();

                Console.WriteLine(string.Format("Orleans silo '{0}' shutdown.", SiloHost.Name));
            }
            catch (Exception exc)
            {
                SiloHost.ReportStartupError(exc);
                var msg = string.Format("{0}:\n{1}\n{2}", exc.GetType().FullName, exc.Message, exc.StackTrace);
                Console.WriteLine(msg);
            }

            return ok;
        }

        private void Init()
        {
            SiloHost.LoadOrleansConfig();
        }


        private void Start(SiloParameters parameters)
        {
            if (parameters == null)
                return;

            ClusterConfiguration config = ClusterConfiguration.LocalhostPrimarySilo();
            //config.AddMemoryStorageProvider();
            config.AddAzureTableStorageProvider(connectionString: "UseDevelopmentStorage=true");

            SiloHost = new SiloHost(parameters.SiloName, config);


            if (parameters.DeploymentId != null)
                SiloHost.DeploymentId = parameters.DeploymentId;
        }

       

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool dispose)
        {
            SiloHost.Dispose();
            SiloHost = null;
        }
    }
}
