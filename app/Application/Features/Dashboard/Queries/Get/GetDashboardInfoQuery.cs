using Application.Common.Dtos;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Dashboard.Queries.Get
{
    public class GetDashboardInfoQuery : IRequest<Result<GetDashboardInfoResponse>>
    {
        public class GetDashboardInfoQueryHandler : IRequestHandler<GetDashboardInfoQuery, Result<GetDashboardInfoResponse>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IMapper _mapper;
            public GetDashboardInfoQueryHandler(ICallContext context, IApplicationDbContext dbContext, IMapper mapper)
            {
                _context = context;
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<Result<GetDashboardInfoResponse>> Handle(GetDashboardInfoQuery request, CancellationToken cancellationToken)
            {
                var response = new GetDashboardInfoResponse();

                response.TotalCreatedTokensCount = await _dbContext.AuditTokenCreations.CountAsync(x => x.Partition == _context.PartitionKey);
                response.TotalTokensCount = await _dbContext.StandardTokens.CountAsync(x => x.Partition == _context.PartitionKey);
                response.TotalTokenLocksCount = await _dbContext.LockTransactions.CountAsync(x => x.Partition == _context.PartitionKey);

                var tokens = await _dbContext.AuditTokenCreations.Where(x => x.Partition == _context.PartitionKey).OrderByDescending(x => x.CreatedDate).Take(10).ToListAsync();
                response.RecentlyCreatedTokens = _mapper.Map<List<AuditTokenCreationDto>>(tokens);

                return await Result<GetDashboardInfoResponse>.SuccessAsync(response);
            }
        }
    }
}
