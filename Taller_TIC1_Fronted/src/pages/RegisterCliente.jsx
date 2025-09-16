import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import AuthForm from '../components/AuthForm';
import '../styles/RegisterCliente.css';

const RegisterCliente = () => {
  const navigate = useNavigate();
  const [error, setError] = useState('');

  const handleRegister = (formData) => {
    // Validación básica
    if (formData.password !== formData.confirmPassword) {
      setError('Las contraseñas no coinciden');
      return;
    }
    
    // Simulación de registro exitoso
    console.log('Cliente registrado:', formData);
    navigate('/login/cliente');
  };

  const registerFields = [
    { name: 'firstName', type: 'text', label: 'Nombre', required: true },
    { name: 'lastName', type: 'text', label: 'Apellido', required: true },
    { name: 'email', type: 'email', label: 'Correo Electrónico', required: true },
    { name: 'phone', type: 'tel', label: 'Teléfono', required: true },
    { name: 'address', type: 'text', label: 'Dirección', required: true },
    { name: 'password', type: 'password', label: 'Contraseña', required: true },
    { name: 'confirmPassword', type: 'password', label: 'Confirmar Contraseña', required: true }
  ];

  return (
    <div className="register-cliente-page">
      <Header />
      
      <div className="auth-container">
        <div className="auth-form-container">
          <div className="auth-header">
            <h2>Registro de Cliente</h2>
            <p>Complete la información para crear su cuenta</p>
          </div>
          
          {error && <div className="error-message">{error}</div>}
          
          <AuthForm 
            fields={registerFields}
            onSubmit={handleRegister}
            submitText="Registrarse"
          />
          
          <div className="auth-links">
            <p>¿Ya tienes una cuenta?</p>
            <button 
              className="btn-link"
              onClick={() => navigate('/login/cliente')}
            >
              Iniciar Sesión
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default RegisterCliente;