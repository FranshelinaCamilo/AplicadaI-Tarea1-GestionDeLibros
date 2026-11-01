using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RegistroLibros.Data.Context;
using RegistroLibros.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace RegistroLibros.Services.Services;

public class LibrosService(IDbContextFactory<BibliotecaBdContext> contextFactory) : IServiceResult<Libro, int>
{
    public async Task<Result> Guardar(Libro libro)
    {
        if (!await Existe(libro.LibroId))
        {
            if (await ExisteTitulo(libro.Titulo))
                return Result.Failure(new Error("Titulo Duplicado", "Ya existe un libro con ese título."));

            var inserto = await Insertar(libro);
            return Result.Success(inserto);
        }
        else
        {
            var modifico = await Modificar(libro);
            return Result.Success(modifico);
        }
    }

    public async Task<bool> Existe(int? LibroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros
            .AnyAsync(l => l.LibroId == LibroId);
    }

    private async Task<bool> ExisteTitulo(string titulo)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros
            .AnyAsync(t => t.Titulo == titulo);
    }

    private async Task<bool> Insertar(Libro libro)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Libros.Add(libro);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Libro libro)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(libro);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<Libro?>> Buscar(int LibroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros
            .FirstOrDefaultAsync(l => l.LibroId == LibroId);
    }

    public async Task<Result<List<Libro>>> GetList(Expression<Func<Libro, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Result> Eliminar(int LibroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var eliminado = await contexto.Libros
            .Where(l => l.LibroId == LibroId)
            .ExecuteDeleteAsync() > 0;
        return Result.Success(eliminado);
    }
}
