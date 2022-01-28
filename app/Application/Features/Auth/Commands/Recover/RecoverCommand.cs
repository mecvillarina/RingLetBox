using MediatR;
using Application.Common.Interfaces;
using Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands.Recover
{
    public class RecoverCommand : IRequest<IResult>
    {
        public string Mnemonic { get; set; }
        public string Password { get; set; }
        public string Passphrase { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }

        public class WalletRecoverCommandHandler : IRequestHandler<RecoverCommand, IResult>
        {
            private readonly IWalletService _walletService;

            public WalletRecoverCommandHandler(IWalletService walletService)
            {
                _walletService = walletService;
            }

            public Task<IResult> Handle(RecoverCommand request, CancellationToken cancellationToken)
            {
                _walletService.Recover(request);
                return Result.SuccessAsync();
            }
        }
    }
}
