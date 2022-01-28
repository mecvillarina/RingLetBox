using Application.Common.Dtos;
using Application.Common.Models;
using Application.Features.Lock.Commands.Add;
using Application.Features.Lock.Commands.Claim;
using Application.Features.Lock.Commands.RefundRevoked;
using Application.Features.Lock.Commands.Revoke;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class LockWebService : WebServiceBase, ILockWebService
    {
        public LockWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult<List<LockTransactionDto>>> GetAllAsync(string accessToken) => GetAsync<List<LockTransactionDto>>(LockEndpoints.GetAll, accessToken);
        public Task<IResult<List<LockTransactionWithTypeDto>>> GetAllBySenderAsync(string sender, string accessToken) => GetAsync<List<LockTransactionWithTypeDto>>(string.Format(LockEndpoints.GetAllBySender, sender), accessToken);
        public Task<IResult> AddNewLockAsync(AddNewLockCommand request, string accessToken) => PostAsync(LockEndpoints.AddNewLock, request, accessToken);
        public Task<IResult> ClaimLockAsync(ClaimLockCommand request, string accessToken) => PostAsync(LockEndpoints.ClaimLock, request, accessToken);
        public Task<IResult> RevokeLockAsync(RevokeLockCommand request, string accessToken) => PostAsync(LockEndpoints.RevokeLock, request, accessToken);
        public Task<IResult> RefundRevokedLockAsync(RefundRevokedLockCommand request, string accessToken) => PostAsync(LockEndpoints.RefundRevokedLock, request, accessToken);
    }
}
