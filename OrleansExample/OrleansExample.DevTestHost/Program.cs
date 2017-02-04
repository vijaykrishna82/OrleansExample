using System;
using System.Net;
using Orleans;
using Orleans.Runtime.Configuration;
using OrleansExample.GrainInterfaces;

namespace OrleansExample.DevTestHost
{
    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program
    {
        private static void Main(string[] args)
        {
            var hostDomain = OrleansAppDomainHost.Create(args);

            Console.WriteLine("Orleans Silo is running.\nPress Enter to terminate...");
            Console.ReadLine();

            try
            {
                DeviceGrainClient.Consume();
            }
            finally
            {


                OrleansAppDomainHost.ShutDown(hostDomain);
            }
        }

        
    }


    
}