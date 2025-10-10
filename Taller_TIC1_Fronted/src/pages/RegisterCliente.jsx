import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import AuthForm from '../components/AuthForm';
import VehiculoFormSection from '../components/VehiculoFormSection';
import '../styles/RegisterCliente.css';
import { registerCliente } from '../services/authService';
import { createCliente } from '../services/clientesService';
import { createVehiculo, updateVehiculo } from '../services/vehiculosService';

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
      // Paso 1: Crear el vehículo primero (sin idCliente)
      let vehiculoId = null;
      if (formData.placa && formData.marca && formData.modelo && formData.anio) {
        const vehiculoData = {
          Placa: formData.placa,
          Marca: formData.marca,
          Modelo: formData.modelo,
          Anio: parseInt(formData.anio),
          Tipo: formData.tipoVehiculo || 'Gasolina'
          // No incluir IdCliente - será null por defecto
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

      // Paso 3: Actualizar el vehículo con el idCliente si se creó uno
      if (vehiculoId) {
        await updateVehiculo(vehiculoId, {
          Placa: formData.placa,
          Marca: formData.marca,
          Modelo: formData.modelo,
          Anio: parseInt(formData.anio),
          IdCliente: clienteResponse.id
        });
      }

      // Paso 4: Registrar en el sistema de autenticación
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
    
    // Credenciales
    { name: 'password', type: 'password', label: 'Contraseña', required: true },
    { name: 'confirmPassword', type: 'password', label: 'Confirmar Contraseña', required: true }
  ];

  // Sección personalizada para el vehículo (se inserta después del campo email - índice 4)
  const customSections = [
    {
      afterFieldIndex: 4, // Después del campo email
      component: <VehiculoFormSection />
    }
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
            customSections={customSections}
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