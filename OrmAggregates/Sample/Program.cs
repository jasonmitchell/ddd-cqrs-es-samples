using System;
using System.Linq;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;
using Sample.Data;
using Sample.Data.Model;
using Sample.Domain;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();
            var ticketId = random.Next(1, 4);
            var quantity = random.Next(1, 6);
            
            var reservationId = RequestReservation(ticketId, quantity);
            QueryReservation(reservationId);
            ConfirmReservation(reservationId);
            QueryReservation(reservationId);
            QueryReport();
            
            Console.ReadLine();
        }

        private static int RequestReservation(int ticketId, int quantity)
        {
            Console.WriteLine($"--- {nameof(RequestReservation)} ---");
            
            using (var context = new SampleWriteContext())
            {
                var reservation = Reservation.Request(ticketId, quantity);
                var memento = ((IWithMemento<ReservationMemento>) reservation).CreateMemento();

                context.ReservationMementos.Add(memento);
                context.SaveChanges();
                
                Console.WriteLine($"\tCreated memento: {JsonConvert.SerializeObject(memento)}");

                return memento.Id;
            }
        }

        private static void ConfirmReservation(int reservationId)
        {
            Console.WriteLine($"--- {nameof(ConfirmReservation)} ---");

            using (var context = new SampleWriteContext())
            {
                var memento = context.ReservationMementos.AsNoTracking().FirstOrDefault(x => x.Id == reservationId);
                var reservation = new Reservation(memento);
                
                reservation.Confirm();
                
                var updatedMemento = ((IWithMemento<ReservationMemento>) reservation).CreateMemento();
                Console.WriteLine($"\tUpdated memento: {JsonConvert.SerializeObject(updatedMemento)}");

                context.ReservationMementos.Update(updatedMemento);
                context.SaveChanges();
            }
        }

        private static void QueryReservation(int reservationId)
        {
            Console.WriteLine($"--- {nameof(QueryReservation)}:{reservationId} ---");
            
            using (var connection = new NpgsqlConnection(@"Host=localhost;Port=4001;Database=orm_sample;Username=docker;Password=docker"))
            {
                var reservation = connection.Query<dynamic>("SELECT * FROM reservation WHERE id=@reservationId", new { reservationId });
                Console.WriteLine($"\tReservation: {JsonConvert.SerializeObject(reservation)}");
            }
        }

        private static void QueryReport()
        {
            Console.WriteLine($"--- {nameof(QueryReport)} ---");
            
            using (var connection = new NpgsqlConnection(@"Host=localhost;Port=4001;Database=orm_sample;Username=docker;Password=docker"))
            {
                var confirmedReservations = connection.Query<dynamic>("SELECT * FROM confirmed_reservation");
                Console.WriteLine($"\tReport: {JsonConvert.SerializeObject(confirmedReservations)}");
            }
        }
    }
}
