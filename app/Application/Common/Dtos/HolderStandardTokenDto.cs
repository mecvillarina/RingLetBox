using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Dtos
{
    public class HolderStandardTokenDto : IMapFrom<HolderStandardToken>
    {
        public string ContractAddress { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public int Decimal { get; set; }
        public string Balance { get; set; }
        public string TotalSupply { get; set; }
    }
}
