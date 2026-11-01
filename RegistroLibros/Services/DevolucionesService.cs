using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RegistroLibros.Context;
using RegistroLibros.Models;
using System.Linq.Expressions;

namespace RegistroLibros.Services;

public class DevolucionesService(
    IDbContextFactory<Contexto> contextFactory
    ) : IServiceResult<Devoluciones, int>
{
    public async Task<Result> Guardar(Devoluciones devolucion)
    {
        if (!await Existe(devolucion.DevolucionId))
        {
            var inserto = await Insertar(devolucion);
            return Result.Success(inserto);
        }
        else
        {
            var modifico = await Modificar(devolucion);
            return Result.Success(modifico);
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

        var resultadoPrestamo = await AfectarPrestamo(contexto, devolucion.PrestamoId, true);
        if (!resultadoPrestamo)
        {
            return false;
        }

        contexto.Devoluciones.Add(devolucion);
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
            var resultadoPrestamoAnterior = await AfectarPrestamo(contexto, existente.PrestamoId, false);
            if (!resultadoPrestamoAnterior)
            {
                return false;
            }

            var resultadoPrestamoNuevo = await AfectarPrestamo(contexto, devolucion.PrestamoId, true);
            if (!resultadoPrestamoNuevo)
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

    public async Task<Result<Devoluciones?>> Buscar(int devolucionId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Devoluciones
            .Include(d => d.Prestamo)
                .ThenInclude(p => p!.Estudiante)
            .Include(d => d.Prestamo)
                .ThenInclude(p => p!.Libro)
            .FirstOrDefaultAsync(d => d.DevolucionId == devolucionId);
    }

    public async Task<Result<List<Devoluciones>>> GetList(Expression<Func<Devoluciones, bool>> criterio)
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

    public async Task<Result> Eliminar(int devolucionId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var devolucion = await contexto.Devoluciones
            .FirstOrDefaultAsync(d => d.DevolucionId == devolucionId);
        if (devolucion == null)
        {
            return Result.Success(false);
        }

        var prestamo = await contexto.Prestamos
            .FirstOrDefaultAsync(p => p.PrestamoId == devolucion.PrestamoId);
        if (prestamo == null)
        {
            return Result.Success(false);
        }

        var otroPrestamoActivo = await contexto.Prestamos
            .AnyAsync(p => p.LibroId == prestamo.LibroId
                && !p.Devuelto
                && p.PrestamoId != prestamo.PrestamoId);
        if (otroPrestamoActivo)
        {
            return Result.Success(false);
        }

        prestamo.Devuelto = false;
        contexto.Devoluciones.Remove(devolucion);
        var eliminado = await contexto.SaveChangesAsync() > 0;
        return Result.Success(eliminado);
    }
}
