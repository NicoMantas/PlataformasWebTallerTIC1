import React, { useState, useEffect } from 'react';
import { getProgresoServicio } from '../services/serviciosService';
import '../styles/ProgresoServicio.css';

const ProgresoServicio = ({ servicioId, onClose }) => {
  const [progreso, setProgreso] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    loadProgreso();
  }, [servicioId]);

  const loadProgreso = async () => {
    try {
      setLoading(true);
      const data = await getProgresoServicio(servicioId);
      setProgreso(data);
    } catch (err) {
      setError(err.message || 'Error al cargar el progreso');
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <div className="loading">Cargando progreso...</div>;
  if (error) return <div className="error">{error}</div>;
  if (!progreso) return <div className="error">No se encontró información del servicio</div>;

  return (
    <div className="progreso-modal-overlay">
      <div className="progreso-modal">
        <div className="progreso-header">
          <h2>Seguimiento del Servicio #{progreso.id}</h2>
          <button className="close-btn" onClick={onClose}>×</button>
        </div>

        <div className="progreso-content">
          {/* Información del servicio */}
          <div className="servicio-info">
            <h3>{progreso.vehiculoMarca} {progreso.vehiculoModelo} - {progreso.vehiculoPlaca}</h3>
            <p><strong>Tipo:</strong> {progreso.tipoServicio}</p>
            <p><strong>Estado actual:</strong> <span className={`estado-badge estado-${progreso.idEstado}`}>{progreso.estadoActual}</span></p>
            {progreso.mecanicoNombre && <p><strong>Mecánico asignado:</strong> {progreso.mecanicoNombre}</p>}
          </div>

          {/* Barra de progreso */}
          <div className="progress-section">
            <div className="progress-header">
              <h4>Progreso del Servicio</h4>
              <span className="progress-percentage">{progreso.porcentajeCompletado}%</span>
            </div>
            <div className="progress-bar">
              <div 
                className="progress-fill" 
                style={{ width: `${progreso.porcentajeCompletado}%` }}
              ></div>
            </div>
          </div>

          {/* Flujo de estados */}
          <div className="estados-flujo">
            <h4>Estados del Servicio</h4>
            <div className="estados-timeline">
              {progreso.flujoEstados.map((estado, index) => (
                <div key={estado.id} className={`estado-item ${estado.completado ? 'completado' : ''} ${estado.actual ? 'actual' : ''}`}>
                  <div className="estado-icon">
                    <span>{estado.icono}</span>
                  </div>
                  <div className="estado-content">
                    <h5>{estado.descripcion}</h5>
                    <p>{estado.mensaje}</p>
                    {estado.fechaCompletado && (
                      <small>Completado: {new Date(estado.fechaCompletado).toLocaleString()}</small>
                    )}
                  </div>
                  {index < progreso.flujoEstados.length - 1 && (
                    <div className="estado-connector"></div>
                  )}
                </div>
              ))}
            </div>
          </div>

          {/* Mensaje del estado actual */}
          <div className="estado-mensaje">
            <h4>Información Actual</h4>
            <p className="mensaje-texto">{progreso.mensajeEstado}</p>
          </div>

          {/* Información adicional */}
          <div className="info-adicional">
            <div className="info-item">
              <strong>Fecha de creación:</strong> {new Date(progreso.fechaCreacion).toLocaleString()}
            </div>
            {progreso.fechaActualizacion && (
              <div className="info-item">
                <strong>Última actualización:</strong> {new Date(progreso.fechaActualizacion).toLocaleString()}
              </div>
            )}
            <div className="info-item">
              <strong>Costo estimado:</strong> ${progreso.costo.toLocaleString()}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProgresoServicio;
