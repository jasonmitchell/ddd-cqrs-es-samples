namespace Sample.Supporting.Projections
{
    internal class SubscriptionPositionAdvanced
    {
        public string SubscriptionStreamId { get; }
        public string SubscriptionName { get; }
        public string EventStreamId { get; }
        public long EventNumber { get; }

        public SubscriptionPositionAdvanced(string subscriptionStreamId, string subscriptionName, string eventStreamId, long eventNumber)
        {
            SubscriptionStreamId = subscriptionStreamId;
            SubscriptionName = subscriptionName;
            EventStreamId = eventStreamId;
            EventNumber = eventNumber;
        }
    }
}