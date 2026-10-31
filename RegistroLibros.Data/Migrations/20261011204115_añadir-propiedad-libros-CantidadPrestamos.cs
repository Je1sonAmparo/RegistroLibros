using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistroLibros.Data.Migrations
{
    /// <inheritdoc />
    public partial class añadirpropiedadlibrosCantidadPrestamos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CantidadPrestamos",
                table: "Libros",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CantidadPrestamos",
                table: "Libros");
        }
    }
}

