using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using OrleansExample.GrainInterfaces;

namespace OrleansExample.GrainImplementations
{
    public class SystemGrain : Grain, ISystemGrain
    {
        private Dictionary<long, double> Temperatures;
        private IDisposable Timer;
        private ObserverSubscriptionManager<ISystemObserver> Observers;



        public override Task OnActivateAsync()
        {
            Temperatures = new Dictionary<long, double>();
            Observers = new ObserverSubscriptionManager<ISystemObserver>();

            Timer = RegisterTimer(HighTemperatureAlert, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));

            return base.OnActivateAsync();
        }

        public override Task OnDeactivateAsync()
        {
            Timer?.Dispose();
            return base.OnDeactivateAsync();
        }


        Task HighTemperatureAlert(object highTemperatureState)
        {
            if (!Temperatures.Values.Any())
                return TaskDone.Done;

            var average = Temperatures.Values.Average();
            if (!(average > 100)) return TaskDone.Done;

            //Console.WriteLine("System temperature is high");
            Observers.Notify(x => x.HighTemperature(average));

            return TaskDone.Done;
        }
        public Task SetTemperature(TemperatureReading reading)
        {
            if (reading == null)
                return TaskDone.Done;

            Temperatures[reading.DeviceId] = reading.Value;

           
            return TaskDone.Done;
        }

        public Task SubscribeObserver(ISystemObserver observer)
        {
            Observers.Subscribe(observer);
            return TaskDone.Done;
        }

        public Task UnsubscribeObserver(ISystemObserver observer)
        {
            Observers.Unsubscribe(observer);
            return TaskDone.Done;
        }
    }
}
