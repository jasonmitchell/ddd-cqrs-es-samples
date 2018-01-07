using System;

namespace Sample.Data.Model
{
    public class Reservation
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int RequestedQuantity { get; set; }
        public int ReservedQuantity { get; set; }
        public string Status { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}