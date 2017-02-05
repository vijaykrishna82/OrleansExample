This project is 2017 technology update to the course example project for the 2014 Pluralsight course:

*Introduction to Microsoft Orleans by Richard Astbury*
https://app.pluralsight.com/library/courses/microsoft-orleans-introduction/table-of-contents




## 1. Steps to Create

### 1.1 Interfaces Project 

-  Create new class library
-  Nuget Install Microsoft.Orleans.OrleansCodeGenerator.Build
-  Add the IDeviceGrain interface inherits from IGrain

### 1.2 Implementations Project

-  Create new class library
-  Nuget Install Microsoft.Orleans.OrleansCodeGenerator.Build
-  Add the DeviceGrain inherits from Grain, implements IGrain
 
- Implement business logic, return TaskDone.Done
- Override OnActivateAsync for constructor logic

### 1.3 Developer TestHost

- Install the Orleans visual studio templates from https://marketplace.visualstudio.com/items?itemName=sbykov.MicrosoftOrleansToolsforVisualStudio
- Or search for Microsoft Orleans Tools for Visual Studio 

- Create new DevTestHost Project
- The hosting code comes with the project

- The following code initializes the client for localhost consumption

>            var config = ClientConfiguration.LocalhostSilo();
>            GrainClient.Initialize(config);
>            var grain = GrainClient.GrainFactory.GetGrain<IDeviceGrain>(0);

## 2. Storage Persistence

### 2.1 Adding State Storage

- Under the Implementations project, Add `DeviceGrainState` (implementing `IGrainState`)
- Change the `DeviceGrain` class to inherit from `Grain<DeviceGrainState>`

### 2.2 Adding File Storage Provider

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

## 3. Multiple Grain Interactions

### 3.1 Adding stateless worker

Purpose: Provides "controller" or "dispatcher" logic. Parses the input and determines which Grain class will do the actual processing.

- Add IDecoderGrain to GrainInterfaces
- Add DecoderGrain to GrainImplementations and decorate it with `[StatelessWorker]` attribute
- Use the DeviceGrain within DecoderGrain as below:

>           var grain = GrainFactory.GetGrain<IDeviceGrain>(long.Parse(parts[0]));

- In the DevTestHost.DeviceGrainClient use the DecoderGrain instead of DeviceGrain

>           var grain = GrainClient.GrainFactory.GetGrain<IDecoderGrain>(0);

### 3.2  Re-entrant Grains

Purpose: Grain can accept requests while awaiting, just one activation instead of per call

- Add ISystemGrain to GrainInterfaces
- Add SystemGrain to GrainImplementations 
- Add `[Reentrant]` attribute to DecoderGrain and DeviceGrain classes so that they can accept requests while awaiting (task interleaving)
- Allow DeviceGrain to join SystemGrain state by adding `JoinSystem` method to IDeviceGrain
- Change the SetTemperature method of DeviceGrain to report the temperature to SystemGrain by calling

>             var systemGrain = GrainFactory.GetGrain<ISystemGrain>(State.System);
>             return systemGrain.SetTemperature(value, this.GetPrimaryKeyLong());

- Activate 3 DeviceGrain instances within the DevTestHost

>            var deviceGrain = GrainClient.GrainFactory.GetGrain<IDeviceGrain>(3);
>            deviceGrain.JoinSystem("vehicle1").Wait();
>
>            deviceGrain = GrainClient.GrainFactory.GetGrain<IDeviceGrain>(4);
>            deviceGrain.JoinSystem("vehicle1").Wait();
>
>            deviceGrain = GrainClient.GrainFactory.GetGrain<IDeviceGrain>(5);
>           deviceGrain.JoinSystem("vehicle1").Wait();

When running the program, user can add device specific temperature by entering [DeviceId, temperature] 
example: 3, 90

### 3.3 Bypass Serialization

Messages within a silo can be passed without serialization if the destination grain can keep itself from modifying the properties of the message (immutable message).

- Change SystemGrain.SetTemperature interface to take a TemperatureReading parameter
- Mark the TemperatureReading class with `[Immutable]` attribute, this will bypass serialization while system grain and device grain are within the same silo


## 4. Scheduled Processing

### 4.1 Timers 

Purpose:
- Grains can register timers to call a function at regular intervals
- Timers are async, single threaded and reentrant 
- Multiple timers can be registered, cancelled
- Last the length of grain activation

Code Example: 
- System Grain will check average temperature regularly and send an alert (here just console writeline)

Within the System Grain:
- Move the temperature check code to a separate method `HighTemperatureCheck`
- Within `OnActivateAsync` register a timer by caling `RegisterTimer`

>            Timer = RegisterTimer(HighTemperatureAlert, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));


### 4.2 Reminders

Purpose:
- Same as timers but Persist beyond the life of the Grain
- Will activate the grain if it does not exist
- Should not be used for high frequency functionality

### 4.3 Observers

Purpose:
- Providing Status to Callers
- Each Caller registers an Observer
- Grain will Notify each Observer

Code Example: 
- Notify observers when SystemGrain detects high temperature

In the GrainInterfaces project:
- Add ISystemObserver interface inheriting from `IGrainObserver` interface
- Change ISystemGrain with new methods AddSubscriber and RemoveSubscriber

In the GrainImplementations project:
- In SystemGrain, add a `ObserverSubscriptionManager<ISystemObserver>` field to hold the subscriptions.
- Implement the AddSubscriber and RemoveSubscriber by adding and removing observers from the above field.
- Within the HighTemperatureAlert method, change Console.writeline to notifying the observers as below:

>            //Console.WriteLine("System temperature is high");
>            Observers.Notify(x => x.HighTemperature(average));

In the DevTestHost project:
- Add a new SystemObserver class that implements ISystemObserver
- In the DeviceGrainClient, create an instance of SystemObserver and convert it to a Grain reference as below:

>            var observer = new SystemObserver();
>            var observerRef = GrainClient.GrainFactory.CreateObjectReference<ISystemObserver>(observer).Result;

- Pass the observer reference to the System Grain to be notified of high temperature, as below:

>            var systemGrain = GrainClient.GrainFactory.GetGrain<ISystemGrain>("vehicle1");
>            systemGrain.SubscribeObserver(observerRef).Wait();









