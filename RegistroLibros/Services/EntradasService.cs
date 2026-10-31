using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RegistroLibros.Data.Context;
using RegistroLibros.Data.Models;
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

        if (entrada.EntradasDetalle.Any(detalle => detalle.Cantidad <= 0))
        {
            return false;
        }

        contexto.Entradas.Add(entrada);

        await AfectarLibros(
            contexto,
            entrada.EntradasDetalle.ToArray(),
            TipoOperacion.Suma);

        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task AfectarLibros(
        Contexto contexto,
        EntradasDetalle[] detalles,
        TipoOperacion tipoOperacion)
    {
        foreach (var detalle in detalles)
        {
            var libro = await contexto.Libros
                .SingleAsync(libro => libro.LibroId == detalle.LibroId);
            if (tipoOperacion == TipoOperacion.Suma)
                libro.CantidadDisponible += detalle.Cantidad;
            else
                libro.CantidadDisponible -= detalle.Cantidad;
        }
    }

    private async Task<bool> Modificar(Entradas entrada)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        if (entrada.EntradasDetalle.Any(detalle => detalle.Cantidad <= 0))
        {
            return false;
        }

        var entradaExistente = await contexto.Entradas
            .Include(e => e.EntradasDetalle)
            .SingleOrDefaultAsync(e => e.EntradaId == entrada.EntradaId);
        if (entradaExistente == null)
        {
            return false;
        }

        await AfectarLibros(
            contexto,
            entradaExistente.EntradasDetalle.ToArray(),
            TipoOperacion.Resta);
        await AfectarLibros(
            contexto,
            entrada.EntradasDetalle.ToArray(),
            TipoOperacion.Suma);

        entradaExistente.Fecha = entrada.Fecha;
        entradaExistente.Concepto = entrada.Concepto;
        entradaExistente.Total = entrada.Total;

        contexto.RemoveRange(entradaExistente.EntradasDetalle);
        foreach (var detalle in entrada.EntradasDetalle)
        {
            entradaExistente.EntradasDetalle.Add(new EntradasDetalle
            {
                EntradaId = entradaExistente.EntradaId,
                LibroId = detalle.LibroId,
                Cantidad = detalle.Cantidad
            });
        }

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

        var entrada = await contexto.Entradas
            .Include(e => e.EntradasDetalle)
            .FirstOrDefaultAsync(e => e.EntradaId == entradaId);
        if (entrada == null)
        {
            return Result.Success(false);
        }

        await AfectarLibros(
            contexto,
            entrada.EntradasDetalle.ToArray(),
            TipoOperacion.Resta);
        contexto.Entradas.Remove(entrada);
        var eliminado = await contexto.SaveChangesAsync() > 0;
        return Result.Success(eliminado);
    }

    private enum TipoOperacion
    {
        Suma,
        Resta
    }
}

