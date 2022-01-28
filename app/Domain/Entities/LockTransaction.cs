using Domain.Common;
using System;

namespace Domain.Entities
{
    public class LockTransaction : AuditableEntity
    {
        public int Id { get; set; }

        public string LockContractAddress { get; set; }
        public long LockTransactionIndex { get; set; }
        public string TokenAddress { get; set; }
        public long CreationTime { get; set; }
        public long StartTime { get; set; }
        public long EndTime { get; set; }
        public string InitiatorAddress { get; set; }
        public string RecipientAddress { get; set; }
        public string Amount { get; set; }
        public bool IsClaimed { get; set; }
        public bool IsRevocable { get; set; }
        public bool IsRevoked { get; set; }
        public bool IsRefunded { get; set; }
        public bool IsActive { get; set; }
        public DateTime ClaimedDate { get; set; }
    }
}
