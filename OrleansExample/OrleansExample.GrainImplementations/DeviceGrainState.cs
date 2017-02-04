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

        public object State
        {
            get
            {
                return LastValue;
            }
            set
            {
                LastValue = value as double?;
            }
        }

        public string ETag { get; set; }
    }
}
