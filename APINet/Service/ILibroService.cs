using APINet.Models;

namespace APINet.Service
{
    public interface ILibroService
    {
        Task<IEnumerable<Libro>> Obtenerc();
        Task<Libro> ObtenerPorId(int id);
        Task<Libro> Crear(Libro libro);
        Task<bool> Actualizar(Libro libro);
        Task<bool> Eliminar(int id);
    }
}
