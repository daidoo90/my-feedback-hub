using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFeedbackHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserProjectAccess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectAccess",
                schema: "public",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectAccess", x => new { x.UserId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_ProjectAccess_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "public",
                        principalTable: "Project",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectAccess_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAccess_ProjectId",
                schema: "public",
                table: "ProjectAccess",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectAccess",
                schema: "public");
        }
    }
}
