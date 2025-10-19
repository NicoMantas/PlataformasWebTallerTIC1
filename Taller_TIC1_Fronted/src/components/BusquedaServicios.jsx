import React, { useState } from 'react';
import { buscarServiciosPorPlaca, buscarServiciosPorFecha, buscarServiciosPorPlacaYFecha } from '../services/serviciosService';
import '../styles/BusquedaServicios.css';

const BusquedaServicios = ({ onClose }) => {
  const [busqueda, setBusqueda] = useState({
    placa: '',
    fechaInicio: '',
    fechaFin: ''
  });
  const [resultados, setResultados] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setBusqueda(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleBuscar = async () => {
    setLoading(true);
    setError('');
    setResultados([]);

    try {
      let data = [];

      if (busqueda.placa && busqueda.fechaInicio && busqueda.fechaFin) {
        // Búsqueda por placa y fechas
        data = await buscarServiciosPorPlacaYFecha(
          busqueda.placa,
          busqueda.fechaInicio,
          busqueda.fechaFin
        );
      } else if (busqueda.placa) {
        // Búsqueda solo por placa
        data = await buscarServiciosPorPlaca(busqueda.placa);
      } else if (busqueda.fechaInicio && busqueda.fechaFin) {
        // Búsqueda solo por fechas
        data = await buscarServiciosPorFecha(busqueda.fechaInicio, busqueda.fechaFin);
      } else {
        setError('Debe ingresar al menos una placa o un rango de fechas');
        return;
      }

      setResultados(data || []);
    } catch (err) {
      setError(err.message || 'Error al realizar la búsqueda');
    } finally {
      setLoading(false);
    }
  };

  const getEstadoColor = (estado) => {
    switch (estado?.toLowerCase()) {
      case 'pendiente': return '#f59e0b';
      case 'en proceso': return '#3b82f6';
      case 'en revisión': return '#8b5cf6';
      case 'completado': return '#10b981';
      case 'cancelado': return '#ef4444';
      default: return '#6b7280';
    }
  };

  const formatearFecha = (fecha) => {
    return new Date(fecha).toLocaleString('es-CO');
  };

  const formatearPrecio = (precio) => {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP'
    }).format(precio);
  };

  return (
    <div className="busqueda-modal-overlay">
      <div className="busqueda-modal">
        <div className="busqueda-header">
          <h2>Búsqueda de Servicios</h2>
          <button className="close-btn" onClick={onClose}>×</button>
        </div>

        <div className="busqueda-content">
          {/* Formulario de búsqueda */}
          <div className="busqueda-form">
            <div className="form-row">
              <div className="form-group">
                <label htmlFor="placa">Placa del Vehículo</label>
                <input
                  type="text"
                  id="placa"
                  name="placa"
                  value={busqueda.placa}
                  onChange={handleInputChange}
                  placeholder="Ej: ABC123"
                  className="form-input"
                />
              </div>
            </div>

            <div className="form-row">
              <div className="form-group">
                <label htmlFor="fechaInicio">Fecha Inicio</label>
                <input
                  type="date"
                  id="fechaInicio"
                  name="fechaInicio"
                  value={busqueda.fechaInicio}
                  onChange={handleInputChange}
                  className="form-input"
                />
              </div>
              <div className="form-group">
                <label htmlFor="fechaFin">Fecha Fin</label>
                <input
                  type="date"
                  id="fechaFin"
                  name="fechaFin"
                  value={busqueda.fechaFin}
                  onChange={handleInputChange}
                  className="form-input"
                />
              </div>
            </div>

            <div className="form-actions">
              <button 
                onClick={handleBuscar} 
                disabled={loading}
                className="btn-buscar"
              >
                {loading ? 'Buscando...' : 'Buscar Servicios'}
              </button>
            </div>
          </div>

          {/* Resultados */}
          {error && (
            <div className="error-message">
              {error}
            </div>
          )}

          {resultados.length > 0 && (
            <div className="resultados-section">
              <h3>Resultados ({resultados.length})</h3>
              <div className="resultados-grid">
                {resultados.map((servicio) => (
                  <div key={servicio.id} className="servicio-card">
                    <div className="servicio-header">
                      <h4>Servicio #{servicio.id}</h4>
                      <span 
                        className="estado-badge"
                        style={{ backgroundColor: getEstadoColor(servicio.estadoDescripcion) }}
                      >
                        {servicio.estadoDescripcion}
                      </span>
                    </div>

                    <div className="servicio-info">
                      <div className="info-row">
                        <strong>Tipo:</strong> {servicio.tipoServicio}
                      </div>
                      <div className="info-row">
                        <strong>Vehículo:</strong> {servicio.vehiculoMarca} {servicio.vehiculoModelo} - {servicio.vehiculoPlaca}
                      </div>
                      <div className="info-row">
                        <strong>Cliente:</strong> {servicio.clienteNombre}
                      </div>
                      {servicio.empleadoNombre && (
                        <div className="info-row">
                          <strong>Mecánico:</strong> {servicio.empleadoNombre}
                        </div>
                      )}
                      <div className="info-row">
                        <strong>Fecha:</strong> {formatearFecha(servicio.fechaCreacion)}
                      </div>
                      <div className="info-row">
                        <strong>Costo:</strong> {formatearPrecio(servicio.costo)}
                      </div>
                    </div>

                    {servicio.detallesRevision && (
                      <div className="servicio-detalles">
                        <strong>Detalles:</strong> {servicio.detallesRevision}
                      </div>
                    )}
                  </div>
                ))}
              </div>
            </div>
          )}

          {resultados.length === 0 && !loading && !error && busqueda.placa && (
            <div className="no-results">
              <p>No se encontraron servicios con los criterios de búsqueda.</p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default BusquedaServicios;
