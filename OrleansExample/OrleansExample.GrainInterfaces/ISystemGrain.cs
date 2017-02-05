using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace OrleansExample.GrainInterfaces
{
    public interface ISystemGrain : IGrainWithStringKey
    {
        Task SetTemperature(TemperatureReading reading);

        Task SubscribeObserver(ISystemObserver observer);

        Task UnsubscribeObserver(ISystemObserver observer);
    }
}
