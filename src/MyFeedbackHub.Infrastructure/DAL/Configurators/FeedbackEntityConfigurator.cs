﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using MyFeedbackHub.Domain.Feedback;

namespace MyFeedbackHub.Infrastructure.DAL.Configurators;

public sealed class FeedbackEntityConfigurator : IEntityTypeConfiguration<FeedbackDomain>, IEntityConfiguration
{
    public void Configure(EntityTypeBuilder<FeedbackDomain> builder)
    {
        builder.ToTable(ConfiguratorConstants.Tables.Feedback, ConfiguratorConstants.Schemas.Feedback);
        builder
            .HasKey(b => b.FeedbackId);

        builder
            .Property(b => b.FeedbackId)
            .HasValueGenerator<SequentialGuidValueGenerator>()
            .ValueGeneratedOnAdd();

        builder
            .Property(b => b.Title)
            .HasMaxLength(400);
    }
}