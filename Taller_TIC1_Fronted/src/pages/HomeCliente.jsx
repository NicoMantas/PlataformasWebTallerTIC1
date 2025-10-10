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
      const activosList = (a || []);
      const historialList = (h || []).filter(x => (x.estadoDescripcion || '').toLowerCase() === 'completado');
      setActivos(activosList);
      setHistorial(historialList);
    } catch (e) {
      // no-op
    }
  };

  const handleReservar = async (servicioId) => {
    if (!selectedVehiculo) {
      alert('Debes seleccionar un vehículo antes de reservar');
      setShowVehiculos(true);
      return;
    }
    
    const servicio = serviciosBasicos.find(s => s.id === servicioId);
    if (!servicio) return;
    
    const clienteId = user?.infoEspecifica?.id || user?.idCliente;
    
    // Determinar si es revisión o reparación basado en el ID del servicio
    const esRevision = servicioId.startsWith('revision');
    const tipoServicio = esRevision ? 'revision' : 'reparacion';
    
    try {
      const servicioData = await createServicio({
        tipoServicio: tipoServicio,
        idCliente: clienteId,
        idVehiculo: selectedVehiculo.id,
        detallesRevision: esRevision ? `${servicio.nombre}: ${servicio.descripcion}` : undefined
      });
      
      const blob = await descargarReservaPdf(servicioData);
      const url = URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = `reserva_servicio_${servicioData.id}.pdf`;
      document.body.appendChild(a);
      a.click();
      a.remove();
      URL.revokeObjectURL(url);
      await refreshPedidos();
      alert(`Reserva para ${servicio.nombre} creada con éxito`);
    } catch (e) {
      alert(e?.message || 'Error al reservar');
    }
  };

  const handleCancelar = async (servicioId) => {
    if (!window.confirm('¿Deseas cancelar este servicio?')) return;
    try {
      await cancelarServicio(servicioId);
      await refreshPedidos();
    } catch (e) {
      alert(e?.message || 'Error al cancelar');
    }
  };

  const serviciosBasicos = [
    // Servicios de Revisión
    {
      id: 'revision-general',
      nombre: 'Revisión General',
      costo: 45000,
      descripcion: 'Inspección completa del vehículo para evaluar su estado general.',
      duracion: '2-3 horas',
      incluye: [
        'Revisión de motor y sistemas básicos',
        'Verificación de frenos y suspensión',
        'Inspección de luces y sistemas eléctricos',
        'Revisión de neumáticos y llantas',
        'Verificación de fluidos básicos',
        'Diagnóstico computarizado básico'
      ],
      notas: 'Servicio preventivo recomendado cada 6 meses'
    },
    {
      id: 'revision-preventiva',
      nombre: 'Revisión Preventiva',
      costo: 65000,
      descripcion: 'Mantenimiento preventivo completo del vehículo.',
      duracion: '3-4 horas',
      incluye: [
        'Cambio de aceite y filtros',
        'Revisión completa del motor',
        'Verificación de sistema de frenos',
        'Inspección de suspensión y dirección',
        'Revisión de sistema eléctrico completo',
        'Verificación de aire acondicionado',
        'Diagnóstico computarizado avanzado',
        'Limpieza de sistemas de combustión'
      ],
      notas: 'Incluye cambio de aceite estándar'
    },
    {
      id: 'revision-tecnomecanica',
      nombre: 'Revisión Técnico-Mecánica',
      costo: 85000,
      descripcion: 'Revisión completa para certificación técnico-mecánica.',
      duracion: '4-5 horas',
      incluye: [
        'Diagnóstico completo de emisiones',
        'Revisión de sistemas de seguridad',
        'Verificación de frenos y ABS',
        'Inspección de dirección y suspensión',
        'Revisión de sistema eléctrico y luces',
        'Verificación de neumáticos y alineación',
        'Diagnóstico computarizado completo',
        'Certificado oficial de revisión'
      ],
      notas: 'Certificado válido por 2 años'
    },

    // Servicios de Reparación
    {
      id: 'reparacion-motor',
      nombre: 'Reparación de Motor',
      costo: 120000,
      descripcion: 'Diagnóstico y reparación de problemas del motor.',
      duracion: '4-8 horas',
      incluye: [
        'Diagnóstico computarizado del motor',
        'Revisión de sistema de combustión',
        'Verificación de inyectores y bujías',
        'Revisión de sistema de refrigeración',
        'Verificación de correas y tensores',
        'Limpieza de sistemas de escape'
      ],
      notas: 'Costo base sin incluir repuestos necesarios'
    },
    {
      id: 'reparacion-frenos',
      nombre: 'Reparación de Frenos',
      costo: 80000,
      descripcion: 'Mantenimiento y reparación del sistema de frenos.',
      duracion: '2-4 horas',
      incluye: [
        'Revisión completa del sistema de frenos',
        'Cambio de pastillas de freno (si es necesario)',
        'Verificación de discos y tambores',
        'Revisión de líquido de frenos',
        'Verificación de sistema ABS',
        'Prueba de funcionamiento'
      ],
      notas: 'Incluye mano de obra, repuestos por separado'
    },
    {
      id: 'reparacion-suspension',
      nombre: 'Reparación de Suspensión',
      costo: 95000,
      descripcion: 'Diagnóstico y reparación del sistema de suspensión.',
      duracion: '3-5 horas',
      incluye: [
        'Diagnóstico de sistema de suspensión',
        'Revisión de amortiguadores',
        'Verificación de resortes y estabilizadores',
        'Revisión de rótulas y bujes',
        'Verificación de geometría',
        'Alineación básica incluida'
      ],
      notas: 'Incluye alineación si no requiere repuestos mayores'
    },
    {
      id: 'reparacion-electrica',
      nombre: 'Reparación Eléctrica',
      costo: 70000,
      descripcion: 'Diagnóstico y reparación de sistemas eléctricos.',
      duracion: '2-4 horas',
      incluye: [
        'Diagnóstico de sistemas eléctricos',
        'Revisión de alternador y batería',
        'Verificación de sistema de carga',
        'Revisión de luces y señalización',
        'Verificación de fusibles y relés',
        'Prueba de sistemas computarizados'
      ],
      notas: 'Diagnóstico completo incluido'
    },
    {
      id: 'reparacion-climatizacion',
      nombre: 'Reparación de Aire Acondicionado',
      costo: 90000,
      descripcion: 'Mantenimiento y reparación del sistema de climatización.',
      duracion: '2-3 horas',
      incluye: [
        'Diagnóstico del sistema de A/C',
        'Verificación de compresor y condensador',
        'Revisión de filtros y conductos',
        'Recarga de gas refrigerante',
        'Verificación de controles y sensores',
        'Limpieza del sistema'
      ],
      notas: 'Incluye recarga de gas estándar'
    }
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
                {/* Sección de Revisiones */}
                <div className="servicios-categoria">
                  <h3>🔍 Servicios de Revisión</h3>
                  {serviciosBasicos.filter(s => s.id.startsWith('revision')).map((s) => (
                    <div key={s.id} className="card servicio-card">
                      <div className="card-header">
                        <div className="card-title">{s.nombre}</div>
                        <div className="card-subtitle">${s.costo.toLocaleString('es-CO')}</div>
                      </div>
                      <p className="servicio-descripcion">{s.descripcion}</p>
                      
                      {showDetailsId === s.id && (
                        <div className="servicio-detalles">
                          <div className="detalle-item">
                            <strong>⏱️ Duración estimada:</strong> {s.duracion}
                          </div>
                          <div className="detalle-item">
                            <strong>✅ Incluye:</strong>
                            <ul className="incluye-lista">
                              {s.incluye.map((item, index) => (
                                <li key={index}>{item}</li>
                              ))}
                            </ul>
                          </div>
                          <div className="detalle-item">
                            <strong>📝 Notas:</strong> {s.notas}
                          </div>
                        </div>
                      )}
                      
                      <div className="servicio-actions">
                        <button 
                          className="btn-secondary" 
                          onClick={() => setShowDetailsId(showDetailsId === s.id ? null : s.id)}
                        >
                          {showDetailsId === s.id ? 'Ocultar Detalles' : 'Ver Detalles'}
                        </button>
                        <button 
                          className="btn-primary" 
                          onClick={() => handleReservar(s.id)}
                        >
                          Reservar - ${s.costo.toLocaleString('es-CO')}
                        </button>
                      </div>
                    </div>
                  ))}
                </div>

                {/* Sección de Reparaciones */}
                <div className="servicios-categoria">
                  <h3>🔧 Servicios de Reparación</h3>
                  {serviciosBasicos.filter(s => s.id.startsWith('reparacion')).map((s) => (
                    <div key={s.id} className="card servicio-card">
                      <div className="card-header">
                        <div className="card-title">{s.nombre}</div>
                        <div className="card-subtitle">${s.costo.toLocaleString('es-CO')}</div>
                      </div>
                      <p className="servicio-descripcion">{s.descripcion}</p>
                      
                      {showDetailsId === s.id && (
                        <div className="servicio-detalles">
                          <div className="detalle-item">
                            <strong>⏱️ Duración estimada:</strong> {s.duracion}
                          </div>
                          <div className="detalle-item">
                            <strong>✅ Incluye:</strong>
                            <ul className="incluye-lista">
                              {s.incluye.map((item, index) => (
                                <li key={index}>{item}</li>
                              ))}
                            </ul>
                          </div>
                          <div className="detalle-item">
                            <strong>📝 Notas:</strong> {s.notas}
                          </div>
                        </div>
                      )}
                      
                      <div className="servicio-actions">
                        <button 
                          className="btn-secondary" 
                          onClick={() => setShowDetailsId(showDetailsId === s.id ? null : s.id)}
                        >
                          {showDetailsId === s.id ? 'Ocultar Detalles' : 'Ver Detalles'}
                        </button>
                        <button 
                          className="btn-primary" 
                          onClick={() => handleReservar(s.id)}
                        >
                          Reservar - ${s.costo.toLocaleString('es-CO')}
                        </button>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            </div>
          ) : tab === 'activos' ? (
            <div className="servicios-section">
              <h2>Pedidos Activos</h2>
              {activos.map((s) => {
                const tipoRaw = s.tipoServicio || (s.detallesRevision ? 'Revision' : 'Reparacion');
                const tipoPretty = tipoRaw === 'Revision' ? 'Revisión' : 'Reparación';
                const fechaCreacion = s.fechaCreacion ? new Date(s.fechaCreacion).toLocaleDateString() : 'N/D';
                const vehiculoInfo = s.vehiculoInfo || 'N/D';
                
                return (
                <div key={s.id} className="card">
                  <div className="card-header">
                    <div className="card-title">Servicio #{s.id} · {tipoPretty}</div>
                    <div className="card-subtitle">Estado: {s.estadoDescripcion}</div>
                  </div>
                  <div className="servicio-info">
                    <p><strong>Cliente:</strong> {s.clienteNombre || 'N/D'}</p>
                    <p><strong>Vehículo:</strong> {vehiculoInfo}</p>
                    {(tipoRaw === 'Revision') && s.detallesRevision && (
                      <p><strong>Detalles:</strong> {s.detallesRevision}</p>
                    )}
                    <p><strong>Fecha Creación:</strong> {fechaCreacion}</p>
                    {s.empleadoNombre && (
                      <p><strong>Mecánico Asignado:</strong> {s.empleadoNombre}</p>
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
              {historial.map((s) => {
                const tipoPretty = s.tipoServicio === 'Revision' ? 'Revisión' : 'Reparación';
                const fechaCreacion = s.fechaCreacion ? new Date(s.fechaCreacion).toLocaleDateString() : 'N/D';
                const vehiculoInfo = s.vehiculoInfo || 'N/D';
                
                return (
                <div key={s.id} className="card">
                  <div className="card-header">
                    <div className="card-title">Servicio #{s.id} · {tipoPretty}</div>
                    <div className="card-subtitle">Estado: {s.estadoDescripcion}</div>
                  </div>
                  <div className="servicio-info">
                    <p><strong>Vehículo:</strong> {vehiculoInfo}</p>
                    {s.detallesRevision && (
                      <p><strong>Detalles:</strong> {s.detallesRevision}</p>
                    )}
                    <p><strong>Fecha Creación:</strong> {fechaCreacion}</p>
                    {s.empleadoNombre && (
                      <p><strong>Mecánico:</strong> {s.empleadoNombre}</p>
                    )}
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
                );
              })}
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