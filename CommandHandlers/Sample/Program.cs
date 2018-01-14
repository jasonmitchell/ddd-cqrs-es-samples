using System;
using Sample.Commands;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = new CommandBus();
            Bootstrap(bus);
            
            Console.WriteLine("Sending commands");
            bus.Send(new RequestTickets());
            bus.Send(new ConfirmReservation());
            bus.Send(new RejectReservation());
            bus.Send(new ExpireReservation());
            
            Console.ReadLine();
        }

        private static void Bootstrap(CommandBus bus)
        {
            var emailer = new Emailer();
            var reservationHandlers = new ReservationHandlers();
            
            bus.RegisterHandler<RequestTickets>(reservationHandlers.When);
            bus.RegisterHandler<ConfirmReservation>(x => reservationHandlers.When(x, emailer));
            bus.RegisterHandler<RejectReservation>(reservationHandlers.When);
            bus.RegisterHandler<ExpireReservation>(reservationHandlers.When);
        }
    }
}
