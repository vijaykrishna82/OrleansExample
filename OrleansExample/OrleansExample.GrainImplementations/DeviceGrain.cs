using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.Providers;
using OrleansExample.GrainInterfaces;

namespace OrleansExample.GrainImplementations
{
    [StorageProvider(ProviderName ="MemoryStore")]
    public class DeviceGrain : Grain<DeviceGrainState>, IDeviceGrain
    {
        private double LastValue
        {
            set { SetLastValue(value); }
            get { return State.LastValue ?? 0.0; }

        }

        private async void SetLastValue(double value)
        {
            if (State.LastValue != value)
            {
                State.LastValue = value;
                await WriteStateAsync();
            }
            else
            {
                Console.WriteLine("No change");
            }
        }


        public override Task OnActivateAsync()
        {
            var id = this.GetPrimaryKeyLong();
            Console.WriteLine("Activated DeviceGrain {0} with state {1}", id, State.LastValue);

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
