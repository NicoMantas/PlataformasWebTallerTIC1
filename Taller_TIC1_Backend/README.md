# Taller TIC1 Backend

API REST para el sistema de gestión de talleres automotrices desarrollada con .NET 9 y Entity Framework Core con PostgreSQL/Supabase.

## Características

- **Arquitectura**: Clean Architecture con separación de capas
- **Base de Datos**: PostgreSQL con Supabase
- **ORM**: Entity Framework Core 9.0
- **API**: RESTful con Swagger/OpenAPI
- **Patrones**: Repository Pattern, Service Layer, DTOs

## Estructura del Proyecto

```
Taller_TIC1_Backend/
├── Controllers/          # Controladores de la API
├── Data/                # Contexto de Entity Framework
├── Models/              # Modelos de datos y DTOs
│   └── DTOs/           # Data Transfer Objects
├── Repositories/        # Patrón Repository
│   └── Interfaces/     # Interfaces de repositorios
├── Services/           # Lógica de negocio
│   └── Interfaces/     # Interfaces de servicios
└── Program.cs          # Configuración de la aplicación
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

## Endpoints Disponibles

### Clientes
- `GET /api/cliente` - Obtener todos los clientes
- `GET /api/cliente/{id}` - Obtener cliente por ID
- `POST /api/cliente` - Crear nuevo cliente
- `PUT /api/cliente/{id}` - Actualizar cliente
- `DELETE /api/cliente/{id}` - Eliminar cliente

### Vehículos
- `GET /api/vehiculo` - Obtener todos los vehículos
- `GET /api/vehiculo/{id}` - Obtener vehículo por ID
- `GET /api/vehiculo/placa/{placa}` - Obtener vehículo por placa
- `POST /api/vehiculo` - Crear nuevo vehículo
- `PUT /api/vehiculo/{id}` - Actualizar vehículo
- `DELETE /api/vehiculo/{id}` - Eliminar vehículo

### Empleados
- `GET /api/empleado` - Obtener todos los empleados
- `GET /api/empleado/{id}` - Obtener empleado por ID
- `GET /api/empleado/cedula/{cedula}` - Obtener empleado por cédula
- `POST /api/empleado` - Crear nuevo empleado
- `PUT /api/empleado/{id}` - Actualizar empleado
- `DELETE /api/empleado/{id}` - Eliminar empleado

### Proveedores
- `GET /api/proveedor` - Obtener todos los proveedores
- `GET /api/proveedor/{id}` - Obtener proveedor por ID
- `POST /api/proveedor` - Crear nuevo proveedor
- `PUT /api/proveedor/{id}` - Actualizar proveedor
- `DELETE /api/proveedor/{id}` - Eliminar proveedor

### Repuestos
- `GET /api/repuesto` - Obtener todos los repuestos
- `GET /api/repuesto/{id}` - Obtener repuesto por ID
- `POST /api/repuesto` - Crear nuevo repuesto
- `PUT /api/repuesto/{id}` - Actualizar repuesto
- `DELETE /api/repuesto/{id}` - Eliminar repuesto

## Configuración

### Variables de Entorno
La aplicación utiliza la cadena de conexión configurada en `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "User Id=postgres.kjxmnuyzbbgjrvwttxrl;Password=MotorLabpatic1;Server=aws-1-us-east-1.pooler.supabase.com;Port=5432;Database=postgres;Ssl Mode=Require;Trust Server Certificate=true"
  }
}
```

### Migraciones
Para crear y aplicar migraciones de Entity Framework:

```bash
# Crear migración
dotnet ef migrations add InitialCreate

# Aplicar migraciones
dotnet ef database update
```

## Ejecución

1. **Clonar el repositorio**
2. **Restaurar dependencias**:
   ```bash
   dotnet restore
   ```
3. **Ejecutar la aplicación**:
   ```bash
   dotnet run
   ```
4. **Acceder a Swagger**: `https://localhost:7000/swagger`

## Dependencias

- **Microsoft.EntityFrameworkCore** (9.0.9)
- **Npgsql.EntityFrameworkCore.PostgreSQL** (9.0.4)
- **Swashbuckle.AspNetCore** (6.6.2)
- **B_TallerAutomoviles** (Biblioteca de clases del dominio)

## Notas Técnicas

- La aplicación está configurada para usar PostgreSQL con Supabase
- Utiliza Entity Framework Core con Code First approach
- Implementa el patrón Repository para abstracción de datos
- Los DTOs se utilizan para la transferencia de datos entre capas
- Swagger está habilitado para documentación automática de la API
