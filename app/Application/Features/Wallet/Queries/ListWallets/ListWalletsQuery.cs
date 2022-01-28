using MediatR;
using Application.Common.Interfaces;
using Application.Common.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Wallet.Queries.ListWallets
{
    public class ListWalletsQuery : IRequest<Result<List<string>>>
    {
        public class ListWalletsQueryHandler : IRequestHandler<ListWalletsQuery, Result<List<string>>>
        {
            private readonly IWalletService _walletService;

            public ListWalletsQueryHandler(IWalletService walletService)
            {
                _walletService = walletService;
            }

            public async Task<Result<List<string>>> Handle(ListWalletsQuery request, CancellationToken cancellationToken)
            {
                return await Result<List<string>>.SuccessAsync(_walletService.ListWallets());
            }
        }
    }
}
