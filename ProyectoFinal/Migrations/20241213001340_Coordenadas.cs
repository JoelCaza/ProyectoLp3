using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinal.Migrations
{
    /// <inheritdoc />
    public partial class Coordenadas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitud",
                table: "Lugares",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitud",
                table: "Lugares",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitud",
                table: "Lugares");

            migrationBuilder.DropColumn(
                name: "Longitud",
                table: "Lugares");
        }
    }
}
