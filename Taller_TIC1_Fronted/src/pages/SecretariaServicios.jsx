import React, { useEffect, useState } from 'react';
import Header from '../components/Header';
import { secretariaListPendientes, secretariaListAsignados, secretariaListCompletados, secretariaAsignarMecanico, listEmpleados, descargarReservaPdf } from '../services/serviciosService';
import { getFacturaByServicio, descargarFacturaPdf } from '../services/facturasService';

const SecretariaServicios = () => {
  const [tab, setTab] = useState('pendientes');
  const [pendientes, setPendientes] = useState([]);
  const [asignados, setAsignados] = useState([]);
  const [completados, setCompletados] = useState([]);
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
      console.log('Iniciando carga de datos...'); // Debug
      
      const [p, a, c, e] = await Promise.all([
        secretariaListPendientes(),
        secretariaListAsignados(),
        secretariaListCompletados(),
        listEmpleados()
      ]);
      
      console.log('Datos cargados - Pendientes:', p?.length || 0, 'Asignados:', a?.length || 0, 'Completados:', c?.length || 0, 'Empleados:', e?.length || 0); // Debug
      
      setPendientes(p || []);
      setAsignados(a || []);
      setCompletados(c || []);
      setEmpleados(e || []);
    } catch (err) {
      console.error('Error en loadAll:', err); // Debug
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

  const handleDescargarFactura = async (servicioId) => {
    try {
      const factura = await getFacturaByServicio(servicioId);
      if (factura) {
        const blob = await descargarFacturaPdf(factura);
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `factura_servicio_${servicioId}.pdf`;
        document.body.appendChild(a);
        a.click();
        a.remove();
        URL.revokeObjectURL(url);
      } else {
        alert('No se encontró factura para este servicio');
      }
    } catch (error) {
      console.error('Error al descargar factura:', error);
      alert('Error al descargar la factura');
    }
  };

  if (loading) return <div className="loading">Cargando...</div>;
  if (error) return <div className="error">{error}</div>;

  const renderCard = (s, showAsignar, showFactura = false) => {
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
        {showFactura && (
          <button className="btn-primary" onClick={() => handleDescargarFactura(s.id)}>
            Descargar Factura
          </button>
        )}
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

  console.log('Renderizando SecretariaServicios, tab actual:', tab); // Debug
  console.log('Completados disponibles:', completados.length); // Debug
  console.log('Estado de las pestañas - Pendientes:', pendientes.length, 'Asignados:', asignados.length, 'Completados:', completados.length); // Debug

  return (
    <div className="home-cliente-page">
      <Header />
      <div className="cliente-container">
        <div className="cliente-header">
          <h1>Servicios - Secretaría</h1>
          <p>Gestiona asignaciones de mecánicos y seguimiento</p>
          <div style={{ fontSize: '12px', color: '#666', marginTop: '10px' }}>
            Debug: Tab actual: {tab} | Completados: {completados.length}
          </div>
        </div>
        <div className="cliente-dashboard">
          <div className="dashboard-tabs" style={{ display: 'flex', gap: '10px', marginBottom: '20px' }}>
            <button className={`tab-button ${tab === 'pendientes' ? 'active' : ''}`} onClick={() => setTab('pendientes')}>Trabajos Pendientes</button>
            <button className={`tab-button ${tab === 'asignados' ? 'active' : ''}`} onClick={() => setTab('asignados')}>Trabajos Asignados</button>
            <button className={`tab-button ${tab === 'completados' ? 'active' : ''}`} onClick={() => setTab('completados')}>Trabajos Completados</button>
          </div>

          {tab === 'pendientes' ? (
            <div className="servicios-section">
              <h2>Trabajos Pendientes</h2>
              {pendientes.map(s => renderCard(s, true, false))}
            </div>
          ) : tab === 'asignados' ? (
            <div className="servicios-section">
              <h2>Trabajos Asignados</h2>
              {asignados.map(s => renderCard(s, false, false))}
            </div>
          ) : tab === 'completados' ? (
            <div className="servicios-section">
              <h2>Trabajos Completados</h2>
              {completados.map(s => renderCard(s, false, true))}
            </div>
          ) : null}
        </div>
      </div>
    </div>
  );
};

export default SecretariaServicios;
