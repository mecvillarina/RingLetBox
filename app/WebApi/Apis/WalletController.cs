using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Wallet.Queries.AccountInfo;
using Application.Features.Wallet.Queries.ListWallets;
using WebApi.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WebApi.Apis
{
    public class WalletController : HttpFunctionBase
    {
        public WalletController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("WalletListWallets")]
        public async Task<IActionResult> ListWallets([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "wallet/list-wallets")] ListWalletsQuery queryArg, HttpRequest req, ExecutionContext context)
        {
            return await ExecuteAsync<ListWalletsQuery, Result<List<string>>>(context, req, queryArg);
        }

        [FunctionName("WalletAccountInfo")]
        public async Task<IActionResult> AccountInfo([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "wallet/account-info")] WalletAccountInfoQuery queryArg, HttpRequest req, ExecutionContext context)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<WalletAccountInfoQuery, Result<WalletAccountInfoQueryResponse>>(context, req, queryArg);
        }

    }
}
