using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using MyFeedbackHub.Domain;

namespace MyFeedbackHub.Infrastructure.DAL.Configurators;

public class ProjectEntityConfigurator : IEntityTypeConfiguration<ProjectDomain>, IEntityConfiguration
{
    public void Configure(EntityTypeBuilder<ProjectDomain> builder)
    {
        builder.ToTable(ConfiguratorConstants.Schemas.Public.Project, ConfiguratorConstants.Schemas.Public.Name);
        builder
            .HasKey(b => b.ProjectId);

        builder
            .Property(b => b.ProjectId)
            .HasValueGenerator<SequentialGuidValueGenerator>()
            .ValueGeneratedOnAdd();

        builder
            .Property(b => b.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .HasOne(u => u.Organization)
            .WithMany(b => b.Projects)
            .HasForeignKey(u => u.OrganizationId);
    }
}
