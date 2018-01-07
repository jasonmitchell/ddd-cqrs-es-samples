using System.Collections.Generic;

namespace Sample.Domain
{
    public class AllocationPool
    {
        private readonly Dictionary<int, int> _pool;

        public AllocationPool(Dictionary<int, int> pool)
        {
            _pool = pool;
        }

        public int ReserveFromPool(int ticketId, int quantity) 
        {
            var availability = _pool[ticketId];
            var reservedQuantity = quantity;
            var availabiliyAfterReservation = availability - quantity;

            if (availabiliyAfterReservation < 0)
            {
                reservedQuantity = quantity + availabiliyAfterReservation;
            }
            
            _pool[ticketId] -= reservedQuantity;
            return reservedQuantity;
        }
    }
}