## Steps to Create

### 1.  Interfaces Project 

-  Create new class library
-  Nuget Install Microsoft.Orleans.OrleansCodeGenerator.Build
-  Add the IDeviceGrain interface inherits from IGrain

### 2. Implementations Project

-  Create new class library
-  Nuget Install Microsoft.Orleans.OrleansCodeGenerator.Build
-  Add the DeviceGrain inherits from Grain, implements IGrain
 
- Implement business logic, return TaskDone.Done
- Override OnActivateAsync for constructor logic

### 3. Developer TestHost

- Install the Orleans visual studio templates from https://marketplace.visualstudio.com/items?itemName=sbykov.MicrosoftOrleansToolsforVisualStudio
- Or search for Microsoft Orleans Tools for Visual Studio 

- Create new DevTestHost Project
- The hosting code comes with the project

- The following code initializes the client for localhost consumption

>            var config = ClientConfiguration.LocalhostSilo();
>            GrainClient.Initialize(config);
>            var grain = GrainClient.GrainFactory.GetGrain<IDeviceGrain>(0);

### 4. Adding State Storage

- Under the Implementations project, Add `DeviceGrainState` (implementing `IGrainState`)
- Change the `DeviceGrain` class to inherit from `Grain<DeviceGrainState>`

### 5. Adding File Storage Provider

- Create a new class library, add Microsoft.Orleans.OrleansCodeGenerator.Build package
- Add CustomStorageProvider class, implementing from `IStorageProvider`
- Add an extension method to `ClusterConfiguration` class for registering CustomStorageProvider
- Add the class library reference to DevTestHost
- Call the extension method in the OrleansHostWrapper.Start method

>            ClusterConfiguration config = ClusterConfiguration.LocalhostPrimarySilo();
>            config.AddFileStorageProvider();
>            SiloHost = new SiloHost(parameters.SiloName, config);

- Run the program. It will create a file in the Debug folder (example: `OrleansExample.GrainImplementations.DeviceGrain-GrainReference=0000000000000000000000000000000003ffffffb6637264.txt`)
- When loading next time it will load state from that file.

### 6.Adding stateless worker

Purpose: Provides "controller" or "dispatcher" logic. Parses the input and determines which Grain class will do the actual processing.

- Add IDecoderGrain to GrainInterfaces
- Add DecoderGrain to GrainImplementations and decorate it with `[StatelessWorker]` attribute
- Use the DeviceGrain within DecoderGrain as below:

>           var grain = GrainFactory.GetGrain<IDeviceGrain>(long.Parse(parts[0]));

- In the DevTestHost.DeviceGrainClient use the DecoderGrain instead of DeviceGrain

>    var grain = GrainClient.GrainFactory.GetGrain<IDecoderGrain>(0);

