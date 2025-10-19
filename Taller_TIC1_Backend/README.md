# Taller TIC1 Backend

API REST completa para el sistema de gestión de talleres automotrices desarrollada con .NET 9, Entity Framework Core y PostgreSQL/Supabase. Implementa una arquitectura limpia con separación de responsabilidades y patrones de diseño modernos.

## 🚀 Características

- **Arquitectura**: Clean Architecture con separación de capas
- **Base de Datos**: PostgreSQL con Supabase (Cloud Database)
- **ORM**: Entity Framework Core 9.0 con Code First
- **API**: RESTful con Swagger/OpenAPI 3.0
- **Patrones**: Repository Pattern, Service Layer, DTOs, AutoMapper
- **Autenticación**: Sistema de roles y autorización
- **CORS**: Configurado para comunicación con frontend React

## 📁 Estructura del Proyecto

```
Taller_TIC1_Backend/
├── Controllers/              # Controladores de la API REST
│   ├── AuthController.cs     # Autenticación y autorización
│   ├── ClienteController.cs  # Gestión de clientes
│   ├── VehiculoController.cs # Gestión de vehículos
│   ├── ServicioController.cs # Gestión de servicios
│   ├── EmpleadoController.cs  # Gestión de empleados
│   ├── FacturaController.cs  # Gestión de facturación
│   └── ...
├── Data/                    # Contexto de Entity Framework
│   ├── ApplicationDbContext.cs # Contexto principal
│   └── MappingProfile.cs     # Configuración de AutoMapper
├── Models/                  # Modelos de datos y DTOs
│   └── DTOs/               # Data Transfer Objects
│       ├── ClienteDTO.cs    # DTOs para clientes
│       ├── VehiculoDTO.cs  # DTOs para vehículos
│       └── ...
├── Repositories/            # Patrón Repository
│   ├── Interfaces/         # Interfaces de repositorios
│   ├── ClienteRepository.cs # Implementación de repositorio
│   ├── VehiculoRepository.cs
│   └── ...
├── Services/               # Lógica de negocio
│   ├── Interfaces/        # Interfaces de servicios
│   ├── ClienteService.cs  # Implementación de servicios
│   ├── AuthService.cs     # Servicios de autenticación
│   └── ...
├── Migrations/            # Migraciones de Entity Framework
├── Program.cs             # Configuración de la aplicación
└── appsettings.json       # Configuración de la aplicación
```

## Entidades Principales

### Clientes
- **Cliente**: Cliente base
- **CNatural**: Cliente persona natural
- **CEmpresa**: Cliente empresa

### Vehículos
- **Vehiculo**: Vehículo base
- **VGasolina**: Vehículo de gasolina
- **VElectrico**: Vehículo eléctrico
- **VHibrido**: Vehículo híbrido

### Servicios
- **Servicio**: Servicio base
- **Reparacion**: Servicio de reparación
- **Revision**: Servicio de revisión

### Otros
- **Empleado**: Personal del taller
- **Proveedor**: Proveedores de repuestos
- **Repuesto**: Repuestos e inventario
- **OrdenDeTrabajo**: Órdenes de trabajo
- **Factura**: Facturación

## 🔗 Endpoints de la API

### 🔐 Autenticación
- `POST /api/auth/login` - Iniciar sesión
- `POST /api/auth/register` - Registro de usuarios
- `POST /api/auth/logout` - Cerrar sesión
- `GET /api/auth/me` - Información del usuario actual

### 👥 Clientes
- `GET /api/cliente` - Obtener todos los clientes
- `GET /api/cliente/{id}` - Obtener cliente por ID
- `GET /api/cliente/email/{email}` - Obtener cliente por email
- `POST /api/cliente` - Crear nuevo cliente
- `PUT /api/cliente/{id}` - Actualizar cliente
- `DELETE /api/cliente/{id}` - Eliminar cliente

### 🚗 Vehículos
- `GET /api/vehiculo` - Obtener todos los vehículos
- `GET /api/vehiculo/{id}` - Obtener vehículo por ID
- `GET /api/vehiculo/placa/{placa}` - Obtener vehículo por placa
- `GET /api/vehiculo/cliente/{clienteId}` - Vehículos de un cliente
- `POST /api/vehiculo` - Crear nuevo vehículo
- `PUT /api/vehiculo/{id}` - Actualizar vehículo
- `DELETE /api/vehiculo/{id}` - Eliminar vehículo

### 👨‍💼 Empleados
- `GET /api/empleado` - Obtener todos los empleados
- `GET /api/empleado/{id}` - Obtener empleado por ID
- `GET /api/empleado/cedula/{cedula}` - Obtener empleado por cédula
- `GET /api/empleado/tipo/{tipo}` - Empleados por tipo
- `POST /api/empleado` - Crear nuevo empleado
- `PUT /api/empleado/{id}` - Actualizar empleado
- `DELETE /api/empleado/{id}` - Eliminar empleado

### 🔧 Servicios
- `GET /api/servicio` - Obtener todos los servicios
- `GET /api/servicio/{id}` - Obtener servicio por ID
- `GET /api/servicio/estado/{estado}` - Servicios por estado
- `GET /api/servicio/empleado/{empleadoId}` - Servicios de un empleado
- `POST /api/servicio` - Crear nuevo servicio
- `PUT /api/servicio/{id}` - Actualizar servicio
- `PUT /api/servicio/{id}/estado` - Actualizar estado del servicio
- `DELETE /api/servicio/{id}` - Eliminar servicio

### 📋 Órdenes de Trabajo
- `GET /api/ordentrabajo` - Obtener todas las órdenes
- `GET /api/ordentrabajo/{id}` - Obtener orden por ID
- `GET /api/ordentrabajo/estado/{estado}` - Órdenes por estado
- `POST /api/ordentrabajo` - Crear nueva orden
- `PUT /api/ordentrabajo/{id}` - Actualizar orden
- `DELETE /api/ordentrabajo/{id}` - Eliminar orden

### 🏢 Proveedores
- `GET /api/proveedor` - Obtener todos los proveedores
- `GET /api/proveedor/{id}` - Obtener proveedor por ID
- `POST /api/proveedor` - Crear nuevo proveedor
- `PUT /api/proveedor/{id}` - Actualizar proveedor
- `DELETE /api/proveedor/{id}` - Eliminar proveedor

### 🔩 Repuestos
- `GET /api/repuesto` - Obtener todos los repuestos
- `GET /api/repuesto/{id}` - Obtener repuesto por ID
- `GET /api/repuesto/proveedor/{proveedorId}` - Repuestos de un proveedor
- `POST /api/repuesto` - Crear nuevo repuesto
- `PUT /api/repuesto/{id}` - Actualizar repuesto
- `DELETE /api/repuesto/{id}` - Eliminar repuesto

### 🧾 Facturas
- `GET /api/factura` - Obtener todas las facturas
- `GET /api/factura/{id}` - Obtener factura por ID
- `GET /api/factura/cliente/{clienteId}` - Facturas de un cliente
- `POST /api/factura` - Crear nueva factura
- `PUT /api/factura/{id}` - Actualizar factura
- `DELETE /api/factura/{id}` - Eliminar factura

### 📊 Estadísticas (Admin)
- `GET /api/estadisticas/admin` - Estadísticas generales del taller
- `GET /api/estadisticas/ventas` - Estadísticas de ventas
- `GET /api/estadisticas/servicios` - Estadísticas de servicios
- `GET /api/estadisticas/empleados` - Estadísticas de empleados

## ⚙️ Configuración

### 🔧 Variables de Entorno
La aplicación utiliza la cadena de conexión configurada en `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "User Id=postgres.kjxmnuyzbbgjrvwttxrl;Password=MotorLabpatic1;Server=aws-1-us-east-1.pooler.supabase.com;Port=5432;Database=postgres;Ssl Mode=Require;Trust Server Certificate=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### 🗄️ Configuración de Base de Datos

#### Supabase (Recomendado)
1. Crear cuenta en [Supabase](https://supabase.com)
2. Crear nuevo proyecto
3. Obtener la cadena de conexión
4. Configurar en `appsettings.json`

#### PostgreSQL Local
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=TallerDB;Username=postgres;Password=tu_password"
  }
}
```

### 🔄 Migraciones
Para crear y aplicar migraciones de Entity Framework:

```bash
# Instalar herramientas de EF Core
dotnet tool install --global dotnet-ef

# Crear migración inicial
dotnet ef migrations add InitialCreate

# Aplicar migraciones a la base de datos
dotnet ef database update

# Crear nueva migración después de cambios
dotnet ef migrations add NombreDeLaMigracion

# Revertir migración
dotnet ef database update MigracionAnterior
```

## 🚀 Ejecución

### Desarrollo
```bash
# 1. Clonar el repositorio
git clone [url-del-repositorio]

# 2. Navegar al directorio del backend
cd Taller_TIC1_Backend

# 3. Restaurar dependencias
dotnet restore

# 4. Aplicar migraciones
dotnet ef database update

# 5. Ejecutar la aplicación
dotnet run
```

### Producción
```bash
# Compilar para producción
dotnet build --configuration Release

# Ejecutar en producción
dotnet run --configuration Release
```

### 🐳 Docker (Opcional)
```dockerfile
# Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Taller_TIC1_Backend.csproj", "."]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Taller_TIC1_Backend.dll"]
```

## 📦 Dependencias

### Principales
- **Microsoft.EntityFrameworkCore** (9.0.9) - ORM principal
- **Npgsql.EntityFrameworkCore.PostgreSQL** (9.0.4) - Proveedor PostgreSQL
- **Swashbuckle.AspNetCore** (6.6.2) - Documentación Swagger
- **AutoMapper** (15.0.1) - Mapeo de objetos

### Referencias del Proyecto
- **B_TallerAutomoviles** - Biblioteca de clases del dominio

### Estructura de Dependencias
```
Taller_TIC1_Backend
├── B_TallerAutomoviles (Domain Library)
├── Microsoft.EntityFrameworkCore
├── Npgsql.EntityFrameworkCore.PostgreSQL
├── Swashbuckle.AspNetCore
└── AutoMapper
```

## 🔧 Configuración Avanzada

### CORS
```csharp
// Configuración en Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
```

### AutoMapper
```csharp
// Configuración en Program.cs
builder.Services.AddAutoMapper(cfg => {
    cfg.AddProfile<Data.MappingProfile>();
});
```

### Inyección de Dependencias
```csharp
// Ejemplo de registro de servicios
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();
```

## 📊 Monitoreo y Logging

### Logging
```csharp
// Configuración de logging en appsettings.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

### Health Checks
```csharp
// Agregar health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

app.MapHealthChecks("/health");
```

## 🔒 Seguridad

### Autenticación
- Sistema de roles implementado
- JWT tokens para autenticación
- Autorización basada en roles

### CORS
- Configurado para permitir comunicación con frontend
- Orígenes específicos configurados

### Validación
- Validación de modelos con Data Annotations
- Validación de entrada en controladores
- Manejo de errores centralizado

## 🧪 Testing

### Estructura de Tests
```
Tests/
├── Unit/                    # Tests unitarios
├── Integration/            # Tests de integración
└── Controllers/           # Tests de controladores
```

### Ejecutar Tests
```bash
# Ejecutar todos los tests
dotnet test

# Ejecutar con cobertura
dotnet test --collect:"XPlat Code Coverage"
```

## 📚 Documentación API

### Swagger/OpenAPI
- **URL**: `https://localhost:7000/swagger`
- **Documentación**: Generada automáticamente
- **Testing**: Interfaz interactiva disponible

### Ejemplos de Uso
```bash
# Obtener todos los clientes
GET /api/cliente

# Crear nuevo cliente
POST /api/cliente
{
  "nombre": "Juan Pérez",
  "email": "juan@email.com",
  "telefono": 1234567890
}

# Actualizar cliente
PUT /api/cliente/1
{
  "nombre": "Juan Carlos Pérez",
  "email": "juan@email.com",
  "telefono": 1234567890
}
```

## 🚀 Despliegue

### Azure
```bash
# Publicar para Azure
dotnet publish -c Release -o ./publish
```

### IIS
```bash
# Publicar para IIS
dotnet publish -c Release --self-contained false
```

### Linux
```bash
# Publicar para Linux
dotnet publish -c Release -r linux-x64 --self-contained true
```

## 🐛 Troubleshooting

### Problemas Comunes

1. **Error de conexión a base de datos**
   - Verificar cadena de conexión
   - Verificar que Supabase esté activo
   - Revisar firewall y puertos

2. **Error de migraciones**
   - Verificar que la base de datos exista
   - Ejecutar `dotnet ef database update`
   - Revisar permisos de usuario

3. **Error de CORS**
   - Verificar configuración de CORS
   - Revisar URLs permitidas
   - Verificar headers de respuesta

### Logs de Debug
```bash
# Ejecutar con logs detallados
dotnet run --verbosity detailed
```

## 📞 Soporte

Para soporte técnico:
- Revisar logs de la aplicación
- Verificar configuración de base de datos
- Consultar documentación de Entity Framework
- Revisar issues en el repositorio

---

**Desarrollado con ❤️ para el curso de Proyecto Aplicado en TIC 1**
