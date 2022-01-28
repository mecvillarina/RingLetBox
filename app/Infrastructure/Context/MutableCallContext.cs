using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Context
{
    [ExcludeFromCodeCoverage]
    public class MutableCallContext : ICallContext
    {
        public Guid CorrelationId { get; set; }
        public string UserName { get; set; }
        public string AuthenticationType { get; set; }
        public string FunctionName { get; set; }
        public IDictionary<string, string> AdditionalProperties { get; } = new Dictionary<string, string>();

        public bool UserRequiresAuthorization { get; set; }
        public string UserBearerAuthorizationToken { get; set; }

        public string WalletName { get; set; }
        public string WalletPassword { get; set; }
        public string WalletAccountName { get; set; }
        public string PartitionKey { get; set; }
    }
}