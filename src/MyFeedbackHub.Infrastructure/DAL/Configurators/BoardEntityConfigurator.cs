//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Microsoft.EntityFrameworkCore.ValueGeneration;
//using MyFeedbackHub.Domain;

//namespace MyFeedbackHub.Infrastructure.DAL.Configurators;

//public class BoardEntityConfigurator : IEntityTypeConfiguration<BoardDomain>, IEntityConfiguration
//{
//    public void Configure(EntityTypeBuilder<BoardDomain> builder)
//    {
//        builder.ToTable(ConfiguratorConstants.Schemas.Public.Board, ConfiguratorConstants.Schemas.Public.Name);
//        builder
//            .HasKey(b => b.BoardId);

//        builder
//            .Property(b => b.BoardId)
//            .HasValueGenerator<SequentialGuidValueGenerator>()
//            .ValueGeneratedOnAdd();

//        builder
//            .Property(b => b.Title)
//            .HasMaxLength(50);

//        builder
//            .Property(b => b.Description)
//            .HasMaxLength(50);

//        builder
//            .HasOne(b => b.Business)
//            .WithMany(b => b.Boards)
//            .HasForeignKey(b => b.BusinessId);
//    }
//}
