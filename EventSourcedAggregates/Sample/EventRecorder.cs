using System.Collections.Generic;

namespace Sample
{
    public class EventRecorder
    {
        private readonly IEventSource _eventSource;
        private readonly Queue<object> _recordedEvents;

        public EventRecorder(IEventSource eventSource)
        {
            _eventSource = eventSource;
            _recordedEvents = new Queue<object>();
        }
        
        public void Replay(IEnumerable<object> events)
        {
            foreach (var e in events)
            {
                _eventSource.Given(e);
            }
        }
        
        public void Then<TEvent>(TEvent e)
        {
            _eventSource.Given(e);
            _recordedEvents.Enqueue(e);
        }

        public object[] Reset()
        {
            var events = _recordedEvents.ToArray();
            _recordedEvents.Clear();
            
            return events;
        }
    }
}