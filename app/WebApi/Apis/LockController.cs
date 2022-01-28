using Application.Common.Dtos;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Lock.Commands.Add;
using Application.Features.Lock.Commands.Claim;
using Application.Features.Lock.Commands.RefundRevoked;
using Application.Features.Lock.Commands.Revoke;
using Application.Features.Lock.Queries.GetAll;
using Application.Features.Lock.Queries.GetSenderAll;
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
    public class LockController : HttpFunctionBase
    {
        public LockController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("LockGetAll")]
        public async Task<IActionResult> GetAll([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "lock/getAll")] GetAllLocksQuery commandArg, HttpRequest req, ExecutionContext context)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<GetAllLocksQuery, Result<List<LockTransactionDto>>>(context, req, commandArg);
        }

        [FunctionName("LockGetAllBySender")]
        public async Task<IActionResult> GetAllBySender([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "lock/getAllBySender/{sender}")] GetSenderAllLocksQuery commandArg, HttpRequest req, ExecutionContext context)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<GetSenderAllLocksQuery, Result<List<LockTransactionWithTypeDto>>>(context, req, commandArg);
        }

        [FunctionName("LockAddNew")]
        public async Task<IActionResult> AddNewLock([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "lock/addNew")] AddNewLockCommand commandArg, HttpRequest req, ExecutionContext context)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<AddNewLockCommand, IResult>(context, req, commandArg);
        }

        [FunctionName("LockClaim")]
        public async Task<IActionResult> ClaimLock([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "lock/claim")] ClaimLockCommand commandArg, HttpRequest req, ExecutionContext context)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<ClaimLockCommand, IResult>(context, req, commandArg);
        }

        [FunctionName("LockRevoke")]
        public async Task<IActionResult> RevokeLock([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "lock/revoke")] RevokeLockCommand commandArg, HttpRequest req, ExecutionContext context)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<RevokeLockCommand, IResult>(context, req, commandArg);
        }

        [FunctionName("LockRefundRevoked")]
        public async Task<IActionResult> RefundRevokedLock([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "lock/refundRevoked")] RefundRevokedLockCommand commandArg, HttpRequest req, ExecutionContext context)
        {
            EnsureAuthorization(req);
            return await ExecuteAsync<RefundRevokedLockCommand, IResult>(context, req, commandArg);
        }
    }
}
