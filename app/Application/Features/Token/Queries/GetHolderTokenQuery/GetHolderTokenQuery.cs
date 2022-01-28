using Application.Common.Dtos;
using Application.Common.Dtos.ApiRequest;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Token.Queries.GetHolderTokenQuery
{
    public class GetHolderTokenQuery : IRequest<Result<HolderStandardTokenDto>>
    {
        public string Sender { get; set; }
        public string ContractAddress { get; set; }
        public class GetTokenQueryHandler : IRequestHandler<GetHolderTokenQuery, Result<HolderStandardTokenDto>>
        {
            private readonly ICallContext _context;
            private readonly IMapper _mapper;
            private readonly IApplicationDbContext _dbContext;
            private readonly ISmartContractsService _smartContractsService;

            public GetTokenQueryHandler(ICallContext context, IMapper mapper, IApplicationDbContext dbContext, ISmartContractsService smartContractsService)
            {
                _context = context;
                _mapper = mapper;
                _dbContext = dbContext;
                _smartContractsService = smartContractsService;
            }

            public async Task<Result<HolderStandardTokenDto>> Handle(GetHolderTokenQuery request, CancellationToken cancellationToken)
            {
                var token = await _dbContext.HolderStandardTokens.AsQueryable().FirstOrDefaultAsync(x => x.Partition == _context.PartitionKey && x.HolderAddress == request.Sender && x.ContractAddress == request.ContractAddress);

                if (token == null) throw new NotFoundException();

                var mappedToken = _mapper.Map<HolderStandardTokenDto>(token);
                mappedToken.Balance = GetMethodValue<string>(request.Sender, token.ContractAddress, "GetBalance");
                mappedToken.TotalSupply = GetMethodValue<string>(request.Sender, token.ContractAddress, "GetBalance");
                return await Result<HolderStandardTokenDto>.SuccessAsync(mappedToken);
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
