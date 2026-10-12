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
            .AnyAsync(d => d.DevolucionId == devolucionId);
    }

    private async Task<bool> Insertar(Devoluciones devolucion)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Devoluciones.Add(devolucion);

        if (!await AfectarPrestamo(contexto, devolucion.PrestamoId, true))
        {
            return false;
        }

        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Devoluciones devolucion)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var existente = await contexto.Devoluciones
            .FirstOrDefaultAsync(d => d.DevolucionId == devolucion.DevolucionId);
        if (existente == null)
        {
            return false;
        }

        if (existente.PrestamoId != devolucion.PrestamoId)
        {
            if (!await AfectarPrestamo(contexto, existente.PrestamoId, false)
                || !await AfectarPrestamo(contexto, devolucion.PrestamoId, true))
            {
                return false;
            }
        }

        existente.PrestamoId = devolucion.PrestamoId;
        existente.FechaDevolucion = devolucion.FechaDevolucion;
        return await contexto.SaveChangesAsync() > 0;
    }

    private static async Task<bool> AfectarPrestamo(Contexto contexto, int prestamoId, bool devuelto)
    {
        var prestamo = await contexto.Prestamos
            .FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);
        if (prestamo == null)
        {
            return false;
        }

        prestamo.Devuelto = devuelto;
        return true;
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
        var devolucion = await contexto.Devoluciones
            .FirstOrDefaultAsync(d => d.DevolucionId == devolucionId);
        if (devolucion == null)
        {
            return false;
        }

        await AfectarPrestamo(contexto, devolucion.PrestamoId, false);
        contexto.Devoluciones.Remove(devolucion);
        return await contexto.SaveChangesAsync() > 0;
    }
}
