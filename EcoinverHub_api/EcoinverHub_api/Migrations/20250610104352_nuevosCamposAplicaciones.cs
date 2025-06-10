using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoinverHub_api.Migrations
{
    /// <inheritdoc />
    public partial class nuevosCamposAplicaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Autor",
                table: "applications",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "applications",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaActualizacion",
                table: "applications",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "applications",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Autor",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "FechaActualizacion",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "applications");
        }
    }
}
