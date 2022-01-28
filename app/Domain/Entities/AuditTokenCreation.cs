using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class AuditTokenCreation : AuditableEntity
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public int Decimal { get; set; }
        public long TotalSupply { get; set; }
        public string ContractAddress { get; set; }
        public string TransactionHash { get; set; }

    }
}
