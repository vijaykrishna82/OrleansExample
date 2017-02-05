using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace OrleansExample.GrainInterfaces
{
    public interface IDeviceGrain : IGrainWithIntegerKey
    {
        Task SetTemperature(double value);

        Task<double> GetTemperature();

        Task JoinSystem(string name);
    }
}
