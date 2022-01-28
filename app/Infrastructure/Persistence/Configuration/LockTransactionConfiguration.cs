using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class LockTransactionConfiguration : IEntityTypeConfiguration<LockTransaction>
    {
        public void Configure(EntityTypeBuilder<LockTransaction> builder)
        {
            builder.Property(t => t.Partition).HasMaxLength(20).IsRequired();
            builder.Property(t => t.LockContractAddress).HasMaxLength(100).IsRequired();
            builder.Property(t => t.TokenAddress).HasMaxLength(100).IsRequired();
            builder.Property(t => t.InitiatorAddress).HasMaxLength(100).IsRequired();
            builder.Property(t => t.RecipientAddress).HasMaxLength(100).IsRequired();
            builder.Property(t => t.Amount).HasMaxLength(100).IsRequired();

            builder.HasIndex(t => t.Partition);
            builder.HasIndex(t => t.LockContractAddress);
            builder.HasIndex(t => t.TokenAddress);
            builder.HasIndex(t => t.InitiatorAddress);
            builder.HasIndex(t => t.RecipientAddress);
        }
    }
}
