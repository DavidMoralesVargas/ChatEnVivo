# Etapa 1: Compilación (Build)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto (.csproj) respetando la estructura de carpetas
COPY ["Hexagonal.API/Hexagonal.API.csproj", "Hexagonal.API/"]
COPY ["Hexagonal.Application/Hexagonal.Application.csproj", "Hexagonal.Application/"]
COPY ["Hexagonal.Domain/Hexagonal.Domain.csproj", "Hexagonal.Domain/"]
COPY ["Hexagonal.Infrastructure/Hexagonal.Infrastructure.csproj", "Hexagonal.Infrastructure/"]

# Restaurar todas las dependencias desde el punto de entrada (API)
RUN dotnet restore "Hexagonal.API/Hexagonal.API.csproj"

# Copiar el resto del código fuente del backend
COPY . .

# Compilar y publicar la aplicación Web API
WORKDIR "/src/Hexagonal.API"
RUN dotnet publish "Hexagonal.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Runtime ligero para producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render mapea dinámicamente el puerto usando variables de entorno
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Ejecutar el DLL de la capa ejecutable (API)
ENTRYPOINT ["dotnet", "Hexagonal.API.dll"]