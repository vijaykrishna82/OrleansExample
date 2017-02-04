using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using OrleansExample.GrainInterfaces;

namespace OrleansExample.DevTestHost
{
    public class DeviceGrainClient
    {
        public static void Consume()
        {
            GrainClient.Initialize("ClientConfiguration.xml");

            var grain = GrainClient.GrainFactory.GetGrain<IDeviceGrain>(0);


            while (true)
            {
                Console.WriteLine("Enter temperature, -1 to quit");
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
