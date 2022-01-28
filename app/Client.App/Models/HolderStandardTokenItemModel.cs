using Application.Common.Dtos;
using System;

namespace Client.App.Models
{
    public class HolderStandardTokenItemModel
    {
        public string ContractAddress { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public int Decimal { get; set; }
        public ulong Balance { get; set; }

        public HolderStandardTokenItemModel(HolderStandardTokenDto dto)
        {
            ContractAddress = dto.ContractAddress;
            Name = dto.Name;
            Symbol = dto.Symbol;
            Decimal = dto.Decimal;
            Balance = (ulong)(ulong.Parse(dto.Balance) / Math.Pow(10, dto.Decimal));
        }
    }
}
