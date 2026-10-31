using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using RegistroLibros.Data.Context;

#nullable disable

namespace RegistroLibros.Data.Migrations;

[DbContext(typeof(Contexto))]
[Migration("20261011204200_QuitarCantidadPrestamos")]
public partial class QuitarCantidadPrestamos : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CantidadPrestamos",
            table: "Libros");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "CantidadPrestamos",
            table: "Libros",
            type: "int",
            nullable: false,
            defaultValue: 0);
    }
}

