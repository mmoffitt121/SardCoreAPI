using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SardCoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class initwithroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "122a3dce-a046-4f13-b6a0-4d1e7780504f", "a97ebb79-da4c-4560-b7f6-d7adad8fa4e8", "Administrator", "ADMINISTRATOR" },
                    { "1b32404e-30cc-436b-be04-77950a7a6455", "6a3349dd-aaa0-4540-ba1f-1671c955c2d9", "Editor", "EDITOR" },
                    { "61017388-d76d-40b4-8f4f-22f5875258ea", "8d687ae6-cb17-454c-885e-1420c2fa980c", "Viewer", "VIEWER" },
                    { "61017388-d76d-40b4-8f4f-22f587525812", "8d687ae6-cb17-454c-885e-1420c2fa9841", "Test", "TEST" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "122a3dce-a046-4f13-b6a0-4d1e7780504f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b32404e-30cc-436b-be04-77950a7a6455");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "61017388-d76d-40b4-8f4f-22f5875258ea");
        }
    }
}
