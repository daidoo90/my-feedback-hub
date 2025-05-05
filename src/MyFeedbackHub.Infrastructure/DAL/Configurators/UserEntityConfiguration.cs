using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using MyFeedbackHub.Domain.Organization;

namespace MyFeedbackHub.Infrastructure.DAL.Configurators;

public sealed class UserEntityConfiguration : IEntityTypeConfiguration<UserDomain>, IEntityConfiguration
{
    public void Configure(EntityTypeBuilder<UserDomain> builder)
    {
        builder.ToTable(ConfiguratorConstants.Tables.User, ConfiguratorConstants.Schemas.Public);
        builder
            .HasKey(u => u.UserId);

        builder
            .Property(u => u.UserId)
            .HasValueGenerator<SequentialGuidValueGenerator>()
            .ValueGeneratedOnAdd();

        builder
            .Property(u => u.FirstName)
            .HasMaxLength(50)
            .HasDefaultValue(null);

        builder
            .Property(u => u.LastName)
            .HasMaxLength(50)
            .HasDefaultValue(null);

        builder
            .Property(u => u.Username)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(u => u.PhoneNumber)
            .HasMaxLength(50)
            .HasDefaultValue(null);

        builder
            .HasOne(u => u.Organization)
            .WithMany(b => b.Users)
            .HasForeignKey(u => u.OrganizationId);
    }
}
