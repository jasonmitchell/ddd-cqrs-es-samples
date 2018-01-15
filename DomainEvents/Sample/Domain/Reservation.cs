using System;

namespace Sample.Domain
{
    public class Reservation : IEmitEvents
    {
        private readonly EventRecorder _events = new EventRecorder();
        
        private readonly Guid _id;
        private readonly Guid _ticketId;
        private readonly int _quantity;
        private bool _confirmed;
        
        private Reservation(Guid id, Guid ticketId, int quantity)
        {
            _id = id;
            _ticketId = ticketId;
            _quantity = quantity;
            
            _events.Record(new ReservationRequested(id, ticketId, quantity));
        }

        object[] IEmitEvents.TakeEvents() => _events.Reset();

        public static Reservation Request(Guid ticketId, int quantity)
        {
            var reservation = new Reservation(Guid.NewGuid(), ticketId, quantity);
            return reservation;
        }

        public void Confirm()
        {
            if (_confirmed) return;

            _confirmed = true;
            _events.Record(new ReservationConfirmed(_id));
        }
    }
}