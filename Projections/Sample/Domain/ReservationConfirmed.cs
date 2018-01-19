using System;

namespace Sample.Domain
{
    public class ReservationConfirmed
    {
        public Guid Id { get; }
        
        public ReservationConfirmed(Guid id)
        {
            Id = id;
        }
    }
}