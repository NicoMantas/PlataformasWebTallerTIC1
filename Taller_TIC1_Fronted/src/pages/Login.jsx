import React, { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import Header from '../components/Header';
import AuthForm from '../components/AuthForm';
import '../styles/Login.css';

const Login = () => {
  const navigate = useNavigate();
  const { role } = useParams();
  const [error, setError] = useState('');

  // Simulación de login que determina el tipo de empleado
  const handleLogin = async (formData) => {
    try {
      // En una app real, esto sería una llamada a la API
      if (formData.email && formData.password) {
        // Simulamos la respuesta del backend con el tipo de empleado
        let userType = '';
        
        // Lógica de ejemplo para determinar el tipo de empleado
        if (formData.email.includes('mecanico')) {
          userType = 'mecanico';
        } else if (formData.email.includes('secretaria')) {
          userType = 'secretaria';
        } else if (formData.email.includes('admin')) {
          userType = 'administrador';
        } else {
          // Por defecto o según lógica de negocio
          userType = 'mecanico';
        }
        
        // Guardar en localStorage o context
        localStorage.setItem('userRole', role);
        localStorage.setItem('employeeType', userType);
        localStorage.setItem('userEmail', formData.email);
        
        // Redirigir según el tipo de usuario
        if (role === 'cliente') {
          navigate('/home/cliente');
        } else if (role === 'empresa') {
          navigate('/home/empresa');
        } else if (role === 'empleado') {
          navigate(`/home/empleado/${userType}`);
        }
      } else {
        setError('Por favor completa todos los campos');
      }
    } catch (err) {
      setError('Error al iniciar sesión. Por favor intenta nuevamente.');
    }
  };

  const loginFields = [
    { name: 'email', type: 'email', label: 'Correo electrónico', required: true },
    { name: 'password', type: 'password', label: 'Contraseña', required: true }
  ];

  const getRoleTitle = () => {
    switch(role) {
      case 'cliente': return 'Cliente';
      case 'empresa': return 'Empresa';
      case 'empleado': return 'Empleado';
      default: return 'Usuario';
    }
  };

  return (
    <div className="login-page">
      <Header />
      
      <div className="auth-container">
        <div className="auth-form-container">
          <div className="auth-header">
            <h2>Iniciar Sesión como {getRoleTitle()}</h2>
            <p>Ingresa tus credenciales para acceder al sistema</p>
            {role === 'empleado' && (
              <p className="login-hint">
                Ejemplo: mecanico@taller.com, secretaria@taller.com, admin@taller.com
              </p>
            )}
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