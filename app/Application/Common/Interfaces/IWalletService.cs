using Application.Common.Dtos.ApiResponses;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Recover;
using System.Collections.Generic;

namespace Application.Common.Interfaces
{
    public interface IWalletService
    {
        WalletGeneralInfoDto GeneralInfo(string walletName);
        List<WalletAddressDto> GetAddresses(string walletName);

        List<string> ListWallets();
        void Load(LoginCommand args);
        void Recover(RecoverCommand command);
    }
}
