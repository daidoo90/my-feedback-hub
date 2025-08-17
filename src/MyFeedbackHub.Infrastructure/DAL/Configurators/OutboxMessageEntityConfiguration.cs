using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Domain.Feedback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFeedbackHub.Infrastructure.DAL.Entities;

namespace MyFeedbackHub.Infrastructure.DAL.Configurators;

internal class OutboxMessageEntityConfiguration : IEntityTypeConfiguration<OutboxMessage>, IEntityConfiguration
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable(ConfiguratorConstants.Tables.OutboxMessage, ConfiguratorConstants.Schemas.Public);
        builder
            .HasKey(b => b.MessageId);

        builder
            .Property(b => b.Payload)
            .IsRequired();
    }
}