using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RegistroLibros.Context;
using RegistroLibros.Models;
using System.Linq.Expressions;

namespace RegistroLibros.Services;

public class PrestamosService(
    IDbContextFactory<Contexto> contextFactory
    ) : IServiceResult<Prestamos, int>
{

    public async Task<Result> Guardar(Prestamos prestamo)
    {
        if (!await Existe(prestamo.PrestamoId))
        {
            var inserto = await Insertar(prestamo);
            return Result.Success(inserto);
        }
        else
        {
            var modifico = await Modificar(prestamo);
            return Result.Success(modifico);
        }
    }

    private async Task<bool> Existe(int prestamoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos
            .AnyAsync(p => p.PrestamoId == prestamoId);
    }

    private async Task<bool> Insertar(Prestamos prestamo)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Prestamos.Add(prestamo);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Prestamos prestamo)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(prestamo);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<Prestamos?>> Buscar(int prestamoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos
            .Include(p => p.Estudiante)
            .Include(p => p.Libro)
            .FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);
    }

    public async Task<Result<List<Prestamos>>> GetList(Expression<Func<Prestamos, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos
            .Include(p => p.Estudiante)
            .Include(p => p.Libro)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Result> Eliminar(int prestamoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var eliminado = await contexto.Prestamos
            .Where(p => p.PrestamoId == prestamoId)
            .ExecuteDeleteAsync() > 0;
        return Result.Success(eliminado);
    }
}
