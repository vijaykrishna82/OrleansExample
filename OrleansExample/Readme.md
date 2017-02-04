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


### 5. Adding Azure Storage
- Download, install and start Azure Storage Emulator
- Add Microsoft.Orleans.OrleansAzureUtils package to the DevTestHost project
- Change the config in OrleansHostWrapper as below:

>	
>            ClusterConfiguration config = ClusterConfiguration.LocalhostPrimarySilo();
>            //config.AddMemoryStorageProvider();
>           config.AddAzureTableStorageProvider(connectionString: "UseDevelopmentStorage=true");

- Change the StorageProvider attribute on DeviceGrain as below:

>		      [StorageProvider(ProviderName ="AzureTableStore")]