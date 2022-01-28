using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Lock.Commands.Add
{
    public class AddNewLockCommand : IRequest<IResult>
    {
        public string Sender { get; set; }
        public string TokenContractAddress { get; set; }
        public string TotalAmount { get; set; }
        public string RecipientAddress { get; set; }
        public long DurationInDays { get; set; }
        public bool IsRevocable { get; set; }

        public class AddNewLockCommandHandler : IRequestHandler<AddNewLockCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly ISmartContractsService _smartContractsService;
            private readonly IConfiguration _configuration;
            private readonly IDateTime _dateTime;
            public AddNewLockCommandHandler(ICallContext context, IApplicationDbContext dbContext, ISmartContractsService smartContractsService, IConfiguration configuration, IDateTime dateTime)
            {
                _context = context;
                _dbContext = dbContext;
                _smartContractsService = smartContractsService;
                _configuration = configuration;
                _dateTime = dateTime;
            }

            public async Task<IResult> Handle(AddNewLockCommand request, CancellationToken cancellationToken)
            {
                var lockContractAddress = _configuration.GetValue<string>("LockContractAddress");
                var allowanceReturn = _smartContractsService.GetMethodValue(_context.WalletName, _context.WalletPassword, request.Sender, request.TokenContractAddress, "Allowance", new() { $"9#{request.Sender}", $"9#{lockContractAddress}" });

                if (allowanceReturn.Success)
                {
                    var approveReturn = _smartContractsService.GetMethodValue(_context.WalletName, _context.WalletPassword, request.Sender, request.TokenContractAddress, "Approve", new() { $"9#{lockContractAddress}", $"12#{allowanceReturn.ReturnValue}", $"12#{request.TotalAmount}" });

                    if (approveReturn.Success && approveReturn.ReturnValue == "True")
                    {
                        var addLockReturn = _smartContractsService.GetMethodValue(_context.WalletName, _context.WalletPassword, request.Sender, lockContractAddress, "AddLock", new() { $"9#{request.TokenContractAddress}", $"12#{request.TotalAmount}", $"7#{request.DurationInDays}", $"9#{request.RecipientAddress}", $"1#{request.IsRevocable}" });

                        if (addLockReturn.Success)
                        {
                            var log = addLockReturn.Logs.FirstOrDefault(x => x.Log.ContainsKey("event") && x.Log["event"].ToString() == "AddLockLog");

                            if (log != null)
                            {
                                _dbContext.LockTransactions.Add(new LockTransaction()
                                {
                                    Partition = _context.PartitionKey,
                                    LockContractAddress = lockContractAddress,
                                    LockTransactionIndex = Convert.ToInt64(log.Log["lockTransactionIndex"].ToString()),
                                    TokenAddress = log.Log["tokenAddress"].ToString(),
                                    CreationTime = Convert.ToInt64(log.Log["creationTime"].ToString()),
                                    StartTime = Convert.ToInt64(log.Log["startTime"].ToString()),
                                    EndTime = Convert.ToInt64(log.Log["endTime"].ToString()),
                                    InitiatorAddress = log.Log["initiatorAddress"].ToString(),
                                    RecipientAddress = log.Log["recipientAddress"].ToString(),
                                    Amount = log.Log["amount"].ToString(),
                                    IsClaimed = Convert.ToBoolean(log.Log["isClaimed"].ToString()),
                                    IsRevocable = Convert.ToBoolean(log.Log["isRevocable"].ToString()),
                                    IsRevoked = Convert.ToBoolean(log.Log["isRevoked"].ToString()),
                                    IsRefunded = Convert.ToBoolean(log.Log["isRefunded"].ToString()),
                                    IsActive = Convert.ToBoolean(log.Log["isActive"].ToString()),
                                    ClaimedDate = _dateTime.UtcNow.AddDays(request.DurationInDays)
                                });

                                await _dbContext.SaveChangesAsync();
                                return await Result.SuccessAsync();
                            }
                        }
                        else
                        {
                            return await Result.FailAsync(addLockReturn.Error.Split('[', ']')[1]);
                        }
                    }
                    else
                    {
                        return await Result.FailAsync(approveReturn.Error);
                    }
                }
                else
                {
                    return await Result.FailAsync(allowanceReturn.Error);
                }

                return await Result.FailAsync();
            }
        }
    }
}
