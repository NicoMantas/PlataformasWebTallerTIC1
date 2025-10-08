import axios from 'axios';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import Footer from '../components/Footer';
import '../styles/LandingPage.css';

const LandingPage = () => {
  const navigate = useNavigate();

  return (
    <div className="landing-page">
      <Header />
      
      <section className="hero-section">
        <div className="hero-content">
          <h1>Expertos en cuidado automotriz</h1>
          <p>Servicios de calidad para tu vehículo con profesionales certificados</p>
          <div className="cta-buttons">
            <button 
              className="btn-primary"
              onClick={() => navigate('/roles')}
            >
              Regístrate
            </button>
            <button 
              className="btn-secondary"
              onClick={() => navigate('/login-roles')}
            >
              Iniciar Sesión
            </button>
          </div>
        </div>
      </section>

      <section className="features">
        <div className="container">
          <h2>Nuestros Servicios</h2>
          <div className="features-grid">
            <div className="feature-card">
              <div className="feature-icon">🔧</div>
              <h3>Mantenimiento Preventivo</h3>
              <p>Alarga la vida de tu vehículo con nuestros servicios programados</p>
            </div>
            <div className="feature-card">
              <div className="feature-icon">🚗</div>
              <h3>Reparación de Motor</h3>
              <p>Expertos en diagnóstico y reparación de sistemas motrices</p>
            </div>
            <div className="feature-card">
              <div className="feature-icon">⚙️</div>
              <h3>Alineación y Balanceo</h3>
              <p>Mejora el rendimiento y seguridad de tu vehículo</p>
            </div>
          </div>
        </div>
      </section>

      <section className="testimonials">
        <div className="container">
          <h2>Lo que dicen nuestros clientes</h2>
          <div className="testimonials-grid">
            <div className="testimonial-card">
              <p>"Excelente servicio, mi auto quedó como nuevo. Muy profesionales."</p>
              <span>- Carlos Mendoza</span>
            </div>
            <div className="testimonial-card">
              <p>"Rápidos y eficientes. Solucionaron el problema de mi motor en tiempo récord."</p>
              <span>- Laura Sánchez</span>
            </div>
            <div className="testimonial-card">
              <p>"Llevo todos mis vehículos de la empresa aquí. Confiable y de calidad."</p>
              <span>- Javier Rodríguez</span>
            </div>
          </div>
        </div>
      </section>

      <Footer />
    </div>
  );
};

export default LandingPage;