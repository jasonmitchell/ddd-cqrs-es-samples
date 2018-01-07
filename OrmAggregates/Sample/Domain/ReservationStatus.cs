using System;

namespace Sample.Domain
{
    public struct ReservationStatus
    {
        public ReservationStage Stage { get; }
        public DateTime AsOf { get; }

        internal ReservationStatus(ReservationStage stage, DateTime asOf)
        {
            Stage = stage;
            AsOf = asOf;
        }

        public static ReservationStatus Open() => new ReservationStatus(ReservationStage.Open, DateTime.UtcNow);
        public static ReservationStatus Confirmed() => new ReservationStatus(ReservationStage.Confirmed, DateTime.UtcNow);
        public static ReservationStatus PartiallyConfirmed() => new ReservationStatus(ReservationStage.PartiallyConfirmed, DateTime.UtcNow);
        public static ReservationStatus InsufficientAvailability() => new ReservationStatus(ReservationStage.InsufficientAvailability, DateTime.UtcNow);
        public static ReservationStatus Expired() => new ReservationStatus(ReservationStage.Expired, DateTime.UtcNow);

        public bool CanAdvance => Stage == ReservationStage.Open;
    }
}