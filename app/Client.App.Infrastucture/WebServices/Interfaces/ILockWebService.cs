using Application.Common.Dtos;
using Application.Common.Models;
using Application.Features.Lock.Commands.Add;
using Application.Features.Lock.Commands.Claim;
using Application.Features.Lock.Commands.RefundRevoked;
using Application.Features.Lock.Commands.Revoke;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface ILockWebService : IWebService
    {
        Task<IResult<List<LockTransactionDto>>> GetAllAsync(string accessToken);
        Task<IResult<List<LockTransactionWithTypeDto>>> GetAllBySenderAsync(string sender, string accessToken);
        Task<IResult> AddNewLockAsync(AddNewLockCommand request, string accessToken);
        Task<IResult> ClaimLockAsync(ClaimLockCommand request, string accessToken);
        Task<IResult> RevokeLockAsync(RevokeLockCommand request, string accessToken);
        Task<IResult> RefundRevokedLockAsync(RefundRevokedLockCommand request, string accessToken);
    }
}