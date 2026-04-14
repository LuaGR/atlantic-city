# Atlantic City - Reto Técnico Fullstack

Este proyecto es la solución al reto técnico Fullstack desarrollado con **.NET 9 (Backend)** y **Next.js 15 (Frontend)**, implementando **Clean Architecture**, resiliencia y diseño orientado al dominio.

## 🚀 Arquitectura y Stack Tecnológico

### Backend (.NET 9)
*   **Clean Architecture (Domain, Application, Infrastructure, Api):** Desacoplamiento total de reglas de negocio.
*   **EF Core & PostgreSQL 16 (Docker):** Gestión de base de datos con Code-First Migrations automáticas.
*   **Seguridad:** JWT Bearer Token, Roles (Admin/User), Passwords hasheadas con BCrypt, Security Headers y Rate Limiting.
*   **Validaciones & Resiliencia:** FluentValidation, Global Exception Handler Middleware, Circuit Breaker & Retry con **Polly**.
*   **Logs:** Serilog centralizado.

### Frontend (Next.js 15 App Router & React 19)
*   **Estructura Limpia Modular:** Separación por módulos (`core`, `modules/pedidos/infrastructure`, `modules/pedidos/presentation`).
*   **UI Custom (styled-components):** Sin uso de TailwindCSS. Interfaz 100% custom basada en reglas corporativas, glassmorphism y micro-animaciones.
*   **Protección de Rutas:** Context API para Auth, enrutamiento privado/público nativo con useEffect/Layouts.
*   **Service Layer:** Integración basada en clases/objetos inyectando Instancias configuradas de Axios (con interceptores JWT automáticos).

---

## 💻 Prerrequisitos

*   [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
*   [Node.js 20+](https://nodejs.org/) (Recomendado v22)
*   [Docker Desktop](https://www.docker.com/) o PostgreSQL instalado localmente.

---

## 🛠️ Instrucciones de Ejecución (Makefile Rápidos)

Este proyecto incorpora un `Makefile` para orquestar rápidamente el entorno de desarrollo y evitar tipear comandos largos.

### 1. Levantar la Base de Datos
Inicia PostgreSQL y pgAdmin en contenedores Docker:
```bash
make up
```
*(Para detener todos los contenedores usar `make down`)*

### 2. Levantar el Backend (.NET 9)
En una terminal corre:
```bash
make dev-backend
```
*Las migraciones de Entity Framework y el seedeo inicial de datos de Master Admin se ejecutan automáticamente al arrancar. La API correrá en `http://localhost:5252`.*

### 3. Levantar el Frontend (Next.js 15)
En una terminal nueva corre:
```bash
make dev-frontend
```
*La App web estará corriendo en `http://localhost:3000`.*

---

## 🧪 Pruebas y Base de Datos
*   **Correr todos los tests:** `make test` (ejecuta tanto el Frontend como el Backend).
*   **Limpiar Base de Datos:** `make db-reset` (Elimina volúmenes y re-levanta Postgres limpio).
*   **Limpiar Entorno:** `make clean` (Elimina binarios ocultos de .NET y node_modules).

---

## 🔑 Credenciales de Prueba (Seed Automático)

Al arrancar el backend por primera vez, el sistema inserta este usuario automáticamente en la DB:

*   **Email Corporativo:** `admin@atlanticcity.com`
*   **Contraseña:** `Password123!`

---

## 📖 Endpoints Principales

Todos los requerimientos fueron cubiertos siguiendo convenciones RESTful.

*   `POST /auth/login` → Login y obtención de JWT.
*   `GET /api/pedidos` → Listar historial y estado de pedidos.
*   `GET /api/pedidos/{id}` → Detalle específico de un pedido.
*   `POST /api/pedidos` → Creación de pedido (Requiere `CreatePedidoDto`).
*   `PUT /api/pedidos/{id}` → Modificación de pedido existente.
*   `DELETE /api/pedidos/{id}` → Eliminación lógica (Soft Delete).

---

## 🧪 Notas de Diseño

*   **.NET JSON Enum Parser:** El pipeline de backend está configurado con `JsonStringEnumConverter` para aceptar y retornar los valores de Enums ("Registrado", "Procesado", etc.) de manera directa y legible mediante string en lugar de ints.
*   **Frontend Axios Interceptor:** Todo request hecho con `api.get` o `api.post` inyecta automáticamente el header `Authorization: Bearer <token>` consumido de LocalStorage para las rutas privadas.
*   **UX/UI Errores:** Next.js atrapa las estructuras de `FluentValidation` desde .NET (Ej: Error 400 - "Cliente es requerido") y los renderiza prolijamente dentro del formulario nativo usando el banner rojp de diseño corporativo.
