using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SardCoreAPI.Migrations.SardLibraryDB
{
    /// <inheritdoc />
    public partial class ZoomLevelPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PersistentZoomLevel",
                table: "PersistentZoomLevel");

            migrationBuilder.AlterColumn<int>(
                name: "Zoom",
                table: "PersistentZoomLevel",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersistentZoomLevel",
                table: "PersistentZoomLevel",
                columns: new[] { "Zoom", "MapLayerId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PersistentZoomLevel",
                table: "PersistentZoomLevel");

            migrationBuilder.AlterColumn<int>(
                name: "Zoom",
                table: "PersistentZoomLevel",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersistentZoomLevel",
                table: "PersistentZoomLevel",
                column: "Zoom");
        }
    }
}
