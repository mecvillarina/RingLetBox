using Application.Common.Dtos;
using System.Collections.Generic;

namespace Application.Features.Dashboard.Queries.Get
{
    public class GetDashboardInfoResponse
    {
        public int TotalCreatedTokensCount { get; set; } //in this platform
        public int TotalTokensCount { get; set; } // in blockchain
        public int TotalTokenLocksCount { get; set; }

        public List<AuditTokenCreationDto> RecentlyCreatedTokens { get; set; } = new List<AuditTokenCreationDto>();
    }

}
