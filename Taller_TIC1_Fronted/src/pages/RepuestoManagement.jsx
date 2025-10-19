import React, { useState, useEffect } from 'react';
import { 
  listRepuestos, 
  createRepuesto, 
  updateRepuesto, 
  deleteRepuesto, 
  searchRepuestosByName,
  getRepuestosByStock,
  getRepuestoByNumeroSerie
} from '../services/repuestosService';
import { getProveedoresByRepuesto } from '../services/repuestoProveedorService';
import '../styles/ManagementStyles.css';

const RepuestoManagement = () => {
  const [repuestos, setRepuestos] = useState([]);
  const [repuestosConProveedores, setRepuestosConProveedores] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [showModal, setShowModal] = useState(false);
  const [editingRepuesto, setEditingRepuesto] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [searchType, setSearchType] = useState('nombre');
  const [stockFilter, setStockFilter] = useState('');
  const [formData, setFormData] = useState({
    nombre: '',
    numero_serie: '',
    precio: '',
    stock: ''
  });

  useEffect(() => {
    loadRepuestos();
  }, []);

  const loadRepuestos = async () => {
    setLoading(true);
    try {
      const data = await listRepuestos();
      setRepuestos(data);
      
      // Cargar proveedores para cada repuesto
      const repuestosConProveedoresData = await Promise.all(
        data.map(async (repuesto) => {
          try {
            const proveedores = await getProveedoresByRepuesto(repuesto.id);
            return {
              ...repuesto,
              proveedores: proveedores || []
            };
          } catch (err) {
            return {
              ...repuesto,
              proveedores: []
            };
          }
        })
      );
      
      setRepuestosConProveedores(repuestosConProveedoresData);
      setError('');
    } catch (err) {
      setError('Error al cargar repuestos: ' + err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = async () => {
    if (!searchTerm.trim()) {
      loadRepuestos();
      return;
    }

    setLoading(true);
    try {
      let data;
      if (searchType === 'nombre') {
        data = await searchRepuestosByName(searchTerm);
      } else if (searchType === 'serie') {
        const numeroSerie = parseInt(searchTerm);
        if (isNaN(numeroSerie)) {
          setError('El número de serie debe ser un número válido');
          return;
        }
        const result = await getRepuestoByNumeroSerie(numeroSerie);
        data = result ? [result] : [];
      }
      setRepuestos(data);
      setError('');
    } catch (err) {
      setError('Error en la búsqueda: ' + err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleStockFilter = async () => {
    if (!stockFilter.trim()) {
      loadRepuestos();
      return;
    }

    const minStock = parseInt(stockFilter);
    if (isNaN(minStock)) {
      setError('El stock mínimo debe ser un número válido');
      return;
    }

    setLoading(true);
    try {
      const data = await getRepuestosByStock(minStock);
      setRepuestos(data);
      setError('');
    } catch (err) {
      setError('Error al filtrar por stock: ' + err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      const submitData = {
        nombre: formData.nombre,
        numeroSerie: parseInt(formData.numero_serie),
        precio: parseFloat(formData.precio),
        stock: parseInt(formData.stock)
      };

      if (editingRepuesto) {
        await updateRepuesto(editingRepuesto.id, {
          ...submitData,
          id: editingRepuesto.id
        });
      } else {
        await createRepuesto(submitData);
      }
      setShowModal(false);
      setEditingRepuesto(null);
      setFormData({ nombre: '', numero_serie: '', precio: '', stock: '' });
      loadRepuestos();
      setError('');
    } catch (err) {
      setError('Error al guardar repuesto: ' + err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = (repuesto) => {
    setEditingRepuesto(repuesto);
    setFormData({
      nombre: repuesto.nombre,
      numero_serie: repuesto.numeroSerie.toString(),
      precio: repuesto.precio.toString(),
      stock: repuesto.stock.toString()
    });
    setShowModal(true);
  };

  const handleDelete = async (id) => {
    if (!window.confirm('¿Está seguro de que desea eliminar este repuesto?')) {
      return;
    }

    setLoading(true);
    try {
      await deleteRepuesto(id);
      loadRepuestos();
      setError('');
    } catch (err) {
      setError('Error al eliminar repuesto: ' + err.message);
    } finally {
      setLoading(false);
    }
  };

  const openModal = () => {
    setEditingRepuesto(null);
    setFormData({ nombre: '', numero_serie: '', precio: '', stock: '' });
    setShowModal(true);
  };

  const closeModal = () => {
    setShowModal(false);
    setEditingRepuesto(null);
    setFormData({ nombre: '', numero_serie: '', precio: '', stock: '' });
  };

  const clearFilters = () => {
    setSearchTerm('');
    setStockFilter('');
    setSearchType('nombre');
    loadRepuestos();
  };

  const getStockStatus = (stock) => {
    if (stock === 0) return { class: 'stock-zero', text: 'Sin stock' };
    if (stock <= 5) return { class: 'stock-low', text: 'Stock bajo' };
    return { class: 'stock-ok', text: 'Stock normal' };
  };

  return (
    <div className="management-container">
      <div className="management-header">
        <div>
          <h2>🔧 Gestión de Repuestos</h2>
          <p style={{ margin: '0.5rem 0 0 0', opacity: 0.9, fontSize: '0.9rem' }}>
            Controla el inventario de repuestos y su disponibilidad
          </p>
        </div>
        <button className="btn-primary" onClick={openModal}>
          ➕ Nuevo Repuesto
        </button>
      </div>

      {error && (
        <div className="error-message">
          ⚠️ {error}
        </div>
      )}

      <div className="search-section">
        <div className="search-controls">
          <div className="search-input-group">
            <select 
              value={searchType} 
              onChange={(e) => setSearchType(e.target.value)}
            >
              <option value="nombre">🔍 Buscar por nombre</option>
              <option value="serie">🔢 Buscar por número de serie</option>
            </select>
            <input
              type="text"
              placeholder={
                searchType === 'nombre' 
                  ? '🔍 Buscar repuesto por nombre...' 
                  : '🔢 Buscar por número de serie...'
              }
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              onKeyPress={(e) => e.key === 'Enter' && handleSearch()}
            />
            <button onClick={handleSearch} disabled={loading}>
              {loading ? '⏳' : '🔍'} Buscar
            </button>
          </div>

          <div className="stock-filter-group">
            <input
              type="number"
              placeholder="📦 Stock mínimo..."
              value={stockFilter}
              onChange={(e) => setStockFilter(e.target.value)}
              onKeyPress={(e) => e.key === 'Enter' && handleStockFilter()}
            />
            <button onClick={handleStockFilter} disabled={loading}>
              📊 Filtrar Stock
            </button>
          </div>

          {(searchTerm || stockFilter) && (
            <button onClick={clearFilters}>
              🗑️ Limpiar Filtros
            </button>
          )}
        </div>
      </div>

      <div className="table-container">
        {loading ? (
          <div className="loading">
            <div style={{ fontSize: '2rem', marginBottom: '1rem' }}>⏳</div>
            Cargando repuestos...
          </div>
        ) : (
          <>
            {repuestos.length > 0 && (
              <div style={{ padding: '1rem', background: '#f8fafc', borderBottom: '1px solid #e2e8f0' }}>
                <strong>Total de repuestos: {repuestos.length}</strong>
                <span style={{ marginLeft: '2rem', color: '#6b7280' }}>
                  Stock bajo: {repuestos.filter(r => r.stock <= 5 && r.stock > 0).length} | 
                  Sin stock: {repuestos.filter(r => r.stock === 0).length}
                </span>
              </div>
            )}
            <table className="data-table">
              <thead>
                <tr>
                  <th>📛 Nombre</th>
                  <th>🔢 Número de Serie</th>
                  <th>💰 Precio</th>
                  <th>📦 Stock</th>
                  <th>📊 Estado</th>
                  <th>🏢 Proveedores</th>
                  <th>⚙️ Acciones</th>
                </tr>
              </thead>
              <tbody>
                {repuestos.length === 0 ? (
                  <tr>
                    <td colSpan="7" className="no-data">
                      <div style={{ fontSize: '3rem', marginBottom: '1rem' }}>🔧</div>
                      <div>No hay repuestos registrados</div>
                      <div style={{ fontSize: '0.9rem', opacity: 0.7, marginTop: '0.5rem' }}>
                        Haz clic en "Nuevo Repuesto" para agregar el primero
                      </div>
                    </td>
                  </tr>
                ) : (
                  repuestosConProveedores.map((repuesto) => {
                    const stockStatus = getStockStatus(repuesto.stock);
                    return (
                      <tr key={repuesto.id}>
                        <td>
                          <div style={{ fontWeight: '600', color: '#374151' }}>
                            {repuesto.nombre}
                          </div>
                        </td>
                        <td>
                          <div style={{ color: '#6b7280', fontFamily: 'monospace' }}>
                            {repuesto.numeroSerie}
                          </div>
                        </td>
                        <td>
                          <div style={{ fontWeight: '600', color: '#059669' }}>
                            ${repuesto.precio.toFixed(2)}
                          </div>
                        </td>
                        <td>
                          <div style={{ 
                            fontWeight: '600', 
                            color: repuesto.stock === 0 ? '#dc2626' : repuesto.stock <= 5 ? '#d97706' : '#059669',
                            fontSize: '1.1rem'
                          }}>
                            {repuesto.stock}
                          </div>
                        </td>
                        <td>
                          <span className={`stock-status ${stockStatus.class}`}>
                            {stockStatus.text}
                          </span>
                        </td>
                        <td>
                          <div style={{ fontSize: '0.85rem' }}>
                            {repuesto.proveedores && repuesto.proveedores.length > 0 ? (
                              repuesto.proveedores.map((proveedor, index) => (
                                <div key={index} style={{ 
                                  background: '#f3f4f6', 
                                  padding: '0.25rem 0.5rem', 
                                  borderRadius: '4px', 
                                  margin: '0.125rem 0',
                                  fontSize: '0.75rem'
                                }}>
                                  {proveedor.nombre || `Proveedor #${proveedor.id || proveedor.proveedorId}`}
                                </div>
                              ))
                            ) : (
                              <span style={{ color: '#9ca3af', fontStyle: 'italic' }}>
                                Sin proveedores
                              </span>
                            )}
                          </div>
                        </td>
                        <td className="actions">
                          <button
                            className="btn-edit"
                            onClick={() => handleEdit(repuesto)}
                            title="Editar repuesto"
                          >
                            ✏️ Editar
                          </button>
                          <button
                            className="btn-delete"
                            onClick={() => handleDelete(repuesto.id)}
                            title="Eliminar repuesto"
                          >
                            🗑️ Eliminar
                          </button>
                        </td>
                      </tr>
                    );
                  })
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
                {editingRepuesto ? '✏️ Editar Repuesto' : '➕ Nuevo Repuesto'}
              </h3>
              <button className="modal-close" onClick={closeModal}>×</button>
            </div>
            <form onSubmit={handleSubmit} className="modal-form">
              <div className="form-group">
                <label htmlFor="nombre">📛 Nombre del Repuesto:</label>
                <input
                  type="text"
                  id="nombre"
                  value={formData.nombre}
                  onChange={(e) => setFormData({ ...formData, nombre: e.target.value })}
                  placeholder="Ingresa el nombre del repuesto"
                  required
                />
              </div>
              <div className="form-group">
                <label htmlFor="numero_serie">🔢 Número de Serie:</label>
                <input
                  type="number"
                  id="numero_serie"
                  value={formData.numero_serie}
                  onChange={(e) => setFormData({ ...formData, numero_serie: e.target.value })}
                  placeholder="Número de serie único del repuesto"
                  required
                />
              </div>
              <div className="form-group">
                <label htmlFor="precio">💰 Precio Unitario:</label>
                <input
                  type="number"
                  step="0.01"
                  min="0"
                  id="precio"
                  value={formData.precio}
                  onChange={(e) => setFormData({ ...formData, precio: e.target.value })}
                  placeholder="0.00"
                  required
                />
              </div>
              <div className="form-group">
                <label htmlFor="stock">📦 Cantidad en Stock:</label>
                <input
                  type="number"
                  min="0"
                  id="stock"
                  value={formData.stock}
                  onChange={(e) => setFormData({ ...formData, stock: e.target.value })}
                  placeholder="0"
                  required
                />
                <div style={{ fontSize: '0.8rem', color: '#6b7280', marginTop: '0.25rem' }}>
                  💡 Stock bajo: ≤ 5 unidades | Sin stock: 0 unidades
                </div>
              </div>
              <div className="modal-actions">
                <button type="button" onClick={closeModal} disabled={loading}>
                  ❌ Cancelar
                </button>
                <button type="submit" disabled={loading}>
                  {loading ? '⏳ Guardando...' : (editingRepuesto ? '💾 Actualizar' : '✅ Crear')}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default RepuestoManagement;
