import api from './api';

// Estadísticas completas del admin
export async function getEstadisticasCompletas() {
  const { data } = await api.get('/EstadisticasAdmin/completas');
  return data;
}

// Resumen general
export async function getResumenGeneral() {
  const { data } = await api.get('/EstadisticasAdmin/resumen');
  return data;
}

// Estadísticas de servicios
export async function getEstadisticasServicios() {
  const { data } = await api.get('/EstadisticasAdmin/servicios');
  return data;
}

// Estadísticas de empleados
export async function getEstadisticasEmpleados() {
  const { data } = await api.get('/EstadisticasAdmin/empleados');
  return data;
}

// Estadísticas de facturación
export async function getEstadisticasFacturacion() {
  const { data } = await api.get('/EstadisticasAdmin/facturacion');
  return data;
}

// Reportes mensuales
export async function getReportesMensuales(mesesAtras = 12) {
  const { data } = await api.get('/EstadisticasAdmin/reportes-mensuales', {
    params: { mesesAtras }
  });
  return data;
}

// Capacidad del taller detallada
export async function getCapacidadTallerDetallada() {
  const { data } = await api.get('/EstadisticasAdmin/capacidad-taller');
  return data;
}

// Datos del dashboard
export async function getDashboardData() {
  const { data } = await api.get('/EstadisticasAdmin/dashboard');
  return data;
}
