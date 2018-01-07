using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sample.Data;
using Sample.Domain;
using ReservationMemento = Sample.Data.Model.Reservation;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var allocationPool = new AllocationPool(new Dictionary<int, int>
            {
                [1] = 10,
                [2] = 20,
                [3] = 30
            });
            
            var reservationId = RequestReservation();
            QueryReservation(reservationId);
            ProcessReservation(reservationId, allocationPool);
            QueryReservation(reservationId);
            
            Console.ReadLine();
        }

        private static int RequestReservation()
        {
            Console.WriteLine($"--- {nameof(RequestReservation)} ---");
            
            using (var context = new SampleWriteContext())
            {
                var reservation = Reservation.Request(2, 4);
                var memento = ((IWithMemento<ReservationMemento>) reservation).CreateMemento();

                context.Reservations.Add(memento);
                context.SaveChanges();
                
                Console.WriteLine($"\tCreated memento: {JsonConvert.SerializeObject(memento)}");

                return memento.Id;
            }
        }

        private static void ProcessReservation(int reservationId, AllocationPool allocationPool)
        {
            Console.WriteLine($"--- {nameof(ProcessReservation)} ---");

            using (var context = new SampleWriteContext())
            {
                var memento = context.Reservations.AsNoTracking().FirstOrDefault(x => x.Id == reservationId);
                var reservation = new Reservation(memento);
                
                reservation.Process(allocationPool);
                
                var updatedMemento = ((IWithMemento<ReservationMemento>) reservation).CreateMemento();
                Console.WriteLine($"\tUpdated memento: {JsonConvert.SerializeObject(updatedMemento)}");

                context.Reservations.Attach(updatedMemento);
                context.Reservations.Update(updatedMemento);
                context.SaveChanges();
            }
        }

        private static ReservationMemento QueryReservation(int reservationId)
        {
            Console.WriteLine($"--- {nameof(QueryReservation)}:{reservationId} ---");
            
            using (var context = new SampleReadContext())
            {
                var reservation = context.Reservations.Find(reservationId);
                Console.WriteLine($"\tQuery result: {JsonConvert.SerializeObject(reservation)}");

                return reservation;
            }
        }
    }
}
