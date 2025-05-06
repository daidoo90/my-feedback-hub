using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using MyFeedbackHub.Domain.Feedback;

namespace MyFeedbackHub.Infrastructure.DAL.Configurators;

public sealed class CommentEntityConfiguration : IEntityTypeConfiguration<CommentDomain>, IEntityConfiguration
{
    public void Configure(EntityTypeBuilder<CommentDomain> builder)
    {
        builder.ToTable(ConfiguratorConstants.Tables.Comment, ConfiguratorConstants.Schemas.Feedback);
        builder
            .HasKey(b => b.CommentId);

        builder
            .Property(b => b.CommentId)
            .HasValueGenerator<SequentialGuidValueGenerator>()
            .ValueGeneratedOnAdd();

        builder
            .Property(b => b.Text)
            .IsRequired();

        builder
        .HasOne(c => c.Feedback)
        .WithMany(f => f.Comments)
        .HasForeignKey(c => c.FeedbackId);
    }
}