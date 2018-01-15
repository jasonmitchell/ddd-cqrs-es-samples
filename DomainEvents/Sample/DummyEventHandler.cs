using System;
using Sample.Domain;

namespace Sample
{
    public class DummyEventHandler
    {
        public void Handle(ReservationRequested e)
        {
            Console.WriteLine($"\t---[{nameof(DummyEventHandler)}] Handling {e.GetType().Name}---");
        }
        
        public void Handle(ReservationConfirmed e)
        {
            Console.WriteLine($"\t---[{nameof(DummyEventHandler)}] Handling {e.GetType().Name}---");
        }
    }
}