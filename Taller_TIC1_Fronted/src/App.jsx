import React from 'react'
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import LandingPage from './pages/LandingPage';
import RoleSelect from './pages/RoleSelect';
import LoginRoleSelect from './pages/LoginRoleSelect';
import Login from './pages/Login';
import RegisterCliente from './pages/RegisterCliente';
import RegisterEmpresa from './pages/RegisterEmpresa';
import HomeCliente from './pages/HomeCliente';
import HomeEmpresa from './pages/HomeEmpresa';
import HomeEmpleado from './pages/HomeEmpleado';
import './App.css';
import { getCurrentUser } from './services/authService';

function PrivateRoute({ element, allowed }) {
  const user = getCurrentUser();
  if (!user) return <Navigate to="/login-roles" replace />;
  if (allowed && !allowed(user)) return <Navigate to="/" replace />;
  return element;
}

function App() {
  return (
    <Router>
      <div className="App">
        <Routes>
          {/* Ruta principal */}
          <Route path="/" element={<LandingPage />} />
          
          {/* Rutas de selección de rol */}
          <Route path="/roles" element={<RoleSelect />} />
          <Route path="/login-roles" element={<LoginRoleSelect />} />
          
          {/* Rutas de autenticación */}
          <Route path="/login/:role" element={<Login />} />
          <Route path="/register/cliente" element={<RegisterCliente />} />
          <Route path="/register/empresa" element={<RegisterEmpresa />} />
          
          {/* Rutas protegidas - Home pages */}
          <Route path="/home/cliente" element={<PrivateRoute element={<HomeCliente />} allowed={(u)=>u.tipoUsuario==='cliente'} />} />
          <Route path="/home/empresa" element={<PrivateRoute element={<HomeEmpresa />} allowed={(u)=>u.tipoUsuario==='cliente'} />} />
          <Route path="/home/empleado/:tipo" element={<PrivateRoute element={<HomeEmpleado />} allowed={(u)=>u.tipoUsuario==='empleado'} />} />
          
          {/* Ruta de fallback - redirigir a la página principal */}
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
