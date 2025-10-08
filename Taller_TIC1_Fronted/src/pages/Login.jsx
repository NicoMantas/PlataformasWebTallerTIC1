import React, { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import Header from '../components/Header';
import AuthForm from '../components/AuthForm';
import '../styles/Login.css';
import { login as loginApi } from '../services/authService';

const Login = () => {
  const navigate = useNavigate();
  const { role } = useParams();
  const [error, setError] = useState('');

  const handleLogin = async (formData) => {
    try {
      if (!formData.email || !formData.password) {
        setError('Por favor completa todos los campos');
        return;
      }

      const res = await loginApi({ email: formData.email, password: formData.password });

      const user = res?.user;
      if (!user) throw new Error('Respuesta inválida del servidor');

      // Store user information in localStorage for use across the app
      localStorage.setItem('user', JSON.stringify(user));
      localStorage.setItem('userEmail', user.email);
      localStorage.setItem('userType', user.tipoUsuario);
      localStorage.setItem('tallerId', user.idTaller);
      localStorage.setItem('tallerNombre', user.nombreTaller);

      // Decide destino por tipo de usuario y tipo empleado si aplica
      if (user.tipoUsuario === 'cliente') {
        navigate('/home/cliente');
      } else if (user.tipoUsuario === 'empleado') {
        // Get employee type from the specific info returned by the backend
        const info = user.infoEspecifica || {};
        const tipo = info.tipoEmpleado || 'mecanico'; // Default to mecanico if not specified
        
        // Map employee types to route parameters
        let routeType = 'mecanico';
        if (tipo.toLowerCase().includes('secretaria') || tipo.toLowerCase().includes('secretary')) {
          routeType = 'secretaria';
        } else if (tipo.toLowerCase().includes('admin') || tipo.toLowerCase().includes('administrador')) {
          routeType = 'administrador';
        }
        
        navigate(`/home/empleado/${routeType}`);
      } else {
        // Empresas pueden estar modeladas como clientes empresa; enviar a home empresa
        navigate('/home/empresa');
      }
    } catch (err) {
      setError(err?.message || 'Error al iniciar sesión. Por favor intenta nuevamente.');
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