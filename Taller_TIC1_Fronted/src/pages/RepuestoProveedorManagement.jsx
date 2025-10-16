import React, { useState, useEffect } from 'react';
import { 
  listRepuestoProveedores, 
  createRepuestoProveedor, 
  deleteRepuestoProveedor,
  getRepuestosByProveedor,
  getProveedoresByRepuesto
} from '../services/repuestoProveedorService';
import { listRepuestos } from '../services/repuestosService';
import { listProveedores } from '../services/proveedoresService';
import '../styles/ManagementStyles.css';

const RepuestoProveedorManagement = () => {
  const [relaciones, setRelaciones] = useState([]);
  const [repuestos, setRepuestos] = useState([]);
  const [proveedores, setProveedores] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [showModal, setShowModal] = useState(false);
  const [formData, setFormData] = useState({
    idRepuesto: '',
    idProveedor: ''
  });
  const [viewMode, setViewMode] = useState('todas'); // 'todas', 'porRepuesto', 'porProveedor'
  const [selectedId, setSelectedId] = useState('');

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    setLoading(true);
    try {
      const [relacionesData, repuestosData, proveedoresData] = await Promise.all([
        listRepuestoProveedores(),
        listRepuestos(),
        listProveedores()
      ]);
      setRelaciones(relacionesData || []);
      setRepuestos(repuestosData || []);
      setProveedores(proveedoresData || []);
      setError('');
    } catch (err) {
      setError('Error al cargar datos: ' + err.message);
    } finally {
      setLoading(false);
    }
  };

  const loadRelacionesFiltradas = async () => {
    if (!selectedId) return;
    
    setLoading(true);
    try {
      let data;
      if (viewMode === 'porRepuesto') {
        data = await getProveedoresByRepuesto(parseInt(selectedId));
      } else if (viewMode === 'porProveedor') {
        data = await getRepuestosByProveedor(parseInt(selectedId));
      } else {
        data = await listRepuestoProveedores();
      }
      setRelaciones(data || []);
      setError('');
    } catch (err) {
      setError('Error al cargar relaciones: ' + err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      await createRepuestoProveedor({
        idRepuesto: parseInt(formData.idRepuesto),
        idProveedor: parseInt(formData.idProveedor)
      });
      setShowModal(false);
      setFormData({ idRepuesto: '', idProveedor: '' });
      loadData();
      setError('');
    } catch (err) {
      setError('Error al crear relación: ' + err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (idRepuesto, idProveedor) => {
    if (!window.confirm('¿Está seguro de que desea eliminar esta relación?')) {
      return;
    }

    setLoading(true);
    try {
      await deleteRepuestoProveedor(idRepuesto, idProveedor);
      loadData();
      setError('');
    } catch (err) {
      setError('Error al eliminar relación: ' + err.message);
    } finally {
      setLoading(false);
    }
  };

  const openModal = () => {
    setFormData({ idRepuesto: '', idProveedor: '' });
    setShowModal(true);
  };

  const closeModal = () => {
    setShowModal(false);
    setFormData({ idRepuesto: '', idProveedor: '' });
  };

  const handleViewModeChange = (mode) => {
    setViewMode(mode);
    setSelectedId('');
    if (mode === 'todas') {
      loadData();
    }
  };

  const getRepuestoName = (id) => {
    const repuesto = repuestos.find(r => r.id === id);
    return repuesto ? repuesto.nombre : `Repuesto #${id}`;
  };

  const getProveedorName = (id) => {
    const proveedor = proveedores.find(p => p.id === id);
    return proveedor ? proveedor.nombre : `Proveedor #${id}`;
  };

  const getFilteredOptions = () => {
    if (viewMode === 'porRepuesto') {
      return repuestos.map(repuesto => ({
        value: repuesto.id,
        label: repuesto.nombre
      }));
    } else if (viewMode === 'porProveedor') {
      return proveedores.map(proveedor => ({
        value: proveedor.id,
        label: proveedor.nombre
      }));
    }
    return [];
  };

  return (
    <div className="management-container">
      <div className="management-header">
        <div>
          <h2>🔗 Gestión de Relaciones Repuesto-Proveedor</h2>
          <p style={{ margin: '0.5rem 0 0 0', opacity: 0.9, fontSize: '0.9rem' }}>
            Administra qué proveedores suministran cada repuesto
          </p>
        </div>
        <button className="btn-primary" onClick={openModal}>
          ➕ Nueva Relación
        </button>
      </div>

      {/* Estadísticas */}
      <div className="stats-section-horizontal">
        <div className="stat-card-horizontal">
          <div className="stat-icon-horizontal">🔗</div>
          <div className="stat-content-horizontal">
            <div className="stat-number-horizontal">{relaciones.length}</div>
            <div className="stat-label-horizontal">Relaciones Totales</div>
          </div>
        </div>
        <div className="stat-card-horizontal">
          <div className="stat-icon-horizontal">🔧</div>
          <div className="stat-content-horizontal">
            <div className="stat-number-horizontal">{repuestos.length}</div>
            <div className="stat-label-horizontal">Repuestos</div>
          </div>
        </div>
        <div className="stat-card-horizontal">
          <div className="stat-icon-horizontal">🏢</div>
          <div className="stat-content-horizontal">
            <div className="stat-number-horizontal">{proveedores.length}</div>
            <div className="stat-label-horizontal">Proveedores</div>
          </div>
        </div>
      </div>

      {error && (
        <div className="error-message">
          ⚠️ {error}
        </div>
      )}

      <div className="search-section-horizontal">
        <div className="search-controls-horizontal">
          <div className="filter-tabs-horizontal">
            <button 
              className={`filter-tab-horizontal ${viewMode === 'todas' ? 'active' : ''}`}
              onClick={() => handleViewModeChange('todas')}
            >
              📋 Todas las Relaciones
            </button>
            <button 
              className={`filter-tab-horizontal ${viewMode === 'porRepuesto' ? 'active' : ''}`}
              onClick={() => handleViewModeChange('porRepuesto')}
            >
              🔧 Por Repuesto
            </button>
            <button 
              className={`filter-tab-horizontal ${viewMode === 'porProveedor' ? 'active' : ''}`}
              onClick={() => handleViewModeChange('porProveedor')}
            >
              🏢 Por Proveedor
            </button>
          </div>

          {(viewMode === 'porRepuesto' || viewMode === 'porProveedor') && (
            <div className="search-input-group-horizontal">
              <select 
                value={selectedId}
                onChange={(e) => setSelectedId(e.target.value)}
                className="search-select-horizontal"
              >
                <option value="">Seleccionar {viewMode === 'porRepuesto' ? 'Repuesto' : 'Proveedor'}</option>
                {getFilteredOptions().map(option => (
                  <option key={option.value} value={option.value}>
                    {option.label}
                  </option>
                ))}
              </select>
              <button 
                onClick={loadRelacionesFiltradas} 
                disabled={loading || !selectedId}
                className="btn-search-horizontal"
              >
                🔍 Filtrar
              </button>
            </div>
          )}
        </div>
      </div>

      <div className="table-container">
        {loading ? (
          <div className="loading">
            <div style={{ fontSize: '2rem', marginBottom: '1rem' }}>⏳</div>
            Cargando relaciones...
          </div>
        ) : (
          <>
            {relaciones.length > 0 && (
              <div style={{ padding: '1rem', background: '#f8fafc', borderBottom: '1px solid #e2e8f0' }}>
                <strong>Total de relaciones: {relaciones.length}</strong>
                {viewMode === 'porRepuesto' && selectedId && (
                  <span style={{ marginLeft: '2rem', color: '#6b7280' }}>
                    Repuesto: {getRepuestoName(parseInt(selectedId))}
                  </span>
                )}
                {viewMode === 'porProveedor' && selectedId && (
                  <span style={{ marginLeft: '2rem', color: '#6b7280' }}>
                    Proveedor: {getProveedorName(parseInt(selectedId))}
                  </span>
                )}
              </div>
            )}
            <table className="data-table">
              <thead>
                <tr>
                  <th>🔧 Repuesto</th>
                  <th>🏢 Proveedor</th>
                  <th>📅 Fecha</th>
                  <th>⚙️ Acciones</th>
                </tr>
              </thead>
              <tbody>
                {relaciones.length === 0 ? (
                  <tr>
                    <td colSpan="4" className="no-data">
                      <div style={{ fontSize: '4rem', marginBottom: '1.5rem', opacity: 0.3 }}>🔗</div>
                      <div style={{ fontSize: '1.2rem', fontWeight: '600', marginBottom: '0.5rem' }}>
                        No hay relaciones registradas
                      </div>
                      <div style={{ fontSize: '0.9rem', opacity: 0.7, marginBottom: '1.5rem' }}>
                        Comienza creando tu primera relación repuesto-proveedor
                      </div>
                      <button className="btn-primary" onClick={openModal}>
                        ➕ Crear Primera Relación
                      </button>
                    </td>
                  </tr>
                ) : (
                  relaciones.map((relacion, index) => (
                    <tr key={index} className="data-row">
                      <td>
                        <div className="item-info">
                          <div className="item-icon">🔧</div>
                          <div className="item-details">
                            <div className="item-name">
                              {getRepuestoName(relacion.idRepuesto || relacion.repuestoId)}
                            </div>
                            <div className="item-id">
                              ID: {relacion.idRepuesto || relacion.repuestoId}
                            </div>
                          </div>
                        </div>
                      </td>
                      <td>
                        <div className="item-info">
                          <div className="item-icon">🏢</div>
                          <div className="item-details">
                            <div className="item-name">
                              {getProveedorName(relacion.idProveedor || relacion.proveedorId)}
                            </div>
                            <div className="item-id">
                              ID: {relacion.idProveedor || relacion.proveedorId}
                            </div>
                          </div>
                        </div>
                      </td>
                      <td>
                        <div className="date-info">
                          <div className="date-icon">📅</div>
                          <div className="date-text">
                            {new Date().toLocaleDateString()}
                          </div>
                        </div>
                      </td>
                      <td className="actions">
                        <button
                          className="btn-delete"
                          onClick={() => handleDelete(
                            relacion.idRepuesto || relacion.repuestoId,
                            relacion.idProveedor || relacion.proveedorId
                          )}
                          title="Eliminar relación"
                        >
                          🗑️ Eliminar
                        </button>
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </>
        )}
      </div>

      {showModal && (
        <div className="modal-overlay">
          <div className="modal relationship-modal">
            <div className="modal-header">
              <div className="modal-header-content">
                <div className="modal-icon">🔗</div>
                <div>
                  <h3>Nueva Relación Repuesto-Proveedor</h3>
                  <p>Conecta un repuesto con su proveedor correspondiente</p>
                </div>
              </div>
              <button className="modal-close" onClick={closeModal}>×</button>
            </div>
            <form onSubmit={handleSubmit} className="modal-form">
              <div className="form-section">
                <div className="section-header">
                  <div className="section-icon">🔧</div>
                  <h4>Seleccionar Repuesto</h4>
                </div>
                <div className="form-group">
                  <label htmlFor="idRepuesto">Repuesto:</label>
                  <select
                    id="idRepuesto"
                    value={formData.idRepuesto}
                    onChange={(e) => setFormData({ ...formData, idRepuesto: e.target.value })}
                    required
                    className="form-select"
                  >
                    <option value="">Seleccionar repuesto...</option>
                    {repuestos.map(repuesto => (
                      <option key={repuesto.id} value={repuesto.id}>
                        {repuesto.nombre} - #{repuesto.numeroSerie}
                      </option>
                    ))}
                  </select>
                </div>
              </div>

              <div className="connection-arrow">
                <div className="arrow-line"></div>
                <div className="arrow-icon">🔗</div>
                <div className="arrow-line"></div>
              </div>

              <div className="form-section">
                <div className="section-header">
                  <div className="section-icon">🏢</div>
                  <h4>Seleccionar Proveedor</h4>
                </div>
                <div className="form-group">
                  <label htmlFor="idProveedor">Proveedor:</label>
                  <select
                    id="idProveedor"
                    value={formData.idProveedor}
                    onChange={(e) => setFormData({ ...formData, idProveedor: e.target.value })}
                    required
                    className="form-select"
                  >
                    <option value="">Seleccionar proveedor...</option>
                    {proveedores.map(proveedor => (
                      <option key={proveedor.id} value={proveedor.id}>
                        {proveedor.nombre} - {proveedor.contacto || 'Sin contacto'}
                      </option>
                    ))}
                  </select>
                </div>
              </div>

              <div className="modal-actions">
                <button type="button" onClick={closeModal} disabled={loading} className="btn-cancel">
                  ❌ Cancelar
                </button>
                <button type="submit" disabled={loading} className="btn-create">
                  {loading ? '⏳ Creando...' : '✅ Crear Relación'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default RepuestoProveedorManagement;
