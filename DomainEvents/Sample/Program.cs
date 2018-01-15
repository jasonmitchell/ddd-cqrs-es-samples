using System;
using Sample.Domain;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            // event bus could easily be message queue or service bus
            var bus = new EventBus();
            Bootstrap(bus);
            
            Console.WriteLine("Creating reservation");

            var reservation = Reservation.Request(Guid.NewGuid(), 4);
            reservation.Confirm();
            
            var events = ((IEmitEvents) reservation).TakeEvents();
            Console.WriteLine($"Got {events.Length} events");
            Console.WriteLine("Publishing events...");

            foreach (var e in events)
            {
                bus.Publish(e);
            }
            
            Console.ReadLine();
        }

        private static void Bootstrap(EventBus bus)
        {
            var dummyEventHandler = new DummyEventHandler();
            bus.RegisterHandler<ReservationRequested>(dummyEventHandler.Handle);
            bus.RegisterHandler<ReservationConfirmed>(dummyEventHandler.Handle);
            
            bus.RegisterHandler<ReservationRequested>(SomeOtherHandler);
        }

        private static void SomeOtherHandler(ReservationRequested e)
        {
            Console.WriteLine($"\t---[{nameof(Program)}] Handling {e.GetType().Name}---");
        }
    }
}
