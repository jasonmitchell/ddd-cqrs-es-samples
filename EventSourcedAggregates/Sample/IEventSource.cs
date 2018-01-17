using System.Collections.Generic;

namespace Sample
{
    public interface IEventSource
    {
        void RestoreFromEvents(IEnumerable<object> events);
        object[] TakeEvents();
    }
}