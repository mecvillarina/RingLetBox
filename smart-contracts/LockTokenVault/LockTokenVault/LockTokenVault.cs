using Stratis.SmartContracts;
using System;

public class LockTokenVault : SmartContract
{
    private const string KeyLockTransactions = "LockTransactions";
    private const string KeyInitiatorLockTransactionIndexes = "InitiatorLockTransactionIndexes";
    private const string KeyRecipientLockTransactionIndexes = "RecipientLockTransactionIndexes";

    public LockTokenVault(ISmartContractState smartContractState) : base(smartContractState)
    {
        CurrentLockTransactionIndex = -1;
    }

    public long CurrentLockTransactionIndex
    {
        get => State.GetInt64(nameof(CurrentLockTransactionIndex));
        private set => State.SetInt64(nameof(CurrentLockTransactionIndex), value);
    }

    public byte[] GetLockTransactionsByInitiator(Address initiator)
    {
        string transactionIndexesKey = $"{KeyInitiatorLockTransactionIndexes}:{initiator}";
        var transactionIndexes = State.GetArray<long>(transactionIndexesKey);
        var transactions = GetLockTransactions();
        Array locksByInitiator = new LockTransaction[transactionIndexes.Length];

        for (int i = 0; i < transactionIndexes.Length; i++)
        {
            locksByInitiator.SetValue(transactions[transactionIndexes[i]], i);
        }

        return Serializer.Serialize(locksByInitiator);
    }

    private LockTransaction[] GetLockTransactions()
    {
        return State.GetArray<LockTransaction>(KeyLockTransactions);
    }

    private void SetLockTransaction(LockTransaction lockEntry)
    {
        var transactions = State.GetArray<LockTransaction>(KeyLockTransactions);
        var newTransactions = new LockTransaction[transactions.Length + 1];

        for (int i = 0; i < transactions.Length; i++)
        {
            newTransactions[i] = transactions[i];
        }

        newTransactions[transactions.Length] = lockEntry;
        State.SetArray(KeyLockTransactions, newTransactions);
        CurrentLockTransactionIndex = transactions.Length;
    }

    private void SetLockTransaction(LockTransaction lockEntry, long lockTransactionIndex)
    {
        var locks = State.GetArray<LockTransaction>(KeyLockTransactions);
        locks[lockTransactionIndex] = lockEntry;
        State.SetArray(KeyLockTransactions, locks);
    }

    private void UpdateInitiatorLockIndex(Address initiator, long lockTransactionIndex)
    {
        string transactionIndexesKey = $"{KeyInitiatorLockTransactionIndexes}:{initiator}";
        var transactionIndexes = State.GetArray<long>(transactionIndexesKey);
        var newTransactionIndexes = new long[transactionIndexes.Length + 1];

        for (int i = 0; i < transactionIndexes.Length; i++)
        {
            newTransactionIndexes[i] = transactionIndexes[i];
        }

        newTransactionIndexes[transactionIndexes.Length] = lockTransactionIndex;
        State.SetArray(transactionIndexesKey, newTransactionIndexes);
    }

    private void UpdateRecipientLockIndex(Address recipient, long lockTransactionIndex)
    {
        string transactionIndexesKey = $"{KeyRecipientLockTransactionIndexes}:{recipient}";
        var transactionIndexes = State.GetArray<long>(transactionIndexesKey);
        var newTransactionIndexes = new long[transactionIndexes.Length + 1];

        for (int i = 0; i < transactionIndexes.Length; i++)
        {
            newTransactionIndexes[i] = transactionIndexes[i];
        }

        newTransactionIndexes[transactionIndexes.Length] = lockTransactionIndex;
        State.SetArray(transactionIndexesKey, newTransactionIndexes);
    }

    public void AddLock(Address tokenAddress, UInt256 totalAmount, ulong durationInDays, Address recipientAddress, bool isRevocable)
    {
        Assert(State.IsContract(tokenAddress), "[LockTokenVault: Token address must be a contract.]");

        Assert(totalAmount > 0, "[LockTokenVault: This lock amount must be greater than 0.]");
        Assert(durationInDays > 0, "[LockTokenVault: Lock duration must be at least 1 day.]");

        var tokenBalanceResult = Call(tokenAddress, 0, "GetBalance", new object[] { Message.Sender });
        Assert(ulong.Parse(tokenBalanceResult.ReturnValue.ToString()) >= totalAmount, "[LockTokenVault: Insufficient token balance.]");

        var transferParams = new object[] { Message.Sender, Message.ContractAddress, totalAmount };
        var tokenTransferResult = Call(tokenAddress, 0, "TransferFrom", transferParams);
        Assert(tokenTransferResult.Success && bool.Parse(tokenTransferResult.ReturnValue.ToString()), "[LockTokenVault: Add lock failed. Try again.]");


        var blockNumber = Block.Number;
        var transaction = new LockTransaction
        {
            TokenAddress = tokenAddress,
            CreationTime = blockNumber,
            StartTime = blockNumber,
            EndTime = checked(blockNumber + (60u * 60 * 24 * durationInDays / 16)),
            InitiatorAddress = Message.Sender,
            RecipientAddress = recipientAddress,
            Amount = totalAmount,
            IsActive = true,
            IsClaimed = false,
            IsRevocable = isRevocable,
            IsRevoked = false,
            IsRefunded = false
        };

        SetLockTransaction(transaction);
        UpdateInitiatorLockIndex(Message.Sender, CurrentLockTransactionIndex);
        UpdateRecipientLockIndex(recipientAddress, CurrentLockTransactionIndex);
        Log(new AddLockLog
        {
            LockTransactionIndex = CurrentLockTransactionIndex,
            TokenAddress = tokenAddress,
            CreationTime = blockNumber,
            StartTime = blockNumber,
            EndTime = checked(blockNumber + (60u * 60 * 24 * durationInDays / 16)),
            InitiatorAddress = Message.Sender,
            RecipientAddress = recipientAddress,
            Amount = totalAmount,
            IsActive = true,
            IsClaimed = false,
            IsRevocable = isRevocable,
            IsRevoked = false,
            IsRefunded = false
        });
    }

    public void ClaimLock(long lockTransactionIndex)
    {
        var transactions = GetLockTransactions();
        Assert(lockTransactionIndex >= 0 && lockTransactionIndex < transactions.Length, "[LockTokenVault: Lock doesn't exist.]");

        var lockTransaction = transactions[lockTransactionIndex];
        Assert(lockTransaction.IsActive, "[LockTokenVault: This lock is not active anymore.]");
        Assert(lockTransaction.RecipientAddress == Message.Sender, "[LockTokenVault: Access Forbidden.]");
        Assert(!lockTransaction.IsRevoked, "[LockTokenVault: This lock has been already revoked by the initiator.]");
        Assert(lockTransaction.EndTime < Block.Number, "[LockTokenVault: This lock is not yet ready to be claimed.]");

        var contractTokenTransferResult = Call(lockTransaction.TokenAddress, 0, "TransferTo", new object[] { lockTransaction.RecipientAddress, lockTransaction.Amount });
        Assert(contractTokenTransferResult.Success && bool.Parse(contractTokenTransferResult.ReturnValue.ToString()), "[LockTokenVault: Claim lock token transfer failed. Try again.]");

        lockTransaction.IsClaimed = true;
        lockTransaction.IsActive = false;
        SetLockTransaction(lockTransaction, lockTransactionIndex);
        Log(new ClaimLockLog() 
        { 
            LockTransactionIndex = lockTransactionIndex, 
            InitiatorAddress = lockTransaction.InitiatorAddress, 
            TokenAddress = lockTransaction.TokenAddress, 
            Amount = lockTransaction.Amount,
            ClaimedTime = Block.Number 
        });
    }

    public void RevokeLock(long lockTransactionIndex)
    {
        var transactions = GetLockTransactions();
        Assert(lockTransactionIndex >= 0 && lockTransactionIndex < transactions.Length, "[LockTokenVault: Lock doesn't exist.]");

        var lockTransaction = transactions[lockTransactionIndex];
        Assert(lockTransaction.IsActive, "[LockTokenVault: This lock is not active anymore.]");
        Assert(lockTransaction.InitiatorAddress == Message.Sender, "[LockTokenVault: Access Forbidden.]");
        Assert(!lockTransaction.IsRevoked, "[LockTokenVault: This lock has been already revoked by the initiator.]");
        Assert(lockTransaction.IsRevocable, "[LockTokenVault: This lock is irrevocable.]");

        lockTransaction.IsRevoked = true;
        SetLockTransaction(lockTransaction, lockTransactionIndex);
        Log(new RevokeLockLog() { LockTransactionIndex = lockTransactionIndex, InitiatorAddress = lockTransaction.InitiatorAddress, TokenAddress = lockTransaction.TokenAddress, Amount = lockTransaction.Amount, RevokedTime = Block.Number });
    }

    public void RefundRevockedLock(long lockTransactionIndex)
    {
        var transactions = GetLockTransactions();
        Assert(lockTransactionIndex >= 0 && lockTransactionIndex < transactions.Length, "[LockTokenVault: Lock doesn't exist.]");

        var lockTransaction = transactions[lockTransactionIndex];
        Assert(lockTransaction.IsActive, "[LockTokenVault: This lock is not active anymore.]");
        Assert(lockTransaction.InitiatorAddress == Message.Sender, "[LockTokenVault: Access Forbidden.]");
        Assert(lockTransaction.IsRevoked, "[LockTokenVault: This lock is not yet revoked by the initiator.]");
        Assert(!lockTransaction.IsRefunded, "[LockTokenVault: This lock has been already refunded.]");

        var contractTokenTransferResult = Call(lockTransaction.TokenAddress, 0, "TransferTo", new object[] { lockTransaction.InitiatorAddress, lockTransaction.Amount });
        Assert(contractTokenTransferResult.Success && bool.Parse(contractTokenTransferResult.ReturnValue.ToString()), "[LockTokenVault: Refund token transfer failed. Try again.]");

        lockTransaction.IsRefunded = true;
        lockTransaction.IsActive = false;
        SetLockTransaction(lockTransaction, lockTransactionIndex);

        Log(new RefundRevokedLockLog() { LockTransactionIndex = lockTransactionIndex, InitiatorAddress = lockTransaction.InitiatorAddress, TokenAddress = lockTransaction.TokenAddress, Amount = lockTransaction.Amount, RefundedTime = Block.Number });
    }

    public struct LockTransaction
    {
        public Address TokenAddress;
        public ulong CreationTime;
        public ulong StartTime;
        public ulong EndTime;
        public Address InitiatorAddress;
        public Address RecipientAddress;
        public UInt256 Amount;
        public bool IsClaimed;
        public bool IsRevocable;
        public bool IsRevoked;
        public bool IsRefunded;
        public bool IsActive;
    }

    public struct AddLockLog
    {
        public long LockTransactionIndex;
        public Address TokenAddress;
        public ulong CreationTime;
        public ulong StartTime;
        public ulong EndTime;
        public Address InitiatorAddress;
        public Address RecipientAddress;
        public UInt256 Amount;
        public bool IsClaimed;
        public bool IsRevocable;
        public bool IsRevoked;
        public bool IsRefunded;
        public bool IsActive;
    }

    public struct RevokeLockLog
    {
        public Address TokenAddress;
        public Address InitiatorAddress;
        public long LockTransactionIndex;
        public UInt256 Amount;
        public ulong RevokedTime;
    }

    public struct RefundRevokedLockLog
    {
        public Address TokenAddress;
        public Address InitiatorAddress;
        public long LockTransactionIndex;
        public UInt256 Amount;
        public ulong RefundedTime;
    }

    public struct ClaimLockLog
    {
        public Address TokenAddress;
        public Address InitiatorAddress;
        public long LockTransactionIndex;
        public UInt256 Amount;
        public ulong ClaimedTime;
    }

}