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

