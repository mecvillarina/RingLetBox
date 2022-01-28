using Microsoft.Extensions.Configuration;
using RestSharp;
using Application.Common.Dtos.ApiResponses;
using Application.Common.Interfaces;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Recover;
using System.Collections.Generic;

namespace Infrastructure.Services
{
    public class WalletService : CirrusCoreRestSharpService, IWalletService
    {
        public WalletService(IConfiguration configuration) : base(configuration)
        {
        }


        public WalletGeneralInfoDto GeneralInfo(string walletName)
        {
            var request = new RestRequest("/api/Wallet/general-info");
            request.AddParameter("Name", walletName);
            return Execute<WalletGeneralInfoDto>(request);
        }

        public List<WalletAddressDto> GetAddresses(string walletName)
        {
            var request = new RestRequest("/api/Wallet/addresses");
            request.AddParameter("WalletName", walletName);
            request.RootElement = "addresses";
            return Execute<List<WalletAddressDto>>(request);
        }

        public List<string> ListWallets()
        {
            var request = new RestRequest("api/wallet/list-wallets");
            request.RootElement = "walletNames";
            return Execute<List<string>>(request);
        }

        public void Load(LoginCommand args)
        {
            var request = new RestRequest("api/wallet/load", Method.POST);
            request.AddJsonBody(args);
            Execute(request);
        }

        public void Recover(RecoverCommand args)
        {
            var request = new RestRequest("api/wallet/recover", Method.POST);
            request.AddJsonBody(args);
            Execute(request);
        }
    }
}
