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
      console.log('🚀 [RegisterCliente] Iniciando registro con datos:', formData);
      
      // Paso 1: Crear el vehículo primero (sin idCliente)
      if (!formData.placa || !formData.marca || !formData.modelo || !formData.anio) {
        setError('Todos los campos del vehículo son obligatorios');
        return;
      }

      const vehiculoData = {
        Placa: formData.placa,
        Marca: formData.marca,
        Modelo: formData.modelo,
        Anio: parseInt(formData.anio),
        Tipo: formData.tipoVehiculo || 'Gasolina',
        Cilindraje: formData.tipoVehiculo === 'Gasolina' ? parseInt(formData.cilindraje) || null : null,
        CapacidadBateria: formData.tipoVehiculo === 'Electrico' ? parseInt(formData.capacidadBateria) || null : null
        // No incluir IdCliente - será null por defecto
      };
      
      console.log('🚗 [RegisterCliente] Creando vehículo con datos:', vehiculoData);
      const vehiculoResponse = await createVehiculo(vehiculoData);
      console.log('✅ [RegisterCliente] Vehículo creado exitosamente:', vehiculoResponse);
      const vehiculoId = vehiculoResponse.id;

      // Paso 2: Crear el cliente natural
      const clienteData = {
        Nombre: formData.nombre,
        Email: formData.email,
        Telefono: parseInt(formData.telefono),
        IdVehiculo: vehiculoId,
        Tipo: 'Natural',
        Cedula: parseInt(formData.cedula),
        Apellido: formData.apellido
      };
      
      console.log('👤 [RegisterCliente] Creando cliente con datos:', clienteData);
      const clienteResponse = await createCliente(clienteData);
      console.log('✅ [RegisterCliente] Cliente creado exitosamente:', clienteResponse);

      // Verificar que el cliente se creó correctamente
      if (!clienteResponse || !clienteResponse.id) {
        console.error('❌ [RegisterCliente] Error: Cliente no se creó correctamente:', clienteResponse);
        throw new Error('Error al crear el cliente');
      }

      // Pequeño delay para asegurar que la transacción se complete
      console.log('⏳ [RegisterCliente] Esperando 200ms para completar transacción...');
      await new Promise(resolve => setTimeout(resolve, 200));

      // Paso 3: Actualizar el vehículo con el idCliente y subtipo
      const vehiculoUpdateData = {
        Placa: formData.placa,
        Marca: formData.marca,
        Modelo: formData.modelo,
        Anio: parseInt(formData.anio),
        IdCliente: clienteResponse.id,
        Tipo: formData.tipoVehiculo || 'Gasolina',
        Cilindraje: formData.tipoVehiculo === 'Gasolina' ? parseInt(formData.cilindraje) || null : null,
        CapacidadBateria: formData.tipoVehiculo === 'Electrico' ? parseInt(formData.capacidadBateria) || null : null
      };
      
      console.log('🔄 [RegisterCliente] Actualizando vehículo con datos:', vehiculoUpdateData);
      await updateVehiculo(vehiculoId, vehiculoUpdateData);
      console.log('✅ [RegisterCliente] Vehículo actualizado exitosamente');

      // Paso 4: Registrar en el sistema de autenticación
      const authData = {
        email: formData.email,
        password: formData.password,
        idTaller: Number(import.meta.env.VITE_ID_TALLER) || 1,
        idCliente: clienteResponse.id
      };
      
      console.log('🔐 [RegisterCliente] Registrando usuario con datos:', authData);
      await registerCliente(authData);
      console.log('✅ [RegisterCliente] Usuario registrado exitosamente');

      // Redirigir al Home del cliente
      console.log('🏠 [RegisterCliente] Redirigiendo al home del cliente');
      navigate('/home/cliente');
    } catch (e) {
      console.error('❌ [RegisterCliente] Error completo:', e);
      console.error('❌ [RegisterCliente] Error message:', e?.message);
      console.error('❌ [RegisterCliente] Error stack:', e?.stack);
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