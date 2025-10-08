import React from 'react';
import './Footer.css';

const Footer = () => {
  return (
    <footer className="footer">
      <div className="footer-container">
        <div className="footer-content">
          <div className="footer-section">
            <h3>AutoCare</h3>
            <p>Expertos en cuidado automotriz con más de 10 años de experiencia.</p>
          </div>
          
          <div className="footer-section">
            <h4>Servicios</h4>
            <ul>
              <li>Mantenimiento Preventivo</li>
              <li>Reparación de Motor</li>
              <li>Alineación y Balanceo</li>
              <li>Cambio de Aceite</li>
            </ul>
          </div>
          
          <div className="footer-section">
            <h4>Contacto</h4>
            <p>📍 Calle 123 #45-67, Bogotá</p>
            <p>📞 +57 (1) 234-5678</p>
            <p>✉️ info@autocare.com</p>
          </div>
          
          <div className="footer-section">
            <h4>Horarios</h4>
            <p>Lunes - Viernes: 7:00 AM - 6:00 PM</p>
            <p>Sábados: 7:00 AM - 4:00 PM</p>
            <p>Domingos: Cerrado</p>
          </div>
        </div>
        
        <div className="footer-bottom">
          <p>&copy; 2024 AutoCare. Todos los derechos reservados.</p>
        </div>
      </div>
    </footer>
  );
};

export default Footer;
