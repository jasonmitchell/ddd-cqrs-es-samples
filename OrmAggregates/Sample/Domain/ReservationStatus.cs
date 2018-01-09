using System;

namespace Sample.Domain
{
    public struct ReservationStatus
    {
        public StatusType Type { get; }
        public DateTime AsOf { get; }

        internal ReservationStatus(StatusType stage, DateTime asOf)
        {
            Type = stage;
            AsOf = asOf;
        }

        public static ReservationStatus Open() => new ReservationStatus(StatusType.Open, DateTime.UtcNow);
        public static ReservationStatus Confirmed() => new ReservationStatus(StatusType.Confirmed, DateTime.UtcNow);

        public bool CanAdvance => Type == StatusType.Open;

        public enum StatusType
        {
            Open,
            Confirmed
        }
    }
}