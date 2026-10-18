using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RegistroLibros.Context;
using RegistroLibros.Models;
using System.Linq.Expressions;

namespace RegistroLibros.Services;

public class EntradasService(IDbContextFactory<Contexto> contextFactory) : IServiceResult<Entradas, int>
{
    public async Task<Result> Guardar(Entradas entrada)
    {
        if (!await Existe(entrada.EntradaId))
        {
            var inserto = await Insertar(entrada);
            return Result.Success(inserto);
        }
        else
        {
            var modifico = await Modificar(entrada);
            return Result.Success(modifico);
        }
    }

    public async Task<bool> Existe(int entradaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Entradas.AnyAsync(e => e.EntradaId == entradaId);
    }

    private async Task<bool> Insertar(Entradas entrada)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        foreach (var detalle in entrada.EntradasDetalle)
        {
            detalle.Libro = null;
        }
        contexto.Entradas.Add(entrada);
        await AfectarLibros(entrada.EntradasDetalle.ToArray(), TipoOperacion.Suma);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Entradas entrada)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var entradaAnterior = await contexto.Entradas
            .Include(e => e.EntradasDetalle)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EntradaId == entrada.EntradaId);

        if (entradaAnterior != null)
        {
            await AfectarLibros(entradaAnterior.EntradasDetalle.ToArray(), TipoOperacion.Resta);
        }

        await AfectarLibros(entrada.EntradasDetalle.ToArray(), TipoOperacion.Suma);

        await contexto.EntradasDetalle
            .Where(d => d.EntradaId == entrada.EntradaId)
            .ExecuteDeleteAsync();

        foreach (var detalle in entrada.EntradasDetalle)
        {
            detalle.Libro = null;
            detalle.Id = 0;
        }

        contexto.Update(entrada);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task AfectarLibros(EntradasDetalle[] detalle, TipoOperacion tipoOperacion)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        foreach (var item in detalle)
        {
            var libro = await contexto.Libros.SingleAsync(l => l.LibroId == item.LibroId);
            if (tipoOperacion == TipoOperacion.Suma)
                libro.Cantidad += item.Cantidad;
            else
                libro.Cantidad -= item.Cantidad;
        }
        await contexto.SaveChangesAsync();
    }

    public async Task<Result<Entradas?>> Buscar(int entradaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Entradas
            .Include(e => e.EntradasDetalle)
            .ThenInclude(d => d.Libro)
            .FirstOrDefaultAsync(e => e.EntradaId == entradaId);
    }

    public async Task<Result<List<Entradas>>> GetList(Expression<Func<Entradas, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Entradas
            .Include(e => e.EntradasDetalle)
            .ThenInclude(d => d.Libro)
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
            return Result.Failure(new Error("EntradaNoEncontrada", "La entrada no fue encontrada."));

        await AfectarLibros(entrada.EntradasDetalle.ToArray(), TipoOperacion.Resta);

        contexto.EntradasDetalle.RemoveRange(entrada.EntradasDetalle);
        contexto.Entradas.Remove(entrada);
        var cantidad = await contexto.SaveChangesAsync();
        return Result.Success(cantidad > 0);
    }

    public async Task<List<Entradas>> Listar(Expression<Func<Entradas, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Entradas
            .Include(e => e.EntradasDetalle)
            .ThenInclude(d => d.Libro)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}

public enum TipoOperacion
{
    Suma = 1,
    Resta = 2
}
