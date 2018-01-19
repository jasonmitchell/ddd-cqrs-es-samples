using System;
using System.Data;
using Dapper;
using Sample.Domain;
using Sample.Supporting.Projections;

namespace Sample.Projections
{
    public class ReservationSummaryProjection : Projection
    {
        public ReservationSummaryProjection(Func<IDbConnection> connectionProvider)
        {
            When<ReservationRequested>(async e =>
            {
                Console.WriteLine($"\t---{GetType().Name} handling {e.GetType().Name}---");
                
                using (var connection = connectionProvider())
                {
                    await connection.ExecuteAsync("UPDATE reservation_summary SET total = total + @quantity WHERE ticket_id=@ticketId",
                        new {ticketId = e.TicketId, quantity = e.Quantity});
                }
            });
        }
    }
}