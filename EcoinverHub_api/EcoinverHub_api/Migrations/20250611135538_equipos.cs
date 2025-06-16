using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoinverHub_api.Migrations
{
    /// <inheritdoc />
    public partial class equipos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EquipoId",
                table: "users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Equipos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JefeEquipoId = table.Column<int>(type: "int", nullable: false),
                    Empresa = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipos_users_JefeEquipoId",
                        column: x => x.JefeEquipoId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_users_EquipoId",
                table: "users",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipos_JefeEquipoId",
                table: "Equipos",
                column: "JefeEquipoId");

            migrationBuilder.AddForeignKey(
                name: "FK_users_Equipos_EquipoId",
                table: "users",
                column: "EquipoId",
                principalTable: "Equipos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_Equipos_EquipoId",
                table: "users");

            migrationBuilder.DropTable(
                name: "Equipos");

            migrationBuilder.DropIndex(
                name: "IX_users_EquipoId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "EquipoId",
                table: "users");
        }
    }
}
