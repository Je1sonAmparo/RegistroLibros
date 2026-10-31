using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistroLibros.Data.Migrations
{
    /// <inheritdoc />
    public partial class añadirpropiedadlibros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CantidadDisponible",
                table: "Libros",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CantidadDisponible",
                table: "Libros");
        }
    }
}

