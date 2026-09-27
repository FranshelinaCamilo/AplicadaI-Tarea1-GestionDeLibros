using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RegistroLibros.Context;
using RegistroLibros.Models;
using System.Linq.Expressions;

namespace RegistroLibros.Services
{
    public class PrestamosService(IDbContextFactory<Contexto> contextFactory) : IService<Prestamos, int>
    {
        public async Task<Prestamos?> Buscar(int PrestamoId)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Prestamos
                .FirstOrDefaultAsync(p => p.PrestamoId == PrestamoId);
        }

        public async Task<bool> Eliminar(int PrestamoId)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Prestamos
                .Where(p => p.PrestamoId == PrestamoId)
                .ExecuteDeleteAsync() > 0;
        }

        public async Task<List<Prestamos>> GetList(Expression<Func<Prestamos, bool>> criterio)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Prestamos
                .Where(criterio)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> Guardar(Prestamos prestamo)
        {
            if (!await Existe(prestamo.PrestamoId))
            {
                return await Insertar(prestamo);
            }
            else
            {
                return await Modificar(prestamo);
            }
        }

        private async Task<bool> Existe(int? prestamosId)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Prestamos
                .AnyAsync(p => p.PrestamoId == prestamosId);
        }

        private async Task<bool> Insertar(Prestamos prestamo)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            contexto.Prestamos.Add(prestamo);
            return await contexto.SaveChangesAsync() > 0;
        }

        private async Task<bool> Modificar(Prestamos prestamo)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            contexto.Update(prestamo);
            return await contexto.SaveChangesAsync() > 0;
        }
    }
}
