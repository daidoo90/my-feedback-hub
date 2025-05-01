using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using MyFeedbackHub.Domain;

namespace MyFeedbackHub.Infrastructure.DAL.Configurators;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserDomain>, IEntityConfiguration
{
    public void Configure(EntityTypeBuilder<UserDomain> builder)
    {
        builder.ToTable(ConfiguratorConstants.Schemas.Public.User, ConfiguratorConstants.Schemas.Public.Name);
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
            .Property(u => u.Password)
            .IsRequired();

        builder
            .Property(u => u.Salt)
            .IsRequired();

        builder
            .HasOne(u => u.Organization)
            .WithMany(b => b.Users)
            .HasForeignKey(u => u.OrganizationId);
    }
}
