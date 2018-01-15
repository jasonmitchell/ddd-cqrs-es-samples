using System;
using System.Collections.Generic;

namespace Sample
{
    public class EventRecorder
    {
        private readonly Queue<object> _recordedEvents;

        public EventRecorder()
        {
            _recordedEvents = new Queue<object>();
        }
        
        public void Record<TEvent>(TEvent e)
        {
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