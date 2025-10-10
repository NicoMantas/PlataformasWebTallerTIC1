import React, { useState, useEffect } from 'react';
import { listVehiculos, createVehiculo, updateVehiculo, deleteVehiculo } from '../services/vehiculosService.js';
import { getCurrentUser } from '../services/authService';
import HistorialVehiculo from './HistorialVehiculo';
import '../styles/VehiculosManager.css';

const VehiculosManager = ({ onVehiculoSelect }) => {
  const [vehiculos, setVehiculos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [showForm, setShowForm] = useState(false);
  const [editingVehiculo, setEditingVehiculo] = useState(null);
  const [currentUser, setCurrentUser] = useState(null);
  const [showHistorial, setShowHistorial] = useState(false);
  const [selectedVehiculoForHistorial, setSelectedVehiculoForHistorial] = useState(null);
  const [formData, setFormData] = useState({
    placa: '',
    marca: '',
    modelo: '',
    anio: new Date().getFullYear(),
    tipo: 'Gasolina',
    cilindraje: '',
    capacidadBateria: ''
  });

  useEffect(() => {
    const u = getCurrentUser();
    setCurrentUser(u);
    loadVehiculos(u);
  }, []);

  const loadVehiculos = async (u) => {
    try {
      setLoading(true);
      const data = await listVehiculos();
      const userClientId = u?.infoEspecifica?.id || u?.idCliente;
      const filtered = (data || []).filter((v) => {
        const candidateIds = [v.idCliente, v.clienteId, v.id_cliente, v.idDueno, v.id_owner].filter((x) => x !== undefined && x !== null);
        if (candidateIds.length === 0) return true; // si backend no envía dueño, mostramos todos (fallback)
        return candidateIds.includes(userClientId);
      });
      setVehiculos(filtered);
    } catch (e) {
      setError(e?.message || 'Error al cargar vehículos');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const vehiculoData = {
        placa: formData.placa,
        marca: formData.marca,
        modelo: formData.modelo,
        anio: formData.anio,
        tipo: formData.tipo,
        cilindraje: formData.tipo === 'Gasolina' || formData.tipo === 'Hibrido' ? formData.cilindraje : null,
        capacidadBateria: formData.tipo === 'Electrico' || formData.tipo === 'Hibrido' ? formData.capacidadBateria : null
      };

      if (editingVehiculo) {
        await updateVehiculo(editingVehiculo.id, vehiculoData);
      } else {
        await createVehiculo(vehiculoData);
      }

      setShowForm(false);
      setEditingVehiculo(null);
      resetForm();
      loadVehiculos(currentUser);
    } catch (e) {
      setError(e?.message || 'Error al guardar vehículo');
    }
  };

  const handleEdit = (vehiculo) => {
    setEditingVehiculo(vehiculo);
    setFormData({
      placa: vehiculo.placa,
      marca: vehiculo.marca,
      modelo: vehiculo.modelo,
      anio: vehiculo.anio,
      tipo: vehiculo.tipoVehiculo || 'Gasolina',
      cilindraje: vehiculo.cilindraje || '',
      capacidadBateria: vehiculo.capacidadBateria || ''
    });
    setShowForm(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('¿Estás seguro de eliminar este vehículo?')) {
      try {
        await deleteVehiculo(id);
        loadVehiculos(currentUser);
      } catch (e) {
        setError(e?.message || 'Error al eliminar vehículo');
      }
    }
  };

  const handleShowHistorial = (vehiculo) => {
    setSelectedVehiculoForHistorial(vehiculo);
    setShowHistorial(true);
  };

  const handleCloseHistorial = () => {
    setShowHistorial(false);
    setSelectedVehiculoForHistorial(null);
  };

  const resetForm = () => {
    setFormData({
      placa: '',
      marca: '',
      modelo: '',
      anio: new Date().getFullYear(),
      tipo: 'Gasolina',
      cilindraje: '',
      capacidadBateria: ''
    });
  };

  const handleCancel = () => {
    setShowForm(false);
    setEditingVehiculo(null);
    resetForm();
  };

  if (loading) return <div className="loading">Cargando vehículos...</div>;
  if (error) return <div className="error">{error}</div>;

  return (
    <div className="vehiculos-manager">
      <div className="vehiculos-header">
        <h2>Mis Vehículos</h2>
        <button 
          className="btn-primary" 
          onClick={() => setShowForm(true)}
        >
          Agregar Vehículo
        </button>
      </div>

      {showForm && (
        <div className="vehiculo-form-overlay">
          <div className="vehiculo-form">
            <h3>{editingVehiculo ? 'Editar' : 'Agregar'} Vehículo</h3>
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label>Placa:</label>
                <input
                  type="text"
                  value={formData.placa}
                  onChange={(e) => setFormData({...formData, placa: e.target.value})}
                  required
                />
              </div>

              <div className="form-group">
                <label>Marca:</label>
                <input
                  type="text"
                  value={formData.marca}
                  onChange={(e) => setFormData({...formData, marca: e.target.value})}
                  required
                />
              </div>

              <div className="form-group">
                <label>Modelo:</label>
                <input
                  type="text"
                  value={formData.modelo}
                  onChange={(e) => setFormData({...formData, modelo: e.target.value})}
                  required
                />
              </div>

              <div className="form-group">
                <label>Año:</label>
                <input
                  type="number"
                  value={formData.anio}
                  onChange={(e) => setFormData({...formData, anio: parseInt(e.target.value)})}
                  min="1900"
                  max="2027"
                  required
                />
              </div>

              <div className="form-group">
                <label>Tipo:</label>
                <select
                  value={formData.tipo}
                  onChange={(e) => setFormData({...formData, tipo: e.target.value})}
                >
                  <option value="Gasolina">Gasolina</option>
                  <option value="Electrico">Eléctrico</option>
                  <option value="Hibrido">Híbrido</option>
                </select>
              </div>

              {(formData.tipo === 'Gasolina' || formData.tipo === 'Hibrido') && (
                <div className="form-group">
                  <label>Cilindraje:</label>
                  <input
                    type="number"
                    value={formData.cilindraje}
                    onChange={(e) => setFormData({...formData, cilindraje: e.target.value})}
                    placeholder="Ej: 1600"
                  />
                </div>
              )}

              {(formData.tipo === 'Electrico' || formData.tipo === 'Hibrido') && (
                <div className="form-group">
                  <label>Capacidad Batería (kWh):</label>
                  <input
                    type="number"
                    value={formData.capacidadBateria}
                    onChange={(e) => setFormData({...formData, capacidadBateria: e.target.value})}
                    placeholder="Ej: 50"
                  />
                </div>
              )}

              <div className="form-actions">
                <button type="submit" className="btn-primary">
                  {editingVehiculo ? 'Actualizar' : 'Crear'}
                </button>
                <button type="button" className="btn-secondary" onClick={handleCancel}>
                  Cancelar
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      <div className="vehiculos-list">
        {vehiculos.length === 0 ? (
          <p>No tienes vehículos registrados</p>
        ) : (
          vehiculos.map(vehiculo => (
            <div key={vehiculo.id} className="vehiculo-card">
              <div className="vehiculo-info">
                <h3>{vehiculo.marca} {vehiculo.modelo} ({vehiculo.anio})</h3>
                <p><strong>Placa:</strong> {vehiculo.placa}</p>
                <p><strong>Tipo:</strong> {vehiculo.tipoVehiculo}</p>
                {vehiculo.cilindraje && <p><strong>Cilindraje:</strong> {vehiculo.cilindraje}cc</p>}
                {vehiculo.capacidadBateria && <p><strong>Batería:</strong> {vehiculo.capacidadBateria}kWh</p>}
              </div>
              <div className="vehiculo-actions">
                <button 
                  className="btn-primary"
                  onClick={() => onVehiculoSelect && onVehiculoSelect(vehiculo)}
                >
                  Seleccionar
                </button>
                <button 
                  className="btn-secondary"
                  onClick={() => handleEdit(vehiculo)}
                >
                  Editar
                </button>
                <button 
                  className="btn-info"
                  onClick={() => handleShowHistorial(vehiculo)}
                >
                  Historial
                </button>
                <button 
                  className="btn-danger"
                  onClick={() => handleDelete(vehiculo.id)}
                >
                  Eliminar
                </button>
              </div>
            </div>
          ))
        )}
      </div>

      {showHistorial && selectedVehiculoForHistorial && (
        <HistorialVehiculo 
          vehiculo={selectedVehiculoForHistorial}
          onClose={handleCloseHistorial}
        />
      )}
    </div>
  );
};

export default VehiculosManager;
