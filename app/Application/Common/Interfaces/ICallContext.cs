using System;
using System.Collections.Generic;

namespace Application.Common.Interfaces
{
    public interface ICallContext
    {
        Guid CorrelationId { get; set; }

        string FunctionName { get; set; }

        string UserName { get; set; }

        string AuthenticationType { get; set; }

        IDictionary<string, string> AdditionalProperties { get; }

        bool UserRequiresAuthorization { get; set; }
        string UserBearerAuthorizationToken { get; set; }

        string WalletName { get; set; }
        string WalletPassword { get; set; }
        string WalletAccountName { get; set; }
        string PartitionKey { get; set; }
    }
}