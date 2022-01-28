using Application.Common.Models;
using Application.Features.Auth.Commands.Login;
using Application.Features.Wallet.Queries.AccountInfo;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class WalletWebService : WebServiceBase, IWalletWebService
    {
        public WalletWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult<List<string>>> ListWalletsAsync() => GetAsync<List<string>>(WalletEndpoints.ListWallets);
        public Task<IResult<WalletAccountInfoQueryResponse>> AccountInfoAsync(string accessToken) => GetAsync<WalletAccountInfoQueryResponse>(WalletEndpoints.AccountInfo, accessToken);
    }
}
