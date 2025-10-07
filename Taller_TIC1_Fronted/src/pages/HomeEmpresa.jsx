// src/pages/HomeEmpresa.jsx
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import '../styles/HomeEmpresa.css';
import VehiculosManager from '../components/VehiculosManager';
import { getCurrentUser } from '../services/authService';
import { createServicio, listActivosByCliente, listHistorialByCliente, cancelarServicio, descargarReservaPdf } from '../services/serviciosService';

const HomeEmpresa = () => {
  const navigate = useNavigate();
  const [user, setUser] = useState(null);
  const [selectedVehiculo, setSelectedVehiculo] = useState(null);
  const [showVehiculos, setShowVehiculos] = useState(false);
  const [showDetailsId, setShowDetailsId] = useState(null);
  const [activos, setActivos] = useState([]);
  const [historial, setHistorial] = useState([]);
  const [tab, setTab] = useState('servicios');

  useEffect(() => {
    // Get current user information
    const currentUser = getCurrentUser();
    setUser(currentUser);
  }, []);

  useEffect(() => {
    if (user?.infoEspecifica?.id) refreshPedidos();
  }, [user]);

  const refreshPedidos = async () => {
    const clienteId = user?.infoEspecifica?.id || user?.idCliente;
    if (!clienteId) return;
    const [a, h] = await Promise.all([
      listActivosByCliente(clienteId),
      listHistorialByCliente(clienteId)
    ]);
    const activosList = (a || []);
    const historialList = (h || []).filter(x => (x.estadoDescripcion || '').toLowerCase() === 'completado');
    setActivos(activosList);
    setHistorial(historialList);
  };

  const handleReservar = async (tipo) => {
    if (!selectedVehiculo) {
      alert('Debes seleccionar un vehículo antes de reservar');
      setShowVehiculos(true);
      return;
    }
    const clienteId = user?.infoEspecifica?.id || user?.idCliente;
    const servicio = await createServicio({
      tipoServicio: tipo,
      idCliente: clienteId,
      idVehiculo: selectedVehiculo.id,
      detallesRevision: tipo === 'revision' ? 'Revisión corporativa' : undefined
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
  };

  const handleCancelar = async (id) => {
    if (!window.confirm('¿Deseas cancelar este servicio?')) return;
    await cancelarServicio(id);
    await refreshPedidos();
  };

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
            <button className={`tab-button ${tab === 'servicios' ? 'active' : ''}`} onClick={() => setTab('servicios')}>Servicios</button>
            <button className={`tab-button ${tab === 'activos' ? 'active' : ''}`} onClick={() => setTab('activos')}>Pedidos Activos</button>
            <button className={`tab-button ${tab === 'historial' ? 'active' : ''}`} onClick={() => setTab('historial')}>Historial</button>
            <button className={`tab-button ${showVehiculos ? 'active' : ''}`} onClick={() => setShowVehiculos(true)}>Flota de Vehículos</button>
          </div>

          {tab === 'servicios' && !showVehiculos ? (
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
              {activos.map((s) => {
                const tipoRaw = s.tipoServicio || (s.detallesRevision ? 'Revision' : 'Reparacion');
                const tipoPretty = tipoRaw === 'Revision' ? 'Revisión' : 'Reparación';
                return (
                <div key={s.id} className="card">
                  <div className="card-header">
                    <div className="card-title">{tipoPretty}</div>
                    <div className="card-subtitle">Estado: {s.estadoDescripcion}</div>
                  </div>
                  <div className="servicio-info">
                    <p><strong>Empresa:</strong> {user?.infoEspecifica?.nombre || 'N/D'}</p>
                    {(tipoRaw === 'Revision') && s.detallesRevision && (
                      <p><strong>Detalles:</strong> {s.detallesRevision}</p>
                    )}
                  </div>
                  <div style={{ display: 'flex', gap: '0.75rem', marginTop: '8px' }}>
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
              );})}
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

export default HomeEmpresa;