using System;
using System.Net;

namespace OrleansExample.DevTestHost
{
    public class SiloParameters
    {
        public string DeploymentId { get; set; }
        public string SiloName { get; set; }

        public void PrintUsage()
        {
            Console.WriteLine(
                @"USAGE: 
    orleans host [<siloName> [<configFile>]] [DeploymentId=<idString>] [/debug]
Where:
    <siloName>      - Name of this silo in the Config file list (optional)
    DeploymentId=<idString> 
                    - Which deployment group this host instance should run in (optional)");
        }
        
        public static SiloParameters ParseArguments(string[] args)
        {
            string deploymentId = null;

            string siloName = Dns.GetHostName(); // Default to machine name

            int argPos = 1;
            for (int i = 0; i < args.Length; i++)
            {
                string a = args[i];
                if (a.StartsWith("-") || a.StartsWith("/"))
                {
                    switch (a.ToLowerInvariant())
                    {
                        case "/?":
                        case "/help":
                        case "-?":
                        case "-help":
                            // Query usage help
                            return null;
                        default:
                            Console.WriteLine("Bad command line arguments supplied: " + a);
                            return null;
                    }
                }
                else if (a.Contains("="))
                {
                    string[] split = a.Split('=');
                    if (String.IsNullOrEmpty(split[1]))
                    {
                        Console.WriteLine("Bad command line arguments supplied: " + a);
                        return null;
                    }
                    switch (split[0].ToLowerInvariant())
                    {
                        case "deploymentid":
                            deploymentId = split[1];
                            break;
                        default:
                            Console.WriteLine("Bad command line arguments supplied: " + a);
                            return null;
                    }
                }
                // unqualified arguments below
                else if (argPos == 1)
                {
                    siloName = a;
                    argPos++;
                }
                else
                {
                    // Too many command line arguments
                    Console.WriteLine("Too many command line arguments supplied: " + a);
                    return null;
                }
            }

            SiloParameters parameters = new SiloParameters
            {
                DeploymentId = deploymentId,
                SiloName = siloName
            };


            return parameters;
        }
    }
}