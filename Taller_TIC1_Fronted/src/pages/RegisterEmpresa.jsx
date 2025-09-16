import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import AuthForm from '../components/AuthForm';
import '../styles/RegisterEmpresa.css';

const RegisterEmpresa = () => {
  const navigate = useNavigate();
  const [error, setError] = useState('');

  const handleRegister = (formData) => {
    // Validación básica
    if (formData.password !== formData.confirmPassword) {
      setError('Las contraseñas no coinciden');
      return;
    }
    
    // Simulación de registro exitoso
    console.log('Empresa registrada:', formData);
    navigate('/login/empresa');
  };

  const registerFields = [
    { name: 'companyName', type: 'text', label: 'Nombre de la Empresa', required: true },
    { name: 'rut', type: 'text', label: 'RUT', required: true },
    { name: 'contactName', type: 'text', label: 'Nombre del Contacto', required: true },
    { name: 'email', type: 'email', label: 'Correo Electrónico', required: true },
    { name: 'phone', type: 'tel', label: 'Teléfono', required: true },
    { name: 'address', type: 'text', label: 'Dirección', required: true },
    { name: 'password', type: 'password', label: 'Contraseña', required: true },
    { name: 'confirmPassword', type: 'password', label: 'Confirmar Contraseña', required: true }
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