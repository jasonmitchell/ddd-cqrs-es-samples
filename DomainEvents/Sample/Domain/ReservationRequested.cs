using System;

namespace Sample.Domain
{
    public class ReservationRequested
    {
        public Guid Id { get; }
        public Guid TicketId { get; }
        public int Quantity { get; }
        
        public ReservationRequested(Guid id, Guid ticketId, int quantity)
        {
            Id = id;
            TicketId = ticketId;
            Quantity = quantity;
        }
    }
}