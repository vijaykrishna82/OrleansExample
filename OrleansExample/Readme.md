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