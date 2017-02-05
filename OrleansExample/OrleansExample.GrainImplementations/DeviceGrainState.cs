using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace OrleansExample.GrainImplementations
{
    public class DeviceGrainState : IGrainState    {
        public double? LastValue { get; set; }

        public string System { get; set; }

        public object State { get; set; }

        public string ETag { get; set; }
    }
}
