using MediatR;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Enums;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using UnauthorizedAccessException = Application.Common.Exceptions.UnauthorizedAccessException;

namespace Application.Common.Behaviours
{
    public class RequestAuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
    {
        private readonly IAuthTokenService _authTokenService;
        private readonly ICallContext _context;

        public RequestAuthorizationBehavior(ICallContext context,
            IAuthTokenService authTokenService)
        {
            _context = context;
            _authTokenService = authTokenService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_context.UserRequiresAuthorization)
            {
                if (string.IsNullOrEmpty(_context.UserBearerAuthorizationToken)) throw new UnauthorizedAccessException();

                var authTokenResult = _authTokenService.ValidateToken(_context.UserBearerAuthorizationToken);
                if (authTokenResult.Status != AuthTokenStatus.Valid) throw new UnauthorizedAccessException();

                var walletNameClaim = authTokenResult.Principal.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.WalletName);
                var walletPasswordClaim = authTokenResult.Principal.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.WalletPassword);

                if (walletNameClaim != null && walletPasswordClaim != null)
                {
                    _context.WalletName = walletNameClaim.Value;
                    _context.WalletPassword = walletPasswordClaim.Value;
                    _context.WalletAccountName = "account 0";
                }
            }

            return await next();
        }
    }
}