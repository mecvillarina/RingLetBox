using Application.Common.Dtos;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Lock.Queries.GetAll
{
    public class GetAllLocksQuery : IRequest<Result<List<LockTransactionDto>>>
    {
        public class GetAllLocksQueryHandler : IRequestHandler<GetAllLocksQuery, Result<List<LockTransactionDto>>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IConfiguration _configuration;
            private readonly IMapper _mapper;
            public GetAllLocksQueryHandler(ICallContext context, IApplicationDbContext dbContext, IConfiguration configuration, IMapper mapper)
            {
                _context = context;
                _dbContext = dbContext;
                _configuration = configuration;
                _mapper = mapper;

            }
            public async Task<Result<List<LockTransactionDto>>> Handle(GetAllLocksQuery request, CancellationToken cancellationToken)
            {
                var lockContractAddress = _configuration.GetValue<string>("LockContractAddress");

                var tokens = await _dbContext.StandardTokens.ToListAsync();

                var lockTransactions = await _dbContext.LockTransactions.AsQueryable().Where(x => x.LockContractAddress == lockContractAddress).OrderByDescending(x => x.LockTransactionIndex).ToListAsync();
                var mappedLockTransactions = _mapper.Map<List<LockTransactionDto>>(lockTransactions);

                foreach (var lockTransaction in mappedLockTransactions)
                {
                    var token = tokens.FirstOrDefault(x => x.ContractAddress == lockTransaction.TokenAddress);
                    if (token != null)
                    {
                        lockTransaction.TokenName = token.Name;
                        lockTransaction.TokenSymbol = token.Symbol;
                        lockTransaction.TokenDecimal = token.Decimal;
                    }
                }

                return await Result<List<LockTransactionDto>>.SuccessAsync(mappedLockTransactions);
            }
        }
    }
}
