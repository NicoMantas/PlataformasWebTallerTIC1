import React from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import Header from '../components/Header';
import '../styles/HomeEmpleado.css';

const HomeEmpleado = () => {
  const navigate = useNavigate();
  const { tipo } = useParams(); // mecanico, secretaria, administrador

  // Contenido específico para cada tipo de empleado
  const getEmpleadoContent = () => {
    switch(tipo) {
      case 'mecanico':
        return {
          title: 'Panel de Mecánico',
          subtitle: 'Gestiona tus reparaciones y mantenimientos',
          features: [
            { title: 'Reparaciones Asignadas', count: 8, action: () => navigate('/reparaciones') },
            { title: 'Vehiculos en Taller', count: 5, action: () => navigate('/vehiculos') },
            { title: 'Tareas Pendientes', count: 3, action: () => navigate('/tareas') }
          ],
          actions: [
            { label: 'Registrar Diagnóstico', action: () => navigate('/diagnostico') },
            { label: 'Actualizar Estado Reparación', action: () => navigate('/estado-reparacion') },
            { label: 'Solicitar Repuestos', action: () => navigate('/repuestos') }
          ]
        };
      
      case 'secretaria':
        return {
          title: 'Panel de Secretaría',
          subtitle: 'Gestiona citas, clientes y administración',
          features: [
            { title: 'Citas Hoy', count: 12, action: () => navigate('/citas') },
            { title: 'Clientes Registrados', count: 45, action: () => navigate('/clientes') },
            { title: 'Facturas Pendientes', count: 7, action: () => navigate('/facturas') }
          ],
          actions: [
            { label: 'Agendar Cita', action: () => navigate('/agendar-cita') },
            { label: 'Registrar Cliente', action: () => navigate('/registrar-cliente') },
            { label: 'Generar Factura', action: () => navigate('/generar-factura') }
          ]
        };
      
      case 'administrador':
        return {
          title: 'Panel de Administración',
          subtitle: 'Gestiona el taller completo',
          features: [
            { title: 'Empleados', count: 15, action: () => navigate('/empleados') },
            { title: 'Vehículos en Taller', count: 8, action: () => navigate('/vehiculos-taller') },
            { title: 'Ingresos Mensuales', count: '$12,450', action: () => navigate('/finanzas') }
          ],
          actions: [
            { label: 'Gestionar Empleados', action: () => navigate('/gestion-empleados') },
            { label: 'Reportes Financieros', action: () => navigate('/reportes') },
            { label: 'Configuración Sistema', action: () => navigate('/configuracion') }
          ]
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
            <span>Conectado como: {localStorage.getItem('userEmail')}</span>
          </div>
        </div>
        
        <div className="dashboard-section">
          <h2>Resumen</h2>
          <div className="stats-grid">
            {content.features.map((feature, index) => (
              <div key={index} className="stat-card" onClick={feature.action}>
                <h3>{feature.title}</h3>
                <div className="stat-value">{feature.count}</div>
                <button className="btn-secondary">Ver detalles</button>
              </div>
            ))}
          </div>
        </div>
        
        <div className="actions-section">
          <h2>Acciones Rápidas</h2>
          <div className="actions-grid">
            {content.actions.map((action, index) => (
              <button
                key={index}
                className="btn-primary action-btn"
                onClick={action.action}
              >
                {action.label}
              </button>
            ))}
          </div>
        </div>
        
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