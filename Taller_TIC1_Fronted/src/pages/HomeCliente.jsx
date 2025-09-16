// src/pages/HomeCliente.jsx
import React from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import '../styles/HomeCliente.css';

const HomeCliente = () => {
  const navigate = useNavigate();

  return (
    <div className="home-cliente-page">
      <Header />
      
      <div className="cliente-container">
        <div className="cliente-header">
          <h1>Bienvenido de vuelta, Juan Pérez</h1>
          <p>Gestiona tus vehículos y servicios de manera fácil y rápida</p>
        </div>
        
        <div className="cliente-dashboard">
          <div className="vehiculos-section">
            <h2>Mis Vehículos</h2>
            <div className="vehiculos-list">
              <div className="vehiculo-card">
                <h3>Toyota Corolla 2020</h3>
                <p>Placa: ABC-1234</p>
                <p>Próximo mantenimiento: 15 Dic 2023</p>
                <button className="btn-primary">Solicitar Servicio</button>
              </div>
              
              <div className="vehiculo-card">
                <h3>Honda Civic 2018</h3>
                <p>Placa: XYZ-5678</p>
                <p>Próximo mantenimiento: 20 Ene 2024</p>
                <button className="btn-primary">Solicitar Servicio</button>
              </div>
            </div>
            
            <button className="btn-secondary">Agregar Vehículo</button>
          </div>
          
          <div className="servicios-section">
            <h2>Servicios Recientes</h2>
            <div className="servicios-list">
              <div className="servicio-item">
                <p>Cambio de aceite y filtro - Toyota Corolla</p>
                <span className="servicio-status completed">Completado</span>
                <span className="servicio-date">05 Nov 2023</span>
              </div>
              <div className="servicio-item">
                <p>Revisión de frenos - Honda Civic</p>
                <span className="servicio-status in-progress">En Proceso</span>
                <span className="servicio-date">02 Nov 2023</span>
              </div>
              <div className="servicio-item">
                <p>Alineación y balanceo - Toyota Corolla</p>
                <span className="servicio-status completed">Completado</span>
                <span className="servicio-date">25 Oct 2023</span>
              </div>
            </div>
            
            <button className="btn-secondary">Ver Historial Completo</button>
          </div>
          
          <div className="quick-actions">
            <h2>Acciones Rápidas</h2>
            <div className="action-buttons">
              <button className="btn-primary">Solicitar Servicio</button>
              <button className="btn-primary">Agendar Cita</button>
              <button className="btn-primary">Consultar Promociones</button>
              <button className="btn-primary">Contactar Soporte</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default HomeCliente;