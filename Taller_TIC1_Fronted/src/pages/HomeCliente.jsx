// src/pages/HomeCliente.jsx
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import VehiculosManager from '../components/VehiculosManager';
import '../styles/HomeCliente.css';
import api from '../services/api';
import { listServicios } from '../services/serviciosService';
import { createOrden } from '../services/ordenesService';
import { getCurrentUser } from '../services/authService';

const HomeCliente = () => {
  const navigate = useNavigate();
  const [servicios, setServicios] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [selectedVehiculo, setSelectedVehiculo] = useState(null);
  const [showVehiculos, setShowVehiculos] = useState(false);
  const [user, setUser] = useState(null);

  useEffect(() => {
    // Get current user information
    const currentUser = getCurrentUser();
    setUser(currentUser);

    (async () => {
      try {
        const data = await listServicios(api);
        setServicios(data || []);
      } catch (e) {
        setError(e?.message || 'No se pudieron cargar los servicios');
      } finally {
        setLoading(false);
      }
    })();
  }, []);

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
              {loading && <p>Cargando servicios...</p>}
              {error && <div className="error-message">{error}</div>}
              
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
                {servicios.map((s) => (
                  <div key={s.id} className="servicio-item">
                    <div className="servicio-info">
                      <h3>{s.nombre}</h3>
                      <p>{s.descripcion}</p>
                      <p><strong>Costo:</strong> ${s.costo}</p>
                    </div>
                    <button
                      className="btn-primary"
                      disabled={!selectedVehiculo}
                      onClick={async () => {
                        if (!selectedVehiculo) {
                          alert('Por favor selecciona un vehículo primero');
                          return;
                        }
                        try {
                          await createOrden({
                            clienteId: 1, // TODO: obtener del usuario autenticado
                            vehiculoId: selectedVehiculo.id,
                            serviciosIds: [s.id],
                            descripcion: `Solicitud de ${s.nombre}`
                          });
                          alert('Solicitud enviada correctamente');
                        } catch (e) {
                          alert(e?.message || 'No se pudo solicitar');
                        }
                      }}
                    >
                      Solicitar
                    </button>
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