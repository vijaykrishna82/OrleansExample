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
            var grain = GrainClient.GrainFactory.GetGrain<IDeviceGrain>(0);

            Console.WriteLine("Keep entering temperature, -1 or non-number to quit");
            while (true)
            {
                var temperature = double.Parse(Console.ReadLine());

                if (temperature > 0)
                {
                    grain.SetTemperature(temperature);
                }
                else
                {
                    break;
                }
            }
        }
    }
}