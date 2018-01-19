using System;
using System.Data;
using Dapper;
using Sample.Domain;
using Sample.Supporting.Projections;

namespace Sample.Projections
{
    public class ReservationProjection : Projection
    {
        public ReservationProjection(Func<IDbConnection> connectionProvider)
        {
            When<ReservationRequested>(async e =>
            {
                Console.WriteLine($"\t---{GetType().Name} handling {e.GetType().Name}---");
                
                using (var connection = connectionProvider())
                {
                    await connection.ExecuteAsync("INSERT INTO reservation VALUES(@id, @ticketId, @quantity, @requestedAt, 'Open')",
                        new {id = e.Id, ticketId = e.TicketId, quantity = e.Quantity, requestedAt = e.RequestedAt});
                }
            });
            
            When<ReservationConfirmed>(async e =>
            {
                Console.WriteLine($"\t---{GetType().Name} handling {e.GetType().Name}---");
                
                using (var connection = connectionProvider())
                {
                    await connection.ExecuteAsync("UPDATE reservation SET status='Confirmed' WHERE id=@id",
                        new {id = e.Id});
                }
            });
        }
    }
}