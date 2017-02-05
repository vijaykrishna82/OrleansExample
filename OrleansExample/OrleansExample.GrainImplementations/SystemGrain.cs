using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using OrleansExample.GrainInterfaces;

namespace OrleansExample.GrainImplementations
{
    public class SystemGrain : Grain, ISystemGrain
    {
        private Dictionary<long, double> Temperatures;


        public override Task OnActivateAsync()
        {
            Temperatures = new Dictionary<long, double>();
            return base.OnActivateAsync();
        }
        public Task SetTemperature(TemperatureReading reading)
        {
            if (reading == null)
                return TaskDone.Done;

            Temperatures[reading.DeviceId] = reading.Value;

            if (Temperatures.Values.Average() > 100)
            {
                Console.WriteLine("System temperature is high");
            }

            return TaskDone.Done;
        }
    }
}
