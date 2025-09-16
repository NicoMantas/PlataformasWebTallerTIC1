import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import LandingPage from './pages/LandingPage';
import RoleSelect from './pages/RoleSelect';
import LoginRoleSelect from './pages/LoginRoleSelect';
import Login from './pages/Login';
import RegisterCliente from './pages/RegisterCliente';
import RegisterEmpresa from './pages/RegisterEmpresa';
import HomeCliente from './pages/HomeCliente';
import HomeEmpresa from './pages/HomeEmpresa';
import './App.css';

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
          <Route path="/home/cliente" element={<HomeCliente />} />
          <Route path="/home/empresa" element={<HomeEmpresa />} />
          
          {/* Ruta de fallback - redirigir a la página principal */}
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
