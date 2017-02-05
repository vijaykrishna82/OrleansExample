using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans.Concurrency;

namespace OrleansExample.GrainInterfaces
{
    [Immutable]
    public class TemperatureReading
    {
        public double Value { get; set; }

        public long DeviceId { get; set; }

        public DateTime Time { get; set; }
    }
}
