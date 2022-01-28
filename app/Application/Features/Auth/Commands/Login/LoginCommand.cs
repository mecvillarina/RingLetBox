using MediatR;
using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<Result<LoginCommandResponse>>
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
        {
            private readonly IAuthTokenService _authTokenService;
            private readonly IWalletService _walletService;

            public LoginCommandHandler(IAuthTokenService authTokenService, IWalletService walletService)
            {
                _authTokenService = authTokenService;
                _walletService = walletService;
            }

            public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                _walletService.Load(request);

                var accountClaims = new Dictionary<string, string>
                {
                    { CustomClaimTypes.WalletName, request.Name },
                    { CustomClaimTypes.WalletPassword, request.Password },
                };

                var authToken = _authTokenService.GenerateToken(accountClaims);
                var response = new LoginCommandResponse()
                {
                    Token = authToken.Token,
                    ExpireAt = authToken.ExpireAt
                };

                return await Result<LoginCommandResponse>.SuccessAsync(response);
            }
        }
    }
}
