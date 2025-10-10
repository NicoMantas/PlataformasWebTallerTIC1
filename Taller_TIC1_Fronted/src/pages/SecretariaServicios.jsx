import React, { useEffect, useState } from 'react';
import Header from '../components/Header';
import { secretariaListPendientes, secretariaListAsignados, secretariaAsignarMecanico, listEmpleados, descargarReservaPdf } from '../services/serviciosService';

const SecretariaServicios = () => {
  const [tab, setTab] = useState('pendientes');
  const [pendientes, setPendientes] = useState([]);
  const [asignados, setAsignados] = useState([]);
  const [empleados, setEmpleados] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  // Función helper para filtrar solo mecánicos
  const filtrarMecanicos = (empleadosList) => {
    return empleadosList.filter(emp => {
      // Múltiples opciones para el tipo de empleado
      const tipo = emp.idTipoEmpleado || emp.tipoEmpleado || emp.idTipo || emp.tipoEmpleadoId || emp.tipo;
      console.log(`Empleado ${emp.nombre}: tipo=${tipo}, idTipoEmpleado=${emp.idTipoEmpleado}, tipoEmpleado=${emp.tipoEmpleado}`); // Debug
      return tipo === 3 || tipo === '3' || tipo === 'Mecánico' || tipo === 'mecanico';
    });
  };

  const loadAll = async () => {
    try {
      setLoading(true);
      const [p, a, e] = await Promise.all([
        secretariaListPendientes(),
        secretariaListAsignados(),
        listEmpleados()
      ]);
      setPendientes(p || []);
      setAsignados(a || []);
      setEmpleados(e || []);
      console.log('Empleados cargados:', e); // Debug
      console.log('Mecánicos filtrados:', filtrarMecanicos(e || [])); // Debug
    } catch (err) {
      setError(err?.message || 'Error al cargar');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { loadAll(); }, []);

  const handleAsignar = async (servicioId, empleadoId) => {
    if (!empleadoId) return;
    await secretariaAsignarMecanico(servicioId, empleadoId);
    await loadAll();
  };

  if (loading) return <div className="loading">Cargando...</div>;
  if (error) return <div className="error">{error}</div>;

  const renderCard = (s, showAsignar) => {
    const tipoServicio = s.tipoServicio === 'Revision' ? 'Revisión' : s.tipoServicio === 'Reparacion' ? 'Reparación' : (s.detallesRevision ? 'Revisión' : 'Reparación');
    const fechaCreacion = s.fechaCreacion ? new Date(s.fechaCreacion).toLocaleDateString() : 'N/D';
    
    return (
      <div key={s.id} className="card">
        <div className="card-header">
          <div className="card-title">Servicio #{s.id} · {tipoServicio}</div>
          <div className="card-subtitle">Estado: {s.estadoDescripcion}</div>
        </div>
        <div className="servicio-info">
          <p><strong>Cliente:</strong> {s.clienteNombre || 'N/D'}</p>
          <p><strong>Vehículo:</strong> {s.vehiculoInfo || 'N/D'}</p>
          {s.detallesRevision && <p><strong>Detalles:</strong> {s.detallesRevision}</p>}
          <p><strong>Fecha Creación:</strong> {fechaCreacion}</p>
        </div>
      <div style={{ display: 'flex', gap: '0.75rem', marginTop: '8px', alignItems: 'center' }}>
        <button className="btn-secondary" onClick={async () => {
          const blob = await descargarReservaPdf(s);
          const url = URL.createObjectURL(blob);
          const a = document.createElement('a');
          a.href = url;
          a.download = `reserva_servicio_${s.id}.pdf`;
          document.body.appendChild(a);
          a.click();
          a.remove();
          URL.revokeObjectURL(url);
        }}>Descargar PDF</button>
        {showAsignar && (
          <>
            <select onChange={(e) => handleAsignar(s.id, parseInt(e.target.value))} defaultValue="">
              <option value="" disabled>Asignar mecánico</option>
              {(() => {
                const mecanicos = filtrarMecanicos(empleados);
                
                if (mecanicos.length === 0) {
                  return <option value="" disabled>No hay mecánicos disponibles</option>;
                }
                
                return mecanicos.map(emp => (
                  <option key={emp.id} value={emp.id}>{emp.nombre} {emp.apellido || ''}</option>
                ));
              })()}
            </select>
          </>
        )}
      </div>
    </div>
    );
  };

  return (
    <div className="home-cliente-page">
      <Header />
      <div className="cliente-container">
        <div className="cliente-header">
          <h1>Servicios - Secretaría</h1>
          <p>Gestiona asignaciones de mecánicos y seguimiento</p>
        </div>
        <div className="cliente-dashboard">
          <div className="dashboard-tabs">
            <button className={`tab-button ${tab === 'pendientes' ? 'active' : ''}`} onClick={() => setTab('pendientes')}>Pendientes</button>
            <button className={`tab-button ${tab === 'asignados' ? 'active' : ''}`} onClick={() => setTab('asignados')}>Asignados</button>
          </div>

          {tab === 'pendientes' ? (
            <div className="servicios-section">
              <h2>Servicios Pendientes</h2>
              {pendientes.map(s => renderCard(s, true))}
            </div>
          ) : (
            <div className="servicios-section">
              <h2>Servicios Asignados</h2>
              {asignados.map(s => renderCard(s, false))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default SecretariaServicios;
