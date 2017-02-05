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
        private IDisposable Timer;



        public override Task OnActivateAsync()
        {
            Temperatures = new Dictionary<long, double>();

            Timer = RegisterTimer(HighTemperatureAlert, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));

            return base.OnActivateAsync();
        }

        public override Task OnDeactivateAsync()
        {
            Timer?.Dispose();
            return base.OnDeactivateAsync();
        }


        Task HighTemperatureAlert(object highTemperatureState)
        {
            if (Temperatures.Values.Average() > 100)
            {
                Console.WriteLine("System temperature is high");
            }

            return TaskDone.Done;
        }
        public Task SetTemperature(TemperatureReading reading)
        {
            if (reading == null)
                return TaskDone.Done;

            Temperatures[reading.DeviceId] = reading.Value;

           
            return TaskDone.Done;
        }
    }
}
