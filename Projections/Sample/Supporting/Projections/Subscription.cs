using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace Sample.Supporting.Projections
{
    internal class Subscription
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };

        private readonly Func<IEventStoreConnection> _connectionProvider;
        private readonly string _streamId;
        private readonly string _subscriptionName;

        private readonly Dictionary<Type, List<Func<object, Task>>> _eventHandlerMap;
        private readonly Dictionary<string, Type> _eventTypeMap;

        private EventStoreStreamCatchUpSubscription _subscription;
        
        private string CheckpointStreamId => $"ProjectionSubscription-{_subscriptionName}";

        public Subscription(Func<IEventStoreConnection> connectionProvider, string streamId, string subscriptionName, IEnumerable<Projection> projections)
        {
            _connectionProvider = connectionProvider;
            _streamId = streamId;
            _subscriptionName = subscriptionName;
            
            var projectionList = projections.ToList();
            _eventHandlerMap = projectionList.SelectMany(x => x.Handlers)
                                             .GroupBy(x => x.Key)
                                             .ToDictionary(x => x.Key, x => x.SelectMany(y => y.Value).ToList());
            
            _eventTypeMap = projectionList.SelectMany(x => x.EventTypeMap)
                                          .Distinct()
                                          .ToDictionary(x =>  x.Key, x => x.Value);
        }

        public async Task StartAsync()
        {
            var lastCheckpoint = await GetLastCheckpoint();
          
            _subscription = _connectionProvider().SubscribeToStreamFrom(_streamId, 
                lastCheckpoint, 
                CatchUpSubscriptionSettings.Default, 
                (_, y) => EventAppeared(y).GetAwaiter().GetResult(),
                LiveProcessingStarted,
                SubscriptionDropped);
        }

        private void LiveProcessingStarted(EventStoreCatchUpSubscription subscription)
        {
            Console.WriteLine("Live processing started...");
        }

        private void SubscriptionDropped(EventStoreCatchUpSubscription subscription, SubscriptionDropReason dropReason, Exception exception)
        {
            Console.WriteLine("Subscription dropped, restarting...");
            Task.Delay(1000).ContinueWith(x => StartAsync());
        }

        public void Stop()
        {
            _subscription?.Stop();
        }

        private async Task EventAppeared(ResolvedEvent resolvedEvent)
        {
            Console.WriteLine($"Received event {resolvedEvent.Event.EventType}");
            
            if (_eventTypeMap.ContainsKey(resolvedEvent.Event.EventType))
            {
                var type = _eventTypeMap[resolvedEvent.Event.EventType];
                var e = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(resolvedEvent.Event.Data), type, SerializerSettings);

                var tasks = _eventHandlerMap[type].Select(x => x(e));
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }

            await SaveLastCheckpoint(resolvedEvent).ConfigureAwait(false);
        }
        
        private async Task<long?> GetLastCheckpoint()
        {
            var slice = await _connectionProvider().ReadStreamEventsBackwardAsync(CheckpointStreamId, StreamPosition.End, 1, false);

            if (slice.Events.Any())
            {
                var resolvedEvent = slice.Events.Single();
                var e = JsonConvert.DeserializeObject<SubscriptionPositionAdvanced>(Encoding.UTF8.GetString(resolvedEvent.Event.Data));

                return e.EventNumber;
            }

            return null;
        }

        private async Task SaveLastCheckpoint(ResolvedEvent resolvedEvent)
        {
            var e = new SubscriptionPositionAdvanced(_streamId, _subscriptionName, resolvedEvent.OriginalStreamId, resolvedEvent.OriginalEventNumber);
            var eventData = new EventData(Guid.NewGuid(),
                typeof(SubscriptionPositionAdvanced).Name,
                true,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(e)),
                null);

            var writeResult = await _connectionProvider().AppendToStreamAsync(CheckpointStreamId, ExpectedVersion.Any, eventData);

            if (writeResult.NextExpectedVersion == 1)
            {
                var metadata = StreamMetadata.Build().SetMaxCount(30);
                await _connectionProvider().SetStreamMetadataAsync(CheckpointStreamId, ExpectedVersion.NoStream, metadata);
            }
        }
    }
}