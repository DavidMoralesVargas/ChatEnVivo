# Chat en Vivo con SignalR

Aplicación de chat en tiempo real desarrollada con **ASP.NET Core**, **Vue.js** y **SignalR**, cuyo objetivo principal fue practicar la implementación de comunicación en tiempo real utilizando tecnologías WebSocket y aplicar una **Arquitectura Hexagonal (Ports and Adapters)**.

---

## Características

- ✅ Registro e inicio de sesión de usuarios.
- ✅ Autenticación mediante JWT.
- ✅ Creación de salas de chat mediante códigos únicos.
- ✅ Comunicación en tiempo real utilizando SignalR.
- ✅ Chats grupales.
- ✅ Persistencia de datos en PostgreSQL.
- ✅ Arquitectura Hexagonal para separar responsabilidades.

---

## ¿Qué es WebSocket?

**WebSocket** es un protocolo que permite la comunicación bidireccional en tiempo real entre el cliente y el servidor mediante una conexión persistente. A diferencia del protocolo HTTP tradicional, no es necesario realizar solicitudes constantes para intercambiar información.

Esta tecnología es ampliamente utilizada en aplicaciones como:

- Chats en vivo.
- Videojuegos multijugador.
- Notificaciones en tiempo real.
- Sistemas de monitoreo.

En este proyecto se utilizó **SignalR**, una biblioteca de ASP.NET Core que simplifica la implementación de comunicación en tiempo real utilizando WebSockets cuando están disponibles.

---

# Arquitectura del Proyecto

El proyecto fue desarrollado siguiendo el patrón de **Arquitectura Hexagonal (Ports and Adapters)**.

## 📂 Dominio

Contiene las entidades principales y las reglas de negocio del sistema.

**Ejemplos:**

- Usuario
- CodigoChat

---

## 📂 Aplicación

Contiene la lógica de negocio mediante:

### Casos de Uso

Implementan las funcionalidades principales del sistema.

**Ejemplos:**

- Registro de usuarios.
- Inicio de sesión.
- Creación de códigos de chat.
- Gestión de mensajes.

### Puertos de Entrada

Definen las operaciones expuestas por la aplicación.

### Puertos de Salida

Definen las interfaces necesarias para acceder a servicios externos o persistencia.

---

## 📂 Infraestructura

Contiene los adaptadores encargados de implementar los puertos definidos en la capa de aplicación.

Incluye:

- Repositorios.
- Configuración de PostgreSQL.
- Implementación de SignalR.
- Servicios externos.
- Adaptadores de persistencia con Dapper.

---

## 📂 API

Funciona como adaptador de entrada.

Sus responsabilidades son:

- Exponer endpoints HTTP.
- Configurar la inyección de dependencias.
- Configurar autenticación JWT.
- Registrar SignalR.
- Conectar las diferentes capas de la aplicación.

---

# 🗄️ Base de Datos

La aplicación utiliza **PostgreSQL** como sistema gestor de bases de datos.

## Tabla `Usuarios`

```sql
CREATE TABLE Usuarios(
    Id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    Nombre_Usuario VARCHAR(200),
    Email VARCHAR(200),
    Contrasena VARCHAR(100)
);
```

## Tabla `CodigoChat`

```sql
CREATE TABLE CodigoChat(
    Id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    CodigoChat VARCHAR(200),
    FechaCreacion TIMESTAMP,
    Activo BOOLEAN,
    UsuarioCreacionId INT,
    CONSTRAINT usuId FOREIGN KEY (UsuarioCreacionId)
    REFERENCES Usuarios(Id)
);
```

---

# 🛠️ Tecnologías Utilizadas

## Backend

- ASP.NET Core Web API
- SignalR
- Dapper
- PostgreSQL
- JWT Authentication

## Frontend

- Vue.js
- Vue Router
- Axios
- SweetAlert2
- SignalR Client

---

# 📦 Librerías Utilizadas

## 🔧 Backend

### Npgsql

Proveedor oficial de PostgreSQL para .NET.

```bash
dotnet add package Npgsql
```

Permite conectar la aplicación con PostgreSQL.

---

### Dapper

Micro ORM ligero para ejecutar consultas SQL y mapear resultados a objetos C#.

```bash
dotnet add package Dapper
```

---

### Microsoft.AspNetCore.Authentication.JwtBearer

Permite implementar autenticación basada en JSON Web Tokens (JWT).

```bash
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

---

### Microsoft.AspNetCore.SignalR.Core

Biblioteca para implementar comunicación en tiempo real entre clientes y servidor.

```bash
dotnet add package Microsoft.AspNetCore.SignalR.Core
```

---

# 🎨 Frontend

### Vue Router

Sistema oficial de navegación de Vue.

```bash
npm install vue-router
```

Permite gestionar las rutas de la aplicación.

---

### Axios

Cliente HTTP basado en promesas.

```bash
npm install axios
```

Permite consumir la API REST desde Vue.

---

### SweetAlert2

Biblioteca para mostrar alertas y notificaciones visuales.

```bash
npm install sweetalert2
```

---

### Cliente SignalR

Cliente oficial de SignalR para JavaScript.

```bash
npm install @microsoft/signalr
```

Permite establecer la conexión en tiempo real entre el frontend y el backend.

---

# ⚙️ Instalación

## Clonar el repositorio

```bash
git clone URL_DEL_REPOSITORIO
```

## Backend

```bash
cd Backend
dotnet restore
dotnet run
```

## Frontend

```bash
cd Frontend
npm install
npm run dev
```

---

# 🔐 Autenticación

La aplicación implementa autenticación utilizando **JSON Web Tokens (JWT)** para proteger los recursos y garantizar que únicamente usuarios autenticados puedan acceder a determinadas funcionalidades.

---

# 🎯 Objetivos del Proyecto

- Aprender y practicar el uso de **SignalR**.
- Implementar comunicación en tiempo real mediante **WebSockets**.
- Aplicar una **Arquitectura Hexagonal**.
- Implementar autenticación y autorización mediante **JWT**.
- Integrar un frontend desarrollado en **Vue.js** con un backend en **ASP.NET Core**.
- Persistir información utilizando **PostgreSQL** y **Dapper**.

---
