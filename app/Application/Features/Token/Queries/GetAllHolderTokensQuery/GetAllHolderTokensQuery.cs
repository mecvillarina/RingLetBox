using Application.Common.Dtos;
using Application.Common.Dtos.ApiRequest;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Token.Queries.GetAllHolderTokensQuery
{
    public class GetAllHolderTokensQuery : IRequest<Result<List<HolderStandardTokenDto>>>
    {
        public string Sender { get; set; }

        public class GetAllStandardTokensQueryHandler : IRequestHandler<GetAllHolderTokensQuery, Result<List<HolderStandardTokenDto>>>
        {
            private readonly ICallContext _context;
            private readonly IMapper _mapper;
            private readonly IApplicationDbContext _dbContext;
            private readonly ISmartContractsService _smartContractsService;

            public GetAllStandardTokensQueryHandler(ICallContext context, IMapper mapper, IApplicationDbContext dbContext, ISmartContractsService smartContractsService)
            {
                _context = context;
                _mapper = mapper;
                _dbContext = dbContext;
                _smartContractsService = smartContractsService;
            }

            public async Task<Result<List<HolderStandardTokenDto>>> Handle(GetAllHolderTokensQuery request, CancellationToken cancellationToken)
            {
                var tokens = await _dbContext.HolderStandardTokens.AsQueryable().Where(x => x.Partition == _context.PartitionKey && x.HolderAddress == request.Sender).OrderBy(x => x.Symbol).ThenBy(x => x.Name).ToListAsync();
                var mappedTokens = _mapper.Map<List<HolderStandardTokenDto>>(tokens);

                foreach (var token in mappedTokens)
                {
                    token.Balance = GetMethodValue<string>(request.Sender, token.ContractAddress, "GetBalance");
                    token.Decimal = GetPropertyValue<int>(request.Sender, token.ContractAddress, "Decimals");
                    token.TotalSupply = GetPropertyValue<string>(request.Sender, token.ContractAddress, "TotalSupply");
                }

                return await Result<List<HolderStandardTokenDto>>.SuccessAsync(mappedTokens);
            }

            private T GetMethodValue<T>(string sender, string contractAddress, string methodName)
            {
                var args = new SmartContractsLocalCallRequestDto()
                {
                    Amount = 0,
                    ContractAddress = contractAddress,
                    GasLimit = 100000,
                    GasPrice = 100,
                    MethodName = methodName,
                    Parameters = new() { $"9#{sender}" },
                    Sender = sender,
                };

                var localCallRequest = _smartContractsService.LocalCall<T>(args);
                return localCallRequest.Return;
            }

            private T GetPropertyValue<T>(string sender, string contractAddress, string propertyName)
            {
                var localCallRequest = _smartContractsService.GetPropertyValue<T>(new SmartContractsPropertyCallRequestDto() { ContractAddress = contractAddress, PropertyName = propertyName, Sender = sender });
                return localCallRequest.Return;
            }
        }
    }
}
