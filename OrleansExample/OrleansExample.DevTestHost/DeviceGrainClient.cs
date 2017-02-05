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

            var deviceGrain = GrainClient.GrainFactory.GetGrain<IDeviceGrain>(3);
            deviceGrain.JoinSystem("vehicle1").Wait();

            deviceGrain = GrainClient.GrainFactory.GetGrain<IDeviceGrain>(4);
            deviceGrain.JoinSystem("vehicle1").Wait();

            deviceGrain = GrainClient.GrainFactory.GetGrain<IDeviceGrain>(5);
            deviceGrain.JoinSystem("vehicle1").Wait();


            var observer = new SystemObserver();
            var observerRef = GrainClient.GrainFactory.CreateObjectReference<ISystemObserver>(observer).Result;

            var systemGrain = GrainClient.GrainFactory.GetGrain<ISystemGrain>("vehicle1");
            systemGrain.SubscribeObserver(observerRef).Wait();

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