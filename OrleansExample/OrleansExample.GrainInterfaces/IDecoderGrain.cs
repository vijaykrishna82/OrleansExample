using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using Orleans.Concurrency;

namespace OrleansExample.GrainInterfaces
{
    public interface IDecoderGrain : IGrainWithIntegerKey
    {
        Task Decode(string message);
    }
}
