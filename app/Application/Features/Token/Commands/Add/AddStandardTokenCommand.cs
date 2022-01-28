using Application.Common.Dtos.ApiRequest;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Token.Commands.Add
{
    public class AddStandardTokenCommand : IRequest<IResult>
    {
        public string Sender { get; set; }
        public string ContractAddress { get; set; }

        public class AddStandardTokenCommandHandler : IRequestHandler<AddStandardTokenCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly ISmartContractsService _smartContractsService;

            public AddStandardTokenCommandHandler(ICallContext context, IApplicationDbContext dbContext, ISmartContractsService smartContractsService)
            {
                _context = context;
                _dbContext = dbContext;
                _smartContractsService = smartContractsService;
            }

            public async Task<IResult> Handle(AddStandardTokenCommand request, CancellationToken cancellationToken)
            {
                var token = await _dbContext.StandardTokens.AsQueryable().FirstOrDefaultAsync(x => x.Partition == _context.PartitionKey && x.ContractAddress == request.ContractAddress);

                if (token == null)
                {
                    try
                    {
                        var dec = GetPropertyValue<int>(request.Sender, request.ContractAddress, "Decimals");
                        var name = GetPropertyValue<string>(request.Sender, request.ContractAddress, "Name");
                        var symbol = GetPropertyValue<string>(request.Sender, request.ContractAddress, "Symbol");

                        token = new StandardToken()
                        {
                            ContractAddress = request.ContractAddress,
                            Decimal = dec,
                            Name = name,
                            Symbol = symbol
                        };

                        _dbContext.StandardTokens.Add(token);
                        await _dbContext.SaveChangesAsync();
                    }
                    catch(ContractCallException)
                    {
                        return await Result.FailAsync("Token contract address cannot be added due to it was made with different Contract Hash/Byte Code.");
                    }
                }

                var holderToken = await _dbContext.HolderStandardTokens.AsQueryable().FirstOrDefaultAsync(x => x.Partition == _context.PartitionKey && x.ContractAddress == token.ContractAddress && x.HolderAddress == request.Sender);

                if (holderToken != null)
                {
                    return await Result.FailAsync("Token is already added.");
                }
                else
                {
                    _dbContext.HolderStandardTokens.Add(new HolderStandardToken()
                    {
                        ContractAddress = token.ContractAddress,
                        HolderAddress = request.Sender,
                        Decimal = token.Decimal,
                        Name = token.Name,
                        Symbol = token.Symbol
                    });

                    await _dbContext.SaveChangesAsync();
                    return await Result.SuccessAsync();
                }
            }

            private T GetPropertyValue<T>(string sender, string contractAddress, string propertyName)
            {
                var localCallRequest = _smartContractsService.GetPropertyValue<T>(new SmartContractsPropertyCallRequestDto() { ContractAddress = contractAddress, PropertyName = propertyName, Sender = sender });
                if (!string.IsNullOrEmpty(localCallRequest.ErrorMessage))
                {
                    throw new ContractCallException();
                }

                return localCallRequest.Return;
            }
        }
    }
}