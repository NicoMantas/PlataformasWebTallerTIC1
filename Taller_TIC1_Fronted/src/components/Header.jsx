import React from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import './Header.css';
import { getCurrentUser, logout } from '../services/authService';

const Header = () => {
  const navigate = useNavigate();
  const location = useLocation();

  const isHomePage = location.pathname === '/';
  const isAuthPage = location.pathname.includes('/login') || location.pathname.includes('/register');
  const user = getCurrentUser();

  const handleLogout = () => {
    logout();
    navigate('/');
  };

  return (
    <header className="header">
      <div className="header-container">
        <div className="logo" onClick={() => navigate('/')}>
          <span className="logo-icon">🔧</span>
          <span className="logo-text">AutoCare</span>
        </div>
        
        <nav className="nav">
          {isAuthPage && (
            <button 
              className="nav-link"
              onClick={() => navigate('/')}
            >
              Volver al Inicio
            </button>
          )}
          {isHomePage && !user && (
            <>
              <button 
                className="nav-link"
                onClick={() => navigate('/roles')}
              >
                Registrarse
              </button>
              <button 
                className="nav-link"
                onClick={() => navigate('/login-roles')}
              >
                Iniciar Sesión
              </button>
            </>
          )}
          
          {user && (
            <div className="user-section">
              <span className="user-info">
                {user.infoEspecifica?.nombre || user.email}
                {user.nombreTaller && ` - ${user.nombreTaller}`}
              </span>
              <button 
                className="nav-link logout-btn"
                onClick={handleLogout}
              >
                Cerrar Sesión
              </button>
            </div>
          )}
          
          {!isAuthPage && !isHomePage && !user && (
            <button 
              className="nav-link"
              onClick={() => navigate('/')}
            >
              Volver al Inicio
            </button>
          )}
        </nav>
      </div>
    </header>
  );
};

export default Header;
