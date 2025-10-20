# Taller TIC1 Frontend

Aplicación web desarrollada en React para el sistema de gestión de talleres automotrices. Proporciona una interfaz moderna y responsiva para todos los roles del sistema.

## 🚀 Tecnologías

- **React 19** - Biblioteca de UI moderna
- **Vite 7** - Build tool y dev server ultra-rápido
- **React Router DOM 7** - Enrutamiento del lado del cliente
- **Axios** - Cliente HTTP para comunicación con la API
- **CSS3** - Estilos personalizados y responsivos

## 📁 Estructura del Proyecto

```
Taller_TIC1_Fronted/
├── public/                    # Archivos estáticos
├── src/
│   ├── components/          # Componentes reutilizables
│   │   ├── AuthForm.jsx      # Formulario de autenticación
│   │   ├── Header.jsx        # Cabecera de la aplicación
│   │   ├── Footer.jsx        # Pie de página
│   │   ├── DashboardAdmin.jsx # Dashboard administrativo
│   │   ├── EmpleadosManager.jsx # Gestión de empleados
│   │   ├── VehiculosManager.jsx # Gestión de vehículos
│   │   └── ...
│   ├── pages/                # Páginas principales
│   │   ├── LandingPage.jsx   # Página de inicio
│   │   ├── Login.jsx         # Página de login
│   │   ├── RegisterCliente.jsx # Registro de clientes
│   │   ├── RegisterEmpresa.jsx # Registro de empresas
│   │   ├── HomeCliente.jsx   # Dashboard del cliente
│   │   ├── HomeEmpresa.jsx   # Dashboard de empresa
│   │   ├── HomeEmpleado.jsx  # Dashboard de empleados
│   │   └── ...
│   ├── services/             # Servicios de API
│   │   ├── api.js           # Configuración base de Axios
│   │   ├── authService.js    # Servicios de autenticación
│   │   ├── clientesService.js # Servicios de clientes
│   │   ├── vehiculosService.js # Servicios de vehículos
│   │   ├── serviciosService.js # Servicios de servicios
│   │   ├── empleadosService.js # Servicios de empleados
│   │   ├── facturasService.js # Servicios de facturación
│   │   └── ...
│   ├── styles/               # Archivos CSS
│   │   ├── LandingPage.css   # Estilos de la página principal
│   │   ├── Login.css         # Estilos de login
│   │   ├── HomeCliente.css   # Estilos del dashboard cliente
│   │   └── ...
│   ├── App.jsx              # Componente principal
│   └── main.jsx             # Punto de entrada
├── package.json             # Dependencias del proyecto
├── vite.config.js          # Configuración de Vite
└── eslint.config.js        # Configuración de ESLint
```

## 🎯 Funcionalidades por Rol

### **Cliente**
- **Landing Page**: Página de bienvenida con información del taller
- **Registro**: Formulario de registro para clientes naturales
- **Login**: Autenticación segura
- **Dashboard Cliente**: 
  - Visualización de vehículos registrados
  - Historial de servicios
  - Seguimiento de reparaciones en tiempo real
  - Gestión de perfil personal

### **Empresa**
- **Registro Empresa**: Formulario especializado para empresas
- **Dashboard Empresa**:
  - Gestión de flota de vehículos
  - Reportes de servicios por vehículo
  - Facturación empresarial
  - Estadísticas de mantenimiento

### **Empleado - Mecánico**
- **Dashboard Mecánico**:
  - Servicios asignados
  - Actualización de estado de reparaciones
  - Registro de trabajo realizado
  - Gestión de repuestos utilizados

### **Empleado - Secretaria**
- **Dashboard Secretaria**:
  - Gestión de citas y servicios
  - Asignación de mecánicos
  - Seguimiento de órdenes de trabajo
  - Gestión de clientes

### **Empleado - Administrador**
- **Dashboard Administrativo**:
  - Estadísticas generales del taller
  - Gestión de empleados
  - Gestión de inventario (repuestos y proveedores)
  - Reportes financieros
  - Configuración del sistema

## 🔧 Instalación y Configuración

### Prerrequisitos
- Node.js 18 o superior
- npm o yarn

### Instalación
```bash
# Clonar el repositorio
git clone [url-del-repositorio]

# Navegar al directorio del frontend
cd Taller_TIC1_Fronted

# Instalar dependencias
npm install
```

### Configuración de Desarrollo
```bash
# Ejecutar en modo desarrollo
npm run dev

# La aplicación estará disponible en http://localhost:5173
```

### Scripts Disponibles
```bash
# Desarrollo
npm run dev          # Servidor de desarrollo con hot reload

# Producción
npm run build        # Construir para producción
npm run preview      # Vista previa de la build de producción

# Calidad de código
npm run lint         # Ejecutar ESLint
```

## 🌐 Configuración de API

La aplicación se conecta al backend a través de Axios. La configuración base se encuentra en `src/services/api.js`:

```javascript
import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7000/api', // URL del backend
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});
```

## 🎨 Componentes Principales

### **AuthForm**
Componente reutilizable para formularios de autenticación con validación.

### **Header**
Cabecera de la aplicación con navegación y información del usuario.

### **DashboardAdmin**
Dashboard completo para administradores con estadísticas y gestión.

### **EmpleadosManager**
Gestión completa de empleados con CRUD operations.

### **VehiculosManager**
Gestión de vehículos con soporte para diferentes tipos.

## 🔐 Autenticación y Autorización

### **Sistema de Roles**
- **Cliente**: Acceso limitado a sus propios datos
- **Empresa**: Acceso a flota de vehículos
- **Empleado**: Acceso según tipo (mecánico, secretaria, administrador)

### **Rutas Protegidas**
```javascript
// Ejemplo de ruta protegida
<Route 
  path="/home/cliente" 
  element={
    <PrivateRoute 
      element={<HomeCliente />} 
      allowed={(u) => u.tipoUsuario === 'cliente'} 
    />
  } 
/>
```

### **Almacenamiento Local**
- Información del usuario en localStorage
- Tokens de autenticación
- Preferencias de la aplicación

## 📱 Responsive Design

La aplicación está diseñada para ser completamente responsiva:

- **Desktop**: Experiencia completa con todas las funcionalidades
- **Tablet**: Adaptación de layouts y navegación
- **Mobile**: Interfaz optimizada para dispositivos móviles

## 🎯 Características Técnicas

### **React 19 Features**
- Hooks modernos (useState, useEffect, useContext)
- Componentes funcionales
- React Router DOM para navegación
- Manejo de estado local y global

### **Vite Benefits**
- Hot Module Replacement (HMR) ultra-rápido
- Build optimizado para producción
- Soporte nativo para ES modules
- Plugin ecosystem extenso

### **Arquitectura de Componentes**
- Componentes reutilizables
- Separación de lógica y presentación
- Hooks personalizados para lógica compleja
- Servicios para comunicación con API

## 🚀 Despliegue

### **Build de Producción**
```bash
npm run build
```

### **Variables de Entorno**
Crear archivo `.env` para configuración:
```
VITE_API_BASE_URL=https://tu-api.com/api
VITE_APP_NAME=Taller TIC1
```

### **Servidor Web**
La aplicación puede ser desplegada en cualquier servidor web estático:
- Netlify
- Vercel
- GitHub Pages
- Apache/Nginx

## 🧪 Testing

### **Estructura de Testing**
```
src/
├── __tests__/           # Tests unitarios
├── components/          # Componentes con tests
└── services/           # Servicios con tests
```

### **Herramientas de Testing**
- Jest para testing unitario
- React Testing Library para componentes
- Axios mock para servicios

## 📊 Performance

### **Optimizaciones Implementadas**
- Lazy loading de componentes
- Code splitting por rutas
- Optimización de imágenes
- Caching de API calls
- Memoización de componentes pesados

### **Métricas de Performance**
- First Contentful Paint < 1.5s
- Largest Contentful Paint < 2.5s
- Cumulative Layout Shift < 0.1

## 🔧 Desarrollo

### **Estructura de Commits**
```
feat: nueva funcionalidad
fix: corrección de bug
docs: documentación
style: formato de código
refactor: refactorización
test: pruebas
```

### **Code Style**
- ESLint configurado para React
- Prettier para formato de código
- Convenciones de nomenclatura consistentes

## 📚 Dependencias Principales

### **Dependencies**
```json
{
  "react": "^19.1.1",
  "react-dom": "^19.1.1",
  "react-router-dom": "^7.9.1",
  "axios": "^1.12.2"
}
```

### **Dev Dependencies**
```json
{
  "vite": "^7.1.2",
  "@vitejs/plugin-react": "^5.0.0",
  "eslint": "^9.33.0",
  "eslint-plugin-react-hooks": "^5.2.0"
}
```

## 🐛 Troubleshooting

### **Problemas Comunes**

1. **Error de CORS**: Verificar configuración del backend
2. **API no responde**: Verificar URL base en `api.js`
3. **Rutas no funcionan**: Verificar configuración de React Router
4. **Estilos no cargan**: Verificar imports de CSS

### **Debug Mode**
```bash
# Ejecutar con debug
npm run dev -- --debug

# Ver logs detallados
DEBUG=* npm run dev
```

## 📞 Soporte

Para soporte técnico o preguntas sobre el frontend:
- Revisar la documentación del backend
- Verificar logs del navegador
- Consultar la documentación de React y Vite

---

**Desarrollado con ❤️ para el curso de Proyecto Aplicado en TIC 1**