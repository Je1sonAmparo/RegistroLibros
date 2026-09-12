using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RegistroLibros.Context;
using RegistroLibros.Models;

namespace RegistroLibros.Services
{
    public class LibrosServices(IDbContextFactory<Contexto> DbFactory)
    {
        private async Task<bool> Existe(int libroId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Libros
                .AnyAsync(p => p.LibroId == libroId);
        }

        private async Task<bool> Insertar(Libros libro)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Libros.Add(libro);
            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task<bool> Guardar(Libros libro)
        {
            if (!await Existe(libro.LibroId))
            {
                return await Insertar(libro);
            }
            else
            {
                return await Modificar(libro);
            }
        }

        private async Task<bool> Modificar(Libros libro)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Update(libro);
            return await contexto
                .SaveChangesAsync() > 0;
        }
    }
}
