using Hexagonal.Application.CasosDeUso.Usuarios;
using Hexagonal.Application.Persistencia;
using Hexagonal.Application.Puertos.Entrada.Usuarios;
using Hexagonal.Infraestructure.Persistencia;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PostgreSqlConnection");

//Puerto y Adaptador de Salida
builder.Services.AddScoped<IUsuariosRepository>(provider =>
    new UsuariosRepository(connectionString!));

//Implementación de Puertos de Entrada (Casos de Uso)
builder.Services.AddScoped<IIngresarUsuarioCasoUso, IngresarUsuarioCasoUso>();
builder.Services.AddScoped<IRegistrarUsuarioCasoUso, RegistrarUsuarioCasoUso>();
builder.Services.AddScoped<IBuscarUsuarioCasoUso, BuscarUsuarioCasoUso>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
