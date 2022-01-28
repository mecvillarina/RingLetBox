using Domain.Common;

namespace Domain.Events
{
    public class OnTokenCreateEvent : DomainEvent
    {
        public string Sender { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public int Decimal { get; set; }
        public long TotalSupply { get; set; }
        public string ContractAddress { get; set; }
        public string TransactionHash { get; set; }
    }
}
