﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using Npgsql;
using Sample.Domain;
using Sample.Projections;
using Sample.Supporting.Projections;

namespace Sample
{
    class Program
    {
        private static readonly Random Random = new Random();
        private static IEventStoreConnection _connection;
        
        static void Main(string[] args)
        {
            using (CreateProjectionService())
            {
                Console.WriteLine("Running projections...");
                
                var cancellationTokenSource = new CancellationTokenSource();

                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    WriteEvents();
                    Task.Delay(Random.Next(500, 1500)).GetAwaiter().GetResult();
                }
                
                Console.ReadLine();
                cancellationTokenSource.Cancel();
            }
            
            _connection?.Dispose();
        }

        private static void WriteEvents()
        {
            var reservationId = Guid.NewGuid();
            var ticketId = Random.Next(1, 4);
            var quantity = Random.Next(1, 7);
            var confirm = quantity % 2 == 0;
            
            Console.WriteLine($"Creating reservation.  Id: [{reservationId}] Ticket: [{ticketId}] Quantity: [{quantity}]");

            var events = new List<object>
            {
                new ReservationRequested(reservationId, ticketId, quantity, DateTime.UtcNow)
            };

            if (confirm)
            {
                events.Add(new ReservationConfirmed(reservationId));
            }

            var eventData = events.Select(x => new EventData(
                Guid.NewGuid(),
                x.GetType().Name,
                true,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(x)),
                null
            ))
            .ToArray();

            var stream = $"reservation-{reservationId}";
            GetEventStoreConnection().AppendToStreamAsync(stream, ExpectedVersion.Any, eventData);
        }

        private static ProjectionService CreateProjectionService()
        {
            var projectionService = new ProjectionService(GetEventStoreConnection);
            
            projectionService.SubscribeTo("$ce-reservation", "Reservations", new Projection[]
            {
                new ReservationProjection(GetDatabaseConnection),
                new ReservationSummaryProjection(GetDatabaseConnection) 
            });
            
            projectionService.StartAsync().GetAwaiter().GetResult();
            
            return projectionService;
        }

        private static IDbConnection GetDatabaseConnection()
        {
            return new NpgsqlConnection(@"Host=localhost;Port=4002;Database=dotnet_projections_sample;Username=docker;Password=docker");
        }

        private static IEventStoreConnection GetEventStoreConnection()
        {
            return _connection ?? (_connection = CreateEventStoreConnection());
        }
        
        private static IEventStoreConnection CreateEventStoreConnection()
        {
            var connectionSettings = ConnectionSettings.Create()
                .KeepRetrying()
                .KeepReconnecting();

            var connection = EventStoreConnection.Create(connectionSettings,
                new IPEndPoint(IPAddress.Loopback, 1113), 
                "Projection Sample");

            connection.ConnectAsync().GetAwaiter().GetResult();

            return connection;
        }
    }
}
