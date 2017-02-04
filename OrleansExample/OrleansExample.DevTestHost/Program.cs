using System;

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

            OrleansAppDomainHost.ShutDown(hostDomain);
        }
    }
}