using Newtonsoft.Json;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;
using Orleans.Storage;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OrleansExample.FileStorageProvider
{
    public class OrleansFileStorageProvider : IStorageProvider
    {
        public Logger Log { get; set; }

        public string Name { get; set; }

        public string Directory { get; set; }

        public Type Type { get; set; }

        public Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var fileInfo = GetFileInfo(grainType, grainReference);
            fileInfo.Delete();
            return TaskDone.Done;
        }

        public Task Close()
        {
            return TaskDone.Done;
        }

        public Task Init(string name, IProviderRuntime providerRuntime, IProviderConfiguration config)
        {
            Name = name;
            Directory = config.Properties["directory"];
            Type = Type.GetType( config.Properties["type"]);
            return TaskDone.Done;
        }

        public async Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var fileInfo = GetFileInfo(grainType, grainReference);

            if (!fileInfo.Exists) return;

            using (var stream = fileInfo.OpenText())
            {
                var data = await stream.ReadToEndAsync();
                try
                {
                    var dataWithType = JsonConvert.DeserializeObject(data, Type);
                    grainState.State = dataWithType;
                }
                catch(Exception ex)
                {
                    Console.Error.WriteLine(ex.ToString());
                }
            }
        }

        private FileInfo GetFileInfo(string grainType, GrainReference grainReference)
        {
            var path = Path.Combine(Directory, string.Format("{0}-{1}.txt", grainType, grainReference.ToKeyString()));
            return new FileInfo(path);
        }

        public Task WriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var json = JsonConvert.SerializeObject(grainState.State, Type, Formatting.Indented, new JsonSerializerSettings ());
            var fileInfo = GetFileInfo(grainType, grainReference);

            using (var stream = fileInfo.OpenWrite())
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLineAsync(json);
            }

            return TaskDone.Done;
        }
    }
}