﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SardCoreAPI.Migrations.SardLibraryDB
{
    /// <inheritdoc />
    public partial class PageHeaderSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HeaderSettings",
                table: "Page",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeaderSettings",
                table: "Page");
        }
    }
}
