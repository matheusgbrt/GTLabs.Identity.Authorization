using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTLabs.Identity.Authorization.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicaoPermissoesServicos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    identifier = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    route = table.Column<string>(type: "text", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleter_id = table.Column<Guid>(type: "uuid", nullable: true),
                    deletion_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modifier_id = table.Column<Guid>(type: "uuid", nullable: true),
                    creation_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_modification_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_app", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "app_permission",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    app_id = table.Column<Guid>(type: "uuid", nullable: false),
                    permitted_app_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleter_id = table.Column<Guid>(type: "uuid", nullable: true),
                    deletion_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modifier_id = table.Column<Guid>(type: "uuid", nullable: true),
                    creation_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_modification_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_app_permission", x => x.id);
                    table.ForeignKey(
                        name: "f_k_app_permission_app_app_id",
                        column: x => x.app_id,
                        principalTable: "app",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_app_permission_app_id",
                table: "app_permission",
                column: "app_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_permission");

            migrationBuilder.DropTable(
                name: "app");
        }
    }
}
