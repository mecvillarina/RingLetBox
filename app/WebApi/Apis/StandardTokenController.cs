using Application.Common.Dtos;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Token.Commands.Add;
using Application.Features.Token.Commands.Create;
using Application.Features.Token.Queries.GetAllHolderTokensQuery;
using Application.Features.Token.Queries.GetHolderTokenQuery;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Base;

namespace WebApi.Apis
{
    public class StandardTokenController : HttpFunctionBase
    {
        public StandardTokenController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("StandardTokenGetAllHolderTokens")]
        public async Task<IActionResult> GetAllHolderTokens([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "standardToken/holder/getAll/{sender}")] GetAllHolderTokensQuery queryArg, HttpRequest req, ExecutionContext context)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<GetAllHolderTokensQuery, Result<List<HolderStandardTokenDto>>>(context, req, queryArg);
        }

        [FunctionName("StandardTokenGetHolderToken")]
        public async Task<IActionResult> GetHolderToken([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "standardToken/holder/get/{contractAddress}/{sender}")] GetHolderTokenQuery queryArg, HttpRequest req, ExecutionContext context)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<GetHolderTokenQuery, Result<HolderStandardTokenDto>>(context, req, queryArg);
        }

        [FunctionName("StandardTokenCreate")]
        public async Task<IActionResult> CreateStandardToken([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "standardToken/create")] CreateStandardTokenCommand commandArg, HttpRequest req, ExecutionContext context)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<CreateStandardTokenCommand, IResult>(context, req, commandArg);
        }

        [FunctionName("StandardTokenAdd")]
        public async Task<IActionResult> AddStandardToken([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "standardToken/add")] AddStandardTokenCommand commandArg, HttpRequest req, ExecutionContext context)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<AddStandardTokenCommand, IResult>(context, req, commandArg);
        }
    }
}
