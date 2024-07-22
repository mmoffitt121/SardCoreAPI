using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SardCoreAPI.Migrations.SardLibraryDB
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DataPointParameter_DataPointValueId",
                table: "DataPointParameter",
                column: "DataPointValueId");

            migrationBuilder.AddForeignKey(
                name: "FK_DataPointParameter_DataPoint_DataPointValueId",
                table: "DataPointParameter",
                column: "DataPointValueId",
                principalTable: "DataPoint",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataPointParameter_DataPoint_DataPointValueId",
                table: "DataPointParameter");

            migrationBuilder.DropIndex(
                name: "IX_DataPointParameter_DataPointValueId",
                table: "DataPointParameter");
        }
    }
}
