using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RegistroLibros.Context;
using RegistroLibros.Models;
using System.Linq.Expressions;

namespace RegistroLibros.Services
{
    public class LibroService(IDbContextFactory<Contexto> contextFactory) : IService<Libro, int>
    {
        public async Task<Libro?> Buscar(int LibroId)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Libros
                .FirstOrDefaultAsync(l => l.LibroId == LibroId);
        }

        public async Task<bool> Eliminar(int LibroId)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Libros
                .Where(l => l.LibroId == LibroId)
                .ExecuteDeleteAsync() > 0;
        }

        public async Task<List<Libro>> GetList(Expression<Func<Libro, bool>> criterio)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Libros
                .Where(criterio)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> Guardar(Libro libro)
        {
            if (!await Existe(libro.LibroId))
            {
                if(await ExisteTitulo(libro.Titulo))
                    return false;

                return await Insertar(libro);
            }
            else
            {
                return await Modificar(libro);
            }
        }

        private async Task<bool> Existe(int? LibroId)
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

        private async Task<bool> Modificar (Libro libro)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            contexto.Update(libro);
            return await contexto.SaveChangesAsync() > 0;
        }
    }
}
