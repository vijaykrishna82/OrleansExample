using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Orleans;
using OrleansExample.GrainInterfaces;

namespace OrleansExample.WebApi.Controllers
{
    public class SystemController : ApiController
    {
        public Task<double> Get(string id)
        {
            var grain = GrainClient.GrainFactory.GetGrain<ISystemGrain>(id);
            return grain.GetTemperature();
        }
    }
}
