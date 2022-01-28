using Application.Common.Dtos;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Lock.Queries.GetSenderAll
{
    public class GetSenderAllLocksQuery : IRequest<Result<List<LockTransactionWithTypeDto>>>
    {
        public string Sender { get; set; }

        public class GetSenderAllLocksQueryHandler : IRequestHandler<GetSenderAllLocksQuery, Result<List<LockTransactionWithTypeDto>>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IConfiguration _configuration;
            private readonly IMapper _mapper;
            private readonly IDateTime _dateTime;
            public GetSenderAllLocksQueryHandler(ICallContext context, IApplicationDbContext dbContext, IConfiguration configuration, IMapper mapper, IDateTime dateTime)
            {
                _context = context;
                _dbContext = dbContext;
                _configuration = configuration;
                _mapper = mapper;
                _dateTime = dateTime;

            }
            public async Task<Result<List<LockTransactionWithTypeDto>>> Handle(GetSenderAllLocksQuery request, CancellationToken cancellationToken)
            {
                var lockContractAddress = _configuration.GetValue<string>("LockContractAddress");

                var mappedLockTransactions = new List<LockTransactionWithTypeDto>();
                var tokens = await _dbContext.StandardTokens.ToListAsync();

                var initiatorlockTransactions = await _dbContext.LockTransactions.AsQueryable().Where(x => x.LockContractAddress == lockContractAddress && x.InitiatorAddress == request.Sender).ToListAsync();
                var mappedInitiatorLockTransactions = _mapper.Map<List<LockTransactionWithTypeDto>>(initiatorlockTransactions);

                foreach (var lockTransactions in mappedInitiatorLockTransactions)
                {
                    lockTransactions.Type = "Sender";
                }

                var recipientlockTransactions = await _dbContext.LockTransactions.AsQueryable().Where(x => x.LockContractAddress == lockContractAddress && x.RecipientAddress == request.Sender).ToListAsync();
                var mappedRecipientlockTransactions = _mapper.Map<List<LockTransactionWithTypeDto>>(recipientlockTransactions);
                foreach (var lockTransactions in mappedRecipientlockTransactions)
                {
                    lockTransactions.Type = "Receiver";
                }

                mappedLockTransactions.AddRange(mappedInitiatorLockTransactions);
                mappedLockTransactions.AddRange(mappedRecipientlockTransactions);
                mappedLockTransactions = mappedLockTransactions.OrderByDescending(x => x.LockTransactionIndex).ThenBy(x => x.Type).ToList();


                foreach (var lockTransaction in mappedLockTransactions)
                {
                    var token = tokens.FirstOrDefault(x => x.ContractAddress == lockTransaction.TokenAddress);
                    if (token != null)
                    {
                        lockTransaction.TokenName = token.Name;
                        lockTransaction.TokenSymbol = token.Symbol;
                        lockTransaction.TokenDecimal = token.Decimal;
                        lockTransaction.Amount = $"{((ulong)(Convert.ToInt64(lockTransaction.Amount) / Math.Pow(10, token.Decimal)))} {token.Symbol}";
                    }

                    if (lockTransaction.IsActive)
                    {
                        if (lockTransaction.IsRevoked)
                        {
                            lockTransaction.Status = "REVOKED BY THE SENDER";
                        }
                        else if(_dateTime.UtcNow >= lockTransaction.ClaimedDate)
                        {
                            lockTransaction.Status = "UNLOCKED";
                        }
                        else
                        {
                            lockTransaction.Status = "LOCKED";
                        }
                    }
                    else
                    {
                        if (lockTransaction.IsClaimed)
                        {
                            lockTransaction.Status = "CLAIMED";
                        }
                        else if (lockTransaction.IsRefunded)
                        {
                            lockTransaction.Status = "REFUNDED TO SENDER";
                        }
                    }
                }

                return await Result<List<LockTransactionWithTypeDto>>.SuccessAsync(mappedLockTransactions);
            }
        }
    }
}
