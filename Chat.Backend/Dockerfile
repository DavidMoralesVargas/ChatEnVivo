# Etapa 1: Compilación (Build)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos sin la '/' inicial
COPY ["Hexagonal.API/Hexagonal.API.csproj", "Hexagonal.API/"]
COPY ["Hexagonal.Application/Hexagonal.Application.csproj", "Hexagonal.Application/"]
COPY ["Hexagonal.Domain/Hexagonal.Domain.csproj", "Hexagonal.Domain/"]
COPY ["Hexagonal.Infrastructure/Hexagonal.Infrastructure.csproj", "Hexagonal.Infrastructure/"]

# Restaurar dependencias
RUN dotnet restore "Hexagonal.API/Hexagonal.API.csproj"

# Copiar todo el código del backend
COPY . .

# Compilar y publicar
WORKDIR "/src/Hexagonal.API"
RUN dotnet publish "Hexagonal.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Hexagonal.API.dll"]