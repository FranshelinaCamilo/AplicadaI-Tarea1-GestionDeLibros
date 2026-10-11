using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RegistroLibros.Context;
using RegistroLibros.Models;
using System.Linq.Expressions;

namespace RegistroLibros.Services;

public class DevolucionesService(IDbContextFactory<Contexto> contextFactory) : IService<Devoluciones, int>
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

    private async Task<bool> Existe(int? DevolucionId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Devoluciones
            .AnyAsync(d => d.DevolucionId == DevolucionId);
    }

    private async Task<bool> Insertar(Devoluciones devolucion)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Devoluciones.Add(devolucion);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Devoluciones devolucion)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(devolucion);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Devoluciones?> Buscar(int DevolucionId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Devoluciones
            .FirstOrDefaultAsync(d => d.DevolucionId == DevolucionId);
    }

    public async Task<List<Devoluciones>> GetList(Expression<Func<Devoluciones, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Devoluciones
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Eliminar(int DevolucionId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Devoluciones
            .Where(d => d.DevolucionId == DevolucionId)
            .ExecuteDeleteAsync() > 0;
    }
}
