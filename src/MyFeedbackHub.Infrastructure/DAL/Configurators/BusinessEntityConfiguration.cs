using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using MyFeedbackHub.Domain;

namespace MyFeedbackHub.Infrastructure.DAL.Configurators;

public class BusinessEntityConfiguration : IEntityTypeConfiguration<BusinessDomain>, IEntityConfiguration
{
    public void Configure(EntityTypeBuilder<BusinessDomain> builder)
    {
        builder.ToTable(ConfiguratorConstants.Schemas.Public.Business, ConfiguratorConstants.Schemas.Public.Name);
        builder
            .HasKey(b => b.BusinessId);

        builder
            .Property(b => b.BusinessId)
            .HasValueGenerator<SequentialGuidValueGenerator>()
            .ValueGeneratedOnAdd();
        builder
            .Property(b => b.Name)
            .HasMaxLength(50);

        builder
            .Property(b => b.Website)
            .HasMaxLength(50);

        builder
            .Property(b => b.Source)
            .HasMaxLength(50);

        builder
            .Property(b => b.Address)
            .HasMaxLength(300);

        builder
            .Property(b => b.VATNumber)
            .HasMaxLength(50);

        builder
            .HasMany(b => b.Users)
            .WithOne(u => u.Business)
            .HasForeignKey(u => u.BusinessId)
            .HasPrincipalKey(b => b.BusinessId);

        builder
            .HasMany(b => b.Boards)
            .WithOne(b => b.Business)
            .HasForeignKey(b => b.BusinessId)
            .HasPrincipalKey(b => b.BusinessId);
    }
}
