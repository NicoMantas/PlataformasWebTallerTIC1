import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import AuthForm from '../components/AuthForm';
import '../styles/RegisterCliente.css';
import { registerCliente } from '../services/authService';
import { createCliente } from '../services/clientesService';
import { createVehiculo } from '../services/vehiculosService';

const RegisterCliente = () => {
  const navigate = useNavigate();
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleRegister = async (formData) => {
    if (formData.password !== formData.confirmPassword) {
      setError('Las contraseñas no coinciden');
      return;
    }

    setLoading(true);
    setError('');

    try {
      // Paso 1: Crear el vehículo primero
      let vehiculoId = null;
      if (formData.placa && formData.marca && formData.modelo && formData.anio) {
        const vehiculoData = {
          placa: formData.placa,
          marca: formData.marca,
          modelo: formData.modelo,
          anio: parseInt(formData.anio),
          tipo: formData.tipoVehiculo || 'Gasolina'
        };
        const vehiculoResponse = await createVehiculo(vehiculoData);
        vehiculoId = vehiculoResponse.id;
      }

      // Paso 2: Crear el cliente natural
      const clienteData = {
        nombre: formData.nombre,
        email: formData.email,
        telefono: parseInt(formData.telefono),
        idVehiculo: vehiculoId || 1, // Usar ID por defecto si no se creó vehículo
        tipo: 'Natural',
        cedula: parseInt(formData.cedula),
        apellido: formData.apellido
      };
      const clienteResponse = await createCliente(clienteData);

      // Paso 3: Registrar en el sistema de autenticación
      await registerCliente({
        email: formData.email,
        password: formData.password,
        idTaller: Number(import.meta.env.VITE_ID_TALLER) || 1,
        idCliente: clienteResponse.id
      });

      // Redirigir al Home del cliente
      navigate('/home/cliente');
    } catch (e) {
      setError(e?.message || 'Error al registrar cliente');
    } finally {
      setLoading(false);
    }
  };

  const registerFields = [
    // Información personal
    { name: 'nombre', type: 'text', label: 'Nombre', required: true },
    { name: 'apellido', type: 'text', label: 'Apellido', required: true },
    { name: 'cedula', type: 'number', label: 'Cédula', required: true },
    { name: 'telefono', type: 'tel', label: 'Teléfono', required: true },
    { name: 'email', type: 'email', label: 'Correo Electrónico', required: true },
    
    // Información del vehículo (opcional)
    { name: 'placa', type: 'text', label: 'Placa del Vehículo', required: false },
    { name: 'marca', type: 'text', label: 'Marca del Vehículo', required: false },
    { name: 'modelo', type: 'text', label: 'Modelo del Vehículo', required: false },
    { name: 'anio', type: 'number', label: 'Año del Vehículo', required: false },
    { name: 'tipoVehiculo', type: 'select', label: 'Tipo de Vehículo', required: false, options: [
      { value: 'Gasolina', label: 'Gasolina' },
      { value: 'Electrico', label: 'Eléctrico' },
      { value: 'Hibrido', label: 'Híbrido' }
    ]},
    
    // Credenciales
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
            submitText={loading ? "Registrando..." : "Registrarse"}
            disabled={loading}
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