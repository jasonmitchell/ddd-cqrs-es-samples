using System;
using Sample.Data.Model;

namespace Sample.Domain
{
    public class Reservation : IWithMemento<ReservationMemento>
    {
        private readonly int _id;
        private readonly int _ticketId;
        private readonly int _requestedQuantity;
        private int _reservedQuantity;
        private ReservationStatus _status;

        internal Reservation(ReservationMemento memento)
        {
            _id = memento.Id;
            _ticketId = memento.TicketId;
            _requestedQuantity = memento.RequestedQuantity;
            _reservedQuantity = memento.ReservedQuantity;

            var statusType = (ReservationStatus.StatusType)Enum.Parse(typeof(ReservationStatus.StatusType), memento.Status);
            _status = new ReservationStatus(statusType, memento.LastUpdated);
        }
        
        private Reservation(int ticketId, int requestedQuantity)
        {
            _ticketId = ticketId;
            _requestedQuantity = requestedQuantity;
            _status = ReservationStatus.Open();
        }

        public static Reservation Request(int ticketId, int quantity)
        {
            return new Reservation(ticketId, quantity);
        }

        ReservationMemento IWithMemento<ReservationMemento>.CreateMemento()
        {
            return new ReservationMemento
            {
                Id = _id,
                TicketId = _ticketId,
                RequestedQuantity = _requestedQuantity,
                ReservedQuantity = _reservedQuantity,
                Status = _status.Type.ToString(),
                LastUpdated = _status.AsOf
            };
        }

        public void Confirm()
        {
            if (!_status.CanAdvance) return; // or throw an exception
            _status = ReservationStatus.Confirmed();
            _reservedQuantity = _requestedQuantity;
        }
    }
}