using System;
using Sample.Availability;
using Sample.Orders;
using Sample.Reservations;
using Sample.Ticketing;

namespace Sample
{
    public class TicketOrderProcess
    {
        private Action<object> Then { get; }
        
        public TicketOrderProcess(Action<object> then)
        {
            Then = then;
        }

        private void Start()
        {
            
        }

        private void End()
        {
            
        }
        
        public void When(TicketsRequested e)
        {
            Start();
            Then(new AllocateTickets());
        }

        public void When(TicketsAllocated e)
        {
            Then(new AcceptReservation());
            Then(new PlaceOrder());
        }

        public void When(TicketsSoldOut e)
        {
            Then(new DeclineReservation());
            End();
        }

        public void When(ReservationExpired e)
        {
            Then(new ReleaseTickets());
            End();
        }

        public void When(OrderPlaced e)
        {
            Then(new MakePayment());
        }

        public void When(CustomerCharged e)
        {
            Then(new DispatchETickets());
        }

        public void When(ETicketsDispatched e)
        {
            End();
        }
    }
}