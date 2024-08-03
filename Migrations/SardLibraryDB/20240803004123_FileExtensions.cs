using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SardCoreAPI.Migrations.SardLibraryDB
{
    /// <inheritdoc />
    public partial class FileExtensions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "Image",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extension",
                table: "Image");
        }
    }
}
