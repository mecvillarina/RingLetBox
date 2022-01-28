using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.SmartContract.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using WebApi.Base;

namespace WebApi.Apis
{
    public class SmartContractController : HttpFunctionBase
    {
        public SmartContractController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("SmartContractCreate")]
        public async Task<IActionResult> Create([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "smartContract/create")] CreateSmartContractCommand commandArg, HttpRequest req, ExecutionContext context)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<CreateSmartContractCommand, Result<string>>(context, req, commandArg);
        }
    }
}
