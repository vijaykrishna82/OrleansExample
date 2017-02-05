using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.Concurrency;
using Orleans.Providers;
using OrleansExample.GrainInterfaces;

namespace OrleansExample.GrainImplementations
{
    [StorageProvider(ProviderName = "DeviceGrainFileProvider")]
    [Reentrant]
    public class DeviceGrain : Grain<DeviceGrainState>, IDeviceGrain
    {
        private double LastValue
        {
            set { SetLastValue(value); }
            get { return State == null ? 0.0 : State.LastValue ?? 0.0; }

        }

        private async void SetLastValue(double value)
        {
            if (State == null)
                return;

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

            State = State ?? new DeviceGrainState();
            State.System = State.System ?? "DefaultSystem";
            State.LastValue = State.LastValue ?? 0.0;

            if (State.LastValue != 0.0)
            {
                //register with system
                RegisterWithSystemGrain(State.LastValue.Value);
            }

            Console.WriteLine("Activated DeviceGrain {0} with state {1}", id, State == null ? "(null)" : string.Format("{0}", State.LastValue));

            

            return base.OnActivateAsync();

        }


        public  Task SetTemperature(double value)
        {
            if (LastValue < 100 && value >= 100)
            {
                Console.WriteLine("High temperature recorded {0}", value);
            }

            LastValue = value;
            return RegisterWithSystemGrain(value);
        }

        private Task RegisterWithSystemGrain(double value)
        {
            var systemGrain = GrainFactory.GetGrain<ISystemGrain>(State.System);

            var temperatureReading = new TemperatureReading
            {
                DeviceId = this.GetPrimaryKeyLong(),
                Value = value,
                Time = DateTime.UtcNow
            };
            return systemGrain.SetTemperature(temperatureReading);
        }

        public Task JoinSystem(string name)
        {
            State.System = name;
            return WriteStateAsync();
        }
    }
}
