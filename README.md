# Sistema de Gestión de Talleres Automotrices

Un sistema completo de gestión para talleres automotrices desarrollado con arquitectura de microservicios, incluyendo frontend en React y backend en .NET 9.

## 🚗 Descripción del Proyecto

Este sistema permite la gestión integral de talleres automotrices, incluyendo:

- **Gestión de Clientes**: Registro y administración de clientes naturales y empresas
- **Gestión de Vehículos**: Control de vehículos de gasolina, eléctricos e híbridos
- **Gestión de Servicios**: Reparaciones y revisiones con seguimiento de estado
- **Gestión de Empleados**: Administración de mecánicos, secretarias y administradores
- **Gestión de Inventario**: Control de repuestos y proveedores
- **Facturación**: Generación de facturas y órdenes de trabajo
- **Dashboard Administrativo**: Estadísticas y reportes del taller

## 🏗️ Arquitectura del Sistema

```
ProyectoGitHub/
├── B_TallerAutomoviles/           # Biblioteca de clases del dominio
├── Taller_TIC1_Backend/           # API REST (.NET 9)
├── Taller_TIC1_Fronted/           # Aplicación Web (React + Vite)
├── Diagrama/                      # Diagramas del sistema
└── Documentos/                    # Documentación del proyecto
```

### Componentes Principales

#### 1. **B_TallerAutomoviles** - Biblioteca de Dominio
- **Propósito**: Contiene las clases de dominio y lógica de negocio
- **Tecnología**: .NET 9 Class Library
- **Entidades**: Cliente, Vehículo, Servicio, Empleado, OrdenDeTrabajo, Factura, etc.

#### 2. **Taller_TIC1_Backend** - API REST
- **Propósito**: Servicios web y lógica de aplicación
- **Tecnología**: .NET 9 Web API
- **Base de Datos**: PostgreSQL con Supabase
- **ORM**: Entity Framework Core 9.0
- **Patrones**: Repository Pattern, Service Layer, DTOs

#### 3. **Taller_TIC1_Fronted** - Aplicación Web
- **Propósito**: Interfaz de usuario para todos los roles
- **Tecnología**: React 19 + Vite
- **Routing**: React Router DOM
- **HTTP Client**: Axios

## 👥 Roles del Sistema

### **Cliente**
- Registro y gestión de perfil
- Visualización de vehículos
- Seguimiento de servicios
- Historial de reparaciones

### **Empresa**
- Gestión de flota de vehículos
- Reportes de servicios
- Facturación empresarial

### **Empleado - Mecánico**
- Gestión de servicios asignados
- Actualización de estado de reparaciones
- Registro de trabajo realizado

### **Empleado - Secretaria**
- Gestión de citas y servicios
- Asignación de mecánicos
- Seguimiento de órdenes de trabajo

### **Empleado - Administrador**
- Dashboard con estadísticas
- Gestión de empleados
- Gestión de inventario
- Reportes del taller

## 🚀 Tecnologías Utilizadas

### Backend
- **.NET 9** - Framework principal
- **Entity Framework Core 9.0** - ORM
- **PostgreSQL** - Base de datos
- **Supabase** - Plataforma de base de datos
- **AutoMapper** - Mapeo de objetos
- **Swagger/OpenAPI** - Documentación de API

### Frontend
- **React 19** - Biblioteca de UI
- **Vite** - Build tool y dev server
- **React Router DOM** - Enrutamiento
- **Axios** - Cliente HTTP
- **CSS3** - Estilos personalizados

### Base de Datos
- **PostgreSQL** - Sistema de gestión de base de datos
- **Supabase** - Plataforma de base de datos en la nube

## 📋 Funcionalidades Principales

### Gestión de Clientes
- Registro de clientes naturales y empresas
- Autenticación y autorización
- Perfiles de usuario personalizados

### Gestión de Vehículos
- Registro de vehículos (gasolina, eléctricos, híbridos)
- Historial de servicios por vehículo
- Seguimiento de mantenimiento

### Gestión de Servicios
- Creación de servicios de reparación y revisión
- Asignación de mecánicos
- Seguimiento de estado (Pendiente, En Proceso, Terminado)
- Gestión de órdenes de trabajo

### Gestión de Inventario
- Control de repuestos
- Gestión de proveedores
- Relación repuesto-proveedor

### Facturación
- Generación de facturas
- Cálculo de costos
- Historial de pagos

## 🛠️ Instalación y Configuración

### Prerrequisitos
- .NET 9 SDK
- Node.js 18+
- PostgreSQL (o acceso a Supabase)

### Configuración del Backend
```bash
cd Taller_TIC1_Backend
dotnet restore
dotnet ef database update
dotnet run
```

### Configuración del Frontend
```bash
cd Taller_TIC1_Fronted
npm install
npm run dev
```

### Configuración de la Base de Datos
1. Crear cuenta en Supabase
2. Configurar la cadena de conexión en `appsettings.json`
3. Ejecutar migraciones de Entity Framework

## 📚 Documentación

- **Backend**: Ver `Taller_TIC1_Backend/README.md`
- **Frontend**: Ver `Taller_TIC1_Fronted/README.md`
- **Diagramas**: Ver carpeta `Diagrama/`
- **API**: Swagger disponible en `https://localhost:7000/swagger`

## 🔧 Desarrollo

### Estructura de Commits
- `feat:` Nueva funcionalidad
- `fix:` Corrección de bugs
- `docs:` Documentación
- `style:` Formato de código
- `refactor:` Refactorización
- `test:` Pruebas

### Patrones de Diseño Implementados
- **Repository Pattern**: Abstracción de acceso a datos
- **Service Layer**: Lógica de negocio
- **DTO Pattern**: Transferencia de datos
- **Factory Pattern**: Creación de objetos de dominio
- **Strategy Pattern**: Diferentes tipos de vehículos y servicios

## 📄 Licencia

Este proyecto es desarrollado como parte del curso de Proyecto Aplicado en TIC 1 de la Universidad Pontificia Bolivariana.

## 👨‍💻 Equipo de Desarrollo

- **Desarrollador**: Nicolás Mantilla, Juan Jose Ramirez, Andres Felipe Navarro y Helmer Andrey Murillo
- **Institución**: Universidad Pontificia Bolivariana
- **Curso**: Proyecto Aplicado en TIC 1
- **Semestre**: Sexto Semestre

## 📞 Contacto

Para más información sobre el proyecto, contactar al desarrollador o revisar la documentación técnica en las carpetas correspondientes.
