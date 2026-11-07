using APINet.Data;
using APINet.Repositories;
using APINet.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurar EF Core con SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar el repositorio
builder.Services.AddScoped<ILibroRepository, LibroRepository>();

// Registrar el servicio
builder.Services.AddScoped<ILibroService, LibroService>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// **Habilitar Swagger siempre, no solo en Development**
app.UseSwagger();
app.UseSwaggerUI();

// Redirigir la raíz "/" a Swagger UI
app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
