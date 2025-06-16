using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoinverHub_api.Migrations
{
    /// <inheritdoc />
    public partial class CreadorAddAnuncio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "nombre",
                table: "anuncios",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "estado",
                table: "anuncios",
                newName: "Estado");

            migrationBuilder.RenameColumn(
                name: "contenido",
                table: "anuncios",
                newName: "Contenido");

            migrationBuilder.AddColumn<string>(
                name: "Creador",
                table: "anuncios",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creador",
                table: "anuncios");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "anuncios",
                newName: "nombre");

            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "anuncios",
                newName: "estado");

            migrationBuilder.RenameColumn(
                name: "Contenido",
                table: "anuncios",
                newName: "contenido");
        }
    }
}
