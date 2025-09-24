// src/pages/HomeEmpresa.jsx
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import '../styles/HomeEmpresa.css';
import api from '../services/api';
import { listServicios, crearOrden } from '../services/serviciosService';

const HomeEmpresa = () => {
  const navigate = useNavigate();
  const [servicios, setServicios] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    (async () => {
      try {
        const data = await listServicios(api);
        setServicios(data || []);
      } catch (e) {
        setError(e?.message || 'No se pudieron cargar los servicios');
      } finally {
        setLoading(false);
      }
    })();
  }, []);

  return (
    <div className="home-empresa-page">
      <Header />
      
      <div className="empresa-container">
        <div className="empresa-header">
          <h1>Panel de Control - Empresa</h1>
          <p>Gestiona tus flotas vehiculares y servicios corporativos</p>
        </div>
        
        <div className="empresa-dashboard">
          <h2>Servicios Disponibles</h2>
          {loading && <p>Cargando servicios...</p>}
          {error && <div className="error-message">{error}</div>}
          <div className="servicios-list">
            {servicios.map((s) => (
              <div key={s.id} className="servicio-item">
                <p>{s.nombre} - {s.descripcion}</p>
                <button
                  className="btn-primary"
                  onClick={async () => {
                    try {
                      await crearOrden(api, {
                        clienteId: 0,
                        vehiculoId: 0,
                        serviciosIds: [s.id]
                      });
                      alert('Solicitud enviada');
                    } catch (e) {
                      alert(e?.message || 'No se pudo solicitar');
                    }
                  }}
                >
                  Solicitar
                </button>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
};

export default HomeEmpresa;