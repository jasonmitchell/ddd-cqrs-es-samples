using System;
using ReservationMemento = Sample.Data.Model.Reservation;

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

            var stage = (ReservationStage)Enum.Parse(typeof(ReservationStage), memento.Status);
            _status = new ReservationStatus(stage, memento.LastUpdated);
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
                Status = _status.Stage.ToString(),
                LastUpdated = _status.AsOf
            };
        }

        public void Process(AllocationPool allocationPool)
        {
            if (!_status.CanAdvance) return; // or throw an exception

            _reservedQuantity = allocationPool.ReserveFromPool(_ticketId, _requestedQuantity);
            if (_reservedQuantity <= 0)
            {
                _status = ReservationStatus.InsufficientAvailability();
            }
            else if (_reservedQuantity < _requestedQuantity)
            {
                _status = ReservationStatus.PartiallyConfirmed();
            }
            else
            {
                _status = ReservationStatus.Confirmed();
            }
        }

        public void Expire()
        {
            if (!_status.CanAdvance) return; // or throw an exception
            _status = ReservationStatus.Expired();
        }
    }
}