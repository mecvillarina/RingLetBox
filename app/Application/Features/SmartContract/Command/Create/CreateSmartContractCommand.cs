using Application.Common.Dtos.ApiRequest;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SmartContract.Commands.Create
{
    public class CreateSmartContractCommand : IRequest<Result<string>>
    {
        public string Sender { get; set; }
        public string ContractCode { get; set; }
        public List<string> Parameters { get; set; }

        public class CreateSmartContractCommandHandler : IRequestHandler<CreateSmartContractCommand, Result<string>>
        {
            private readonly ICallContext _context;
            private readonly ISmartContractsService _smartContractsService;

            public CreateSmartContractCommandHandler(ICallContext context, IApplicationDbContext dbContext, ISmartContractsService smartContractsService)
            {
                _context = context;
                _smartContractsService = smartContractsService;
            }

            public async Task<Result<string>> Handle(CreateSmartContractCommand request, CancellationToken cancellationToken)
            {
                var args = new SmartContractWalletCreateRequestDto()
                {
                    Amount = 0,
                    ContractCode = request.ContractCode,
                    FeeAmount = 0.001,
                    GasLimit = 250000,
                    GasPrice = 100,
                    Parameters = request.Parameters,
                    Sender = request.Sender,
                    Password = _context.WalletPassword,
                    WalletName = _context.WalletName
                };

                var transactionHash = _smartContractsService.Create(args);

                while (true)
                {
                    try
                    {
                        var receipt = _smartContractsService.Receipt(transactionHash);

                        if (!receipt.Success) return await Result<string>.FailAsync(receipt.Error);

                        return await Result<string>.SuccessAsync(receipt.NewContractAddress);
                    }
                    catch
                    {
                        Task.Delay(1000).Wait();
                    }
                }
            }
        }
    }
}