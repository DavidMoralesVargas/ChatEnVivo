using Hexagonal.Application.CasosDeUso.Chats;
using Hexagonal.Application.CasosDeUso.Usuarios;
using Hexagonal.Application.Persistencia;
using Hexagonal.Application.Puertos.Entrada.Chats;
using Hexagonal.Application.Puertos.Entrada.Usuarios;
using Hexagonal.Application.Puertos.Salida;
using Hexagonal.Infraestructure.AdaptadorSalida;
using Hexagonal.Infraestructure.Persistencia;
using Hexagonal.Infraestructure.WebSocket;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PostgreSqlConnection");

builder.Services.AddSignalR();

//Puerto y Adaptador de Salida
builder.Services.AddScoped<IUsuariosRepository>(provider =>
    new UsuariosRepository(connectionString!));
builder.Services.AddScoped<IChatsSignalR, ChatSignalR>();
builder.Services.AddScoped<IChatsRepository>(provider =>
    new ChatsRepository(connectionString!));

//Implementación de Puertos de Entrada (Casos de Uso)
builder.Services.AddScoped<IIngresarUsuarioCasoUso, IngresarUsuarioCasoUso>();
builder.Services.AddScoped<IRegistrarUsuarioCasoUso, RegistrarUsuarioCasoUso>();
builder.Services.AddScoped<IBuscarUsuarioCasoUso, BuscarUsuarioCasoUso>();
builder.Services.AddScoped<IBuscarTodosCasoUso, BuscarTodosCasoUso>();
builder.Services.AddScoped<IEnviarGrupoCasoUso, EnviarGrupoCasoUso>();
builder.Services.AddScoped<IEnviarTodosCasoUso, EnviarTodosCasoUso>();
builder.Services.AddScoped<IEnviarUsuarioCasoUso, EnviarUsuarioCasoUso>();
builder.Services.AddScoped<IBuscarTodosCodigoCasoUso, BuscarTodosCodigoCasoUso>();
builder.Services.AddScoped<ICrearCodigoCasoUso, CrearCodigoCasoUso>();


//Configuramos autenticación de Token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    //Configuración de Parámetros de Token
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwtKey"]!)),
        ClockSkew = TimeSpan.Zero
    };
    //Configuración de token para autenticación de SignalR
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            var path = context.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments("/chathub"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});



var app = builder.Build();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<ChatHub>("/chathub");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
