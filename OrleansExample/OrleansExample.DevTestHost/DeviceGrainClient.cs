using Orleans;
using Orleans.Runtime.Configuration;
using OrleansExample.GrainInterfaces;
using System;

namespace OrleansExample.DevTestHost
{
    public class DeviceGrainClient
    {
        public static void Consume()
        {
            var config = ClientConfiguration.LocalhostSilo();
            GrainClient.Initialize(config);
            var grain = GrainClient.GrainFactory.GetGrain<IDecoderGrain>(0);

            Console.WriteLine("Keep entering [GrainId, Temperature]. Example 10, 20");
            while (true)
            {
                var temperature = Console.ReadLine();

                if (temperature != "-1")
                {
                    grain.Decode(temperature);
                }
                else
                {
                    break;
                }
            }
        }
    }
}