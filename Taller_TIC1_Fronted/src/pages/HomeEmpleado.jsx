import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import Header from '../components/Header';
import '../styles/HomeEmpleado.css';
import { listOrdenes, updateOrden } from '../services/ordenesService';
import { listServicios } from '../services/serviciosService';
import { listFacturas } from '../services/facturasService';
import { getCurrentUser, registerEmpleado } from '../services/authService';
import { createEmpleado, listEmpleados } from '../services/empleadosService';

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

  useEffect(() => {
    // Get current user information
    const currentUser = getCurrentUser();
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
    } catch (e) {
      setError(e?.message || 'Error al cargar datos');
    } finally {
      setLoading(false);
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
            { title: 'Órdenes Asignadas', count: ordenes.filter(o => o.idTipoEstadoOrden === 2).length },
            { title: 'En Proceso', count: ordenes.filter(o => o.idTipoEstadoOrden === 3).length },
            { title: 'Completadas', count: ordenes.filter(o => o.idTipoEstadoOrden === 4).length }
          ],
          content: (
            <div className="mecanico-content">
              <h3>Órdenes de Trabajo</h3>
              {loading ? <p>Cargando...</p> : (
                <div className="ordenes-list">
                  {ordenes.map(orden => (
                    <div key={orden.id} className="orden-card">
                      <div className="orden-info">
                        <h4>Orden #{orden.id}</h4>
                        <p>Cliente: {orden.clienteNombre}</p>
                        <p>Vehículo: {orden.vehiculoPlaca}</p>
                        <p>Estado: {orden.estadoDescripcion}</p>
                        <p>Fecha: {new Date(orden.fechaCreacion).toLocaleDateString()}</p>
                      </div>
                      <div className="orden-actions">
                        <select 
                          value={orden.idTipoEstadoOrden}
                          onChange={async (e) => {
                            try {
                              await updateOrden(orden.id, {
                                ...orden,
                                idTipoEstadoOrden: parseInt(e.target.value)
                              });
                              loadData();
                            } catch (err) {
                              alert('Error al actualizar estado');
                            }
                          }}
                        >
                          <option value={1}>Pendiente</option>
                          <option value={2}>Asignada</option>
                          <option value={3}>En Proceso</option>
                          <option value={4}>Completada</option>
                        </select>
                      </div>
                    </div>
                  ))}
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
            { title: 'Órdenes Pendientes', count: ordenes.filter(o => o.idTipoEstadoOrden === 1).length },
            { title: 'Servicios Disponibles', count: servicios.length },
            { title: 'Facturas Pendientes', count: facturas.filter(f => f.estado === 'Pendiente').length }
          ],
          content: (
            <div className="secretaria-content">
              <h3>Gestión de Órdenes</h3>
              {loading ? <p>Cargando...</p> : (
                <div className="ordenes-list">
                  {ordenes.filter(o => o.idTipoEstadoOrden === 1).map(orden => (
                    <div key={orden.id} className="orden-card">
                      <div className="orden-info">
                        <h4>Orden #{orden.id}</h4>
                        <p>Cliente: {orden.clienteNombre}</p>
                        <p>Vehículo: {orden.vehiculoPlaca}</p>
                        <p>Servicios: {orden.serviciosIds.length}</p>
                        <p>Fecha: {new Date(orden.fechaCreacion).toLocaleDateString()}</p>
                      </div>
                      <div className="orden-actions">
                        <button 
                          className="btn-primary"
                          onClick={async () => {
                            try {
                              await updateOrden(orden.id, {
                                ...orden,
                                idTipoEstadoOrden: 2 // Asignada
                              });
                              loadData();
                              alert('Orden asignada a mecánico');
                            } catch (err) {
                              alert('Error al asignar orden');
                            }
                          }}
                        >
                          Asignar a Mecánico
                        </button>
                      </div>
                    </div>
                  ))}
                </div>
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
            { title: 'Servicios Activos', count: servicios.length },
            { title: 'Facturas Generadas', count: facturas.length }
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
                  <h3>Servicios Más Solicitados</h3>
                  <div className="servicios-stats">
                    {servicios.slice(0, 5).map(servicio => (
                      <div key={servicio.id} className="servicio-stat">
                        <span>{servicio.nombre}</span>
                        <span>${servicio.costo}</span>
                      </div>
                    ))}
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
            <div className="activity-item">
              <p>Sistema actualizado a la versión 2.1.0</p>
              <span className="activity-time">Ayer a las 14:30</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default HomeEmpleado;