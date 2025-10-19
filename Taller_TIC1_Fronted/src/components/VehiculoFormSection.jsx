import React, { useState } from 'react';
import './VehiculoFormSection.css';

const VehiculoFormSection = ({ formData, setFormData, errors }) => {
  const [showVehiculoForm, setShowVehiculoForm] = useState(false);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    // Si setFormData es una función que acepta (name, value), usarla
    if (typeof setFormData === 'function' && setFormData.length === 2) {
      setFormData(name, value);
    } else {
      // Si es la función normal de React, usarla como antes
      setFormData(prev => ({
        ...prev,
        [name]: value
      }));
    }
  };

  const toggleVehiculoForm = () => {
    setShowVehiculoForm(!showVehiculoForm);
    if (!showVehiculoForm) {
      // Si se está ocultando el formulario, limpiar los datos del vehículo
      const camposVehiculo = ['placa', 'marca', 'modelo', 'anio', 'tipoVehiculo'];
      camposVehiculo.forEach(campo => {
        if (typeof setFormData === 'function' && setFormData.length === 2) {
          setFormData(campo, campo === 'tipoVehiculo' ? 'Gasolina' : '');
        } else {
          setFormData(prev => ({
            ...prev,
            [campo]: campo === 'tipoVehiculo' ? 'Gasolina' : ''
          }));
        }
      });
    }
  };

  return (
    <div className="vehiculo-form-section">
      <div className="vehiculo-toggle">
        <button
          type="button"
          className={`toggle-btn ${showVehiculoForm ? 'active' : ''}`}
          onClick={toggleVehiculoForm}
        >
          <span className="toggle-icon">{showVehiculoForm ? '−' : '+'}</span>
          <span className="toggle-text">
            {showVehiculoForm ? 'Ocultar información del vehículo' : 'Agregar información del vehículo (opcional)'}
          </span>
        </button>
      </div>

      {showVehiculoForm && (
        <div className="vehiculo-form-grid">
          <div className="form-section-header">
            <h4>Información del Vehículo</h4>
            <p>Complete los datos de su vehículo (opcional)</p>
          </div>
          
          <div className="vehiculo-grid">
            <div className="form-group">
              <label htmlFor="placa" className="form-label">
                Placa del Vehículo
              </label>
              <input
                type="text"
                id="placa"
                name="placa"
                value={formData.placa || ''}
                onChange={handleInputChange}
                className={`form-input ${errors.placa ? 'error' : ''}`}
                placeholder="ABC123"
              />
              {errors.placa && <span className="error-message">{errors.placa}</span>}
            </div>

            <div className="form-group">
              <label htmlFor="marca" className="form-label">
                Marca
              </label>
              <input
                type="text"
                id="marca"
                name="marca"
                value={formData.marca || ''}
                onChange={handleInputChange}
                className={`form-input ${errors.marca ? 'error' : ''}`}
                placeholder="Toyota, Chevrolet, etc."
              />
              {errors.marca && <span className="error-message">{errors.marca}</span>}
            </div>

            <div className="form-group">
              <label htmlFor="modelo" className="form-label">
                Modelo
              </label>
              <input
                type="text"
                id="modelo"
                name="modelo"
                value={formData.modelo || ''}
                onChange={handleInputChange}
                className={`form-input ${errors.modelo ? 'error' : ''}`}
                placeholder="Corolla, Spark, etc."
              />
              {errors.modelo && <span className="error-message">{errors.modelo}</span>}
            </div>

            <div className="form-group">
              <label htmlFor="anio" className="form-label">
                Año
              </label>
              <input
                type="number"
                id="anio"
                name="anio"
                value={formData.anio || ''}
                onChange={handleInputChange}
                className={`form-input ${errors.anio ? 'error' : ''}`}
                placeholder="2020"
                min="1900"
                max={new Date().getFullYear() + 1}
              />
              {errors.anio && <span className="error-message">{errors.anio}</span>}
            </div>

            <div className="form-group full-width">
              <label htmlFor="tipoVehiculo" className="form-label">
                Tipo de Vehículo
              </label>
              <select
                id="tipoVehiculo"
                name="tipoVehiculo"
                value={formData.tipoVehiculo || 'Gasolina'}
                onChange={handleInputChange}
                className={`form-input ${errors.tipoVehiculo ? 'error' : ''}`}
              >
                <option value="Gasolina">Gasolina</option>
                <option value="Electrico">Eléctrico</option>
                <option value="Hibrido">Híbrido</option>
              </select>
              {errors.tipoVehiculo && <span className="error-message">{errors.tipoVehiculo}</span>}
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default VehiculoFormSection;
