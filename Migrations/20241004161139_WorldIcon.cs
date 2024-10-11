using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SardCoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class WorldIcon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1c8401f8-6c49-43fa-9db2-d92f3cf36195");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "51beff88-9f00-40f9-b86f-266273c1cecb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c9c03812-4b12-4806-9621-8a81b6a139bd");

            migrationBuilder.AddColumn<string>(
                name: "IconId",
                table: "World",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9aa37772-8964-45ff-a589-d8fba62873d2", null, "Editor", "EDITOR" },
                    { "d86c7ced-946a-4aff-98f9-3954d02bc297", null, "Viewer", "VIEWER" },
                    { "dae88353-892b-46b6-bf02-fffa706a49fd", null, "Administrator", "ADMINISTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9aa37772-8964-45ff-a589-d8fba62873d2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d86c7ced-946a-4aff-98f9-3954d02bc297");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dae88353-892b-46b6-bf02-fffa706a49fd");

            migrationBuilder.DropColumn(
                name: "IconId",
                table: "World");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1c8401f8-6c49-43fa-9db2-d92f3cf36195", null, "Editor", "EDITOR" },
                    { "51beff88-9f00-40f9-b86f-266273c1cecb", null, "Administrator", "ADMINISTRATOR" },
                    { "c9c03812-4b12-4806-9621-8a81b6a139bd", null, "Viewer", "VIEWER" }
                });
        }
    }
}
