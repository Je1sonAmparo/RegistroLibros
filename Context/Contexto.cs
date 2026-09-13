using Microsoft.EntityFrameworkCore;
using RegistroLibros.Models;

namespace RegistroLibros.Context;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options)
    {
    }

    public DbSet<Libros> Libros { get; set; }
}

