using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Token.EventHandlers
{
    public class OnTokenCreateEventHandler : INotificationHandler<DomainEventNotification<OnTokenCreateEvent>>
    {
        private readonly IApplicationDbContext _dbContext;

        public OnTokenCreateEventHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(DomainEventNotification<OnTokenCreateEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _dbContext.AuditTokenCreations.Add(new AuditTokenCreation()
            {
                TransactionHash = domainEvent.TransactionHash,
                Decimal = domainEvent.Decimal,
                Name = domainEvent.Name,
                Sender = domainEvent.Sender,
                Symbol = domainEvent.Symbol,
                TotalSupply = domainEvent.TotalSupply,
                ContractAddress = domainEvent.ContractAddress,
            });

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
