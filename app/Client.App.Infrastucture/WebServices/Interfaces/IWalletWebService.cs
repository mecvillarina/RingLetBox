using Application.Common.Models;
using Application.Features.Wallet.Queries.AccountInfo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface IWalletWebService : IWebService
    {
        Task<IResult<List<string>>> ListWalletsAsync();
        Task<IResult<WalletAccountInfoQueryResponse>> AccountInfoAsync(string accessToken);

    }
}