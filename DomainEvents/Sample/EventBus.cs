using System;
using System.Collections.Generic;
using System.Threading;

namespace Sample
{
    public class EventBus
    {
        private readonly Dictionary<Type, List<Action<object>>> _handlers = new Dictionary<Type, List<Action<object>>>();
        
        public void RegisterHandler<TEvent>(Action<TEvent> handler)
        {
            if (!_handlers.TryGetValue(typeof(TEvent), out var handlers))
            {
                handlers = new List<Action<object>>();
                _handlers.Add(typeof(TEvent), handlers);
            }
            
            handlers.Add(x => handler((TEvent)x));
        }

        public void Publish(object e)
        {
            if (!_handlers.TryGetValue(e.GetType(), out var handlers)) return;
            
            foreach(var handler in handlers)
            {
                ThreadPool.QueueUserWorkItem(x => handler(e));
            }
        }
    }
}