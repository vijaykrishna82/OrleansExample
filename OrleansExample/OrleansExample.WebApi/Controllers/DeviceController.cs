using Orleans;
using OrleansExample.GrainInterfaces;
using System.Threading.Tasks;
using System.Web.Http;

namespace OrleansExample.WebApi.Controllers
{
    public class DeviceController : ApiController
    {
        public Task<double> Get(long id)
        {
            var grain = GrainClient.GrainFactory.GetGrain<IDeviceGrain>(id);
            return grain.GetTemperature();
        }

        public Task Post([FromBody] string message)
        {
            var grain = GrainClient.GrainFactory.GetGrain<IDecoderGrain>(0);
            return grain.Decode(message);
        }
    }
}