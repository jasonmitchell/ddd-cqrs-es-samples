namespace Sample
{
    public interface IEventSource
    {
        void Given(object @event);
        object[] TakeEvents();
    }
}