import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import Header from '../components/Header';
import EmpleadosManager from '../components/EmpleadosManager';
import DashboardAdmin from '../components/DashboardAdmin';
import BusquedaServicios from '../components/BusquedaServicios';
import ProveedorManagement from './ProveedorManagement';
import RepuestoManagement from './RepuestoManagement';
import RepuestoProveedorManagement from './RepuestoProveedorManagement';
import '../styles/HomeEmpleado.css';
import '../styles/EmpleadoForm.css';
import { listOrdenes, updateOrden } from '../services/ordenesService';
import { listServicios, secretariaListPendientes, secretariaListAsignados, secretariaListCompletados, secretariaAsignarMecanico, getServicioById, updateServicioEstado, desasignarEmpleado, mecanicoListAsignados, mecanicoListCompletados, debugMecanicoCompletados } from '../services/serviciosService';
import { listFacturas, getFacturaByServicio, descargarFacturaPdf } from '../services/facturasService';
import { getCurrentUser, registerEmpleado } from '../services/authService';
import { createEmpleado, listEmpleados } from '../services/empleadosService';
import { listRepuestos } from '../services/repuestosService';
import { listProveedores } from '../services/proveedoresService';
import { getDetalleRevision, createDetalleRevision, updateDetalleRevision, getDetalleReparacion, getRepuestosByDetalleReparacion, addRepuestoToReparacion, updateRepuestoInReparacion } from '../services/detallesService';

const HomeEmpleado = () => {
  const navigate = useNavigate();
  const { tipo } = useParams(); // mecanico, secretaria, administrador
  const [ordenes, setOrdenes] = useState([]);
  const [servicios, setServicios] = useState([]);
  const [facturas, setFacturas] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [user, setUser] = useState(null);
  const [empleados, setEmpleados] = useState([]);
  const [secPendientes, setSecPendientes] = useState([]);
  const [secAsignados, setSecAsignados] = useState([]);
  const [secCompletados, setSecCompletados] = useState([]);
  const [secTab, setSecTab] = useState('pendientes');
  const [creating, setCreating] = useState(false);
  const [showDashboard, setShowDashboard] = useState(false);
  const [showBusqueda, setShowBusqueda] = useState(false);
  const [showProveedores, setShowProveedores] = useState(false);
  const [showRepuestos, setShowRepuestos] = useState(false);
  const [showRepuestoProveedor, setShowRepuestoProveedor] = useState(false);
  const [empleadoForm, setEmpleadoForm] = useState({
    nombre: '',
    apellido: '',
    cedula: '',
    salario: '',
    idTipoEmpleado: 1,
    email: '',
    password: '',
    idTaller: ''
  });
  
  // Mechanic specific states
  const [mecanicoAsignados, setMecanicoAsignados] = useState([]);
  const [mecanicoCompletados, setMecanicoCompletados] = useState([]);
  
  // Estados organizados por tipo
  const [serviciosPendientes, setServiciosPendientes] = useState([]);
  const [serviciosAsignados, setServiciosAsignados] = useState([]);
  const [serviciosEnProceso, setServiciosEnProceso] = useState([]);
  const [serviciosCompletados, setServiciosCompletados] = useState([]);
  const [repuestos, setRepuestos] = useState([]);
  const [selectedServicio, setSelectedServicio] = useState(null);
  const [showReparacionModal, setShowReparacionModal] = useState(false);
  const [showRevisionModal, setShowRevisionModal] = useState(false);
  const [repuestosReparacion, setRepuestosReparacion] = useState([]);
  const [repuestoForm, setRepuestoForm] = useState({
    idRepuesto: '',
    cantidad: 1
  });
  const [revisionForm, setRevisionForm] = useState({
    detalles: ''
  });

  useEffect(() => {
    // Get current user information
    const currentUser = getCurrentUser();
    console.log('Usuario obtenido:', currentUser);
    setUser(currentUser);
  }, []);

  // Cargar datos cuando el usuario esté disponible
  useEffect(() => {
    if (user) {
      loadData();
    }
  }, [user]);

  const handleDescargarFactura = async (servicioId) => {
    try {
      console.log('Descargando factura para servicio:', servicioId); // Debug
      const factura = await getFacturaByServicio(servicioId);
      console.log('Factura obtenida:', factura); // Debug
      
      if (factura) {
        console.log('Generando PDF...'); // Debug
        const blob = await descargarFacturaPdf(factura);
        console.log('PDF generado:', blob); // Debug
        
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `factura_servicio_${servicioId}.pdf`;
        document.body.appendChild(a);
        a.click();
        a.remove();
        URL.revokeObjectURL(url);
        console.log('Descarga completada'); // Debug
      } else {
        console.log('No se encontró factura'); // Debug
        alert('No se encontró factura para este servicio');
      }
    } catch (error) {
      console.error('Error al descargar factura:', error);
      console.error('Detalles del error:', error.message, error.stack); // Debug
      
      if (error.status === 404) {
        // Si no hay factura, crear una temporal basada en el servicio
        console.log('Creando factura temporal para servicio:', servicioId);
        const servicioCompletado = secCompletados.find(s => s.id === servicioId);
        
        if (servicioCompletado) {
          // El costo del servicio ya incluye los repuestos
          let costoTotal = servicioCompletado.costo || 0;
          
          // Calcular IVA (19%)
          const iva = costoTotal * 0.19;
          const totalConIva = costoTotal + iva;
          
          // Construir servicios realizados con más detalles
          const serviciosRealizados = [];
          
          if (servicioCompletado.tipoServicio === 'Revision') {
            serviciosRealizados.push('Revisión técnica');
            if (servicioCompletado.detallesRevision) {
              serviciosRealizados.push(`Detalles: ${servicioCompletado.detallesRevision}`);
            }
            if (servicioCompletado.detallesEncontrados) {
              serviciosRealizados.push(`Hallazgos: ${servicioCompletado.detallesEncontrados}`);
            }
          } else if (servicioCompletado.tipoServicio === 'Reparacion') {
            serviciosRealizados.push('Reparación');
            if (servicioCompletado.repuestosReparacion && servicioCompletado.repuestosReparacion.length > 0) {
              serviciosRealizados.push(`Repuestos utilizados: ${servicioCompletado.repuestosReparacion.length}`);
              servicioCompletado.repuestosReparacion.forEach(rep => {
                const precio = rep.precioUnitario || 0;
                const nombre = rep.nombreRepuesto || 'Repuesto';
                serviciosRealizados.push(`• ${nombre} (${rep.cantidad}x $${precio})`);
              });
            }
          }
          
          const facturaTemporal = {
            id: servicioId,
            clienteNombre: servicioCompletado.clienteNombre || 'Cliente',
            vehiculoPlaca: servicioCompletado.vehiculoInfo || 'N/D',
            subtotal: costoTotal,
            impuestos: iva,
            total: totalConIva,
            serviciosRealizados: serviciosRealizados
          };
          
          console.log('Generando PDF con factura temporal:', facturaTemporal);
          const blob = await descargarFacturaPdf(facturaTemporal);
          
          const url = URL.createObjectURL(blob);
          const a = document.createElement('a');
          a.href = url;
          a.download = `factura_temporal_servicio_${servicioId}.pdf`;
          document.body.appendChild(a);
          a.click();
          a.remove();
          URL.revokeObjectURL(url);
          console.log('Descarga de factura temporal completada');
        } else {
          alert('No se pudo generar la factura temporal. Servicio no encontrado.');
        }
      } else {
        alert(`Error al descargar la factura: ${error.message}`);
      }
    }
  };

  const loadData = async () => {
    try {
      setLoading(true);
      const [ordenesData, serviciosData, facturasData, empleadosData] = await Promise.all([
        listOrdenes(),
        listServicios(),
        listFacturas(),
        listEmpleados().catch(() => [])
      ]);
      setOrdenes(ordenesData || []);
      setServicios(serviciosData || []);
      setFacturas(facturasData || []);
      setEmpleados(empleadosData || []);
      // cargar datos específicos según el tipo de empleado
      if (tipo === 'secretaria') {
        const [p, a, c] = await Promise.all([
          secretariaListPendientes().catch(() => []),
          secretariaListAsignados().catch(() => []),
          secretariaListCompletados().catch(() => [])
        ]);
        setSecPendientes(p || []);
        setSecAsignados(a || []);
        setSecCompletados(c || []);
      } else if (tipo === 'mecanico') {
        // Debug: mostrar información del usuario
        console.log('Usuario mecánico:', user);
        console.log('ID empleado:', user?.id);
        console.log('Tipo de usuario:', user?.tipo);
        console.log('Info específica:', user?.infoEspecifica);
        
        if (user?.id) {
          // Usar el ID del empleado correcto (3 para Gustavo)
          const empleadoId = user.infoEspecifica?.id || user.id || 3; // Fallback a ID 3
          console.log('Usando ID de empleado:', empleadoId);
          
          const [asignados, completados, repuestosData] = await Promise.all([
            mecanicoListAsignados(empleadoId).catch((err) => {
              console.error('Error cargando servicios asignados:', err);
              return [];
            }),
            mecanicoListCompletados(empleadoId).catch((err) => {
              console.error('Error cargando servicios completados:', err);
              return [];
            }),
            listRepuestos().catch((err) => {
              console.error('Error cargando repuestos:', err);
              return [];
            })
          ]);
          
          console.log('Servicios asignados recibidos:', asignados);
          console.log('Servicios completados recibidos:', completados);
          console.log('Cantidad de servicios completados:', completados?.length || 0);
          
          // Debug: probar endpoint de debug
          try {
            const debugData = await debugMecanicoCompletados(empleadoId);
            console.log('Debug data:', debugData);
            console.log('Debug - Cantidad completados:', debugData?.Completados?.length || 0);
          } catch (err) {
            console.error('Error en debug:', err);
          }
          
          // Combinar todos los servicios y organizarlos por estado
          const todosServicios = [...(asignados || []), ...(completados || [])];
          console.log('Todos los servicios del mecánico:', todosServicios);
          
          // Eliminar duplicados por ID
          const serviciosUnicos = todosServicios.filter((servicio, index, self) => 
            index === self.findIndex(s => s.id === servicio.id)
          );
          
          console.log('Servicios únicos:', serviciosUnicos);
          organizarServiciosPorEstado(serviciosUnicos);
          
          // Mantener compatibilidad con el código existente
          setMecanicoAsignados(asignados || []);
          setMecanicoCompletados(completados || []);
          setRepuestos(repuestosData || []);
        } else {
          console.log('No se encontró ID de empleado para el mecánico, usando endpoint de secretaría');
          // Fallback: usar endpoint de secretaría para obtener servicios asignados
          try {
            const asignados = await secretariaListAsignados();
            console.log('Servicios asignados (fallback):', asignados);
            
            // Organizar también los servicios del fallback por estado
            organizarServiciosPorEstado(asignados || []);
            setMecanicoAsignados(asignados || []);
          } catch (err) {
            console.error('Error en fallback:', err);
            setMecanicoAsignados([]);
          }
        }
      }
    } catch (e) {
      setError(e?.message || 'Error al cargar datos');
    } finally {
      setLoading(false);
    }
  };

  // Función para organizar servicios por estados
  const organizarServiciosPorEstado = (servicios) => {
    console.log('Organizando servicios por estado:', servicios);
    
    const pendientes = servicios.filter(s => s.idEstado === 1 || s.estadoDescripcion?.toLowerCase() === 'pendiente');
    const enProceso = servicios.filter(s => s.idEstado === 2 || s.estadoDescripcion?.toLowerCase() === 'en proceso');
    const completados = servicios.filter(s => s.idEstado === 3 || s.estadoDescripcion?.toLowerCase() === 'completado');
    const cancelados = servicios.filter(s => s.idEstado === 4 || s.estadoDescripcion?.toLowerCase() === 'cancelado');
    
    console.log('Servicios pendientes:', pendientes);
    console.log('Servicios en proceso:', enProceso);
    console.log('Servicios completados:', completados);
    console.log('Servicios cancelados:', cancelados);
    
    setServiciosPendientes(pendientes);
    setServiciosAsignados([]); // No existe estado "Asignado" en la BD
    setServiciosEnProceso(enProceso);
    setServiciosCompletados(completados);
  };

  // Función para renderizar lista de servicios
  const renderizarServicios = (servicios, esCompletado = false) => {
    if (servicios.length === 0) {
      return (
        <div style={{ textAlign: 'center', padding: '2rem', color: '#666' }}>
          <p>No hay servicios en este estado</p>
          <p><small>Los servicios aparecerán aquí cuando cambien a este estado</small></p>
        </div>
      );
    }

    return servicios.map(servicio => {
      // Debug: mostrar información del servicio
      console.log(`Servicio #${servicio.id}:`, {
        tipoServicio: servicio.tipoServicio,
        esRevision: servicio.tipoServicio === 'Revision',
        mostrarBotonRepuestos: servicio.tipoServicio !== 'Revision'
      });
      
      return (
      <div key={servicio.id} className="orden-card">
        <div className="orden-info">
          <h4>Servicio #{servicio.id} · {(servicio.tipoServicio === 'Revision' ? 'Revisión' : 'Reparación')}</h4>
          <p><strong>Cliente:</strong> {servicio.clienteNombre || 'N/D'}</p>
          <p><strong>Vehículo:</strong> {servicio.vehiculoMarca} {servicio.vehiculoModelo} - {servicio.vehiculoPlaca || 'Sin placa'}</p>
          <p><strong>Estado:</strong> {servicio.estadoDescripcion}</p>
          <p><strong>Fecha {esCompletado ? 'Completado' : 'Creación'}:</strong> {
            esCompletado 
              ? (servicio.fechaActualizacion ? new Date(servicio.fechaActualizacion).toLocaleDateString() : 'N/D')
              : (servicio.fechaCreacion ? new Date(servicio.fechaCreacion).toLocaleDateString() : 'N/D')
          }</p>
          {servicio.detallesRevision && (
            <p><strong>Detalles Revisión:</strong> {servicio.detallesRevision}</p>
          )}
        </div>
        {!esCompletado && (
          <div className="orden-actions">
            <select 
              value={servicio.idEstado}
              onChange={(e) => handleServicioEstadoChange(servicio.id, parseInt(e.target.value))}
            >
              <option value={1}>Pendiente</option>
              <option value={2}>En Proceso</option>
              <option value={3}>Completado</option>
              {tipo !== 'mecanico' && <option value={4}>Cancelado</option>}
              {tipo === 'mecanico' && <option value={1}>Devolver a Secretaria</option>}
            </select>
            {servicio.tipoServicio !== 'Revision' && (
              <button 
                className="btn-secondary" 
                onClick={() => openReparacionModal(servicio)}
                style={{ marginTop: '0.5rem' }}
              >
                Agregar Repuestos
              </button>
            )}
            {servicio.tipoServicio === 'Revision' && (
              <button 
                className="btn-secondary" 
                onClick={() => openRevisionModal(servicio)}
                style={{ marginTop: '0.5rem' }}
              >
                {servicio.detallesRevision ? 'Editar Detalles' : 'Agregar Detalles'}
              </button>
            )}
          </div>
        )}
      </div>
      );
    });
  };

  // Mechanic specific functions
  const handleServicioEstadoChange = async (servicioId, nuevoEstado) => {
    try {
      if (tipo === 'mecanico' && nuevoEstado === 1) {
        // Cuando el mecánico devuelve a secretaria, también desasignar el empleado
        await updateServicioEstado(servicioId, nuevoEstado);
        await desasignarEmpleado(servicioId);
      } else {
        await updateServicioEstado(servicioId, nuevoEstado);
      }
      await loadData();
    } catch (err) {
      alert('Error al actualizar estado del servicio');
    }
  };

  const openReparacionModal = async (servicio) => {
    setSelectedServicio(servicio);
    setShowReparacionModal(true);
    
    // Cargar repuestos ya utilizados en esta reparación
    try {
      const repuestosUtilizados = await getRepuestosByDetalleReparacion(servicio.id);
      setRepuestosReparacion(repuestosUtilizados || []);
      
      // Si ya hay un repuesto, pre-cargar el formulario
      if (repuestosUtilizados && repuestosUtilizados.length > 0) {
        const repuestoExistente = repuestosUtilizados[0];
        setRepuestoForm({ 
          idRepuesto: repuestoExistente.idRepuesto.toString(), 
          cantidad: repuestoExistente.cantidad 
        });
      } else {
        setRepuestoForm({ idRepuesto: '', cantidad: 1 });
      }
    } catch (err) {
      console.error('Error cargando repuestos de reparación:', err);
      setRepuestosReparacion([]);
      setRepuestoForm({ idRepuesto: '', cantidad: 1 });
    }
  };

  const openRevisionModal = async (servicio) => {
    setSelectedServicio(servicio);
    try {
      const detalle = await getDetalleRevision(servicio.id);
      setRevisionForm({ 
        detalles: detalle?.detalles || '',
        detallesEncontrados: detalle?.detallesEncontrados || ''
      });
    } catch (err) {
      setRevisionForm({ 
        detalles: '',
        detallesEncontrados: ''
      });
    }
    setShowRevisionModal(true);
  };

  const handleAddRepuesto = async () => {
    try {
      const repuestoData = {
        idRepuesto: parseInt(repuestoForm.idRepuesto),
        cantidad: parseInt(repuestoForm.cantidad)
      };

      let response;
      // Si ya hay repuestos, intentar actualizar; si no, crear nuevo
      if (repuestosReparacion.length > 0) {
        response = await updateRepuestoInReparacion(selectedServicio.id, repuestoData);
        alert('Repuesto actualizado correctamente');
      } else {
        response = await addRepuestoToReparacion(selectedServicio.id, repuestoData);
        alert('Repuesto agregado correctamente');
      }

      // Actualizar el costo del servicio en el estado local
      if (response && response.costoTotalServicio !== undefined) {
        setSelectedServicio(prev => ({
          ...prev,
          costo: response.costoTotalServicio
        }));
      }
      
      // Recargar la lista de repuestos utilizados
      const repuestosUtilizados = await getRepuestosByDetalleReparacion(selectedServicio.id);
      setRepuestosReparacion(repuestosUtilizados || []);
      
      // Limpiar el formulario
      setRepuestoForm({ idRepuesto: '', cantidad: 1 });
      
      await loadData();
    } catch (err) {
      console.error('Error al procesar repuesto:', err);
      const errorMessage = err.response?.data?.message || err.response?.data || err.message || 'Error desconocido';
      alert(`Error: ${errorMessage}`);
    }
  };

  const handleSaveRevision = async () => {
    try {
      if (selectedServicio.detallesRevision) {
        await updateDetalleRevision(selectedServicio.id, {
          idServicio: selectedServicio.id,
          detalles: revisionForm.detalles,
          detallesEncontrados: revisionForm.detallesEncontrados
        });
      } else {
        await createDetalleRevision({
          idServicio: selectedServicio.id,
          detalles: revisionForm.detalles,
          detallesEncontrados: revisionForm.detallesEncontrados
        });
      }
      alert('Detalles de revisión guardados');
      setShowRevisionModal(false);
      await loadData();
    } catch (err) {
      alert('Error al guardar detalles de revisión');
    }
  };

  // Contenido específico para cada tipo de empleado
  // Función para obtener el nombre del tipo de empleado
  const getTipoEmpleadoNombre = (idTipoEmpleado) => {
    const tipos = {
      1: 'Administrador',
      2: 'Secretaria',
      3: 'Mecánico'
    };
    return tipos[idTipoEmpleado] || 'Empleado';
  };

  // Función helper para filtrar solo mecánicos
  const filtrarMecanicos = (empleadosList) => {
    return empleadosList.filter(emp => {
      // Múltiples opciones para el tipo de empleado
      const tipo = emp.idTipoEmpleado || emp.tipoEmpleado || emp.idTipo || emp.tipoEmpleadoId || emp.tipo;
      return tipo === 3 || tipo === '3' || tipo === 'Mecánico' || tipo === 'mecanico';
    });
  };

  const getEmpleadoContent = () => {
    switch(tipo) {
      case 'mecanico':
        return {
          title: 'Panel de Mecánico',
          subtitle: 'Gestiona tus reparaciones y mantenimientos',
          features: [
            { title: 'Pendientes', count: serviciosPendientes.length },
            { title: 'En Proceso', count: serviciosEnProceso.length },
            { title: 'Completados', count: serviciosCompletados.length }
          ],
          content: (
            <div className="mecanico-content">
              <div className="dashboard-tabs" style={{ marginBottom: '1rem' }}>
                <button 
                  className={`tab-button ${secTab === 'pendientes' ? 'active' : ''}`} 
                  onClick={() => setSecTab('pendientes')}
                >
                  Pendientes ({serviciosPendientes.length})
                </button>
                <button 
                  className={`tab-button ${secTab === 'en-proceso' ? 'active' : ''}`} 
                  onClick={() => setSecTab('en-proceso')}
                >
                  En Proceso ({serviciosEnProceso.length})
                </button>
                <button 
                  className={`tab-button ${secTab === 'completados' ? 'active' : ''}`} 
                  onClick={() => setSecTab('completados')}
                >
                  Completados ({serviciosCompletados.length})
                </button>
                <button 
                  className={`tab-button ${secTab === 'repuestos' ? 'active' : ''}`} 
                  onClick={() => setSecTab('repuestos')}
                >
                  Repuestos Disponibles ({repuestos.length})
                </button>
              </div>
              
              {loading ? (
                <p>Cargando...</p>
              ) : (
                <div className="ordenes-list">
                  {secTab === 'pendientes' && renderizarServicios(serviciosPendientes)}
                  {secTab === 'en-proceso' && renderizarServicios(serviciosEnProceso)}
                  {secTab === 'completados' && (
                    <>
                      {console.log('Renderizando completados, cantidad:', serviciosCompletados.length, 'servicios:', serviciosCompletados)}
                      {serviciosCompletados.length === 0 ? (
                        <div style={{ textAlign: 'center', padding: '2rem', color: '#666' }}>
                          <p>No hay servicios completados</p>
                          <p><small>Los servicios completados aparecerán aquí</small></p>
                          <p><small>Debug: serviciosCompletados.length = {serviciosCompletados.length}</small></p>
                        </div>
                      ) : (
                        renderizarServicios(serviciosCompletados, true)
                      )}
                    </>
                  )}
                  {secTab === 'repuestos' && (
                    <div className="repuestos-grid">
                      <h3 style={{ marginBottom: '1rem', color: '#333' }}>Repuestos Disponibles</h3>
                      {repuestos.length === 0 ? (
                        <div style={{ textAlign: 'center', padding: '2rem', color: '#666' }}>
                          <p>No hay repuestos disponibles</p>
                        </div>
                      ) : (
                        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))', gap: '1rem' }}>
                          {repuestos.map(repuesto => (
                            <div key={repuesto.id} className="repuesto-card" style={{
                              border: '1px solid #ddd',
                              borderRadius: '8px',
                              padding: '1rem',
                              backgroundColor: '#fff',
                              boxShadow: '0 2px 4px rgba(0,0,0,0.1)'
                            }}>
                              <h4 style={{ margin: '0 0 0.5rem 0', color: '#333' }}>{repuesto.nombre}</h4>
                              <p style={{ margin: '0.25rem 0', color: '#666' }}><strong>N° Serie:</strong> {repuesto.numeroSerie}</p>
                              <p style={{ margin: '0.25rem 0', color: '#666' }}><strong>Precio:</strong> ${repuesto.precio}</p>
                              <p style={{ margin: '0.25rem 0', color: repuesto.stock > 5 ? '#28a745' : repuesto.stock > 0 ? '#ffc107' : '#dc3545' }}>
                                <strong>Stock:</strong> {repuesto.stock} {repuesto.stock === 1 ? 'unidad' : 'unidades'}
                              </p>
                            </div>
                          ))}
                        </div>
                      )}
                    </div>
                  )}
                </div>
              )}
            </div>
          )
        };
      
      case 'secretaria':
        return {
          title: 'Panel de Secretaría',
          subtitle: 'Gestiona citas, clientes y administración',
          features: [
            { title: 'Trabajos Pendientes', count: secPendientes.length },
            { title: 'Trabajos Asignados', count: secAsignados.length },
            { title: 'Trabajos Completados', count: secCompletados.length },
            { title: 'Servicios Activos', count: (secPendientes.length + secAsignados.length) }
          ],
          content: (
            <div className="secretaria-content">
              <div className="dashboard-tabs" style={{ marginBottom: '1rem' }}>
                <button className={`tab-button ${secTab==='pendientes'?'active':''}`} onClick={()=>setSecTab('pendientes')}>Trabajos Pendientes</button>
                <button className={`tab-button ${secTab==='asignados'?'active':''}`} onClick={()=>setSecTab('asignados')}>Trabajos Asignados</button>
                <button className={`tab-button ${secTab==='completados'?'active':''}`} onClick={()=>setSecTab('completados')}>Trabajos Completados</button>
                <button className={`tab-button ${secTab==='proveedores'?'active':''}`} onClick={()=>setSecTab('proveedores')}>Proveedores</button>
                <button className={`tab-button ${secTab==='repuestos'?'active':''}`} onClick={()=>setSecTab('repuestos')}>Repuestos</button>
              </div>
              {loading ? <p>Cargando...</p> : (
                secTab === 'pendientes' ? (
                  <div className="ordenes-list">
                    {secPendientes.map(s => (
                      <div key={s.id} className="orden-card">
                        <div className="orden-info">
                          <h4>Servicio #{s.id} · {(s.tipoServicio==='Revision'?'Revisión':s.tipoServicio==='Reparacion'?'Reparación':(s.detallesRevision?'Revisión':'Reparación'))}</h4>
                          <p>Cliente: {s.clienteNombre || 'N/D'}</p>
                          <p>Estado: {s.estadoDescripcion}</p>
                          {s.detallesRevision && <p>Detalles: {s.detallesRevision}</p>}
                        </div>
                        <div className="orden-actions">
                          <select onChange={async (e)=>{ const empId = parseInt(e.target.value); if(!empId) return; await secretariaAsignarMecanico(s.id, empId); await loadData(); e.target.value=''; }} defaultValue="">
                            <option value="" disabled>Asignar mecánico</option>
                            {filtrarMecanicos(empleados).map(emp => (
                              <option key={emp.id} value={emp.id}>{emp.nombre} {emp.apellido || ''}</option>
                            ))}
                          </select>
                        </div>
                      </div>
                    ))}
                  </div>
                ) : secTab === 'asignados' ? (
                  <div className="ordenes-list">
                    {secAsignados.map(s => (
                      <div key={s.id} className="orden-card">
                        <div className="orden-info">
                          <h4>Servicio #{s.id} · {(s.tipoServicio==='Revision'?'Revisión':s.tipoServicio==='Reparacion'?'Reparación':(s.detallesRevision?'Revisión':'Reparación'))}</h4>
                          <p>Cliente: {s.clienteNombre || 'N/D'}</p>
                          <p>Estado: {s.estadoDescripcion}</p>
                          <p>Mecánico: {s.empleadoNombre || 'Asignado'}</p>
                        </div>
                      </div>
                    ))}
                  </div>
                ) : secTab === 'completados' ? (
                  <div className="ordenes-list">
                    {secCompletados.map(s => (
                      <div key={s.id} className="orden-card">
                        <div className="orden-info">
                          <h4>Servicio #{s.id} · {(s.tipoServicio==='Revision'?'Revisión':s.tipoServicio==='Reparacion'?'Reparación':(s.detallesRevision?'Revisión':'Reparación'))}</h4>
                          <p>Cliente: {s.clienteNombre || 'N/D'}</p>
                          <p>Estado: {s.estadoDescripcion}</p>
                          <p>Mecánico: {s.empleadoNombre || 'Completado'}</p>
                        </div>
                        <div className="orden-actions">
                          <button className="btn-primary" onClick={() => handleDescargarFactura(s.id)}>
                            📄 Descargar Factura
                          </button>
                        </div>
                      </div>
                    ))}
                  </div>
                ) : secTab === 'proveedores' ? (
                  <ProveedorManagement />
                ) : secTab === 'repuestos' ? (
                  <RepuestoManagement />
                ) : null
              )}

              {/* Búsqueda de servicios para secretaria */}
              <div className="secretaria-actions" style={{ marginTop: '2rem' }}>
                <button 
                  className="btn-secondary"
                  onClick={() => setShowBusqueda(true)}
                >
                  🔍 Búsqueda de Servicios
                </button>
              </div>
            </div>
          )
        };
      
      case 'administrador':
        return {
          title: 'Panel de Administración',
          subtitle: 'Gestiona el taller completo',
          features: [
            { title: 'Total Órdenes', count: ordenes.length },
            { title: 'Servicios Activos', count: servicios.filter(s => s.idEstado !== 4).length },
            { title: 'Facturas Generadas', count: facturas.length },
            { title: 'Empleados Activos', count: empleados.length }
          ],
          content: (
            <div className="admin-content">
              <div className="dashboard-grid">
                <div className="dashboard-card">
                  <h3>Resumen de Órdenes</h3>
                  <div className="stats">
                    <div className="stat">
                      <span className="stat-label">Pendientes:</span>
                      <span className="stat-value">{ordenes.filter(o => o.idTipoEstadoOrden === 1).length}</span>
                    </div>
                    <div className="stat">
                      <span className="stat-label">En Proceso:</span>
                      <span className="stat-value">{ordenes.filter(o => o.idTipoEstadoOrden === 3).length}</span>
                    </div>
                    <div className="stat">
                      <span className="stat-label">Completadas:</span>
                      <span className="stat-value">{ordenes.filter(o => o.idTipoEstadoOrden === 4).length}</span>
                    </div>
                  </div>
                </div>
                
                <div className="dashboard-card">
                  <h3>Estados de Servicios</h3>
                  <div className="servicios-stats">
                    <div className="stat">
                      <span className="stat-label">Pendientes:</span>
                      <span className="stat-value">{servicios.filter(s => s.idEstado === 1).length}</span>
                    </div>
                    <div className="stat">
                      <span className="stat-label">Asignados:</span>
                      <span className="stat-value">{servicios.filter(s => s.idEstado === 2).length}</span>
                    </div>
                    <div className="stat">
                      <span className="stat-label">En Proceso:</span>
                      <span className="stat-value">{servicios.filter(s => s.idEstado === 3).length}</span>
                    </div>
                    <div className="stat">
                      <span className="stat-label">Completados:</span>
                      <span className="stat-value">{servicios.filter(s => s.idEstado === 4).length}</span>
                    </div>
                  </div>
                </div>
                
                <div className="dashboard-card">
                  <h3>Facturación</h3>
                  <div className="facturas-stats">
                    <div className="stat">
                      <span className="stat-label">Pendientes:</span>
                      <span className="stat-value">{facturas.filter(f => f.estado === 'Pendiente').length}</span>
                    </div>
                    <div className="stat">
                      <span className="stat-label">Pagadas:</span>
                      <span className="stat-value">{facturas.filter(f => f.estado === 'Pagada').length}</span>
                    </div>
                    <div className="stat">
                      <span className="stat-label">Total Facturado:</span>
                      <span className="stat-value">${facturas.reduce((sum, f) => sum + f.total, 0).toFixed(2)}</span>
                    </div>
                  </div>
                </div>
              </div>
              <div className="empleado-form-container">
                <div className="empleado-form-header">
                  <h3>Registro de Empleados</h3>
                  <p>Gestiona el personal del taller con información completa</p>
                </div>
                
                <form
                  className="empleado-form"
                  onSubmit={async (e) => {
                    e.preventDefault();
                    try {
                      setCreating(true);
                      setError('');
                      const created = await createEmpleado({
                        nombre: empleadoForm.nombre,
                        apellido: empleadoForm.apellido,
                        cedula: Number(empleadoForm.cedula),
                        salario: Number(empleadoForm.salario),
                        idTipoEmpleado: Number(empleadoForm.idTipoEmpleado)
                      });
                      const idEmpleado = created?.id;
                      const idTaller = empleadoForm.idTaller || user?.idTaller || user?.tallerId;
                      if (!idEmpleado || !idTaller) {
                        alert('Empleado creado, pero falta idTaller para registro de acceso');
                        await loadData();
                        setCreating(false);
                        return;
                      }
                      await registerEmpleado({
                        email: empleadoForm.email,
                        password: empleadoForm.password,
                        idEmpleado,
                        idTaller
                      });
                      alert('Empleado registrado y credenciales creadas');
                      setEmpleadoForm({ nombre: '', apellido: '', cedula: '', salario: '', idTipoEmpleado: 1, email: '', password: '', idTaller: '' });
                      await loadData();
                    } catch (err) {
                      setError(err?.message || 'Error en registro de empleado');
                    } finally {
                      setCreating(false);
                    }
                  }}
                >
                  {error && <div className="empleado-form-error">{error}</div>}
                  
                  <div className="empleado-form-grid">
                    <div className="empleado-form-group">
                      <label htmlFor="nombre">Nombre</label>
                      <input 
                        id="nombre"
                        className="empleado-form-input"
                        placeholder="Ingresa el nombre del empleado" 
                        required 
                        value={empleadoForm.nombre} 
                        onChange={(e) => setEmpleadoForm({ ...empleadoForm, nombre: e.target.value })} 
                      />
                    </div>
                    
                    <div className="empleado-form-group">
                      <label htmlFor="apellido">Apellido</label>
                      <input 
                        id="apellido"
                        className="empleado-form-input"
                        placeholder="Ingresa el apellido del empleado" 
                        required 
                        value={empleadoForm.apellido} 
                        onChange={(e) => setEmpleadoForm({ ...empleadoForm, apellido: e.target.value })} 
                      />
                    </div>
                    
                    <div className="empleado-form-group">
                      <label htmlFor="cedula">Cédula</label>
                      <input 
                        id="cedula"
                        className="empleado-form-input"
                        placeholder="Número de cédula" 
                        required 
                        type="number" 
                        value={empleadoForm.cedula} 
                        onChange={(e) => setEmpleadoForm({ ...empleadoForm, cedula: e.target.value })} 
                      />
                    </div>
                    
                    <div className="empleado-form-group">
                      <label htmlFor="salario">Salario</label>
                      <input 
                        id="salario"
                        className="empleado-form-input"
                        placeholder="Salario mensual" 
                        required 
                        type="number" 
                        step="0.01" 
                        value={empleadoForm.salario} 
                        onChange={(e) => setEmpleadoForm({ ...empleadoForm, salario: e.target.value })} 
                      />
                    </div>
                    
                    <div className="empleado-form-group">
                      <label htmlFor="tipo">Tipo de Empleado</label>
                      <select 
                        id="tipo"
                        className="empleado-form-select"
                        value={empleadoForm.idTipoEmpleado} 
                        onChange={(e) => setEmpleadoForm({ ...empleadoForm, idTipoEmpleado: e.target.value })}
                      >
                        <option value={1}>Administrador</option>
                        <option value={2}>Secretaria</option>
                        <option value={3}>Mecánico</option>
                      </select>
                    </div>
                    
                    <div className="empleado-form-group">
                      <label htmlFor="email">Email</label>
                      <input 
                        id="email"
                        className="empleado-form-input"
                        placeholder="correo@ejemplo.com" 
                        required 
                        type="email" 
                        value={empleadoForm.email} 
                        onChange={(e) => setEmpleadoForm({ ...empleadoForm, email: e.target.value })} 
                      />
                    </div>
                    
                    <div className="empleado-form-group">
                      <label htmlFor="password">Contraseña</label>
                      <input 
                        id="password"
                        className="empleado-form-input"
                        placeholder="Contraseña de acceso" 
                        required 
                        type="password" 
                        value={empleadoForm.password} 
                        onChange={(e) => setEmpleadoForm({ ...empleadoForm, password: e.target.value })} 
                      />
                    </div>
                    
                    <div className="empleado-form-group">
                      <label htmlFor="idTaller">ID del Taller</label>
                      <input 
                        id="idTaller"
                        className="empleado-form-input"
                        placeholder="Identificador del taller" 
                        type="number" 
                        value={empleadoForm.idTaller} 
                        onChange={(e) => setEmpleadoForm({ ...empleadoForm, idTaller: e.target.value })} 
                      />
                    </div>
                  </div>
                  
                  <div className="empleado-form-actions">
                    <button 
                      className="empleado-form-btn empleado-form-btn-primary" 
                      type="submit" 
                      disabled={creating}
                    >
                      {creating ? 'Creando...' : 'Crear y Registrar'}
                    </button>
                    <button 
                      className="empleado-form-btn empleado-form-btn-secondary" 
                      type="button" 
                      onClick={() => setEmpleadoForm({ nombre: '', apellido: '', cedula: '', salario: '', idTipoEmpleado: 1, email: '', password: '', idTaller: '' })}
                    >
                      Limpiar Formulario
                    </button>
                  </div>
                </form>
                
                <div className="empleados-actuales">
                  <h4>Empleados Actuales</h4>
                  <div className="empleados-grid">
                    {empleados.map((emp) => (
                      <div key={emp.id} className={`empleado-card ${!emp.activo ? 'inactivo' : ''}`}>
                        <div className="empleado-card-title">{emp.nombre} {emp.apellido}</div>
                        <div className="empleado-card-subtitle">{getTipoEmpleadoNombre(emp.idTipoEmpleado)}</div>
                        <div className="empleado-card-info">
                          <strong>Cédula:</strong> {emp.cedula}
                        </div>
                        <div className="empleado-card-info">
                          <strong>Salario:</strong> ${emp.salario?.toLocaleString()}
                        </div>
                        <div className={`empleado-estado ${emp.activo ? 'activo' : 'inactivo'}`}>
                          {emp.activo ? 'Activo' : 'Inactivo'}
                        </div>
                      </div>
                    ))}
                  </div>
                </div>
              </div>
              
              {/* Componente de gestión avanzada de empleados */}
              <div style={{ marginTop: '2rem' }}>
                <EmpleadosManager />
              </div>

              {/* Nuevas funcionalidades del admin */}
              <div className="admin-actions" style={{ marginTop: '2rem' }}>
                <button 
                  className="btn-primary"
                  onClick={() => setShowDashboard(true)}
                  style={{ marginRight: '1rem' }}
                >
                  📊 Dashboard Avanzado
                </button>
                <button 
                  className="btn-secondary"
                  onClick={() => setShowBusqueda(true)}
                  style={{ marginRight: '1rem' }}
                >
                  🔍 Búsqueda de Servicios
                </button>
                <button 
                  className="btn-secondary"
                  onClick={() => setShowProveedores(true)}
                  style={{ marginRight: '1rem' }}
                >
                  🏢 Gestión de Proveedores
                </button>
                <button 
                  className="btn-secondary"
                  onClick={() => setShowRepuestos(true)}
                  style={{ marginRight: '1rem' }}
                >
                  🔧 Gestión de Repuestos
                </button>
                <button 
                  className="btn-secondary"
                  onClick={() => setShowRepuestoProveedor(true)}
                >
                  🔗 Relaciones Repuesto-Proveedor
                </button>
              </div>
            </div>
          )
        };
      
      default:
        return {
          title: 'Panel de Empleado',
          subtitle: 'Bienvenido al sistema',
          features: [],
          actions: []
        };
    }
  };

  const content = getEmpleadoContent();

  return (
    <div className="home-empleado-page">
      <Header />
      
      <div className="empleado-container">
        <div className="empleado-header">
          <h1>{content.title}</h1>
          <p>{content.subtitle}</p>
          <div className="user-info">
            <span>Conectado como: {user?.infoEspecifica?.nombre || user?.email || 'Empleado'}</span>
            {user?.infoEspecifica?.tipoEmpleado && (
              <span className="tipo-empleado"> ({user.infoEspecifica.tipoEmpleado})</span>
            )}
            {user?.nombreTaller && (
              <span className="taller-info"> - {user.nombreTaller}</span>
            )}
          </div>
        </div>
        
        <div className="dashboard-section">
          <h2>Resumen</h2>
          <div className="stats-grid">
            {content.features.map((feature, index) => (
              <div key={index} className="stat-card">
                <h3>{feature.title}</h3>
                <div className="stat-value">{feature.count}</div>
              </div>
            ))}
          </div>
        </div>
        
        {content.content && (
          <div className="content-section">
            {content.content}
          </div>
        )}
        
        <div className="recent-activity">
          <h2>Actividad Reciente</h2>
          <div className="activity-list">
            <div className="activity-item">
              <p>Sesión iniciada correctamente</p>
              <span className="activity-time">Hace unos momentos</span>
            </div>
            {tipo === 'mecanico' && mecanicoCompletados.length > 0 && (
              <div className="activity-item">
                <p>Último servicio completado: #{mecanicoCompletados[0].id}</p>
                <span className="activity-time">
                  {new Date(mecanicoCompletados[0].fechaActualizacion || mecanicoCompletados[0].fechaCreacion).toLocaleString()}
                </span>
              </div>
            )}
            {tipo === 'secretaria' && secAsignados.length > 0 && (
              <div className="activity-item">
                <p>Servicios asignados: {secAsignados.length}</p>
                <span className="activity-time">Actualmente</span>
              </div>
            )}
            {tipo === 'administrador' && (
              <>
                <div className="activity-item">
                  <p>Total de empleados registrados: {empleados.length}</p>
                  <span className="activity-time">Sistema</span>
                </div>
                <div className="activity-item">
                  <p>Servicios activos: {servicios.filter(s => s.idEstado !== 4).length}</p>
                  <span className="activity-time">Actualmente</span>
                </div>
              </>
            )}
          </div>
        </div>
      </div>

      {/* Modal para agregar repuestos a reparación */}
      {showReparacionModal && selectedServicio && (
        <div className="modal-overlay">
          <div className="modal-content">
            <div className="modal-header">
              <h3>{repuestosReparacion.length > 0 ? 'Gestionar Repuesto' : 'Agregar Repuesto'} - Servicio #{selectedServicio.id}</h3>
              <p style={{ margin: '0.5rem 0', color: '#666', fontSize: '0.9rem' }}>
                <strong>Costo actual del servicio:</strong> ${selectedServicio.costo || 0}
              </p>
              <button className="modal-close" onClick={() => setShowReparacionModal(false)}>×</button>
            </div>
            <div className="modal-body">
              {/* Lista de repuestos ya utilizados */}
              {repuestosReparacion.length > 0 && (
                <div style={{ marginBottom: '1.5rem' }}>
                  <h4 style={{ marginBottom: '1rem', color: '#333' }}>Repuestos Utilizados:</h4>
                  <div style={{ 
                    border: '1px solid #ddd', 
                    borderRadius: '8px', 
                    padding: '1rem',
                    backgroundColor: '#f9f9f9',
                    maxHeight: '200px',
                    overflowY: 'auto'
                  }}>
                    {repuestosReparacion.map((item, index) => (
                      <div key={index} style={{
                        display: 'flex',
                        justifyContent: 'space-between',
                        alignItems: 'center',
                        padding: '0.5rem 0',
                        borderBottom: index < repuestosReparacion.length - 1 ? '1px solid #eee' : 'none'
                      }}>
                        <div>
                          <strong>{item.nombreRepuesto || item.repuesto?.nombre || 'Repuesto'}</strong>
                          <span style={{ marginLeft: '1rem', color: '#666' }}>
                            Cantidad: {item.cantidad} | Precio: ${item.precioUnitario || item.repuesto?.precio || 0}
                          </span>
                        </div>
                        <div style={{ color: '#666', fontSize: '0.9rem' }}>
                          Total: ${item.subtotal || ((item.precioUnitario || item.repuesto?.precio || 0) * item.cantidad)}
                        </div>
                      </div>
                    ))}
                    <div style={{
                      marginTop: '1rem',
                      paddingTop: '1rem',
                      borderTop: '2px solid #ddd',
                      fontWeight: 'bold',
                      textAlign: 'right'
                    }}>
                      Total Reparación: ${repuestosReparacion.reduce((total, item) => 
                        total + (item.subtotal || ((item.precioUnitario || item.repuesto?.precio || 0) * item.cantidad)), 0
                      )}
                    </div>
                  </div>
                </div>
              )}

              {/* Formulario para agregar nuevo repuesto */}
              <h4 style={{ marginBottom: '1rem', color: '#333' }}>Agregar Nuevo Repuesto:</h4>
              <div className="form-group">
                <label>Repuesto:</label>
                <select 
                  value={repuestoForm.idRepuesto}
                  onChange={(e) => setRepuestoForm({...repuestoForm, idRepuesto: e.target.value})}
                >
                  <option value="">Seleccionar repuesto</option>
                  {repuestos.filter(repuesto => repuesto.stock > 0).map(repuesto => (
                    <option key={repuesto.id} value={repuesto.id}>
                      {repuesto.nombre} - ${repuesto.precio} (Stock: {repuesto.stock})
                    </option>
                  ))}
                </select>
              </div>
              <div className="form-group">
                <label>Cantidad:</label>
                <input 
                  type="number" 
                  min="1" 
                  max={repuestos.find(r => r.id === parseInt(repuestoForm.idRepuesto))?.stock || 1}
                  value={repuestoForm.cantidad}
                  onChange={(e) => setRepuestoForm({...repuestoForm, cantidad: e.target.value})}
                />
              </div>
            </div>
            <div className="modal-footer">
              <button className="btn-secondary" onClick={() => setShowReparacionModal(false)}>
                Cancelar
              </button>
              <button 
                className="btn-primary" 
                onClick={handleAddRepuesto}
                disabled={!repuestoForm.idRepuesto || repuestoForm.cantidad < 1}
              >
                {repuestosReparacion.length > 0 ? 'Actualizar Repuesto' : 'Agregar Repuesto'}
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Modal para detalles de revisión */}
      {showRevisionModal && selectedServicio && (
        <div className="modal-overlay">
          <div className="modal-content">
            <div className="modal-header">
              <h3>Detalles de Revisión - Servicio #{selectedServicio.id}</h3>
              <button className="modal-close" onClick={() => setShowRevisionModal(false)}>×</button>
            </div>
            <div className="modal-body">
              <div className="form-group">
                <label>Descripción del Servicio:</label>
                <textarea 
                  rows="4"
                  value={revisionForm.detalles}
                  onChange={(e) => setRevisionForm({...revisionForm, detalles: e.target.value})}
                  placeholder="Describe el tipo de revisión a realizar..."
                />
              </div>
              <div className="form-group">
                <label>Detalles Encontrados (Completar después de la revisión):</label>
                <textarea 
                  rows="6"
                  value={revisionForm.detallesEncontrados}
                  onChange={(e) => setRevisionForm({...revisionForm, detallesEncontrados: e.target.value})}
                  placeholder="Describe los hallazgos de la revisión, problemas encontrados, recomendaciones, etc."
                />
              </div>
            </div>
            <div className="modal-footer">
              <button className="btn-secondary" onClick={() => setShowRevisionModal(false)}>
                Cancelar
              </button>
              <button 
                className="btn-primary" 
                onClick={handleSaveRevision}
                disabled={!revisionForm.detalles.trim() && !revisionForm.detallesEncontrados.trim()}
              >
                Guardar Detalles
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Modal del Dashboard Admin */}
      {showDashboard && (
        <div className="modal-overlay">
          <div className="modal-content dashboard-modal">
            <div className="modal-header">
              <h3>Dashboard Administrativo</h3>
              <button className="modal-close" onClick={() => setShowDashboard(false)}>×</button>
            </div>
            <div className="modal-body">
              <DashboardAdmin />
            </div>
          </div>
        </div>
      )}

      {/* Modal de Búsqueda de Servicios */}
      {showBusqueda && (
        <BusquedaServicios
          onClose={() => setShowBusqueda(false)}
        />
      )}

      {/* Modal de Gestión de Proveedores */}
      {showProveedores && (
        <div className="modal-overlay">
          <div className="modal-content proveedor-modal">
            <div className="modal-header">
              <h3>Gestión de Proveedores</h3>
              <button className="modal-close" onClick={() => setShowProveedores(false)}>×</button>
            </div>
            <div className="modal-body">
              <ProveedorManagement />
            </div>
          </div>
        </div>
      )}

      {/* Modal de Gestión de Repuestos */}
      {showRepuestos && (
        <div className="modal-overlay">
          <div className="modal-content repuesto-modal">
            <div className="modal-header">
              <h3>Gestión de Repuestos</h3>
              <button className="modal-close" onClick={() => setShowRepuestos(false)}>×</button>
            </div>
            <div className="modal-body">
              <RepuestoManagement />
            </div>
          </div>
        </div>
      )}

      {/* Modal de Gestión de Relaciones Repuesto-Proveedor */}
      {showRepuestoProveedor && (
        <div className="modal-overlay">
          <div className="modal-content repuesto-proveedor-modal">
            <div className="modal-header">
              <h3>Gestión de Relaciones Repuesto-Proveedor</h3>
              <button className="modal-close" onClick={() => setShowRepuestoProveedor(false)}>×</button>
            </div>
            <div className="modal-body">
              <RepuestoProveedorManagement />
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default HomeEmpleado;