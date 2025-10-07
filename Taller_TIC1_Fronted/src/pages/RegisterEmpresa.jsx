import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import AuthForm from '../components/AuthForm';
import '../styles/RegisterEmpresa.css';
import { registerCliente } from '../services/authService';
import { createCliente } from '../services/clientesService';
import { createVehiculo, updateVehiculo } from '../services/vehiculosService';

const RegisterEmpresa = () => {
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

      // Paso 2: Crear el cliente empresa
      const clienteData = {
        nombre: formData.nombreEmpresa,
        email: formData.email,
        telefono: parseInt(formData.telefono),
        idVehiculo: vehiculoId || 1, // Usar ID por defecto si no se creó vehículo
        tipo: 'Empresa',
        nit: parseInt(formData.nit),
        representanteLegal: formData.representanteLegal
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

      // Redirigir al Home de la empresa
      navigate('/home/empresa');
    } catch (e) {
      setError(e?.message || 'Error al registrar empresa');
    } finally {
      setLoading(false);
    }
  };

  const registerFields = [
    // Información de la empresa
    { name: 'nombreEmpresa', type: 'text', label: 'Nombre de la Empresa', required: true },
    { name: 'nit', type: 'number', label: 'NIT', required: true },
    { name: 'representanteLegal', type: 'text', label: 'Representante Legal', required: true },
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
            submitText={loading ? "Registrando..." : "Registrar Empresa"}
            disabled={loading}
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