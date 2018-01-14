using System;

namespace Sample.Commands
{
    public class ReservationHandlers
    {
        public void When(RequestTickets command)
        {
            Console.WriteLine($"\t---Handling {command.GetType().Name}---");
        }
        
        // Dependency example
        public void When(ConfirmReservation command, Emailer emailer)
        {
            Console.WriteLine($"\t---Handling {command.GetType().Name}---");
            emailer.Send();
        }
        
        public void When(RejectReservation command)
        {
            Console.WriteLine($"\t---Handling {command.GetType().Name}---");
        }
        
        public void When(ExpireReservation command)
        {
            Console.WriteLine($"\t---Handling {command.GetType().Name}---");
        }
    }
}