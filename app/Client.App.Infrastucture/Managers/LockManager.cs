using Application.Common.Dtos;
using Application.Common.Models;
using Application.Features.Lock.Commands.Add;
using Application.Features.Lock.Commands.Claim;
using Application.Features.Lock.Commands.RefundRevoked;
using Application.Features.Lock.Commands.Revoke;
using Client.App.Infrastructure.WebServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class LockManager : ManagerBase, ILockManager
    {
        private readonly ILockWebService _lockWebService;

        public LockManager(IManagerToolkit managerToolkit, ILockWebService lockWebService) : base(managerToolkit)
        {
            _lockWebService = lockWebService;
        }

        public async Task<IResult<List<LockTransactionDto>>> GetAllAsync()
        {
            await PrepareForWebserviceCall();
            return await _lockWebService.GetAllAsync(AccessToken);
        }

        public async Task<IResult<List<LockTransactionWithTypeDto>>> GetAllBySenderAsync(string sender)
        {
            await PrepareForWebserviceCall();
            return await _lockWebService.GetAllBySenderAsync(sender, AccessToken);
        }

        public async Task<IResult> AddNewLockAsync(AddNewLockCommand request)
        {
            await PrepareForWebserviceCall();
            return await _lockWebService.AddNewLockAsync(request, AccessToken);
        }

        public async Task<IResult> ClaimLockAsync(ClaimLockCommand request)
        {
            await PrepareForWebserviceCall();
            return await _lockWebService.ClaimLockAsync(request, AccessToken);
        }

        public async Task<IResult> RevokeLockAsync(RevokeLockCommand request)
        {
            await PrepareForWebserviceCall();
            return await _lockWebService.RevokeLockAsync(request, AccessToken);
        }

        public async Task<IResult> RefundRevokedLockAsync(RefundRevokedLockCommand request)
        {
            await PrepareForWebserviceCall();
            return await _lockWebService.RefundRevokedLockAsync(request, AccessToken);
        }
    }
}
