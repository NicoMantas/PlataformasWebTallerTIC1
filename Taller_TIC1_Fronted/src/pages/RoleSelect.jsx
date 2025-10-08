import React from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import '../styles/RoleSelect.css';

const RoleSelect = () => {
  const navigate = useNavigate();

  const handleRoleSelect = (role) => {
    if (role === 'cliente') {
      navigate('/register/cliente');
    } else if (role === 'empresa') {
      navigate('/register/empresa');
    }
  };

  return (
    <div className="role-select-page">
      <Header />
      
      <div className="container">
        <div className="role-select-header">
          <h1>Selecciona tu tipo de cuenta</h1>
          <p>Elige cómo deseas registrarte en nuestro sistema</p>
        </div>
        
        <div className="role-cards">
          <div className="role-card" onClick={() => handleRoleSelect('cliente')}>
            <div className="role-icon">👤</div>
            <h3>Cliente</h3>
            <p>Registrate para solicitar servicios y hacer seguimiento a tus vehículos</p>
            <button className="btn-primary">Seleccionar</button>
          </div>
          
          <div className="role-card" onClick={() => handleRoleSelect('empresa')}>
            <div className="role-icon">🏢</div>
            <h3>Empresa</h3>
            <p>Registra tu empresa para gestionar flotas vehiculares y servicios corporativos</p>
            <button className="btn-primary">Seleccionar</button>
          </div>
        </div>
        
        <div className="login-redirect">
          <p>¿Ya tienes una cuenta?</p>
          <button 
            className="btn-link"
            onClick={() => navigate('/login-roles')}
          >
            Iniciar Sesión
          </button>
        </div>
      </div>
    </div>
  );
};

export default RoleSelect;