using Application.Common.Dtos;
using Application.Common.Models;
using Application.Features.Lock.Commands.Add;
using Application.Features.Lock.Commands.Claim;
using Application.Features.Lock.Commands.RefundRevoked;
using Application.Features.Lock.Commands.Revoke;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface ILockManager : IManager
    {
        Task<IResult<List<LockTransactionDto>>> GetAllAsync();
        Task<IResult<List<LockTransactionWithTypeDto>>> GetAllBySenderAsync(string sender);
        Task<IResult> AddNewLockAsync(AddNewLockCommand request);
        Task<IResult> ClaimLockAsync(ClaimLockCommand request);
        Task<IResult> RevokeLockAsync(RevokeLockCommand request);
        Task<IResult> RefundRevokedLockAsync(RefundRevokedLockCommand request);
    }
}