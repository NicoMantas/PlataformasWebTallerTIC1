import React, { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import Header from '../components/Header';
import AuthForm from '../components/AuthForm';
import '../styles/Login.css';

const Login = () => {
  const navigate = useNavigate();
  const { role } = useParams();
  const [error, setError] = useState('');

  const handleLogin = (formData) => {
    // Simulación de login
    if (formData.email && formData.password) {
      if (role === 'cliente') {
        navigate('/home/cliente');
      } else if (role === 'empresa') {
        navigate('/home/empresa');
      }
    } else {
      setError('Por favor completa todos los campos');
    }
  };

  const loginFields = [
    { name: 'email', type: 'email', label: 'Correo electrónico', required: true },
    { name: 'password', type: 'password', label: 'Contraseña', required: true }
  ];

  return (
    <div className="login-page">
      <Header />
      
      <div className="auth-container">
        <div className="auth-form-container">
          <div className="auth-header">
            <h2>Iniciar Sesión como {role === 'cliente' ? 'Cliente' : 'Empresa'}</h2>
            <p>Ingresa tus credenciales para acceder a tu cuenta</p>
          </div>
          
          {error && <div className="error-message">{error}</div>}
          
          <AuthForm 
            fields={loginFields}
            onSubmit={handleLogin}
            submitText="Iniciar Sesión"
          />
          
          <div className="auth-links">
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
    </div>
  );
};

export default Login;