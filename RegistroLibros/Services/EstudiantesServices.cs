using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RegistroLibros.Context;
using RegistroLibros.Models;
using System.Linq.Expressions;

namespace RegistroLibros.Services;

public class EstudiantesServices(IDbContextFactory<Contexto> 
    DbFactory) : IService<Estudiantes, int>
{
    private async Task<bool> Existe(int estudianteId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Estudiantes
            .AnyAsync(p => p.EstudianteId == estudianteId);
    }

    private async Task<bool> Insertar(Estudiantes estudiante)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Estudiantes.Add(estudiante);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Estudiantes estudiante)
    {
        if (!await Existe(estudiante.EstudianteId))
        {
            return await Insertar(estudiante);
        }
        else
        {
            return await Modificar(estudiante);
        }
    }

    private async Task<bool> Modificar(Estudiantes estudiante)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Update(estudiante);
        return await contexto
            .SaveChangesAsync() > 0;
    }

    public async Task<Estudiantes?> Buscar(int estudianteId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Estudiantes
            .FirstOrDefaultAsync(p => p.EstudianteId == estudianteId);
    }

    public async Task<bool> Eliminar(int estudianteId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Estudiantes
            .Where(p => p.EstudianteId == estudianteId)
            .ExecuteDeleteAsync() > 0;
    }

    public async Task<List<Estudiantes>> GetList(Expression<Func<Estudiantes, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Estudiantes
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}