using System;
using System.Collections.Generic;

namespace Sample.Domain
{
    public class Reservation : IEventSource
    {
        private readonly EventRecorder _events;
        
        private Guid _id;
        private Guid _ticketId;
        private int _quantity;
        private bool _confirmed;

        internal Reservation()
        {
            _events = new EventRecorder()
                .Given<ReservationRequested>(e =>
                {
                    _id = e.Id;
                    _ticketId = e.TicketId;
                    _quantity = e.Quantity;
                })
                .Given<ReservationConfirmed>(_ => _confirmed = true);
        }
        
        void IEventSource.RestoreFromEvents(IEnumerable<object> events) => _events.Replay(events);
        object[] IEventSource.TakeEvents() => _events.Reset();

        public static Reservation Request(Guid ticketId, int quantity)
        {
            var reservation = new Reservation();
            reservation._events.Then(new ReservationRequested(Guid.NewGuid(), ticketId, quantity));
            
            return reservation;
        }
        
        public void Confirm()
        {
            if (_confirmed) return;
            _events.Then(new ReservationConfirmed(_id));
        }
    }
}