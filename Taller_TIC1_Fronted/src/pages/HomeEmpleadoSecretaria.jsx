import React, { useEffect } from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import Header from '../components/Header';

const HomeEmpleadoSecretaria = () => {
  const location = useLocation();
  const navigate = useNavigate();

  useEffect(() => {
    // Si entra a /home/empleado/secretaria sin subruta, redirigir a /servicios
    if (location.pathname === '/home/empleado/secretaria') {
      navigate('/home/empleado/secretaria/servicios', { replace: true });
    }
  }, [location.pathname, navigate]);

  const isActive = (path) => location.pathname.startsWith(path);

  return (
    <div className="home-cliente-page">
      <Header />
      <div className="cliente-container">
        <div className="cliente-header">
          <h1>Secretaría</h1>
          <p>Panel de gestión</p>
        </div>
        <div className="cliente-dashboard">
          <div className="dashboard-tabs">
            <Link className={`tab-button ${isActive('/home/empleado/secretaria/servicios') ? 'active' : ''}`} to="/home/empleado/secretaria/servicios">Servicios</Link>
            {/* Más pestañas futuras se agregan aquí */}
          </div>
        </div>
      </div>
    </div>
  );
};

export default HomeEmpleadoSecretaria;


