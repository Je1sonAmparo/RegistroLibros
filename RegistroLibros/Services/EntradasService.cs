using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RegistroLibros.Context;
using RegistroLibros.Models;
using System.Linq.Expressions;

namespace RegistroLibros.Services;

public class EntradasService(
    IDbContextFactory<Contexto> contextFactory
    ) : IServiceResult<Entradas, int>
{
    public async Task<Result> Guardar(Entradas entrada)
    {
        if (!await Existe(entrada.EntradaId))
        {
            var inserto = await Insertar(entrada);
            return Result.Success(inserto);
        }

        var modifico = await Modificar(entrada);
        return Result.Success(modifico);
    }

    private async Task<bool> Existe(int entradaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Entradas
            .AnyAsync(e => e.EntradaId == entradaId);
    }

    private async Task<bool> Insertar(Entradas entrada)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Entradas.Add(entrada);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Entradas entrada)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(entrada);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<Entradas?>> Buscar(int entradaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Entradas
            .Include(e => e.EntradasDetalle)
                .ThenInclude(detalle => detalle.Libro)
            .FirstOrDefaultAsync(e => e.EntradaId == entradaId);
    }

    public async Task<Result<List<Entradas>>> GetList(Expression<Func<Entradas, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Entradas
            .Include(e => e.EntradasDetalle)
                .ThenInclude(detalle => detalle.Libro)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Result> Eliminar(int entradaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var eliminado = await contexto.Entradas
            .Where(e => e.EntradaId == entradaId)
            .ExecuteDeleteAsync() > 0;
        return Result.Success(eliminado);
    }
}
