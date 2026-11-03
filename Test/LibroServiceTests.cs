using APINet.Models;
using APINet.Repositories;
using APINet.Service;
using Moq;
namespace Tests
{
    [TestFixture]
    public class LibroServiceTests
    {
        private Mock<ILibroRepository> _repoMock;
        private LibroService _service;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<ILibroRepository>();
            _service = new LibroService(_repoMock.Object);
        }

        [Test]
        public async Task Crear_DeberiaCrearLibro_Exitoso()
        {
            // Arrange
            var nuevoLibro = new Libro { Id = 1, Titulo = "Clean Code", Autor = "Robert Martin", Anio = 2008, Genero = "Programación" };
            _repoMock.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(new List<Libro>());
            _repoMock.Setup(r => r.CrearAsync(It.IsAny<Libro>())).ReturnsAsync(nuevoLibro);

            // Act
            var resultado = await _service.Crear(nuevoLibro);

            // Assert
            Assert.That(resultado, Is.Not.Null);
            Assert.That(resultado.Titulo, Is.EqualTo("Clean Code"));
        }

        [Test]
        public void Crear_DeberiaLanzarExcepcion_CuandoTituloEsVacio()
        {
            // Arrange
            var libroInvalido = new Libro { Autor = "Autor X", Anio = 2020, Genero = "Ficción" };

            // Act
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _service.Crear(libroInvalido));


            //Assert
            Assert.That(ex.Message, Is.EqualTo("El título del libro es obligatorio."));
        }

        [Test]
        public void Crear_DeberiaLanzarExcepcion_CuandoExisteDuplicado()
        {
            // Arrange
            var libroExistente = new Libro { Id = 1, Titulo = "Clean Code", Autor = "Robert C. Martin", Anio = 2008, Genero = "Software" };
            var nuevoLibro = new Libro { Id = 2, Titulo = "Clean Code", Autor = "Robert C. Martin", Anio = 2010, Genero = "Programación" };

            _repoMock.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(new List<Libro> { libroExistente });

            // Act  
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _service.Crear(nuevoLibro));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("Ya existe un libro con el mismo título y autor."));

        }
    }
}
