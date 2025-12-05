using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTLabs.Identity.Authorization.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicaoReverseNavigationApps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_app_permission_permitted_app_id",
                table: "app_permission",
                column: "permitted_app_id");

            migrationBuilder.AddForeignKey(
                name: "f_k_app_permission_app_permitted_app_id",
                table: "app_permission",
                column: "permitted_app_id",
                principalTable: "app",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_app_permission_app_permitted_app_id",
                table: "app_permission");

            migrationBuilder.DropIndex(
                name: "IX_app_permission_permitted_app_id",
                table: "app_permission");
        }
    }
}
