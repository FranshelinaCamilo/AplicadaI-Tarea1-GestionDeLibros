using Aplicada1.Core;
using RegistroLibros.Models;
using System.Linq.Expressions;

namespace RegistroLibros.Services;

public interface IEntradasService
{
    Task<Result> Guardar(Entradas entrada);
    Task<Result<Entradas?>> Buscar(int entradaId);
    Task<Result> Eliminar(int entradaId);
    Task<Result<List<Entradas>>> GetList(Expression<Func<Entradas, bool>> criterio);
    Task<List<Entradas>> Listar(Expression<Func<Entradas, bool>> criterio);
    Task<bool> Existe(int entradaId);
}
