using Application.Common.Mappings;
using Domain.Entities;
using System;

namespace Application.Common.Dtos
{
    public class AuditTokenCreationDto : IMapFrom<AuditTokenCreation>
    {
        public string Sender { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public int Decimal { get; set; }
        public long TotalSupply { get; set; }
        public string ContractAddress { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
