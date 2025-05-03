using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFeedbackHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAuditColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedOnByUserId",
                schema: "public",
                table: "User",
                newName: "UpdatedByUserId");

            migrationBuilder.RenameColumn(
                name: "UpdatedOnByUserId",
                schema: "public",
                table: "Project",
                newName: "UpdatedByUserId");

            migrationBuilder.RenameColumn(
                name: "UpdatedOnByUserId",
                schema: "public",
                table: "Organization",
                newName: "UpdatedByUserId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "public",
                table: "Organization",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedByUserId",
                schema: "public",
                table: "User",
                newName: "UpdatedOnByUserId");

            migrationBuilder.RenameColumn(
                name: "UpdatedByUserId",
                schema: "public",
                table: "Project",
                newName: "UpdatedOnByUserId");

            migrationBuilder.RenameColumn(
                name: "UpdatedByUserId",
                schema: "public",
                table: "Organization",
                newName: "UpdatedOnByUserId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "public",
                table: "Organization",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);
        }
    }
}
