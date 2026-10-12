using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RegistroLibros.Context;
using RegistroLibros.Models;
using System.Linq.Expressions;

namespace RegistroLibros.Services;

public class LibrosService(
    IDbContextFactory<Contexto> contextFactory
    ) : IServiceResult<Libros, int>
{

    public async Task<Result> Guardar(Libros libro)
    {
        if (!await Existe(libro.LibroId))
        {
            var inserto = await Insertar(libro);
            return Result.Success(inserto);
        }
        else
        {
            var modifico = await Modificar(libro);
            return Result.Success(modifico);
        }
    }

    private async Task<bool> Existe(int libroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros
            .AnyAsync(p => p.LibroId == libroId);
    }

    private async Task<bool> Insertar(Libros libro)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Libros.Add(libro);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Libros libro)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(libro);
        return await contexto
            .SaveChangesAsync() > 0;
    }

    public async Task<Result<Libros?>> Buscar(int libroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros
            .FirstOrDefaultAsync(p => p.LibroId == libroId);
    }

    public async Task<Result<List<Libros>>> GetList(Expression<Func<Libros, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Result> Eliminar(int libroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var prestamoActivo = await contexto.Prestamos
            .AnyAsync(p => p.LibroId == libroId && !p.Devuelto);
        if (prestamoActivo)
        {
            return Result.Success(false);
        }

        var eliminado = await contexto.Libros
            .Where(p => p.LibroId == libroId)
            .ExecuteDeleteAsync() > 0;
        return Result.Success(eliminado);
    }
}
