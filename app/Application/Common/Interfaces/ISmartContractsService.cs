using Application.Common.Dtos.ApiRequest;
using Application.Common.Dtos.ApiResponses;
using System.Collections.Generic;

namespace Application.Common.Interfaces
{
    public interface ISmartContractsService
    {
        SmartContractWalletCallDto Call(SmartContractWalletCallRequestDto args);
        string Create(SmartContractWalletCreateRequestDto args);
        SmartContractsLocalCallDto<T> LocalCall<T>(SmartContractsLocalCallRequestDto args);
        SmartContractsReceiptDto Receipt(string txHash);
        SmartContractsPropertyLocalCallDto<T> GetPropertyValue<T>(SmartContractsPropertyCallRequestDto args);
        SmartContractsReceiptDto GetMethodValue(string walletName, string walletPassword, string sender, string contractAddress, string methodName, List<string> parameters);
    }
}
