namespace Application.Common.Dtos.SmartContractLogs
{
    public class AddLockLogDto
    {
        public long LockTransactionIndex { get; set; }
        public string TokenAddress { get; set; }
        public ulong CreationTime { get; set; }
        public ulong StartTime { get; set; }
        public ulong EndTime { get; set; }
        public string InitiatorAddress { get; set; }
        public string RecipientAddress { get; set; }
        public string Amount { get; set; }
        public bool IsClaimed { get; set; }
        public bool IsRevocable { get; set; }
        public bool IsRevoked { get; set; }
        public bool IsRefunded { get; set; }
        public bool IsActive { get; set; }
    }
}
