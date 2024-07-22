using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SardCoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class fixDataPointType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "46da3efc-0139-4ac9-a0fb-e0813042e454");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8e59b17b-2204-4548-9d43-c9070616cdfd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db1e9f53-4726-4476-bbf0-277cd46e7407");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "46da3efc-0139-4ac9-a0fb-e0813042e454", null, "Viewer", "VIEWER" },
                    { "8e59b17b-2204-4548-9d43-c9070616cdfd", null, "Editor", "EDITOR" },
                    { "db1e9f53-4726-4476-bbf0-277cd46e7407", null, "Administrator", "ADMINISTRATOR" }
                });
        }
    }
}
