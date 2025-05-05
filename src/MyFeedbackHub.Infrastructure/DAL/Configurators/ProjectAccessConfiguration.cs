using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFeedbackHub.Domain.Organization;

namespace MyFeedbackHub.Infrastructure.DAL.Configurators;

public sealed class ProjectAccessConfiguration : IEntityTypeConfiguration<ProjectAccess>, IEntityConfiguration
{
    public void Configure(EntityTypeBuilder<ProjectAccess> builder)
    {
        builder.ToTable(ConfiguratorConstants.Tables.ProjectAccess, ConfiguratorConstants.Schemas.Public);

        builder.HasKey(up => new { up.UserId, up.ProjectId });

        builder
            .HasOne(up => up.User)
            .WithMany(u => u.ProjectAccess)
            .HasForeignKey(up => up.UserId);

        builder
            .HasOne(up => up.Project)
            .WithMany(p => p.ProjectAccess)
            .HasForeignKey(up => up.ProjectId);
    }
}
