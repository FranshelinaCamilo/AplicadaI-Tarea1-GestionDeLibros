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
                .Include(a => a.Autor)
                .FirstOrDefaultAsync(l => l.LibroId == LibroId);
        }

        public Task<bool> Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Libro>> GetList(Expression<Func<Libro, bool>> criterio)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Guardar(Libro entidad)
        {
            throw new NotImplementedException();
        }
    }
}
