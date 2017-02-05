﻿using System;
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

            var systemGrain = GrainFactory.GetGrain<ISystemGrain>(State.System);
            return systemGrain.SetTemperature(value, this.GetPrimaryKeyLong());
        }

        public Task JoinSystem(string name)
        {
            State.System = name;
            return WriteStateAsync();
        }
    }
}
