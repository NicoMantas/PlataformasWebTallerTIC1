import React from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import '../styles/LoginRoleSelect.css';

const LoginRoleSelect = () => {
  const navigate = useNavigate();

  const handleRoleSelect = (role) => {
    navigate(`/login/${role}`);
  };

  return (
    <div className="login-role-select-page">
      <Header />
      
      <div className="container">
        <div className="login-role-header">
          <h1>Selecciona tu tipo de usuario</h1>
          <p>Elige cómo deseas iniciar sesión en nuestro sistema</p>
        </div>
        
        <div className="role-cards">
          <div className="role-card" onClick={() => handleRoleSelect('cliente')}>
            <div className="role-icon">👤</div>
            <h3>Cliente</h3>
            <p>Accede para gestionar tus vehículos y solicitar servicios</p>
            <button className="btn-primary">Seleccionar</button>
          </div>
          
          <div className="role-card" onClick={() => handleRoleSelect('empresa')}>
            <div className="role-icon">🏢</div>
            <h3>Empresa</h3>
            <p>Acceso para empresas con flotas vehiculares</p>
            <button className="btn-primary">Seleccionar</button>
          </div>

          <div className="role-card" onClick={() => handleRoleSelect('empleado')}>
            <div className="role-icon">🔧</div>
            <h3>Empleado</h3>
            <p>Acceso para el personal del taller</p>
            <button className="btn-primary">Seleccionar</button>
          </div>
        </div>
        
        <div className="register-redirect">
          <p>¿No tienes una cuenta?</p>
          <button 
            className="btn-link"
            onClick={() => navigate('/roles')}
          >
            Regístrate
          </button>
        </div>
      </div>
    </div>
  );
};

export default LoginRoleSelect;