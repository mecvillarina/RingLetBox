using Domain.Common;

namespace Domain.Entities
{
    public class HolderStandardToken : AuditableEntity
    {
        public int Id { get; set; }
        public string HolderAddress { get; set; }
        public string ContractAddress { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public int Decimal { get; set; }
    }
}
