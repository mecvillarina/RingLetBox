using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Recover;
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
    public class AuthController : HttpFunctionBase
    {
        public AuthController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }


        [FunctionName("AuthLogin")]
        public async Task<IActionResult> Login([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth/login")] LoginCommand commandArg, HttpRequest req, ExecutionContext context)
        {
            return await ExecuteAsync<LoginCommand, Result<LoginCommandResponse>>(context, req, commandArg);
        }

        [FunctionName("AuthRecover")]
        public async Task<IActionResult> Recover([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth/recover")] RecoverCommand commandArg, HttpRequest req, ExecutionContext context)
        {
            return await ExecuteAsync<RecoverCommand, IResult>(context, req, commandArg);
        }

    }
}
