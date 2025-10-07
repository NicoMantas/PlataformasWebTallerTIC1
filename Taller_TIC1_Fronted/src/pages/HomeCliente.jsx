// src/pages/HomeCliente.jsx
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import VehiculosManager from '../components/VehiculosManager';
import { createServicio, listActivosByCliente, listHistorialByCliente, cancelarServicio, descargarReservaPdf } from '../services/serviciosService';
import '../styles/HomeCliente.css';
import { getCurrentUser } from '../services/authService';

const HomeCliente = () => {
  const navigate = useNavigate();
  const [selectedVehiculo, setSelectedVehiculo] = useState(null);
  const [showVehiculos, setShowVehiculos] = useState(false);
  const [user, setUser] = useState(null);
  const [showDetailsId, setShowDetailsId] = useState(null);
  const [activos, setActivos] = useState([]);
  const [historial, setHistorial] = useState([]);
  const [tab, setTab] = useState('servicios'); // servicios | activos | historial

  useEffect(() => {
    // Get current user information
    const currentUser = getCurrentUser();
    setUser(currentUser);
  }, []);

  useEffect(() => {
    if (user?.infoEspecifica?.id) {
      refreshPedidos();
    }
  }, [user]);

  const refreshPedidos = async () => {
    try {
      const clienteId = user?.infoEspecifica?.id || user?.idCliente;
      if (!clienteId) return;
      const [a, h] = await Promise.all([
        listActivosByCliente(clienteId),
        listHistorialByCliente(clienteId)
      ]);
      setActivos(a || []);
      setHistorial(h || []);
    } catch (e) {
      // no-op
    }
  };

  const handleReservar = async (tipo) => {
    if (!selectedVehiculo) {
      alert('Debes seleccionar un vehículo antes de reservar');
      setShowVehiculos(true);
      return;
    }
    const clienteId = user?.infoEspecifica?.id || user?.idCliente;
    try {
      const servicio = await createServicio({
        tipoServicio: tipo,
        idCliente: clienteId,
        idVehiculo: selectedVehiculo.id,
        detallesRevision: tipo === 'revision' ? 'Revisión general solicitada' : undefined
      });
      const blob = await descargarReservaPdf(servicio);
      const url = URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = `reserva_servicio_${servicio.id}.pdf`;
      document.body.appendChild(a);
      a.click();
      a.remove();
      URL.revokeObjectURL(url);
      await refreshPedidos();
      alert('Reserva creada con éxito');
    } catch (e) {
      alert(e?.message || 'Error al reservar');
    }
  };

  const handleCancelar = async (servicioId) => {
    try {
      await cancelarServicio(servicioId);
      await refreshPedidos();
    } catch (e) {
      alert(e?.message || 'Error al cancelar');
    }
  };

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
            <button className={`tab-button ${tab === 'servicios' ? 'active' : ''}`} onClick={() => setTab('servicios')}>Servicios</button>
            <button className={`tab-button ${tab === 'activos' ? 'active' : ''}`} onClick={() => setTab('activos')}>Pedidos Activos</button>
            <button className={`tab-button ${tab === 'historial' ? 'active' : ''}`} onClick={() => setTab('historial')}>Historial</button>
            <button className={`tab-button ${showVehiculos ? 'active' : ''}`} onClick={() => setShowVehiculos(true)}>Mis Vehículos</button>
          </div>

          {tab === 'servicios' && !showVehiculos ? (
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
                      <button className="btn-primary" onClick={() => handleReservar(s.id)}>
                        Reservar
                      </button>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          ) : tab === 'activos' ? (
            <div className="servicios-section">
              <h2>Pedidos Activos</h2>
              {activos.map((s) => (
                <div key={s.id} className="card">
                  <div className="card-header">
                    <div className="card-title">{s.tipoServicio}</div>
                    <div className="card-subtitle">Estado: {s.estadoDescripcion}</div>
                  </div>
                  <div style={{ display: 'flex', gap: '0.75rem' }}>
                    <button className="btn-secondary" onClick={async () => {
                      const blob = await descargarReservaPdf(s);
                      const url = URL.createObjectURL(blob);
                      const a = document.createElement('a');
                      a.href = url;
                      a.download = `reserva_servicio_${s.id}.pdf`;
                      document.body.appendChild(a);
                      a.click();
                      a.remove();
                      URL.revokeObjectURL(url);
                    }}>Descargar PDF</button>
                    <button className="btn-danger" onClick={() => handleCancelar(s.id)}>Cancelar</button>
                  </div>
                </div>
              ))}
            </div>
          ) : tab === 'historial' ? (
            <div className="servicios-section">
              <h2>Historial</h2>
              {historial.map((s) => (
                <div key={s.id} className="card">
                  <div className="card-header">
                    <div className="card-title">{s.tipoServicio}</div>
                    <div className="card-subtitle">Estado: {s.estadoDescripcion}</div>
                  </div>
                  <div style={{ display: 'flex', gap: '0.75rem' }}>
                    <button className="btn-secondary" onClick={async () => {
                      const blob = await descargarReservaPdf(s);
                      const url = URL.createObjectURL(blob);
                      const a = document.createElement('a');
                      a.href = url;
                      a.download = `reserva_servicio_${s.id}.pdf`;
                      document.body.appendChild(a);
                      a.click();
                      a.remove();
                      URL.revokeObjectURL(url);
                    }}>Descargar PDF</button>
                  </div>
                </div>
              ))}
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