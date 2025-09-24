import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import AuthForm from '../components/AuthForm';
import '../styles/RegisterEmpresa.css';
import { registerCliente } from '../services/authService';

const RegisterEmpresa = () => {
  const navigate = useNavigate();
  const [error, setError] = useState('');

  const handleRegister = async (formData) => {
    if (formData.password !== formData.confirmPassword) {
      setError('Las contraseñas no coinciden');
      return;
    }
    try {
      // Empresas también son clientes tipo empresa en el backend
      await registerCliente({
        email: formData.email,
        password: formData.password,
        idTaller: Number(import.meta.env.VITE_ID_TALLER) || 1,
        idCliente: formData.idEmpresa ? Number(formData.idEmpresa) : null
      });
      navigate('/login/empresa');
    } catch (e) {
      setError(e?.message || 'Error al registrar empresa');
    }
  };

  const registerFields = [
    { name: 'email', type: 'email', label: 'Correo Electrónico', required: true },
    { name: 'password', type: 'password', label: 'Contraseña', required: true },
    { name: 'confirmPassword', type: 'password', label: 'Confirmar Contraseña', required: true },
    { name: 'idEmpresa', type: 'number', label: 'ID Empresa (opcional)', required: false }
  ];

  return (
    <div className="register-empresa-page">
      <Header />
      
      <div className="auth-container">
        <div className="auth-form-container">
          <div className="auth-header">
            <h2>Registro de Empresa</h2>
            <p>Complete la información para registrar su empresa</p>
          </div>
          
          {error && <div className="error-message">{error}</div>}
          
          <AuthForm 
            fields={registerFields}
            onSubmit={handleRegister}
            submitText="Registrar Empresa"
          />
          
          <div className="auth-links">
            <p>¿Ya tienes una cuenta?</p>
            <button 
              className="btn-link"
              onClick={() => navigate('/login/empresa')}
            >
              Iniciar Sesión
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default RegisterEmpresa;