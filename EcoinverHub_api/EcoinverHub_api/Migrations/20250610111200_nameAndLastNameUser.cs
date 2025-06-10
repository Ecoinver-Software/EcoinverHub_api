using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoinverHub_api.Migrations
{
    /// <inheritdoc />
    public partial class nameAndLastNameUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "nombre",
                table: "users",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "apellidos",
                table: "users",
                newName: "lastname");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "users",
                newName: "nombre");

            migrationBuilder.RenameColumn(
                name: "lastname",
                table: "users",
                newName: "apellidos");
        }
    }
}
