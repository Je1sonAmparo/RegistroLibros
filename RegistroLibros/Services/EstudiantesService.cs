using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RegistroLibros.Context;
using RegistroLibros.Models;
using System.Linq.Expressions;

namespace RegistroLibros.Services;

public class EstudiantesService(
    IDbContextFactory<Contexto> contextFactory
    ) : IServiceResult<Estudiantes, int>
{

    public async Task<Result> Guardar(Estudiantes estudiante)
    {
        if (!await Existe(estudiante.EstudianteId))
        {
            var inserto = await Insertar(estudiante);
            return Result.Success(inserto);
        }
        else
        {
            var modifico = await Modificar(estudiante);
            return Result.Success(modifico);
        }
    }

    private async Task<bool> Existe(int estudianteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Estudiantes
            .AnyAsync(p => p.EstudianteId == estudianteId);
    }

    private async Task<bool> Insertar(Estudiantes estudiante)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Estudiantes.Add(estudiante);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Estudiantes estudiante)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(estudiante);
        return await contexto
            .SaveChangesAsync() > 0;
    }

    public async Task<Result<Estudiantes?>> Buscar(int estudianteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Estudiantes
            .FirstOrDefaultAsync(p => p.EstudianteId == estudianteId);
    }

    public async Task<Result<List<Estudiantes>>> GetList(Expression<Func<Estudiantes, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Estudiantes
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Result> Eliminar(int estudianteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var tienePrestamos = await contexto.Prestamos
            .AnyAsync(p => p.EstudianteId == estudianteId);
        if (tienePrestamos)
        {
            return Result.Success(false);
        }

        var eliminado = await contexto.Estudiantes
            .Where(p => p.EstudianteId == estudianteId)
            .ExecuteDeleteAsync() > 0;
        return Result.Success(eliminado);
    }
}
