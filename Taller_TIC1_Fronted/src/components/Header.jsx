import React from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import './Header.css';

const Header = () => {
  const navigate = useNavigate();
  const location = useLocation();

  const isHomePage = location.pathname === '/';
  const isAuthPage = location.pathname.includes('/login') || location.pathname.includes('/register');

  return (
    <header className="header">
      <div className="header-container">
        <div className="logo" onClick={() => navigate('/')}>
          <span className="logo-icon">🔧</span>
          <span className="logo-text">AutoCare</span>
        </div>
        
        <nav className="nav">
          {isHomePage && (
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
          
          {!isAuthPage && !isHomePage && (
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
