using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFeedbackHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedProjectAccessRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                schema: "public",
                table: "ProjectAccess");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role",
                schema: "public",
                table: "ProjectAccess",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
