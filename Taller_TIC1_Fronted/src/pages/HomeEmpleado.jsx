import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import Header from '../components/Header';
import '../styles/HomeEmpleado.css';
import { listOrdenes, updateOrden } from '../services/ordenesService';
import { listServicios } from '../services/serviciosService';
import { listFacturas } from '../services/facturasService';
import { getCurrentUser } from '../services/authService';

const HomeEmpleado = () => {
  const navigate = useNavigate();
  const { tipo } = useParams(); // mecanico, secretaria, administrador
  const [ordenes, setOrdenes] = useState([]);
  const [servicios, setServicios] = useState([]);
  const [facturas, setFacturas] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [user, setUser] = useState(null);

  useEffect(() => {
    // Get current user information
    const currentUser = getCurrentUser();
    setUser(currentUser);
    loadData();
  }, []);

  const loadData = async () => {
    try {
      setLoading(true);
      const [ordenesData, serviciosData, facturasData] = await Promise.all([
        listOrdenes(),
        listServicios(),
        listFacturas()
      ]);
      setOrdenes(ordenesData || []);
      setServicios(serviciosData || []);
      setFacturas(facturasData || []);
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