using Aplicada1.Core;
using RegistroLibros.Context;
using Microsoft.EntityFrameworkCore;
using RegistroLibros.Models;
using System.Linq.Expressions;

namespace RegistroLibros.Services;

public class EstudiantesService(IDbContextFactory<Contexto> contextFactory) : IServiceResult<Estudiantes, int>
{
    public async Task<Result> Guardar(Estudiantes estudiante)
    {
        if (!await Existe(estudiante.EstudianteId))
        {
            if (await ExisteNombre(estudiante.Nombres))
                return Result.Failure(new Error("Nombre Duplicado", "Ya existe un estudiante con ese nombre"));

            var inserto = await Insertar(estudiante);
            return Result.Success(inserto);
        }
        else
        {
            var modifico = await Modificar(estudiante);
            return Result.Success(modifico);
        }
    }

    private async Task<bool> Existe(int? EstudianteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Estudiantes
            .AnyAsync(e => e.EstudianteId == EstudianteId);
    }

    private async Task<bool> ExisteNombre(string nombre)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Estudiantes
            .AnyAsync(n => n.Nombres == nombre);

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
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<Estudiantes?>> Buscar(int EstudianteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Estudiantes
            .FirstOrDefaultAsync(e => e.EstudianteId == EstudianteId);
    }

    public async Task<Result<List<Estudiantes>>> GetList(Expression<Func<Estudiantes, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Estudiantes
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Result> Eliminar(int EstudianteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var eliminado = await contexto.Estudiantes
            .Where(e => e.EstudianteId == EstudianteId)
            .ExecuteDeleteAsync() > 0;
        return Result.Success(eliminado);
    }
}
