using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistroLibros.Migrations
{
    /// <inheritdoc />
    public partial class añadirdevolucion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaDevolucion",
                table: "Prestamos");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Devoluciones");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDevolucion",
                table: "Prestamos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Devoluciones",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
