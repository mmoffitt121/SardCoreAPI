using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SardCoreAPI.Migrations.SardLibraryDB
{
    /// <inheritdoc />
    public partial class MapTiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tile",
                table: "MapTile");

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "MapTile",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "MapTile");

            migrationBuilder.AddColumn<byte[]>(
                name: "Tile",
                table: "MapTile",
                type: "longblob",
                nullable: false);
        }
    }
}
