import React, { useState, useEffect } from 'react';
import { listHistorialByVehiculo } from '../services/serviciosService';
import './HistorialVehiculo.css';

const HistorialVehiculo = ({ vehiculo, onClose }) => {
  const [historial, setHistorial] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    if (vehiculo?.id) {
      loadHistorial();
    }
  }, [vehiculo?.id]);

  const loadHistorial = async () => {
    try {
      setLoading(true);
      const data = await listHistorialByVehiculo(vehiculo.id);
      setHistorial(data || []);
    } catch (err) {
      setError(err?.message || 'Error al cargar historial');
    } finally {
      setLoading(false);
    }
  };

  const formatFecha = (fecha) => {
    if (!fecha) return 'N/D';
    return new Date(fecha).toLocaleDateString('es-CO', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  };

  const getEstadoColor = (estado) => {
    switch (estado?.toLowerCase()) {
      case 'completado':
        return 'estado-completado';
      case 'cancelado':
        return 'estado-cancelado';
      case 'en proceso':
        return 'estado-proceso';
      case 'pendiente':
        return 'estado-pendiente';
      default:
        return 'estado-default';
    }
  };

  if (loading) {
    return (
      <div className="historial-modal-overlay">
        <div className="historial-modal-content">
          <div className="historial-loading">Cargando historial...</div>
        </div>
      </div>
    );
  }

  return (
    <div className="historial-modal-overlay" onClick={onClose}>
      <div className="historial-modal-content" onClick={(e) => e.stopPropagation()}>
        <div className="historial-header">
          <h3>Historial de Servicios</h3>
          <button className="historial-close-btn" onClick={onClose}>×</button>
        </div>
        
        <div className="historial-vehiculo-info">
          <h4>{vehiculo?.marca} {vehiculo?.modelo}</h4>
          <p><strong>Placa:</strong> {vehiculo?.placa}</p>
          <p><strong>Año:</strong> {vehiculo?.anio}</p>
          <p><strong>Tipo:</strong> {vehiculo?.tipoVehiculo}</p>
        </div>

        {error && (
          <div className="historial-error">
            {error}
          </div>
        )}

        {historial.length === 0 ? (
          <div className="historial-empty">
            <p>No hay servicios registrados para este vehículo.</p>
          </div>
        ) : (
          <div className="historial-list">
            {historial.map((servicio) => (
              <div key={servicio.id} className="historial-item">
                <div className="historial-item-header">
                  <div className="historial-item-title">
                    Servicio #{servicio.id} · {servicio.tipoServicio === 'Revision' ? 'Revisión' : 'Reparación'}
                  </div>
                  <div className={`historial-estado ${getEstadoColor(servicio.estadoDescripcion)}`}>
                    {servicio.estadoDescripcion}
                  </div>
                </div>
                
                <div className="historial-item-details">
                  <div className="historial-detail-row">
                    <span className="historial-detail-label">Fecha:</span>
                    <span className="historial-detail-value">{formatFecha(servicio.fechaCreacion)}</span>
                  </div>
                  
                  {servicio.detallesRevision && (
                    <div className="historial-detail-row">
                      <span className="historial-detail-label">Detalles:</span>
                      <span className="historial-detail-value">{servicio.detallesRevision}</span>
                    </div>
                  )}
                  
                  {servicio.empleadoNombre && (
                    <div className="historial-detail-row">
                      <span className="historial-detail-label">Mecánico:</span>
                      <span className="historial-detail-value">{servicio.empleadoNombre}</span>
                    </div>
                  )}
                  
                  {servicio.costo > 0 && (
                    <div className="historial-detail-row">
                      <span className="historial-detail-label">Costo:</span>
                      <span className="historial-detail-value">${servicio.costo.toLocaleString()}</span>
                    </div>
                  )}
                  
                  {servicio.repuestosReparacion && servicio.repuestosReparacion.length > 0 && (
                    <div className="historial-detail-row">
                      <span className="historial-detail-label">Repuestos:</span>
                      <div className="historial-repuestos">
                        {servicio.repuestosReparacion.map((repuesto, index) => (
                          <span key={index} className="historial-repuesto">
                            {repuesto.nombre} (x{repuesto.cantidad})
                          </span>
                        ))}
                      </div>
                    </div>
                  )}
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default HistorialVehiculo;
