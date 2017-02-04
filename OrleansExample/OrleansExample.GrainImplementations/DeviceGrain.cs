using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using OrleansExample.GrainInterfaces;

namespace OrleansExample.GrainImplementations
{
    public class DeviceGrain : Orleans.Grain, IDeviceGrain
    {
        private double LastValue;


        public override Task OnActivateAsync()
        {
            var id = this.GetPrimaryKeyLong();
            Console.WriteLine("Activated DeviceGrain {0}", id);

            return base.OnActivateAsync();

        }


        public Task SetTemperature(double value)
        {
            if (LastValue < 100 && value >= 100)
            {
                Console.WriteLine("High temperature recorded {0}", value);
            }

            LastValue = value;

            return TaskDone.Done;
        }
    }
}
