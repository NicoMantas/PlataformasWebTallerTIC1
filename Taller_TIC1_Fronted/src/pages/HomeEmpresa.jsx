// src/pages/HomeEmpresa.jsx
import React from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import '../styles/HomeEmpresa.css';

const HomeEmpresa = () => {
  const navigate = useNavigate();

  return (
    <div className="home-empresa-page">
      <Header />
      
      <div className="empresa-container">
        <div className="empresa-header">
          <h1>Panel de Control - Empresa</h1>
          <p>Gestiona tus flotas vehiculares y servicios corporativos</p>
        </div>
        
        <div className="empresa-dashboard">
          <div className="dashboard-cards">
            <div className="dashboard-card">
              <h3>Vehículos Registrados</h3>
              <p className="stat">24</p>
              <button className="btn-secondary">Ver Detalles</button>
            </div>
            
            <div className="dashboard-card">
              <h3>Servicios Activos</h3>
              <p className="stat">8</p>
              <button className="btn-secondary">Ver Detalles</button>
            </div>
            
            <div className="dashboard-card">
              <h3>Próximos Mantenimientos</h3>
              <p className="stat">5</p>
              <button className="btn-secondary">Ver Detalles</button>
            </div>
          </div>
          
          <div className="empresa-actions">
            <h2>Acciones Rápidas</h2>
            <div className="action-buttons">
              <button className="btn-primary">Registrar Nuevo Vehículo</button>
              <button className="btn-primary">Solicitar Servicio</button>
              <button className="btn-primary">Ver Historial</button>
              <button className="btn-primary">Generar Reporte</button>
            </div>
          </div>
          
          <div className="recent-activity">
            <h2>Actividad Reciente</h2>
            <div className="activity-list">
              <div className="activity-item">
                <p>Cambio de aceite - Toyota Corolla</p>
                <span className="activity-date">15 Nov 2023</span>
              </div>
              <div className="activity-item">
                <p>Revisión de frenos - Honda Civic</p>
                <span className="activity-date">12 Nov 2023</span>
              </div>
              <div className="activity-item">
                <p>Alineación y balanceo - Nissan Sentra</p>
                <span className="activity-date">10 Nov 2023</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default HomeEmpresa;