using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RegistroLibros.Context;
using RegistroLibros.Models;
using System.Linq.Expressions;

namespace RegistroLibros.Services;

public class DevolucionesService(IDbContextFactory<Contexto> contextFactory) : IServiceResult<Devoluciones, int>
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

    private async Task<bool> Existe(int? DevolucionId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Devoluciones
            .AnyAsync(d => d.DevolucionId == DevolucionId);
    }

    private async Task<bool> Insertar(Devoluciones devolucion)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var prestamo = await contexto.Prestamos
            .Include(p => p.Libro)
            .FirstOrDefaultAsync(p => p.PrestamoId == devolucion.PrestamoId);

        if (prestamo == null || prestamo.Libro == null)
        {
            return false;
        }

        prestamo.Libro.Disponible = true;

        contexto.Devoluciones.Add(devolucion);

        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Devoluciones devolucion)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(devolucion);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<Devoluciones?>> Buscar(int DevolucionId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Devoluciones
            .FirstOrDefaultAsync(d => d.DevolucionId == DevolucionId);
    }

    public async Task<Result<List<Devoluciones>>> GetList(Expression<Func<Devoluciones, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Devoluciones
        .Include(d => d.Prestamo)
            .ThenInclude(p => p.Estudiante)
        .Include(d => d.Prestamo)
            .ThenInclude(p => p.Libro)
        .Where(criterio)
        .AsNoTracking()
        .ToListAsync();
    }

    public async Task<Result> Eliminar(int DevolucionId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var devolucion = await contexto.Devoluciones.FirstOrDefaultAsync(d => d.DevolucionId == DevolucionId);

        if(devolucion == null)
        {
            return Result.Failure(new Error("Devolucion vacia", "La devolucion debe contener valores"));
        }
        else
        {
            devolucion.Prestamo.Libro.Disponible = false;
            contexto.Libros.Update(devolucion.Prestamo.Libro);
            await contexto.SaveChangesAsync();
        }

        var elimino = await contexto.Devoluciones
            .Where(d => d.DevolucionId == DevolucionId)
            .ExecuteDeleteAsync() > 0;
        return Result.Success(elimino);
    }
}
