import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import AuthForm from '../components/AuthForm';
import '../styles/RegisterCliente.css';
import { registerCliente } from '../services/authService';

const RegisterCliente = () => {
  const navigate = useNavigate();
  const [error, setError] = useState('');

  const handleRegister = async (formData) => {
    if (formData.password !== formData.confirmPassword) {
      setError('Las contraseñas no coinciden');
      return;
    }
    try {
      // En este flujo asumimos que el Cliente ya existe (IdCliente) y el IdTaller es conocido
      // Como mínimo requerimos email, password e idTaller; idCliente puede ser null si backend lo permite
      await registerCliente({
        email: formData.email,
        password: formData.password,
        idTaller: Number(import.meta.env.VITE_ID_TALLER) || 1,
        idCliente: formData.idCliente ? Number(formData.idCliente) : null
      });
      navigate('/login/cliente');
    } catch (e) {
      setError(e?.message || 'Error al registrar cliente');
    }
  };

  const registerFields = [
    { name: 'email', type: 'email', label: 'Correo Electrónico', required: true },
    { name: 'password', type: 'password', label: 'Contraseña', required: true },
    { name: 'confirmPassword', type: 'password', label: 'Confirmar Contraseña', required: true },
    { name: 'idCliente', type: 'number', label: 'ID Cliente (opcional)', required: false }
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