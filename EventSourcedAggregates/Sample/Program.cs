using System;
using System.Collections.Generic;
using System.Linq;
using Sample.Domain;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var events = CreateReservation();
            ReconstructReservation(events);

            Console.ReadLine();
        }

        private static IEnumerable<object> CreateReservation()
        {
            Console.WriteLine("Creating new Reservation");

            var reservation = Reservation.Request(Guid.NewGuid(), 4);
            reservation.Confirm();

            var events = ((IEventSource) reservation).TakeEvents();
            var eventTypes = string.Join(", ", events.Select(x => x.GetType().Name));
            Console.WriteLine($"\tRetrieved {events.Length} events: [{eventTypes}]");
            
            return events;
        }
        
        private static void ReconstructReservation(IEnumerable<object> events)
        {
            Console.WriteLine("Reconstructing Reservation from events");
            
            var reservation = new Reservation();
            var eventSourced = ((IEventSource) reservation);

            foreach (var e in events)
            {
                eventSourced.Given(e);
            }

            Console.WriteLine("Reservation rehydrated (attach debugger and inspect to see state...because I'm lazy)");
        }
    }
}
