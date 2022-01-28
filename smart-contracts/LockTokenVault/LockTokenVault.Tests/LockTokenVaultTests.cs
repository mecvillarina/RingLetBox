using Moq;
using Stratis.SmartContracts;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using static LockTokenVault;

namespace LockTokenVaultTests
{
    public class LockTokenVaultTests
    {
        private readonly Mock<ISmartContractState> mockContractState;
        private readonly Mock<IPersistentState> mockPersistentState;
        private readonly Mock<IContractLogger> mockContractLogger;
        private readonly Mock<IMessage> mockMessage;
        private readonly Mock<IBlock> mockBlock;
        private readonly Mock<IInternalTransactionExecutor> mockInternalExecutor;
        private Address senderAddress;
        private Address tokenContractAddress;
        private Address recipientAddress;
        private Address notATokenContractAddress;

        public LockTokenVaultTests()
        {
            this.mockContractLogger = new Mock<IContractLogger>();
            this.mockPersistentState = new Mock<IPersistentState>();
            this.mockContractState = new Mock<ISmartContractState>();
            this.mockMessage = new Mock<IMessage>();
            this.mockBlock = new Mock<IBlock>();
            this.mockInternalExecutor = new Mock<IInternalTransactionExecutor>();
            this.mockContractState.Setup(s => s.PersistentState).Returns(this.mockPersistentState.Object);
            this.mockContractState.Setup(s => s.ContractLogger).Returns(this.mockContractLogger.Object);
            this.mockContractState.Setup(s => s.Message).Returns(this.mockMessage.Object);
            this.mockContractState.Setup(s => s.Block).Returns(this.mockBlock.Object);
            this.mockContractState.Setup(s => s.InternalTransactionExecutor).Returns(this.mockInternalExecutor.Object);
            this.senderAddress = "0x0000000000000000000000000000000000000001".HexToAddress();
            this.tokenContractAddress = "0x0000000000000000000000000000000000000002".HexToAddress();
            this.recipientAddress = "0x0000000000000000000000000000000000000003".HexToAddress();
            this.notATokenContractAddress = "0x0000000000000000000000000000000000000004".HexToAddress();
        }

        [Fact]
        public void AddLock_TokenAddress_IsNot_Contract_ThrowsAssertException()
        {
            UInt256 amount = 100;
            ulong durationInDays = 1;
            bool isRevocable = true;

            this.mockPersistentState.Setup(m => m.IsContract(notATokenContractAddress)).Returns(false);

            var lockTokenVault = new LockTokenVault(this.mockContractState.Object);

            Assert.ThrowsAny<SmartContractAssertException>(() =>
                lockTokenVault.AddLock(notATokenContractAddress, amount, durationInDays, recipientAddress, isRevocable));
        }

        [Fact]
        public void AddLock_TokenAddress_Is_Contract_And_Amount_IsEqualTo_Zero_ThrowsAssertException()
        {
            UInt256 amount = 0;
            ulong durationInDays = 1;
            bool isRevocable = true;

            this.mockPersistentState.Setup(m => m.IsContract(tokenContractAddress)).Returns(true);

            var lockTokenVault = new LockTokenVault(this.mockContractState.Object);

            Assert.ThrowsAny<SmartContractAssertException>(() =>
                lockTokenVault.AddLock(tokenContractAddress, amount, durationInDays, recipientAddress, isRevocable));
        }

        [Fact]
        public void AddLock_DurationInDays_IsEqualTo_Zero_ThrowsAssertException()
        {
            UInt256 amount = 100;
            ulong durationInDays = 0;
            bool isRevocable = true;

            this.mockPersistentState.Setup(m => m.IsContract(tokenContractAddress)).Returns(true);

            var lockTokenVault = new LockTokenVault(this.mockContractState.Object);

            Assert.ThrowsAny<SmartContractAssertException>(() =>
                lockTokenVault.AddLock(tokenContractAddress, amount, durationInDays, recipientAddress, isRevocable));
        }

        [Fact]
        public void AddLock_GetBalance_BalanceAmount_IsLessThanTo_TotalAmount_ThrowsAssertException()
        {
            UInt256 totalAmount = 100;
            ulong durationInDays = 1;
            bool isRevocable = true;

            this.mockPersistentState.Setup(m => m.IsContract(tokenContractAddress)).Returns(true);

            this.mockInternalExecutor.Setup(s =>
                s.Call(It.IsAny<ISmartContractState>(),
                    It.IsAny<Address>(),
                    It.IsAny<ulong>(),
                    "GetBalance",
                    It.IsAny<object[]>(),
                    It.IsAny<ulong>()))
                    .Returns(TransferResult.Transferred("99"));

            var lockTokenVault = new LockTokenVault(this.mockContractState.Object);

            Assert.ThrowsAny<SmartContractAssertException>(() =>
                lockTokenVault.AddLock(tokenContractAddress, totalAmount, durationInDays, recipientAddress, isRevocable));
        }

        [Fact]
        public void AddLock_WithRightConditions_Success()
        {
            UInt256 totalAmount = 100;
            ulong durationInDays = 1;
            bool isRevocable = true;

            this.mockPersistentState.Setup(m => m.IsContract(tokenContractAddress)).Returns(true);

            this.mockInternalExecutor.Setup(s =>
                s.Call(It.IsAny<ISmartContractState>(),
                    It.IsAny<Address>(),
                    It.IsAny<ulong>(),
                    "GetBalance",
                    It.IsAny<object[]>(),
                    It.IsAny<ulong>()))
                    .Returns(TransferResult.Transferred("100"));

            this.mockInternalExecutor.Setup(s =>
                s.Call(It.IsAny<ISmartContractState>(),
                    It.IsAny<Address>(),
                    It.IsAny<ulong>(),
                    "TransferFrom",
                    It.IsAny<object[]>(),
                    It.IsAny<ulong>()))
                    .Returns(TransferResult.Transferred(true));

            this.mockMessage.Setup(m => m.Sender).Returns(senderAddress);

            var lockTokenVault = new LockTokenVault(this.mockContractState.Object);
            lockTokenVault.AddLock(tokenContractAddress, totalAmount, durationInDays, recipientAddress, isRevocable);
         
            mockContractLogger.Verify(l => l.Log(It.IsAny<ISmartContractState>(), It.IsAny<AddLockLog>()));
        }

        [Fact]
        public void ClaimLock_AddOneTransaction_And_LockTransactionIndex_IsGreaterThanTo_TransactionIndex_ThrowsAssertException()
        {
            this.mockPersistentState.Setup(m => m.GetArray<LockTransaction>("LockTransactions")).Returns(new LockTransaction[] {
                new LockTransaction()
                {
                    Amount = 100,
                    CreationTime = 0,
                    EndTime = 5400,
                    InitiatorAddress =
                    senderAddress,
                    IsActive = true,
                    IsClaimed = false,
                    IsRefunded =false ,
                    IsRevocable = false,
                    IsRevoked = false,
                    RecipientAddress = recipientAddress,
                    StartTime = 0,
                    TokenAddress = tokenContractAddress
                }});

            var lockTokenVault = new LockTokenVault(this.mockContractState.Object);

            long lockTransactionIndex = 10;
            Assert.ThrowsAny<SmartContractAssertException>(() => lockTokenVault.ClaimLock(lockTransactionIndex));
        }

        [Fact]
        public void ClaimLock_Lock_IsNot_Active_ThrowsAssertException()
        {
            this.mockPersistentState.Setup(m => m.GetArray<LockTransaction>("LockTransactions")).Returns(new LockTransaction[] {
                new LockTransaction()
                {
                    Amount = 100,
                    CreationTime = 0,
                    EndTime = 5400,
                    InitiatorAddress = senderAddress,
                    IsActive = false,
                    IsClaimed = false,
                    IsRefunded =false ,
                    IsRevocable = false,
                    IsRevoked = false,
                    RecipientAddress = recipientAddress,
                    StartTime = 0,
                    TokenAddress = tokenContractAddress
                }});

            var lockTokenVault = new LockTokenVault(this.mockContractState.Object);

            long lockTransactionIndex = 0;
            Assert.ThrowsAny<SmartContractAssertException>(() => lockTokenVault.ClaimLock(lockTransactionIndex));
        }

        [Fact]
        public void ClaimLock_Receipient_IsNotMatch_With_MessageSender_ThrowsAssertException()
        {
            this.mockPersistentState.Setup(m => m.GetArray<LockTransaction>("LockTransactions")).Returns(new LockTransaction[] {
                new LockTransaction()
                {
                    Amount = 100,
                    CreationTime = 0,
                    EndTime = 5400,
                    InitiatorAddress = senderAddress,
                    IsActive = true,
                    IsClaimed = false,
                    IsRefunded =false ,
                    IsRevocable = false,
                    IsRevoked = false,
                    RecipientAddress = recipientAddress,
                    StartTime = 0,
                    TokenAddress = tokenContractAddress
                }});

            this.mockMessage.Setup(m => m.Sender).Returns(senderAddress);

            var lockTokenVault = new LockTokenVault(this.mockContractState.Object);

            long lockTransactionIndex = 0;
            Assert.ThrowsAny<SmartContractAssertException>(() => lockTokenVault.ClaimLock(lockTransactionIndex));
        }

        [Fact]
        public void ClaimLock_TransactionEndTime_Is_Still_Greater_Than_BlockNumber_ThrowsAssertException()
        {
            this.mockPersistentState.Setup(m => m.GetArray<LockTransaction>("LockTransactions")).Returns(new LockTransaction[] {
                new LockTransaction()
                {
                    Amount = 100,
                    CreationTime = 0,
                    EndTime = 5400,
                    InitiatorAddress = senderAddress,
                    IsActive = true,
                    IsClaimed = false,
                    IsRefunded =false ,
                    IsRevocable = false,
                    IsRevoked = false,
                    RecipientAddress = recipientAddress,
                    StartTime = 0,
                    TokenAddress = tokenContractAddress
                }});

            this.mockMessage.Setup(m => m.Sender).Returns(recipientAddress);
            this.mockBlock.Setup(m => m.Number).Returns(2000);

            var lockTokenVault = new LockTokenVault(this.mockContractState.Object);

            long lockTransactionIndex = 0;
            Assert.ThrowsAny<SmartContractAssertException>(() => lockTokenVault.ClaimLock(lockTransactionIndex));
        }

        [Fact]
        public void ClaimLock_WithRightConditions_Success()
        {
            this.mockPersistentState.Setup(m => m.GetArray<LockTransaction>("LockTransactions")).Returns(new LockTransaction[] {
                new LockTransaction()
                {
                    Amount = 100,
                    CreationTime = 0,
                    EndTime = 5400,
                    InitiatorAddress = senderAddress,
                    IsActive = true,
                    IsClaimed = false,
                    IsRefunded =false ,
                    IsRevocable = false,
                    IsRevoked = false,
                    RecipientAddress = recipientAddress,
                    StartTime = 0,
                    TokenAddress = tokenContractAddress
                }});

            this.mockMessage.Setup(m => m.Sender).Returns(recipientAddress);
            this.mockBlock.Setup(m => m.Number).Returns(10000);

            this.mockInternalExecutor.Setup(s =>
                s.Call(
                    It.IsAny<ISmartContractState>(),
                    It.IsAny<Address>(),
                    It.IsAny<ulong>(),
                    "TransferTo",
                    It.IsAny<object[]>(),
                    It.IsAny<ulong>()))
                .Returns(TransferResult.Transferred(true));

            var lockTokenVault = new LockTokenVault(this.mockContractState.Object);
            long lockTransactionIndex = 0;
            lockTokenVault.ClaimLock(lockTransactionIndex);

            mockContractLogger.Verify(l => l.Log(It.IsAny<ISmartContractState>(), It.IsAny<ClaimLockLog>()));
        }

        [Fact]
        public void RevokeLock_AddOneTransaction_And_LockTransactionIndex_IsGreaterThanTo_TransactionIndex_ThrowsAssertException()
        {
            this.mockPersistentState.Setup(m => m.GetArray<LockTransaction>("LockTransactions")).Returns(new LockTransaction[] {
                new LockTransaction()
                {
                    Amount = 100,
                    CreationTime = 0,
                    EndTime = 5400,
                    InitiatorAddress =
                    senderAddress,
                    IsActive = true,
                    IsClaimed = false,
                    IsRefunded =false ,
                    IsRevocable = false,
                    IsRevoked = false,
                    RecipientAddress = recipientAddress,
                    StartTime = 0,
                    TokenAddress = tokenContractAddress
                }});

            var lockTokenVault = new LockTokenVault(this.mockContractState.Object);

            long lockTransactionIndex = 10;
            Assert.ThrowsAny<SmartContractAssertException>(() => lockTokenVault.RevokeLock(lockTransactionIndex));
        }


        [Fact]
        public void RevokeLock_Lock_IsNot_Active_ThrowsAssertException()
        {
            this.mockPersistentState.Setup(m => m.GetArray<LockTransaction>("LockTransactions")).Returns(new LockTransaction[] {
                new LockTransaction()
                {
                    Amount = 100,
                    CreationTime = 0,
                    EndTime = 5400,
                    InitiatorAddress = senderAddress,
                    IsActive = false,
                    IsClaimed = true,
                    IsRefunded = false ,
                    IsRevocable = false,
                    IsRevoked = false,
                    RecipientAddress = recipientAddress,
                    StartTime = 0,
                    TokenAddress = tokenContractAddress
                }});

            var lockTokenVault = new LockTokenVault(this.mockContractState.Object);

            long lockTransactionIndex = 0;
            Assert.ThrowsAny<SmartContractAssertException>(() => lockTokenVault.RevokeLock(lockTransactionIndex));
        }

        [Fact]
        public void RevokeLock_LockSender_IsNotMatch_With_MessageSender_ThrowsAssertException()
        {
            this.mockPersistentState.Setup(m => m.GetArray<LockTransaction>("LockTransactions")).Returns(new LockTransaction[] {
                new LockTransaction()
                {
                    Amount = 100,
                    CreationTime = 0,
                    EndTime = 5400,
                    InitiatorAddress = senderAddress,
                    IsActive = true,
                    IsClaimed = false,
                    IsRefunded =false ,
                    IsRevocable = false,
                    IsRevoked = false,
                    RecipientAddress = recipientAddress,
                    StartTime = 0,
                    TokenAddress = tokenContractAddress
                }});

            this.mockMessage.Setup(m => m.Sender).Returns(recipientAddress);

            var lockTokenVault = new LockTokenVault(this.mockContractState.Object);

            long lockTransactionIndex = 0;
            Assert.ThrowsAny<SmartContractAssertException>(() => lockTokenVault.RevokeLock(lockTransactionIndex));
        }

        [Fact]
        public void RevokeLock_Lock_HasAlready_Revoked_ThrowsAssertException()
        {
            this.mockPersistentState.Setup(m => m.GetArray<LockTransaction>("LockTransactions")).Returns(new LockTransaction[] {
                new LockTransaction()
                {
                    Amount = 100,
                    CreationTime = 0,
                    EndTime = 5400,
                    InitiatorAddress = senderAddress,
                    IsActive = true,
                    IsClaimed = false,
                    IsRefunded = false ,
                    IsRevocable = false,
                    IsRevoked = true,
                    RecipientAddress = recipientAddress,
                    StartTime = 0,
                    TokenAddress = tokenContractAddress
                }});

            this.mockMessage.Setup(m => m.Sender).Returns(senderAddress);

            var lockTokenVault = new LockTokenVault(this.mockContractState.Object);

            long lockTransactionIndex = 0;
            Assert.ThrowsAny<SmartContractAssertException>(() => lockTokenVault.RevokeLock(lockTransactionIndex));
        }

        [Fact]
        public void RevokeLock_Lock_IsNot_Revocable_ThrowsAssertException()
        {
            this.mockPersistentState.Setup(m => m.GetArray<LockTransaction>("LockTransactions")).Returns(new LockTransaction[] {
                new LockTransaction()
                {
                    Amount = 100,
                    CreationTime = 0,
                    EndTime = 5400,
                    InitiatorAddress = senderAddress,
                    IsActive = true,
                    IsClaimed = false,
                    IsRefunded = false ,
                    IsRevocable = false,
                    IsRevoked = false,
                    RecipientAddress = recipientAddress,
                    StartTime = 0,
                    TokenAddress = tokenContractAddress
                }});

            this.mockMessage.Setup(m => m.Sender).Returns(senderAddress);

            var lockTokenVault = new LockTokenVault(this.mockContractState.Object);

            long lockTransactionIndex = 0;
            Assert.ThrowsAny<SmartContractAssertException>(() => lockTokenVault.RevokeLock(lockTransactionIndex));
        }

        [Fact]
        public void RevokeLock_WithRightConditions_Success()
        {
            this.mockPersistentState.Setup(m => m.GetArray<LockTransaction>("LockTransactions")).Returns(new LockTransaction[] {
                new LockTransaction()
                {
                    Amount = 100,
                    CreationTime = 0,
                    EndTime = 5400,
                    InitiatorAddress = senderAddress,
                    IsActive = true,
                    IsClaimed = false,
                    IsRefunded =false ,
                    IsRevocable = true,
                    IsRevoked = false,
                    RecipientAddress = recipientAddress,
                    StartTime = 0,
                    TokenAddress = tokenContractAddress
                }});

            this.mockMessage.Setup(m => m.Sender).Returns(senderAddress);

            var lockTokenVault = new LockTokenVault(this.mockContractState.Object);
            long lockTransactionIndex = 0;
            lockTokenVault.RevokeLock(lockTransactionIndex);

            mockContractLogger.Verify(l => l.Log(It.IsAny<ISmartContractState>(), It.IsAny<RevokeLockLog>()));
        }

        [Fact]
        public void RefundRevockedLock_AddOneTransaction_And_LockTransactionIndex_IsGreaterThanTo_TransactionIndex_ThrowsAssertException()
        {
            this.mockPersistentState.Setup(m => m.GetArray<LockTransaction>("LockTransactions")).Returns(new LockTransaction[] {
                new LockTransaction()
                {
                    Amount = 100,
                    CreationTime = 0,
                    EndTime = 5400,
                    InitiatorAddress =
                    senderAddress,
                    IsActive = true,
                    IsClaimed = false,
                    IsRefunded =false ,
                    IsRevocable = true,
                    IsRevoked = true,
                    RecipientAddress = recipientAddress,
                    StartTime = 0,
                    TokenAddress = tokenContractAddress
                }});

            var lockTokenVault = new LockTokenVault(this.mockContractState.Object);

            long lockTransactionIndex = 10;
            Assert.ThrowsAny<SmartContractAssertException>(() => lockTokenVault.RefundRevockedLock(lockTransactionIndex));
        }

        [Fact]
        public void RefundRevockedLock_Lock_IsNot_Active_ThrowsAssertException()
        {
            this.mockPersistentState.Setup(m => m.GetArray<LockTransaction>("LockTransactions")).Returns(new LockTransaction[] {
                new LockTransaction()
                {
                    Amount = 100,
                    CreationTime = 0,
                    EndTime = 5400,
                    InitiatorAddress = senderAddress,
                    IsActive = false,
                    IsClaimed = true,
                    IsRefunded = false,
                    IsRevocable = false,
                    IsRevoked = false,
                    RecipientAddress = recipientAddress,
                    StartTime = 0,
                    TokenAddress = tokenContractAddress
                }});

            var lockTokenVault = new LockTokenVault(this.mockContractState.Object);

            long lockTransactionIndex = 0;
            Assert.ThrowsAny<SmartContractAssertException>(() => lockTokenVault.RefundRevockedLock(lockTransactionIndex));
        }

        [Fact]
        public void RefundRevockedLock_LockSender_IsNotMatch_With_MessageSender_ThrowsAssertException()
        {
            this.mockPersistentState.Setup(m => m.GetArray<LockTransaction>("LockTransactions")).Returns(new LockTransaction[] {
                new LockTransaction()
                {
                    Amount = 100,
                    CreationTime = 0,
                    EndTime = 5400,
                    InitiatorAddress = senderAddress,
                    IsActive = true,
                    IsClaimed = false,
                    IsRefunded =false,
                    IsRevocable = true,
                    IsRevoked = true,
                    RecipientAddress = recipientAddress,
                    StartTime = 0,
                    TokenAddress = tokenContractAddress
                }});

            this.mockMessage.Setup(m => m.Sender).Returns(recipientAddress);

            var lockTokenVault = new LockTokenVault(this.mockContractState.Object);

            long lockTransactionIndex = 0;
            Assert.ThrowsAny<SmartContractAssertException>(() => lockTokenVault.RefundRevockedLock(lockTransactionIndex));
        }

        [Fact]
        public void RefundRevockedLock_Lock_HasAlready_Refunded_ThrowsAssertException()
        {
            this.mockPersistentState.Setup(m => m.GetArray<LockTransaction>("LockTransactions")).Returns(new LockTransaction[] {
                new LockTransaction()
                {
                    Amount = 100,
                    CreationTime = 0,
                    EndTime = 5400,
                    InitiatorAddress = senderAddress,
                    IsActive = true,
                    IsClaimed = false,
                    IsRefunded = true,
                    IsRevocable = true,
                    IsRevoked = true,
                    RecipientAddress = recipientAddress,
                    StartTime = 0,
                    TokenAddress = tokenContractAddress
                }});

            this.mockMessage.Setup(m => m.Sender).Returns(senderAddress);

            var lockTokenVault = new LockTokenVault(this.mockContractState.Object);

            long lockTransactionIndex = 0;
            Assert.ThrowsAny<SmartContractAssertException>(() => lockTokenVault.RefundRevockedLock(lockTransactionIndex));
        }

        [Fact]
        public void RefundRevockedLock_WithRightConditions_Success()
        {
            this.mockPersistentState.Setup(m => m.GetArray<LockTransaction>("LockTransactions")).Returns(new LockTransaction[] {
                new LockTransaction()
                {
                    Amount = 100,
                    CreationTime = 0,
                    EndTime = 5400,
                    InitiatorAddress = senderAddress,
                    IsActive = true,
                    IsClaimed = false,
                    IsRefunded = false,
                    IsRevocable = true,
                    IsRevoked = true,
                    RecipientAddress = recipientAddress,
                    StartTime = 0,
                    TokenAddress = tokenContractAddress
                }});

            this.mockMessage.Setup(m => m.Sender).Returns(senderAddress);

            this.mockInternalExecutor.Setup(s =>
                s.Call(
                    It.IsAny<ISmartContractState>(),
                    It.IsAny<Address>(),
                    It.IsAny<ulong>(),
                    "TransferTo",
                    It.IsAny<object[]>(),
                    It.IsAny<ulong>()))
                .Returns(TransferResult.Transferred(true));

            var lockTokenVault = new LockTokenVault(this.mockContractState.Object);
            long lockTransactionIndex = 0;
            lockTokenVault.RefundRevockedLock(lockTransactionIndex);

            mockContractLogger.Verify(l => l.Log(It.IsAny<ISmartContractState>(), It.IsAny<RefundRevokedLockLog>()));
        }
    }
}
