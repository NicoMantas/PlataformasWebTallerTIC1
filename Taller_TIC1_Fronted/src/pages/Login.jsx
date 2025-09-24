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

      // Decide destino por tipo de usuario y tipo empleado si aplica
      if (user.tipoUsuario === 'cliente') {
        navigate('/home/cliente');
      } else if (user.tipoUsuario === 'empleado') {
        // employee specific info may contain role, fallback by email hint
        const info = user.infoEspecifica || {};
        const tipo = info.tipoEmpleado ||
          (formData.email.includes('mecanico') ? 'mecanico' :
          formData.email.includes('secretaria') ? 'secretaria' : 'administrador');
        navigate(`/home/empleado/${tipo}`);
      } else {
        // Empresas pueden estar modeladas como clientes empresa; enviar a home empresa si title contiene taller
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