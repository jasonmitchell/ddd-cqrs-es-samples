using System;

namespace Sample.Domain
{
    public class ReservationRequested
    {
        public Guid Id { get; }
        public int TicketId { get; }
        public int Quantity { get; }
        public DateTime RequestedAt { get; }
        
        public ReservationRequested(Guid id, int ticketId, int quantity, DateTime requestedAt)
        {
            Id = id;
            TicketId = ticketId;
            Quantity = quantity;
            RequestedAt = requestedAt;
        }
    }
}