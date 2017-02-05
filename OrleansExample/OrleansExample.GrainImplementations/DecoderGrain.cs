using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using OrleansExample.GrainInterfaces;

namespace OrleansExample.GrainImplementations
{
    public class DecoderGrain : Grain, IDecoderGrain
    {
        public Task Decode(string message)
        {
            var parts = message.Split(',');
            var grain = GrainFactory.GetGrain<IDeviceGrain>(long.Parse(parts[0]));

            return grain.SetTemperature(double.Parse(parts[1]));
        }
    }
}
