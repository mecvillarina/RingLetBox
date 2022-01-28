﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class StandardTokenConfiguration : IEntityTypeConfiguration<StandardToken>
    {
        public void Configure(EntityTypeBuilder<StandardToken> builder)
        {
            builder.Property(t => t.Partition).HasMaxLength(20).IsRequired();
            builder.Property(t => t.ContractAddress).HasMaxLength(100).IsRequired();
            builder.Property(t => t.Symbol).HasMaxLength(100).IsRequired();
            builder.Property(t => t.Name).HasMaxLength(100).IsRequired();
            builder.Property(t => t.Decimal).IsRequired();

            builder.HasIndex(t => t.Partition);
            builder.HasIndex(t => t.Symbol);
            builder.HasIndex(t => t.ContractAddress).IsUnique();
        }
    }
}
