// src/pages/HomeEmpresa.jsx
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import '../styles/HomeEmpresa.css';
import VehiculosManager from '../components/VehiculosManager';
import { getCurrentUser } from '../services/authService';

const HomeEmpresa = () => {
  const navigate = useNavigate();
  const [user, setUser] = useState(null);
  const [selectedVehiculo, setSelectedVehiculo] = useState(null);
  const [showVehiculos, setShowVehiculos] = useState(false);
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
    <div className="home-empresa-page">
      <Header />
      
      <div className="empresa-container">
        <div className="empresa-header">
          <h1>Panel de Control - Empresa</h1>
          <p>Gestiona tus flotas vehiculares y servicios corporativos</p>
          <div className="user-info">
            <span>Conectado como: {user?.infoEspecifica?.nombre || user?.email || 'Empresa'}</span>
            {user?.nombreTaller && (
              <span className="taller-info"> - {user.nombreTaller}</span>
            )}
          </div>
        </div>
        
        <div className="empresa-dashboard">
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
              Flota de Vehículos
            </button>
          </div>

          {!showVehiculos ? (
            <div className="servicios-section">
              <h2>Servicios Disponibles</h2>

              {!selectedVehiculo && (
                <div className="vehiculo-selection">
                  <p>Selecciona un vehículo de tu flota para solicitar servicios:</p>
                  <button 
                    className="btn-primary"
                    onClick={() => setShowVehiculos(true)}
                  >
                    Gestionar Flota
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

export default HomeEmpresa;