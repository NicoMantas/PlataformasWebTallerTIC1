import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import Header from '../components/Header';
import '../styles/HomeEmpleado.css';
import { listOrdenes, updateOrden } from '../services/ordenesService';
import { listServicios, secretariaListPendientes, secretariaListAsignados, secretariaAsignarMecanico, getServicioById, updateServicioEstado, mecanicoListAsignados, mecanicoListCompletados } from '../services/serviciosService';
import { listFacturas } from '../services/facturasService';
import { getCurrentUser, registerEmpleado } from '../services/authService';
import { createEmpleado, listEmpleados } from '../services/empleadosService';
import { listRepuestos } from '../services/repuestosService';
import { getDetalleRevision, createDetalleRevision, updateDetalleRevision, getDetalleReparacion, getRepuestosByDetalleReparacion, addRepuestoToReparacion } from '../services/detallesService';

const HomeEmpleado = () => {
  const navigate = useNavigate();
  const { tipo } = useParams(); // mecanico, secretaria, administrador
  const [ordenes, setOrdenes] = useState([]);
  const [servicios, setServicios] = useState([]);
  const [facturas, setFacturas] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [user, setUser] = useState(null);
  const [empleados, setEmpleados] = useState([]);
  const [secPendientes, setSecPendientes] = useState([]);
  const [secAsignados, setSecAsignados] = useState([]);
  const [secTab, setSecTab] = useState('pendientes');
  const [creating, setCreating] = useState(false);
  const [empleadoForm, setEmpleadoForm] = useState({
    nombre: '',
    apellido: '',
    cedula: '',
    salario: '',
    idTipoEmpleado: 1,
    email: '',
    password: '',
    idTaller: ''
  });
  
  // Mechanic specific states
  const [mecanicoAsignados, setMecanicoAsignados] = useState([]);
  const [mecanicoCompletados, setMecanicoCompletados] = useState([]);
  
  // Estados organizados por tipo
  const [serviciosPendientes, setServiciosPendientes] = useState([]);
  const [serviciosAsignados, setServiciosAsignados] = useState([]);
  const [serviciosEnProceso, setServiciosEnProceso] = useState([]);
  const [serviciosCompletados, setServiciosCompletados] = useState([]);
  const [repuestos, setRepuestos] = useState([]);
  const [selectedServicio, setSelectedServicio] = useState(null);
  const [showReparacionModal, setShowReparacionModal] = useState(false);
  const [showRevisionModal, setShowRevisionModal] = useState(false);
  const [repuestoForm, setRepuestoForm] = useState({
    idRepuesto: '',
    cantidad: 1
  });
  const [revisionForm, setRevisionForm] = useState({
    detalles: ''
  });

  useEffect(() => {
    // Get current user information
    const currentUser = getCurrentUser();
    console.log('Usuario obtenido:', currentUser);
    setUser(currentUser);
    loadData();
  }, []);

  const loadData = async () => {
    try {
      setLoading(true);
      const [ordenesData, serviciosData, facturasData, empleadosData] = await Promise.all([
        listOrdenes(),
        listServicios(),
        listFacturas(),
        listEmpleados().catch(() => [])
      ]);
      setOrdenes(ordenesData || []);
      setServicios(serviciosData || []);
      setFacturas(facturasData || []);
      setEmpleados(empleadosData || []);
      // cargar datos específicos según el tipo de empleado
      if (tipo === 'secretaria') {
        const [p, a] = await Promise.all([
          secretariaListPendientes().catch(() => []),
          secretariaListAsignados().catch(() => [])
        ]);
        setSecPendientes(p || []);
        setSecAsignados(a || []);
      } else if (tipo === 'mecanico') {
        // Debug: mostrar información del usuario
        console.log('Usuario mecánico:', user);
        console.log('ID empleado:', user?.id);
        
        if (user?.id) {
          const [asignados, completados, repuestosData] = await Promise.all([
            mecanicoListAsignados(user.id).catch((err) => {
              console.error('Error cargando servicios asignados:', err);
              return [];
            }),
            mecanicoListCompletados(user.id).catch((err) => {
              console.error('Error cargando servicios completados:', err);
              return [];
            }),
            listRepuestos().catch((err) => {
              console.error('Error cargando repuestos:', err);
              return [];
            })
          ]);
          
          // Combinar todos los servicios y organizarlos por estado
          const todosServicios = [...(asignados || []), ...(completados || [])];
          console.log('Todos los servicios del mecánico:', todosServicios);
          
          // Eliminar duplicados por ID
          const serviciosUnicos = todosServicios.filter((servicio, index, self) => 
            index === self.findIndex(s => s.id === servicio.id)
          );
          
          console.log('Servicios únicos:', serviciosUnicos);
          organizarServiciosPorEstado(serviciosUnicos);
          
          // Mantener compatibilidad con el código existente
          setMecanicoAsignados(asignados || []);
          setMecanicoCompletados(completados || []);
          setRepuestos(repuestosData || []);
        } else {
          console.log('No se encontró ID de empleado para el mecánico, usando endpoint de secretaría');
          // Fallback: usar endpoint de secretaría para obtener servicios asignados
          try {
            const asignados = await secretariaListAsignados();
            console.log('Servicios asignados (fallback):', asignados);
            
            // Organizar también los servicios del fallback por estado
            organizarServiciosPorEstado(asignados || []);
            setMecanicoAsignados(asignados || []);
          } catch (err) {
            console.error('Error en fallback:', err);
            setMecanicoAsignados([]);
          }
        }
      }
    } catch (e) {
      setError(e?.message || 'Error al cargar datos');
    } finally {
      setLoading(false);
    }
  };

  // Función para organizar servicios por estados
  const organizarServiciosPorEstado = (servicios) => {
    const pendientes = servicios.filter(s => s.idEstado === 1 || s.estadoDescripcion?.toLowerCase() === 'pendiente');
    const enProceso = servicios.filter(s => s.idEstado === 2 || s.estadoDescripcion?.toLowerCase() === 'en proceso');
    const completados = servicios.filter(s => s.idEstado === 3 || s.estadoDescripcion?.toLowerCase() === 'completado');
    const cancelados = servicios.filter(s => s.idEstado === 4 || s.estadoDescripcion?.toLowerCase() === 'cancelado');
    
    setServiciosPendientes(pendientes);
    setServiciosAsignados([]); // No existe estado "Asignado" en la BD
    setServiciosEnProceso(enProceso);
    setServiciosCompletados(completados);
  };

  // Función para renderizar lista de servicios
  const renderizarServicios = (servicios, esCompletado = false) => {
    if (servicios.length === 0) {
      return (
        <div style={{ textAlign: 'center', padding: '2rem', color: '#666' }}>
          <p>No hay servicios en este estado</p>
          <p><small>Los servicios aparecerán aquí cuando cambien a este estado</small></p>
        </div>
      );
    }

    return servicios.map(servicio => (
      <div key={servicio.id} className="orden-card">
        <div className="orden-info">
          <h4>Servicio #{servicio.id} · {(servicio.tipoServicio === 'Revision' ? 'Revisión' : 'Reparación')}</h4>
          <p><strong>Cliente:</strong> {servicio.clienteNombre || 'N/D'}</p>
          <p><strong>Vehículo:</strong> {servicio.vehiculoMarca} {servicio.vehiculoModelo} - {servicio.vehiculoPlaca || 'Sin placa'}</p>
          <p><strong>Estado:</strong> {servicio.estadoDescripcion}</p>
          <p><strong>Fecha {esCompletado ? 'Completado' : 'Creación'}:</strong> {
            esCompletado 
              ? (servicio.fechaActualizacion ? new Date(servicio.fechaActualizacion).toLocaleDateString() : 'N/D')
              : (servicio.fechaCreacion ? new Date(servicio.fechaCreacion).toLocaleDateString() : 'N/D')
          }</p>
          {servicio.detallesRevision && (
            <p><strong>Detalles Revisión:</strong> {servicio.detallesRevision}</p>
          )}
        </div>
        {!esCompletado && (
          <div className="orden-actions">
            <select 
              value={servicio.idEstado}
              onChange={(e) => handleServicioEstadoChange(servicio.id, parseInt(e.target.value))}
            >
              <option value={1}>Pendiente</option>
              <option value={2}>En Proceso</option>
              <option value={3}>Completado</option>
              <option value={4}>Cancelado</option>
            </select>
            {servicio.tipoServicio === 'Reparacion' && (
              <button 
                className="btn-secondary" 
                onClick={() => openReparacionModal(servicio)}
                style={{ marginTop: '0.5rem' }}
              >
                Agregar Repuestos
              </button>
            )}
            {servicio.tipoServicio === 'Revision' && (
              <button 
                className="btn-secondary" 
                onClick={() => openRevisionModal(servicio)}
                style={{ marginTop: '0.5rem' }}
              >
                {servicio.detallesRevision ? 'Editar Detalles' : 'Agregar Detalles'}
              </button>
            )}
          </div>
        )}
      </div>
    ));
  };

  // Mechanic specific functions
  const handleServicioEstadoChange = async (servicioId, nuevoEstado) => {
    try {
      await updateServicioEstado(servicioId, nuevoEstado);
      await loadData();
    } catch (err) {
      alert('Error al actualizar estado del servicio');
    }
  };

  const openReparacionModal = (servicio) => {
    setSelectedServicio(servicio);
    setShowReparacionModal(true);
    setRepuestoForm({ idRepuesto: '', cantidad: 1 });
  };

  const openRevisionModal = async (servicio) => {
    setSelectedServicio(servicio);
    try {
      const detalle = await getDetalleRevision(servicio.id);
      setRevisionForm({ detalles: detalle?.detalles || '' });
    } catch (err) {
      setRevisionForm({ detalles: '' });
    }
    setShowRevisionModal(true);
  };

  const handleAddRepuesto = async () => {
    try {
      await addRepuestoToReparacion(selectedServicio.id, {
        idRepuesto: parseInt(repuestoForm.idRepuesto),
        cantidad: parseInt(repuestoForm.cantidad)
      });
      alert('Repuesto agregado correctamente');
      setShowReparacionModal(false);
      await loadData();
    } catch (err) {
      console.error('Error al agregar repuesto:', err);
      alert('Error al agregar repuesto: ' + (err.response?.data?.message || err.message));
    }
  };

  const handleSaveRevision = async () => {
    try {
      if (selectedServicio.detallesRevision) {
        await updateDetalleRevision(selectedServicio.id, {
          idServicio: selectedServicio.id,
          detalles: revisionForm.detalles
        });
      } else {
        await createDetalleRevision({
          idServicio: selectedServicio.id,
          detalles: revisionForm.detalles
        });
      }
      alert('Detalles de revisión guardados');
      setShowRevisionModal(false);
      await loadData();
    } catch (err) {
      alert('Error al guardar detalles de revisión');
    }
  };

  // Contenido específico para cada tipo de empleado
  const getEmpleadoContent = () => {
    switch(tipo) {
      case 'mecanico':
        return {
          title: 'Panel de Mecánico',
          subtitle: 'Gestiona tus reparaciones y mantenimientos',
          features: [
            { title: 'Pendientes', count: serviciosPendientes.length },
            { title: 'En Proceso', count: serviciosEnProceso.length },
            { title: 'Completados', count: serviciosCompletados.length }
          ],
          content: (
            <div className="mecanico-content">
              <div className="dashboard-tabs" style={{ marginBottom: '1rem' }}>
                <button 
                  className={`tab-button ${secTab === 'pendientes' ? 'active' : ''}`} 
                  onClick={() => setSecTab('pendientes')}
                >
                  Pendientes ({serviciosPendientes.length})
                </button>
                <button 
                  className={`tab-button ${secTab === 'en-proceso' ? 'active' : ''}`} 
                  onClick={() => setSecTab('en-proceso')}
                >
                  En Proceso ({serviciosEnProceso.length})
                </button>
                <button 
                  className={`tab-button ${secTab === 'completados' ? 'active' : ''}`} 
                  onClick={() => setSecTab('completados')}
                >
                  Completados ({serviciosCompletados.length})
                </button>
              </div>
              
              {loading ? (
                <p>Cargando...</p>
              ) : (
                <div className="ordenes-list">
                  {secTab === 'pendientes' && renderizarServicios(serviciosPendientes)}
                  {secTab === 'en-proceso' && renderizarServicios(serviciosEnProceso)}
                  {secTab === 'completados' && renderizarServicios(serviciosCompletados, true)}
                </div>
              )}
            </div>
          )
        };
      
      case 'secretaria':
        return {
          title: 'Panel de Secretaría',
          subtitle: 'Gestiona citas, clientes y administración',
          features: [
            { title: 'Trabajos Pendientes', count: secPendientes.length },
            { title: 'Trabajos Asignados', count: secAsignados.length },
            { title: 'Servicios Activos', count: (secPendientes.length + secAsignados.length) }
          ],
          content: (
            <div className="secretaria-content">
              <div className="dashboard-tabs" style={{ marginBottom: '1rem' }}>
                <button className={`tab-button ${secTab==='pendientes'?'active':''}`} onClick={()=>setSecTab('pendientes')}>Trabajos Pendientes</button>
                <button className={`tab-button ${secTab==='asignados'?'active':''}`} onClick={()=>setSecTab('asignados')}>Trabajos Asignados</button>
              </div>
              {loading ? <p>Cargando...</p> : (
                secTab === 'pendientes' ? (
                  <div className="ordenes-list">
                    {secPendientes.map(s => (
                      <div key={s.id} className="orden-card">
                        <div className="orden-info">
                          <h4>Servicio #{s.id} · {(s.tipoServicio==='Revision'?'Revisión':s.tipoServicio==='Reparacion'?'Reparación':(s.detallesRevision?'Revisión':'Reparación'))}</h4>
                          <p>Cliente: {s.clienteNombre || 'N/D'}</p>
                          <p>Estado: {s.estadoDescripcion}</p>
                          {s.detallesRevision && <p>Detalles: {s.detallesRevision}</p>}
                        </div>
                        <div className="orden-actions">
                          <select onChange={async (e)=>{ const empId = parseInt(e.target.value); if(!empId) return; await secretariaAsignarMecanico(s.id, empId); await loadData(); e.target.value=''; }} defaultValue="">
                            <option value="" disabled>Asignar mecánico</option>
                            {empleados.map(emp => (
                              <option key={emp.id} value={emp.id}>{emp.nombre} {emp.apellido || ''}</option>
                            ))}
                          </select>
                        </div>
                      </div>
                    ))}
                  </div>
                ) : (
                  <div className="ordenes-list">
                    {secAsignados.map(s => (
                      <div key={s.id} className="orden-card">
                        <div className="orden-info">
                          <h4>Servicio #{s.id} · {(s.tipoServicio==='Revision'?'Revisión':s.tipoServicio==='Reparacion'?'Reparación':(s.detallesRevision?'Revisión':'Reparación'))}</h4>
                          <p>Cliente: {s.clienteNombre || 'N/D'}</p>
                          <p>Estado: {s.estadoDescripcion}</p>
                          <p>Mecánico: {s.empleadoNombre || 'Asignado'}</p>
                        </div>
                      </div>
                    ))}
                  </div>
                )
              )}
            </div>
          )
        };
      
      case 'administrador':
        return {
          title: 'Panel de Administración',
          subtitle: 'Gestiona el taller completo',
          features: [
            { title: 'Total Órdenes', count: ordenes.length },
            { title: 'Servicios Activos', count: servicios.filter(s => s.idEstado !== 4).length },
            { title: 'Facturas Generadas', count: facturas.length },
            { title: 'Empleados Activos', count: empleados.length }
          ],
          content: (
            <div className="admin-content">
              <div className="dashboard-grid">
                <div className="dashboard-card">
                  <h3>Resumen de Órdenes</h3>
                  <div className="stats">
                    <div className="stat">
                      <span className="stat-label">Pendientes:</span>
                      <span className="stat-value">{ordenes.filter(o => o.idTipoEstadoOrden === 1).length}</span>
                    </div>
                    <div className="stat">
                      <span className="stat-label">En Proceso:</span>
                      <span className="stat-value">{ordenes.filter(o => o.idTipoEstadoOrden === 3).length}</span>
                    </div>
                    <div className="stat">
                      <span className="stat-label">Completadas:</span>
                      <span className="stat-value">{ordenes.filter(o => o.idTipoEstadoOrden === 4).length}</span>
                    </div>
                  </div>
                </div>
                
                <div className="dashboard-card">
                  <h3>Estados de Servicios</h3>
                  <div className="servicios-stats">
                    <div className="stat">
                      <span className="stat-label">Pendientes:</span>
                      <span className="stat-value">{servicios.filter(s => s.idEstado === 1).length}</span>
                    </div>
                    <div className="stat">
                      <span className="stat-label">Asignados:</span>
                      <span className="stat-value">{servicios.filter(s => s.idEstado === 2).length}</span>
                    </div>
                    <div className="stat">
                      <span className="stat-label">En Proceso:</span>
                      <span className="stat-value">{servicios.filter(s => s.idEstado === 3).length}</span>
                    </div>
                    <div className="stat">
                      <span className="stat-label">Completados:</span>
                      <span className="stat-value">{servicios.filter(s => s.idEstado === 4).length}</span>
                    </div>
                  </div>
                </div>
                
                <div className="dashboard-card">
                  <h3>Facturación</h3>
                  <div className="facturas-stats">
                    <div className="stat">
                      <span className="stat-label">Pendientes:</span>
                      <span className="stat-value">{facturas.filter(f => f.estado === 'Pendiente').length}</span>
                    </div>
                    <div className="stat">
                      <span className="stat-label">Pagadas:</span>
                      <span className="stat-value">{facturas.filter(f => f.estado === 'Pagada').length}</span>
                    </div>
                    <div className="stat">
                      <span className="stat-label">Total Facturado:</span>
                      <span className="stat-value">${facturas.reduce((sum, f) => sum + f.total, 0).toFixed(2)}</span>
                    </div>
                  </div>
                </div>
              </div>
              <div className="dashboard-card" style={{ marginTop: '1.5rem' }}>
                <h3>Registro de Empleados</h3>
                <p>Tipos: 1 Administrador, 2 Secretaria, 3 Mecánico</p>
                <form
                  onSubmit={async (e) => {
                    e.preventDefault();
                    try {
                      setCreating(true);
                      setError('');
                      const created = await createEmpleado({
                        nombre: empleadoForm.nombre,
                        apellido: empleadoForm.apellido,
                        cedula: Number(empleadoForm.cedula),
                        salario: Number(empleadoForm.salario),
                        idTipoEmpleado: Number(empleadoForm.idTipoEmpleado)
                      });
                      const idEmpleado = created?.id;
                      const idTaller = empleadoForm.idTaller || user?.idTaller || user?.tallerId;
                      if (!idEmpleado || !idTaller) {
                        alert('Empleado creado, pero falta idTaller para registro de acceso');
                        await loadData();
                        setCreating(false);
                        return;
                      }
                      await registerEmpleado({
                        email: empleadoForm.email,
                        password: empleadoForm.password,
                        idEmpleado,
                        idTaller
                      });
                      alert('Empleado registrado y credenciales creadas');
                      setEmpleadoForm({ nombre: '', apellido: '', cedula: '', salario: '', idTipoEmpleado: 1, email: '', password: '', idTaller: '' });
                      await loadData();
                    } catch (err) {
                      setError(err?.message || 'Error en registro de empleado');
                    } finally {
                      setCreating(false);
                    }
                  }}
                  className="form-container"
                >
                  {error && <div className="error-message">{error}</div>}
                  <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit,minmax(180px,1fr))', gap: '1rem' }}>
                    <input placeholder="Nombre" required value={empleadoForm.nombre} onChange={(e) => setEmpleadoForm({ ...empleadoForm, nombre: e.target.value })} />
                    <input placeholder="Apellido" required value={empleadoForm.apellido} onChange={(e) => setEmpleadoForm({ ...empleadoForm, apellido: e.target.value })} />
                    <input placeholder="Cédula" required type="number" value={empleadoForm.cedula} onChange={(e) => setEmpleadoForm({ ...empleadoForm, cedula: e.target.value })} />
                    <input placeholder="Salario" required type="number" step="0.01" value={empleadoForm.salario} onChange={(e) => setEmpleadoForm({ ...empleadoForm, salario: e.target.value })} />
                    <select value={empleadoForm.idTipoEmpleado} onChange={(e) => setEmpleadoForm({ ...empleadoForm, idTipoEmpleado: e.target.value })}>
                      <option value={1}>Administrador</option>
                      <option value={2}>Secretaria</option>
                      <option value={3}>Mecánico</option>
                    </select>
                    <input placeholder="Email" required type="email" value={empleadoForm.email} onChange={(e) => setEmpleadoForm({ ...empleadoForm, email: e.target.value })} />
                    <input placeholder="Password" required type="password" value={empleadoForm.password} onChange={(e) => setEmpleadoForm({ ...empleadoForm, password: e.target.value })} />
                    <input placeholder="Id Taller" type="number" value={empleadoForm.idTaller} onChange={(e) => setEmpleadoForm({ ...empleadoForm, idTaller: e.target.value })} />
                  </div>
                  <div style={{ marginTop: '1rem', display: 'flex', gap: '.75rem' }}>
                    <button className="btn-primary" type="submit" disabled={creating}>{creating ? 'Creando...' : 'Crear y Registrar'}</button>
                    <button className="btn-secondary" type="button" onClick={() => setEmpleadoForm({ nombre: '', apellido: '', cedula: '', salario: '', idTipoEmpleado: 1, email: '', password: '', idTaller: '' })}>Limpiar</button>
                  </div>
                </form>
                <div className="card" style={{ marginTop: '1rem' }}>
                  <h4>Empleados actuales</h4>
                  <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit,minmax(220px,1fr))', gap: '1rem' }}>
                    {empleados.map((emp) => (
                      <div key={emp.id} className="card" style={{ padding: '1rem' }}>
                        <div className="card-title">{emp.nombre} {emp.apellido}</div>
                        <div className="card-subtitle">Tipo: {emp.idTipoEmpleado}</div>
                        <p className="mb-0">Cédula: {emp.cedula}</p>
                      </div>
                    ))}
                  </div>
                </div>
              </div>
            </div>
          )
        };
      
      default:
        return {
          title: 'Panel de Empleado',
          subtitle: 'Bienvenido al sistema',
          features: [],
          actions: []
        };
    }
  };

  const content = getEmpleadoContent();

  return (
    <div className="home-empleado-page">
      <Header />
      
      <div className="empleado-container">
        <div className="empleado-header">
          <h1>{content.title}</h1>
          <p>{content.subtitle}</p>
          <div className="user-info">
            <span>Conectado como: {user?.infoEspecifica?.nombre || user?.email || 'Empleado'}</span>
            {user?.infoEspecifica?.tipoEmpleado && (
              <span className="tipo-empleado"> ({user.infoEspecifica.tipoEmpleado})</span>
            )}
            {user?.nombreTaller && (
              <span className="taller-info"> - {user.nombreTaller}</span>
            )}
          </div>
        </div>
        
        <div className="dashboard-section">
          <h2>Resumen</h2>
          <div className="stats-grid">
            {content.features.map((feature, index) => (
              <div key={index} className="stat-card">
                <h3>{feature.title}</h3>
                <div className="stat-value">{feature.count}</div>
              </div>
            ))}
          </div>
        </div>
        
        {content.content && (
          <div className="content-section">
            {content.content}
          </div>
        )}
        
        <div className="recent-activity">
          <h2>Actividad Reciente</h2>
          <div className="activity-list">
            <div className="activity-item">
              <p>Sesión iniciada correctamente</p>
              <span className="activity-time">Hace unos momentos</span>
            </div>
            {tipo === 'mecanico' && mecanicoCompletados.length > 0 && (
              <div className="activity-item">
                <p>Último servicio completado: #{mecanicoCompletados[0].id}</p>
                <span className="activity-time">
                  {new Date(mecanicoCompletados[0].fechaActualizacion || mecanicoCompletados[0].fechaCreacion).toLocaleString()}
                </span>
              </div>
            )}
            {tipo === 'secretaria' && secAsignados.length > 0 && (
              <div className="activity-item">
                <p>Servicios asignados: {secAsignados.length}</p>
                <span className="activity-time">Actualmente</span>
              </div>
            )}
            {tipo === 'administrador' && (
              <>
                <div className="activity-item">
                  <p>Total de empleados registrados: {empleados.length}</p>
                  <span className="activity-time">Sistema</span>
                </div>
                <div className="activity-item">
                  <p>Servicios activos: {servicios.filter(s => s.idEstado !== 4).length}</p>
                  <span className="activity-time">Actualmente</span>
                </div>
              </>
            )}
          </div>
        </div>
      </div>

      {/* Modal para agregar repuestos a reparación */}
      {showReparacionModal && selectedServicio && (
        <div className="modal-overlay">
          <div className="modal-content">
            <div className="modal-header">
              <h3>Agregar Repuestos - Servicio #{selectedServicio.id}</h3>
              <button className="modal-close" onClick={() => setShowReparacionModal(false)}>×</button>
            </div>
            <div className="modal-body">
              <div className="form-group">
                <label>Repuesto:</label>
                <select 
                  value={repuestoForm.idRepuesto}
                  onChange={(e) => setRepuestoForm({...repuestoForm, idRepuesto: e.target.value})}
                >
                  <option value="">Seleccionar repuesto</option>
                  {repuestos.map(repuesto => (
                    <option key={repuesto.id} value={repuesto.id}>
                      {repuesto.nombre} - ${repuesto.precio} (Stock: {repuesto.stock})
                    </option>
                  ))}
                </select>
              </div>
              <div className="form-group">
                <label>Cantidad:</label>
                <input 
                  type="number" 
                  min="1" 
                  value={repuestoForm.cantidad}
                  onChange={(e) => setRepuestoForm({...repuestoForm, cantidad: e.target.value})}
                />
              </div>
            </div>
            <div className="modal-footer">
              <button className="btn-secondary" onClick={() => setShowReparacionModal(false)}>
                Cancelar
              </button>
              <button 
                className="btn-primary" 
                onClick={handleAddRepuesto}
                disabled={!repuestoForm.idRepuesto || repuestoForm.cantidad < 1}
              >
                Agregar Repuesto
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Modal para detalles de revisión */}
      {showRevisionModal && selectedServicio && (
        <div className="modal-overlay">
          <div className="modal-content">
            <div className="modal-header">
              <h3>Detalles de Revisión - Servicio #{selectedServicio.id}</h3>
              <button className="modal-close" onClick={() => setShowRevisionModal(false)}>×</button>
            </div>
            <div className="modal-body">
              <div className="form-group">
                <label>Detalles de la Revisión:</label>
                <textarea 
                  rows="6"
                  value={revisionForm.detalles}
                  onChange={(e) => setRevisionForm({...revisionForm, detalles: e.target.value})}
                  placeholder="Describe los hallazgos de la revisión, problemas encontrados, recomendaciones, etc."
                />
              </div>
            </div>
            <div className="modal-footer">
              <button className="btn-secondary" onClick={() => setShowRevisionModal(false)}>
                Cancelar
              </button>
              <button 
                className="btn-primary" 
                onClick={handleSaveRevision}
                disabled={!revisionForm.detalles.trim()}
              >
                Guardar Detalles
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default HomeEmpleado;