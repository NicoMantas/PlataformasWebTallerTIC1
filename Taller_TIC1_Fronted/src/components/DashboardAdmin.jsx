import React, { useState, useEffect } from 'react';
import { getDashboardData } from '../services/estadisticasAdminService';
import '../styles/DashboardAdmin.css';

const DashboardAdmin = () => {
  const [dashboardData, setDashboardData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    try {
      setLoading(true);
      const data = await getDashboardData();
      setDashboardData(data);
    } catch (err) {
      setError(err.message || 'Error al cargar datos del dashboard');
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <div className="loading">Cargando dashboard...</div>;
  if (error) return <div className="error">{error}</div>;
  if (!dashboardData) return <div className="error">No se pudieron cargar los datos</div>;

  const { resumen, capacidad, servicios, facturacion } = dashboardData;

  const StatCard = ({ title, value, icon, color, subtitle }) => (
    <div className="stat-card">
      <div className="stat-icon" style={{ backgroundColor: color }}>
        {icon}
      </div>
      <div className="stat-content">
        <h3 className="stat-value">{value}</h3>
        <p className="stat-title">{title}</p>
        {subtitle && <p className="stat-subtitle">{subtitle}</p>}
      </div>
    </div>
  );

  const ProgressBar = ({ percentage, color, label }) => (
    <div className="progress-item">
      <div className="progress-label">
        <span>{label}</span>
        <span>{percentage}%</span>
      </div>
      <div className="progress-bar">
        <div 
          className="progress-fill" 
          style={{ 
            width: `${percentage}%`,
            backgroundColor: color
          }}
        ></div>
      </div>
    </div>
  );

  return (
    <div className="dashboard-admin">
      <div className="dashboard-header">
        <h1>Dashboard Administrativo</h1>
        <p>Vista general del estado del taller</p>
      </div>

      {/* Tarjetas de resumen - MEJORADAS */}
      <div className="stats-grid">
        <StatCard
          title="Servicios Totales"
          value={resumen.totalServicios || 0}
          icon="🔧"
          color="#3b82f6"
          subtitle={`${resumen.serviciosActivos || 0} activos`}
        />
        <StatCard
          title="Servicios Completados"
          value={resumen.serviciosCompletados || 0}
          icon="✅"
          color="#10b981"
          subtitle={`${resumen.totalServicios > 0 ? Math.round((resumen.serviciosCompletados / resumen.totalServicios) * 100) : 0}% completados`}
        />
        <StatCard
          title="Total Clientes"
          value={resumen.totalClientes || 0}
          icon="👥"
          color="#8b5cf6"
          subtitle="Clientes registrados"
        />
        <StatCard
          title="Empleados Activos"
          value={resumen.totalEmpleados || 0}
          icon="👷"
          color="#06b6d4"
          subtitle="Personal activo"
        />
        <StatCard
          title="Ingresos Totales"
          value={`$${(facturacion.totalFacturado || 0).toLocaleString()}`}
          icon="💰"
          color="#10b981"
          subtitle={`Mes actual: $${(facturacion.facturadoMesActual || 0).toLocaleString()}`}
        />
        <StatCard
          title="Promedio por Servicio"
          value={`$${resumen.totalServicios > 0 ? Math.round((facturacion.totalFacturado || 0) / resumen.totalServicios).toLocaleString() : '0'}`}
          icon="📊"
          color="#f59e0b"
          subtitle="Valor promedio"
        />
      </div>

      <div className="dashboard-content">
        {/* Capacidad del taller - MEJORADA */}
        <div className="dashboard-section">
          <h2>🏭 Capacidad del Taller</h2>
          <div className="capacidad-card">
            <div className="capacidad-header">
              <div className="capacidad-title">
                <h3>Estado de Ocupación</h3>
                <p className="capacidad-subtitle">Monitoreo en tiempo real</p>
              </div>
              <div className={`capacidad-status-badge ${capacidad.capacidadDisponible ? 'disponible' : 'lleno'}`}>
                <div className="status-indicator"></div>
                <span>{capacidad.capacidadDisponible ? 'DISPONIBLE' : 'CAPACIDAD LLENA'}</span>
              </div>
            </div>
            
            <div className="capacidad-content">
              <div className="capacidad-metrics">
                <div className="metric-card pendientes">
                  <div className="metric-icon">⏳</div>
                  <div className="metric-content">
                    <span className="metric-number">{capacidad.serviciosPendientes || 0}</span>
                    <span className="metric-label">PENDIENTES</span>
                  </div>
                </div>
                
                <div className="metric-card en-proceso">
                  <div className="metric-icon">🔧</div>
                  <div className="metric-content">
                    <span className="metric-number">{capacidad.serviciosAsignados || 0}</span>
                    <span className="metric-label">EN PROCESO</span>
                  </div>
                </div>
                
                <div className="metric-card disponibles">
                  <div className="metric-icon">✅</div>
                  <div className="metric-content">
                    <span className="metric-number">{capacidad.espaciosDisponibles || 0}</span>
                    <span className="metric-label">ESPACIOS LIBRES</span>
                  </div>
                </div>
              </div>
              
              <div className="capacidad-progress">
                <div className="progress-header">
                  <span className="progress-title">Ocupación del Taller</span>
                  <span className="progress-percentage">{capacidad.porcentajeOcupacion || 0}%</span>
                </div>
                <div className="progress-container">
                  <div className="progress-bar">
                    <div 
                      className="progress-fill"
                      style={{ 
                        width: `${capacidad.porcentajeOcupacion || 0}%`,
                        backgroundColor: (capacidad.porcentajeOcupacion || 0) > 80 ? '#ef4444' : 
                                       (capacidad.porcentajeOcupacion || 0) > 60 ? '#f59e0b' : '#10b981'
                      }}
                    ></div>
                  </div>
                  <div className="progress-labels">
                    <span>0</span>
                    <span>Capacidad: {capacidad.capacidadMaxima || 15}</span>
                    <span>100%</span>
                  </div>
                </div>
              </div>
              
              <div className="capacidad-summary">
                <div className="summary-item">
                  <span className="summary-label">Servicios Activos:</span>
                  <span className="summary-value">{(capacidad.serviciosPendientes || 0) + (capacidad.serviciosAsignados || 0)}</span>
                </div>
                <div className="summary-item">
                  <span className="summary-label">Capacidad Total:</span>
                  <span className="summary-value">{capacidad.capacidadMaxima || 15}</span>
                </div>
                <div className="summary-item">
                  <span className="summary-label">Eficiencia:</span>
                  <span className="summary-value">
                    {capacidad.capacidadMaxima > 0 ? 
                      Math.round(((capacidad.serviciosPendientes || 0) + (capacidad.serviciosAsignados || 0)) / capacidad.capacidadMaxima * 100) : 0}%
                  </span>
                </div>
              </div>
            </div>
          </div>
        </div>

        {/* Estadísticas de servicios */}
        <div className="dashboard-section">
          <h2>Distribución de Servicios</h2>
          <div className="servicios-grid">
            <div className="servicios-card">
              <h3>Por Estado</h3>
              <div className="estados-list">
                {servicios.porEstado.map((estado) => (
                  <div key={estado.estado} className="estado-item">
                    <div className="estado-info">
                      <span 
                        className="estado-dot" 
                        style={{ backgroundColor: estado.color }}
                      ></span>
                      <span className="estado-name">{estado.estado}</span>
                    </div>
                    <div className="estado-stats">
                      <span className="estado-count">{estado.cantidad}</span>
                      <span className="estado-percentage">({estado.porcentaje}%)</span>
                    </div>
                  </div>
                ))}
              </div>
            </div>

            <div className="servicios-card">
              <h3>Top Mecánicos</h3>
              <div className="mecanicos-list">
                {servicios.topMecanicos.slice(0, 5).map((mecanico, index) => (
                  <div key={index} className="mecanico-item">
                    <div className="mecanico-rank">#{index + 1}</div>
                    <div className="mecanico-info">
                      <span className="mecanico-name">{mecanico.nombre}</span>
                      <span className="mecanico-services">{mecanico.serviciosCompletados} servicios</span>
                    </div>
                    <div className="mecanico-revenue">
                      ${mecanico.ingresosGenerados.toLocaleString()}
                    </div>
                  </div>
                ))}
              </div>
            </div>
          </div>
        </div>

        {/* Estadísticas de facturación - MEJORADA */}
        <div className="dashboard-section">
          <h2>💰 Análisis Financiero</h2>
          <div className="facturacion-grid">
            {/* Resumen Financiero Principal */}
            <div className="facturacion-card principal">
              <h3>📊 Resumen Financiero</h3>
              <div className="facturacion-stats">
                <div className="facturacion-stat principal">
                  <div className="stat-icon">💵</div>
                  <div className="stat-content">
                    <span className="stat-label">Total Facturado</span>
                    <span className="stat-value">${facturacion.totalFacturado.toLocaleString()}</span>
                  </div>
                </div>
                <div className="facturacion-stat">
                  <div className="stat-icon">📅</div>
                  <div className="stat-content">
                    <span className="stat-label">Mes Actual</span>
                    <span className="stat-value">${facturacion.facturadoMesActual.toLocaleString()}</span>
                  </div>
                </div>
                <div className="facturacion-stat">
                  <div className="stat-icon">📈</div>
                  <div className="stat-content">
                    <span className="stat-label">Crecimiento</span>
                    <span className={`stat-value ${facturacion.porcentajeCrecimiento >= 0 ? 'positive' : 'negative'}`}>
                      {facturacion.porcentajeCrecimiento >= 0 ? '+' : ''}{facturacion.porcentajeCrecimiento}%
                    </span>
                  </div>
                </div>
              </div>
              
              {/* Métricas adicionales */}
              <div className="metricas-adicionales">
                <div className="metrica">
                  <span className="metrica-label">Promedio por Servicio</span>
                  <span className="metrica-value">
                    ${resumen.totalServicios > 0 ? Math.round(facturacion.totalFacturado / resumen.totalServicios).toLocaleString() : '0'}
                  </span>
                </div>
                <div className="metrica">
                  <span className="metrica-label">Servicios Facturados</span>
                  <span className="metrica-value">{resumen.serviciosCompletados}</span>
                </div>
              </div>
              
            </div>

            {/* Top Clientes Mejorado */}
            <div className="facturacion-card">
              <h3>🏆 Top Clientes</h3>
              <div className="clientes-list">
                {facturacion.topClientes && facturacion.topClientes.length > 0 ? (
                  facturacion.topClientes.slice(0, 5).map((cliente, index) => (
                    <div key={index} className="cliente-item">
                      <div className="cliente-rank">#{index + 1}</div>
                      <div className="cliente-info">
                        <span className="cliente-name">{cliente.nombre}</span>
                        <span className="cliente-services">{cliente.serviciosRealizados} servicios</span>
                      </div>
                      <div className="cliente-total">
                        ${cliente.totalGastado.toLocaleString()}
                      </div>
                    </div>
                  ))
                ) : (
                  <div className="no-data">
                    <p>No hay datos de clientes disponibles</p>
                  </div>
                )}
              </div>
            </div>

            {/* Análisis de Rentabilidad */}
            <div className="facturacion-card">
              <h3>📈 Análisis de Rentabilidad</h3>
              <div className="rentabilidad-stats">
                <div className="rentabilidad-item">
                  <div className="rentabilidad-header">
                    <span className="rentabilidad-label">Eficiencia del Taller</span>
                    <span className="rentabilidad-value">
                      {resumen.totalServicios > 0 ? Math.round((resumen.serviciosCompletados / resumen.totalServicios) * 100) : 0}%
                    </span>
                  </div>
                  <div className="rentabilidad-bar">
                    <div 
                      className="rentabilidad-fill"
                      style={{ 
                        width: `${resumen.totalServicios > 0 ? Math.round((resumen.serviciosCompletados / resumen.totalServicios) * 100) : 0}%` 
                      }}
                    ></div>
                  </div>
                </div>
                
                <div className="rentabilidad-item">
                  <div className="rentabilidad-header">
                    <span className="rentabilidad-label">Capacidad Utilizada</span>
                    <span className="rentabilidad-value">
                      {capacidad.porcentajeOcupacion}%
                    </span>
                  </div>
                  <div className="rentabilidad-bar">
                    <div 
                      className="rentabilidad-fill"
                      style={{ 
                        width: `${capacidad.porcentajeOcupacion}%`,
                        backgroundColor: capacidad.porcentajeOcupacion > 80 ? '#ef4444' : capacidad.porcentajeOcupacion > 60 ? '#f59e0b' : '#10b981'
                      }}
                    ></div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        {/* Actividad Reciente - MEJORADA */}
        <div className="dashboard-section">
          <h2>📊 Actividad Reciente (Últimos 7 días)</h2>
          <div className="actividad-card">
            <div className="actividad-header">
              <div className="actividad-title">
                <h3>Servicios por Día</h3>
                <p>Distribución de servicios en los últimos 7 días</p>
              </div>
              <div className="actividad-summary">
                <div className="summary-stat">
                  <span className="stat-label">Total Servicios</span>
                  <span className="stat-value">
                    {servicios.serviciosPorDia.reduce((total, dia) => total + dia.cantidad, 0)}
                  </span>
                </div>
                <div className="summary-stat">
                  <span className="stat-label">Promedio Diario</span>
                  <span className="stat-value">
                    {Math.round(servicios.serviciosPorDia.reduce((total, dia) => total + dia.cantidad, 0) / 7)}
                  </span>
                </div>
              </div>
            </div>
            
            <div className="actividad-chart">
              {servicios.serviciosPorDia && servicios.serviciosPorDia.length > 0 ? (
                servicios.serviciosPorDia.map((dia, index) => {
                  const maxCantidad = Math.max(...servicios.serviciosPorDia.map(d => d.cantidad));
                  const porcentaje = maxCantidad > 0 ? (dia.cantidad / maxCantidad) * 100 : 0;
                  
                  return (
                    <div key={index} className="dia-bar">
                      <div className="dia-info">
                        <span className="dia-fecha">
                          {new Date(dia.fecha).toLocaleDateString('es-CO', { 
                            weekday: 'short',
                            day: '2-digit', 
                            month: '2-digit' 
                          })}
                        </span>
                        <span className="dia-count">{dia.cantidad}</span>
                        <span className="dia-ingresos">${dia.ingresos?.toLocaleString() || '0'}</span>
                      </div>
                      <div className="dia-bar-container">
                        <div 
                          className="dia-bar-fill"
                          style={{ 
                            height: `${Math.max(porcentaje, 10)}%`,
                            backgroundColor: porcentaje > 70 ? '#10b981' : porcentaje > 40 ? '#f59e0b' : '#3b82f6'
                          }}
                        ></div>
                      </div>
                    </div>
                  );
                })
              ) : (
                <div className="no-data">
                  <p>No hay datos de actividad reciente</p>
                </div>
              )}
            </div>
            
            <div className="actividad-footer">
              <div className="footer-stats">
                <div className="footer-stat">
                  <span className="footer-label">Día más activo:</span>
                  <span className="footer-value">
                    {servicios.serviciosPorDia && servicios.serviciosPorDia.length > 0 ? 
                      new Date(servicios.serviciosPorDia.reduce((max, dia) => 
                        dia.cantidad > max.cantidad ? dia : max
                      ).fecha).toLocaleDateString('es-CO', { 
                        day: '2-digit', 
                        month: '2-digit' 
                      }) : 'N/A'
                    }
                  </span>
                </div>
                <div className="footer-stat">
                  <span className="footer-label">Servicios máximos:</span>
                  <span className="footer-value">
                    {servicios.serviciosPorDia && servicios.serviciosPorDia.length > 0 ? 
                      Math.max(...servicios.serviciosPorDia.map(d => d.cantidad)) : 0
                    }
                  </span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default DashboardAdmin;
