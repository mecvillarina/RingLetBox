using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Lock.Commands.RefundRevoked
{
    public class RefundRevokedLockCommand : IRequest<IResult>
    {
        public string Sender { get; set; }
        public long LockTransactionIndex { get; set; }

        public class RefundRevokedLockCommandHandler : IRequestHandler<RefundRevokedLockCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly ISmartContractsService _smartContractsService;
            private readonly IConfiguration _configuration;
            private readonly IDateTime _dateTime;

            public RefundRevokedLockCommandHandler(ICallContext context, IApplicationDbContext dbContext, ISmartContractsService smartContractsService, IConfiguration configuration, IDateTime dateTime)
            {
                _context = context;
                _dbContext = dbContext;
                _smartContractsService = smartContractsService;
                _configuration = configuration;
                _dateTime = dateTime;
            }

            public async Task<IResult> Handle(RefundRevokedLockCommand request, CancellationToken cancellationToken)
            {
                var lockContractAddress = _configuration.GetValue<string>("LockContractAddress");
                var revokeLockReturn = _smartContractsService.GetMethodValue(_context.WalletName, _context.WalletPassword, request.Sender, lockContractAddress, "RefundRevockedLock", new() { $"8#{request.LockTransactionIndex}" });

                if (revokeLockReturn.Success)
                {
                    var lockTransaction = _dbContext.LockTransactions.AsQueryable().FirstOrDefault(x => x.LockTransactionIndex == request.LockTransactionIndex && x.LockContractAddress == lockContractAddress);

                    if (lockTransaction != null)
                    {
                        lockTransaction.IsRefunded = true;
                        lockTransaction.IsActive = false;
                        _dbContext.LockTransactions.Update(lockTransaction);
                        await _dbContext.SaveChangesAsync();
                        return await Result.SuccessAsync();
                    }
                }
                else
                {
                    return await Result.FailAsync(revokeLockReturn.Error.Split('[', ']')[1]);
                }

                return await Result.FailAsync();
            }
        }
    }
}