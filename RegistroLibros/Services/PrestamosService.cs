using Microsoft.EntityFrameworkCore;
using RegistroLibros.Context;
using RegistroLibros.Models;
using System.Linq.Expressions;

namespace RegistroLibros.Services
{
    public class PrestamosService(IDbContextFactory<Contexto> DbFactory)
        : Aplicada1.Core.IService<Prestamos, int>
    {
        private async Task<bool> Existe(int prestamoId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Prestamos
                .AnyAsync(p => p.PrestamoId == prestamoId);
        }

        private async Task<bool> Insertar(Prestamos prestamo)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Prestamos.Add(prestamo);
            return await contexto.SaveChangesAsync() > 0;
        }

        private async Task<bool> Modificar(Prestamos prestamo)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Update(prestamo);
            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task<bool> Guardar(Prestamos prestamo)
        {
            if (!await Existe(prestamo.PrestamoId))
            {
                return await Insertar(prestamo);
            }
            else
            {
                return await Modificar(prestamo);
            }
        }

        public async Task<Prestamos?> Buscar(int prestamoId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Prestamos
                .Include(p => p.Estudiante)
                .Include(p => p.Libro)
                .FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);
        }

        public async Task<bool> Eliminar(int prestamoId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Prestamos
                .Where(p => p.PrestamoId == prestamoId)
                .ExecuteDeleteAsync() > 0;
        }

        public async Task<List<Prestamos>> GetList(Expression<Func<Prestamos, bool>> criterio)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Prestamos
                .Include(p => p.Estudiante)
                .Include(p => p.Libro)
                .Where(criterio)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
