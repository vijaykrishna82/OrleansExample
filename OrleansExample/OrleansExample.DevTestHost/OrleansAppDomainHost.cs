using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime.Configuration;

namespace OrleansExample.DevTestHost
{
    public static class OrleansAppDomainHost
    {
        

        public static AppDomain Create(string[] args)
        {
            // The Orleans silo environment is initialized in its own app domain in order to more
            // closely emulate the distributed situation, when the client and the server cannot
            // pass data via shared memory.
            var hostDomain = AppDomain.CreateDomain("OrleansHost", null, new AppDomainSetup
            {
                AppDomainInitializer = InitSilo,
                AppDomainInitializerArguments = args,
            });



            return hostDomain;
            // TODO: once the previous call returns, the silo is up and running.
            //       This is the place your custom logic, for example calling client logic
            //       or initializing an HTTP front end for accepting incoming requests.
        }

        public static void ShutDown(AppDomain domain)
        {
            domain.DoCallBack(ShutdownSilo);
        }

        static void ShutdownSilo()
        {
            if (HostWrapper != null)
            {
                HostWrapper.Dispose();
                GC.SuppressFinalize(HostWrapper);
            }
        }


        static void InitSilo(string[] args)
        {
            HostWrapper = new OrleansHostWrapper(args);

            if (!HostWrapper.Run())
            {
                Console.Error.WriteLine("Failed to initialize Orleans silo");
            }
        }


        private static OrleansHostWrapper HostWrapper;
    }
}
