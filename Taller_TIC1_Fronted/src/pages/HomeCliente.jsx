// src/pages/HomeCliente.jsx
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import VehiculosManager from '../components/VehiculosManager';
import '../styles/HomeCliente.css';
import { getCurrentUser } from '../services/authService';

const HomeCliente = () => {
  const navigate = useNavigate();
  const [selectedVehiculo, setSelectedVehiculo] = useState(null);
  const [showVehiculos, setShowVehiculos] = useState(false);
  const [user, setUser] = useState(null);
  const [showDetailsId, setShowDetailsId] = useState(null);

  useEffect(() => {
    // Get current user information
    const currentUser = getCurrentUser();
    setUser(currentUser);
  }, []);

  const serviciosBasicos = [
    { id: 'revision', nombre: 'Revisión', costo: 0, descripcion: 'Inspección general del vehículo para evaluar su estado.' },
    { id: 'reparacion', nombre: 'Reparación', costo: 0, descripcion: 'Reparación de componentes y sistemas del vehículo.' }
  ];

  return (
    <div className="home-cliente-page">
      <Header />
      
      <div className="cliente-container">
        <div className="cliente-header">
          <h1>Bienvenido de vuelta, {user?.infoEspecifica?.nombre || user?.email || 'Cliente'}</h1>
          <p>Gestiona tus vehículos y servicios de manera fácil y rápida</p>
          {user?.nombreTaller && (
            <p className="taller-info">Taller: {user.nombreTaller}</p>
          )}
        </div>
        
        <div className="cliente-dashboard">
          <div className="dashboard-tabs">
            <button 
              className={`tab-button ${!showVehiculos ? 'active' : ''}`}
              onClick={() => setShowVehiculos(false)}
            >
              Servicios
            </button>
            <button 
              className={`tab-button ${showVehiculos ? 'active' : ''}`}
              onClick={() => setShowVehiculos(true)}
            >
              Mis Vehículos
            </button>
          </div>

          {!showVehiculos ? (
            <div className="servicios-section">
              <h2>Servicios Disponibles</h2>
              
              {!selectedVehiculo && (
                <div className="vehiculo-selection">
                  <p>Selecciona un vehículo para solicitar servicios:</p>
                  <button 
                    className="btn-primary"
                    onClick={() => setShowVehiculos(true)}
                  >
                    Gestionar Vehículos
                  </button>
                </div>
              )}

              {selectedVehiculo && (
                <div className="selected-vehiculo">
                  <p><strong>Vehículo seleccionado:</strong> {selectedVehiculo.marca} {selectedVehiculo.modelo} ({selectedVehiculo.placa})</p>
                  <button 
                    className="btn-secondary"
                    onClick={() => setSelectedVehiculo(null)}
                  >
                    Cambiar Vehículo
                  </button>
                </div>
              )}

              <div className="servicios-list">
                {serviciosBasicos.map((s) => (
                  <div key={s.id} className="card">
                    <div className="card-header">
                      <div className="card-title">{s.nombre}</div>
                      <div className="card-subtitle">Desde ${s.costo}</div>
                    </div>
                    <p>{s.descripcion}</p>
                    {showDetailsId === s.id && (
                      <div className="mb-2">
                        <p className="mb-1"><strong>Duración estimada:</strong> 1-3 horas</p>
                        <p className="mb-1"><strong>Incluye:</strong> Mano de obra básica, diagnóstico inicial</p>
                        <p className="mb-0"><strong>Notas:</strong> Puede requerir repuestos adicionales según evaluación</p>
                      </div>
                    )}
                    <div style={{ display: 'flex', gap: '0.75rem' }}>
                      <button className="btn-secondary" onClick={() => setShowDetailsId(showDetailsId === s.id ? null : s.id)}>
                        {showDetailsId === s.id ? 'Ocultar Detalles' : 'Detalles'}
                      </button>
                      <button className="btn-primary" onClick={() => {}}>
                        Reservar
                      </button>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          ) : (
            <VehiculosManager 
              onVehiculoSelect={(vehiculo) => {
                setSelectedVehiculo(vehiculo);
                setShowVehiculos(false);
              }}
            />
          )}
        </div>
      </div>
    </div>
  );
};

export default HomeCliente;