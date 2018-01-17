using System;
using System.Collections.Generic;

namespace Sample
{
    public class EventRecorder
    {
        private readonly Dictionary<Type, Action<EventRecorder, object>> _handlers = new Dictionary<Type, Action<EventRecorder, object>>();
        private readonly Queue<object> _recordedEvents;

        public EventRecorder()
        {
            _recordedEvents = new Queue<object>();
        }
        
        public EventRecorder Given<TEvent>(Action<TEvent> handler)
        {
            _handlers.Add(typeof(TEvent), (ctx, e) => handler((TEvent)e));
            return this;
        }
        
        public EventRecorder Given<TEvent>(Action<EventRecorder, TEvent> handler)
        {
            _handlers.Add(typeof(TEvent), (ctx, e) => handler(ctx, (TEvent)e));
            return this;
        }

        public void Replay(IEnumerable<object> events)
        {
            foreach (var e in events)
            {
                Apply(e);
            }
        }
        
        public void Then<TEvent>(TEvent e)
        {
            Apply(e);
            _recordedEvents.Enqueue(e);
        }

        private void Apply(object e)
        {
            var type = e.GetType();
            if (_handlers.ContainsKey(type))
            {
                _handlers[type](this, e);
            }
        }

        public object[] Reset()
        {
            var events = _recordedEvents.ToArray();
            _recordedEvents.Clear();
            
            return events;
        }
    }
}