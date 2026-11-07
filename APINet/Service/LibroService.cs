using APINet.Models;
using APINet.Repositories;

namespace APINet.Service
{
    public class LibroService : ILibroService
    {
        private readonly ILibroRepository _libroRepository;

        public LibroService(ILibroRepository libroRepository)
        {
            _libroRepository = libroRepository;
        }

        public async Task<IEnumerable<Libro>> Obtenerc()
        {
            var libros = await _libroRepository.ObtenerTodosAsync();

            // Regla de negocio: devolver ordenados por ID
            return libros.OrderByDescending(l => l.Id);
        }

        public async Task<Libro> ObtenerPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El Id debe ser mayor a cero.");

            var libro = await _libroRepository.ObtenerPorIdAsync(id);

            if (libro == null)
                throw new KeyNotFoundException($"No se encontró un libro con Id {id}.");

            return libro;
        }

        public async Task<Libro> Crear(Libro libro)
        {
            // Validaciones de negocio
            if (string.IsNullOrWhiteSpace(libro.Titulo))
                throw new ArgumentException("El título del libro es obligatorio.");

            if (string.IsNullOrWhiteSpace(libro.Autor))
                throw new ArgumentException("El autor del libro es obligatorio.");

            if (libro.Anio < 1500 || libro.Anio > DateTime.Now.Year)
                throw new ArgumentException("El año del libro no es válido.");

            // No permitir duplicados por título y autor
            var existentes = await _libroRepository.ObtenerTodosAsync();
            if (existentes.Any(l =>
                l.Titulo.Equals(libro.Titulo, StringComparison.OrdinalIgnoreCase) &&
                l.Autor.Equals(libro.Autor, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Ya existe un libro con el mismo título y autor.");
            }

            return await _libroRepository.CrearAsync(libro);
        }

        public async Task<bool> Actualizar(Libro libro)
        {
            if (libro.Id <= 0)
                throw new ArgumentException("El Id del libro no es válido.");

            if (string.IsNullOrWhiteSpace(libro.Titulo))
                throw new ArgumentException("El título del libro es obligatorio.");

            if (string.IsNullOrWhiteSpace(libro.Autor))
                throw new ArgumentException("El autor del libro es obligatorio.");

            if (libro.Anio < 1500 || libro.Anio > DateTime.Now.Year)
                throw new ArgumentException("El año del libro no es válido.");

            var existente = await _libroRepository.ObtenerPorIdAsync(libro.Id);


            if (existente == null)
                throw new KeyNotFoundException($"No se encontró un libro con Id {libro.Id}.");

            // Validación: no permitir duplicados de título + autor
            var todos = await _libroRepository.ObtenerTodosAsync();
            if (todos.Any(l =>
                l.Id != libro.Id &&
                l.Titulo.Equals(libro.Titulo, StringComparison.OrdinalIgnoreCase) &&
                l.Autor.Equals(libro.Autor, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Ya existe otro libro con el mismo título y autor.");
            }

            return await _libroRepository.ActualizarAsync(libro);
        }

        public async Task<bool> Eliminar(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El Id debe ser mayor a cero.");

            var libro = await _libroRepository.ObtenerPorIdAsync(id);
            if (libro == null)
                throw new KeyNotFoundException($"No se encontró un libro con Id {id}.");

            // Eliminación física desde repositorio
            return await _libroRepository.EliminarAsync(id);
        }
    }
    }
