using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using MyFeedbackHub.Domain;

namespace MyFeedbackHub.Infrastructure.DAL.Configurators;

public class OrganizationEntityConfiguration : IEntityTypeConfiguration<OrganizationDomain>, IEntityConfiguration
{
    public void Configure(EntityTypeBuilder<OrganizationDomain> builder)
    {
        builder.ToTable(ConfiguratorConstants.Schemas.Public.Organization, ConfiguratorConstants.Schemas.Public.Name);
        builder
            .HasKey(b => b.OrganizationId);

        builder
            .Property(b => b.OrganizationId)
            .HasValueGenerator<SequentialGuidValueGenerator>()
            .ValueGeneratedOnAdd();

        builder
            .Property(b => b.Name)
            .HasMaxLength(50);

        builder.OwnsOne(o => o.Address, addressBuilder =>
        {
            addressBuilder
                .Property(b => b.Country)
                .HasColumnName("Country")
                .HasMaxLength(50)
                .HasDefaultValue(null);

            addressBuilder
                .Property(b => b.City)
                .HasColumnName("City")
                .HasMaxLength(50)
                .HasDefaultValue(null);

            addressBuilder
                .Property(b => b.ZipCode)
                .HasColumnName("ZipCode")
                .HasMaxLength(10)
                .HasDefaultValue(null);

            addressBuilder
                .Property(b => b.State)
                .HasColumnName("State")
                .HasMaxLength(100)
                .HasDefaultValue(null);

            addressBuilder
                .Property(b => b.StreetLine1)
                .HasColumnName("StreetLine1")
                .HasMaxLength(100)
                .HasDefaultValue(null);

            addressBuilder
                .Property(b => b.StreetLine2)
                .HasColumnName("StreetLine2")
                .HasMaxLength(100)
                .HasDefaultValue(null);
        });

        //builder
        //    .Property(o => o.CreatedByUserId)
        //    .IsRequired();

        builder
            .HasMany(b => b.Users)
            .WithOne(u => u.Organization)
            .HasForeignKey(u => u.OrganizationId)
            .HasPrincipalKey(b => b.OrganizationId);

        builder
            .HasMany(b => b.Projects)
            .WithOne(p => p.Organization)
            .HasForeignKey(p => p.OrganizationId)
            .HasPrincipalKey(b => b.OrganizationId);
    }
}
