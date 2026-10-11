using Microsoft.EntityFrameworkCore;
using RegistroLibros.Context;
using RegistroLibros.Models;
using System.Linq.Expressions;

namespace RegistroLibros.Services;

public class DevolucionesService(
    IDbContextFactory<Contexto> contextFactory
    ) : Aplicada1.Core.IService<Devoluciones, int>
{

    public async Task<bool> Guardar(Devoluciones devolucion)
    {
        if (!await Existe(devolucion.DevolucionId))
        {
            return await Insertar(devolucion);
        }
        else
        {
            return await Modificar(devolucion);
        }
    }

    private async Task<bool> Existe(int devolucionId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Devoluciones
            .AnyAsync(p => p.DevolucionId == devolucionId);
    }

    private async Task<bool> Insertar(Devoluciones devolucion)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Devoluciones.Add(devolucion);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Devoluciones devolucion)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(devolucion);
        return await contexto
            .SaveChangesAsync() > 0;
    }

    public async Task<Devoluciones?> Buscar(int devolucionId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Devoluciones
            .Include(d => d.Prestamo)
                .ThenInclude(p => p!.Estudiante)
            .Include(d => d.Prestamo)
                .ThenInclude(p => p!.Libro)
            .FirstOrDefaultAsync(d => d.DevolucionId == devolucionId);
    }

    public async Task<List<Devoluciones>> GetList(Expression<Func<Devoluciones, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Devoluciones
            .Include(d => d.Prestamo)
                .ThenInclude(p => p!.Estudiante)
            .Include(d => d.Prestamo)
                .ThenInclude(p => p!.Libro)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Eliminar(int devolucionId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Devoluciones
            .Where(p => p.DevolucionId == devolucionId)
            .ExecuteDeleteAsync() > 0;
    }
}
