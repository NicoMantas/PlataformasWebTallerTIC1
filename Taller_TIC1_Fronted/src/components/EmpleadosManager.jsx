import React, { useState, useEffect } from 'react';
import { listEmpleados, desactivarEmpleado, activarEmpleado } from '../services/empleadosService';
import { getCurrentUser } from '../services/authService';
import './EmpleadosManager.css';

const EmpleadosManager = () => {
  const [empleados, setEmpleados] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [showDesactivarModal, setShowDesactivarModal] = useState(false);
  const [empleadoSeleccionado, setEmpleadoSeleccionado] = useState(null);
  const [detallesDesactivacion, setDetallesDesactivacion] = useState('');
  const [filtroEstado, setFiltroEstado] = useState('todos'); // 'todos', 'activos', 'inactivos'
  const [currentUser, setCurrentUser] = useState(null);

  useEffect(() => {
    loadEmpleados();
    loadCurrentUser();
  }, []);

  const loadCurrentUser = async () => {
    try {
      const user = await getCurrentUser();
      setCurrentUser(user);
    } catch (err) {
      console.error('Error al cargar usuario actual:', err);
    }
  };

  const loadEmpleados = async () => {
    try {
      setLoading(true);
      const data = await listEmpleados();
      setEmpleados(data);
    } catch (err) {
      setError(err?.message || 'Error al cargar empleados');
    } finally {
      setLoading(false);
    }
  };

  const handleDesactivar = (empleado) => {
    setEmpleadoSeleccionado(empleado);
    setDetallesDesactivacion('');
    setShowDesactivarModal(true);
  };

  const handleConfirmarDesactivacion = async () => {
    if (!detallesDesactivacion.trim()) {
      alert('Debe proporcionar una razón para la desactivación');
      return;
    }

    try {
      // Obtener el ID del empleado administrador desde el usuario actual
      const adminId = currentUser?.infoEspecifica?.id || currentUser?.id;
      await desactivarEmpleado(empleadoSeleccionado.id, detallesDesactivacion, adminId);
      await loadEmpleados();
      setShowDesactivarModal(false);
      setEmpleadoSeleccionado(null);
      setDetallesDesactivacion('');
    } catch (err) {
      setError(err?.message || 'Error al desactivar empleado');
    }
  };

  const handleActivar = async (id) => {
    if (window.confirm('¿Está seguro de que desea reactivar este empleado?')) {
      try {
        // Obtener el ID del empleado administrador desde el usuario actual
        const adminId = currentUser?.infoEspecifica?.id || currentUser?.id;
        await activarEmpleado(id, adminId);
        await loadEmpleados();
      } catch (err) {
        setError(err?.message || 'Error al activar empleado');
      }
    }
  };

  const getTipoEmpleadoDescripcion = (idTipoEmpleado) => {
    const tipos = {
      1: 'Administrador',
      2: 'Secretaria',
      3: 'Mecánico'
    };
    return tipos[idTipoEmpleado] || 'Empleado';
  };

  const empleadosFiltrados = empleados.filter(emp => {
    if (filtroEstado === 'activos') return emp.activo;
    if (filtroEstado === 'inactivos') return !emp.activo;
    return true;
  });

  if (loading) return <div className="loading">Cargando empleados...</div>;
  if (error) return <div className="error">{error}</div>;

  return (
    <div className="empleados-manager">
      <div className="empleados-header">
        <h2>Gestión de Empleados</h2>
        <div className="filtros">
          <label>Filtrar por estado:</label>
          <select 
            value={filtroEstado} 
            onChange={(e) => setFiltroEstado(e.target.value)}
            className="filtro-select"
          >
            <option value="todos">Todos</option>
            <option value="activos">Activos</option>
            <option value="inactivos">Inactivos</option>
          </select>
        </div>
      </div>

      <div className="empleados-grid">
        {empleadosFiltrados.map((empleado) => (
          <div 
            key={empleado.id} 
            className={`empleado-card ${!empleado.activo ? 'inactivo' : ''}`}
          >
            <div className="empleado-header">
              <h3>{empleado.nombre} {empleado.apellido}</h3>
              <div className={`estado-badge ${empleado.activo ? 'activo' : 'inactivo'}`}>
                {empleado.activo ? 'Activo' : 'Inactivo'}
              </div>
            </div>
            
            <div className="empleado-info">
              <p><strong>Cédula:</strong> {empleado.cedula}</p>
              <p><strong>Tipo:</strong> {getTipoEmpleadoDescripcion(empleado.idTipoEmpleado)}</p>
              <p><strong>Salario:</strong> ${empleado.salario?.toLocaleString()}</p>
              <p><strong>Contratación:</strong> {new Date(empleado.fechaContratacion).toLocaleDateString()}</p>
              
              {!empleado.activo && empleado.detallesDesactivacion && (
                <div className="detalles-desactivacion">
                  <p><strong>Razón de desactivación:</strong></p>
                  <p className="detalles-texto">{empleado.detallesDesactivacion}</p>
                  <p><strong>Fecha:</strong> {new Date(empleado.fechaDesactivacion).toLocaleDateString()}</p>
                </div>
              )}
            </div>

            <div className="empleado-actions">
              {empleado.activo ? (
                <button 
                  className="btn-danger"
                  onClick={() => handleDesactivar(empleado)}
                >
                  Desactivar
                </button>
              ) : (
                <button 
                  className="btn-success"
                  onClick={() => handleActivar(empleado.id)}
                >
                  Reactivar
                </button>
              )}
            </div>
          </div>
        ))}
      </div>

      {empleadosFiltrados.length === 0 && (
        <div className="no-data">
          No hay empleados que coincidan con el filtro seleccionado.
        </div>
      )}

      {/* Modal de desactivación */}
      {showDesactivarModal && (
        <div className="modal-overlay">
          <div className="modal-content">
            <h3>Desactivar Empleado</h3>
            <p>
              <strong>{empleadoSeleccionado?.nombre} {empleadoSeleccionado?.apellido}</strong>
            </p>
            <p>Por favor, proporcione una razón para la desactivación:</p>
            
            <textarea
              value={detallesDesactivacion}
              onChange={(e) => setDetallesDesactivacion(e.target.value)}
              placeholder="Ej: Renuncia, despido, baja temporal..."
              rows={4}
              className="detalles-textarea"
              required
            />
            
            <div className="modal-actions">
              <button 
                className="btn-secondary"
                onClick={() => setShowDesactivarModal(false)}
              >
                Cancelar
              </button>
              <button 
                className="btn-danger"
                onClick={handleConfirmarDesactivacion}
              >
                Confirmar Desactivación
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default EmpleadosManager;
