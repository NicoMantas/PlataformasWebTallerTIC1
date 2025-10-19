import React, { useState, useEffect } from 'react';
import { listProveedores, createProveedor, updateProveedor, deleteProveedor, searchProveedoresByName } from '../services/proveedoresService';
import '../styles/ManagementStyles.css';

const ProveedorManagement = () => {
  const [proveedores, setProveedores] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [showModal, setShowModal] = useState(false);
  const [editingProveedor, setEditingProveedor] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [formData, setFormData] = useState({
    nombre: '',
    contacto: ''
  });

  useEffect(() => {
    loadProveedores();
  }, []);

  const loadProveedores = async () => {
    setLoading(true);
    try {
      const data = await listProveedores();
      setProveedores(data);
      setError('');
    } catch (err) {
      setError('Error al cargar proveedores: ' + err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = async () => {
    if (!searchTerm.trim()) {
      loadProveedores();
      return;
    }

    setLoading(true);
    try {
      const data = await searchProveedoresByName(searchTerm);
      setProveedores(data);
      setError('');
    } catch (err) {
      setError('Error en la búsqueda: ' + err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      if (editingProveedor) {
        await updateProveedor(editingProveedor.id, {
          ...formData,
          id: editingProveedor.id
        });
      } else {
        await createProveedor(formData);
      }
      setShowModal(false);
      setEditingProveedor(null);
      setFormData({ nombre: '', contacto: '' });
      loadProveedores();
      setError('');
    } catch (err) {
      setError('Error al guardar proveedor: ' + err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = (proveedor) => {
    setEditingProveedor(proveedor);
    setFormData({
      nombre: proveedor.nombre,
      contacto: proveedor.contacto
    });
    setShowModal(true);
  };

  const handleDelete = async (id) => {
    if (!window.confirm('¿Está seguro de que desea eliminar este proveedor?')) {
      return;
    }

    setLoading(true);
    try {
      await deleteProveedor(id);
      loadProveedores();
      setError('');
    } catch (err) {
      setError('Error al eliminar proveedor: ' + err.message);
    } finally {
      setLoading(false);
    }
  };

  const openModal = () => {
    setEditingProveedor(null);
    setFormData({ nombre: '', contacto: '' });
    setShowModal(true);
  };

  const closeModal = () => {
    setShowModal(false);
    setEditingProveedor(null);
    setFormData({ nombre: '', contacto: '' });
  };

  return (
    <div className="management-container">
      <div className="management-header">
        <div>
          <h2>🏢 Gestión de Proveedores</h2>
          <p style={{ margin: '0.5rem 0 0 0', opacity: 0.9, fontSize: '0.9rem' }}>
            Administra la información de tus proveedores de repuestos
          </p>
        </div>
        <button className="btn-primary" onClick={openModal}>
          ➕ Nuevo Proveedor
        </button>
      </div>

      {error && (
        <div className="error-message">
          ⚠️ {error}
        </div>
      )}

      <div className="search-section">
        <div className="search-input-group">
          <input
            type="text"
            placeholder="🔍 Buscar proveedor por nombre..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            onKeyPress={(e) => e.key === 'Enter' && handleSearch()}
          />
          <button onClick={handleSearch} disabled={loading}>
            {loading ? '⏳' : '🔍'} Buscar
          </button>
          {searchTerm && (
            <button onClick={() => { setSearchTerm(''); loadProveedores(); }}>
              🗑️ Limpiar
            </button>
          )}
        </div>
      </div>

      <div className="table-container">
        {loading ? (
          <div className="loading">
            <div style={{ fontSize: '2rem', marginBottom: '1rem' }}>⏳</div>
            Cargando proveedores...
          </div>
        ) : (
          <>
            {proveedores.length > 0 && (
              <div style={{ padding: '1rem', background: '#f8fafc', borderBottom: '1px solid #e2e8f0' }}>
                <strong>Total de proveedores: {proveedores.length}</strong>
              </div>
            )}
            <table className="data-table">
              <thead>
                <tr>
                  <th>📛 Nombre</th>
                  <th>📞 Contacto</th>
                  <th>⚙️ Acciones</th>
                </tr>
              </thead>
              <tbody>
                {proveedores.length === 0 ? (
                  <tr>
                    <td colSpan="3" className="no-data">
                      <div style={{ fontSize: '3rem', marginBottom: '1rem' }}>📋</div>
                      <div>No hay proveedores registrados</div>
                      <div style={{ fontSize: '0.9rem', opacity: 0.7, marginTop: '0.5rem' }}>
                        Haz clic en "Nuevo Proveedor" para agregar el primero
                      </div>
                    </td>
                  </tr>
                ) : (
                  proveedores.map((proveedor) => (
                    <tr key={proveedor.id}>
                      <td>
                        <div style={{ fontWeight: '600', color: '#374151' }}>
                          {proveedor.nombre}
                        </div>
                      </td>
                      <td>
                        <div style={{ color: '#6b7280' }}>
                          {proveedor.contacto}
                        </div>
                      </td>
                      <td className="actions">
                        <button
                          className="btn-edit"
                          onClick={() => handleEdit(proveedor)}
                          title="Editar proveedor"
                        >
                          ✏️ Editar
                        </button>
                        <button
                          className="btn-delete"
                          onClick={() => handleDelete(proveedor.id)}
                          title="Eliminar proveedor"
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
          <div className="modal">
            <div className="modal-header">
              <h3>
                {editingProveedor ? '✏️ Editar Proveedor' : '➕ Nuevo Proveedor'}
              </h3>
              <button className="modal-close" onClick={closeModal}>×</button>
            </div>
            <form onSubmit={handleSubmit} className="modal-form">
              <div className="form-group">
                <label htmlFor="nombre">📛 Nombre del Proveedor:</label>
                <input
                  type="text"
                  id="nombre"
                  value={formData.nombre}
                  onChange={(e) => setFormData({ ...formData, nombre: e.target.value })}
                  placeholder="Ingresa el nombre del proveedor"
                  required
                />
              </div>
              <div className="form-group">
                <label htmlFor="contacto">📞 Información de Contacto:</label>
                <input
                  type="text"
                  id="contacto"
                  value={formData.contacto}
                  onChange={(e) => setFormData({ ...formData, contacto: e.target.value })}
                  placeholder="Teléfono, email o dirección de contacto"
                  required
                />
              </div>
              <div className="modal-actions">
                <button type="button" onClick={closeModal} disabled={loading}>
                  ❌ Cancelar
                </button>
                <button type="submit" disabled={loading}>
                  {loading ? '⏳ Guardando...' : (editingProveedor ? '💾 Actualizar' : '✅ Crear')}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default ProveedorManagement;
