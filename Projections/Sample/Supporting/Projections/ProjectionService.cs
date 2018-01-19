using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace Sample.Supporting.Projections
{
    public class ProjectionService : IDisposable
    {
        private readonly Func<IEventStoreConnection> _connectionProvider;
        private readonly List<Subscription> _subscriptions = new List<Subscription>();

        public ProjectionService(Func<IEventStoreConnection> connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }
        
        public void SubscribeTo(string streamId, string subscriptionName, IEnumerable<Projection> eventHandlers)
        {
            var subscription = new Subscription(_connectionProvider, streamId, subscriptionName, eventHandlers);
            _subscriptions.Add(subscription);
        }

        public Task StartAsync()
        {
            var tasks = _subscriptions.Select(x => x.StartAsync());
            return Task.WhenAll(tasks);
        }

        public void Stop()
        {
            foreach (var subscription in _subscriptions)
            {
                subscription.Stop();
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}