using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTLabs.Identity.Authorization.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicaoCascadeBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_app_permission_app_permitted_app_id",
                table: "app_permission");

            migrationBuilder.AddForeignKey(
                name: "f_k_app_permission_app_permitted_app_id",
                table: "app_permission",
                column: "permitted_app_id",
                principalTable: "app",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_app_permission_app_permitted_app_id",
                table: "app_permission");

            migrationBuilder.AddForeignKey(
                name: "f_k_app_permission_app_permitted_app_id",
                table: "app_permission",
                column: "permitted_app_id",
                principalTable: "app",
                principalColumn: "id");
        }
    }
}
