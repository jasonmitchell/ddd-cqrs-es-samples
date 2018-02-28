using System;
using System.Linq;
using Newtonsoft.Json;
using static Reservation.Commands;
using static Reservation.Events;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running sample");

            var reservationId = Guid.NewGuid();
            var ticketId = Guid.NewGuid();
            var requestTickets = Command.NewRequestTickets(new RequestTickets(reservationId, ticketId, 4));

            Console.WriteLine("--- REQUEST TICKETS ---");
            var events = handle(null, requestTickets).ToList();
            Console.WriteLine(JsonConvert.SerializeObject(events));

            Console.WriteLine("--- RESERVATION ---");
            var reservation = given(events);
            Console.WriteLine(JsonConvert.SerializeObject(reservation));

            Console.WriteLine("--- CONFIRM RESERVATION ---");
            var confirmReservation = Command.NewConfirmReservation(new ConfirmReservation(reservationId));
            events.AddRange(handle(reservation, confirmReservation));
            Console.WriteLine(JsonConvert.SerializeObject(events));
            
            Console.WriteLine("--- RESERVATION ---");
            reservation = given(events);
            Console.WriteLine(JsonConvert.SerializeObject(reservation));
            
            Console.ReadKey();
        }
    }
}
