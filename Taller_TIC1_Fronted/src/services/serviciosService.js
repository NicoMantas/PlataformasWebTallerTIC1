import api from './api';
import { jsPDF } from 'jspdf';

export async function listServicios() {
  const { data } = await api.get('/Servicios');
  return data;
}

export async function createServicio({ tipoServicio, idCliente, idVehiculo, detallesRevision, repuestosReparacion, costo }) {
  // Si se proporciona un costo específico, usarlo; de lo contrario, usar costos base por defecto
  const costosBase = {
    'revision': 80000,  // $80,000 para revisión (por defecto)
    'reparacion': 120000 // $120,000 para reparación (por defecto)
  };
  
  const payload = {
    costo: costo || costosBase[tipoServicio] || 0,
    idCliente,
    idVehiculo,
    idEmpleado: null,
    idEstado: 1,
    tipoServicio: tipoServicio === 'revision' ? 'Revision' : 'Reparacion',
    detallesRevision: tipoServicio === 'revision' ? (detallesRevision || '') : null,
    repuestosReparacion: tipoServicio === 'reparacion' ? (repuestosReparacion || []) : null
  };
  const { data } = await api.post('/Servicios', payload);
  return data;
}

export async function listServiciosByCliente(clienteId) {
  const { data } = await api.get(`/Servicios/por-cliente/${clienteId}`);
  return data;
}

export async function listActivosByCliente(clienteId) {
  const { data } = await api.get(`/Servicios/by-cliente/${clienteId}/activos`);
  return data;
}

export async function listHistorialByCliente(clienteId) {
  const { data } = await api.get(`/Servicios/by-cliente/${clienteId}/historial`);
  return data;
}

export async function listHistorialByVehiculo(vehiculoId) {
  const { data } = await api.get(`/Servicios/by-vehiculo/${vehiculoId}/historial`);
  return data;
}

export async function cancelarServicio(servicioId) {
  const { data } = await api.post(`/Servicios/${servicioId}/cancelar`);
  return data;
}

export async function crearOrden(payload) {
  const { data } = await api.post('/OrdenesTrabajo', payload);
  return data;
}

export async function secretariaListPendientes() {
  const { data } = await api.get('/Servicios/secretaria/pendientes');
  return data;
}

export async function secretariaListAsignados() {
  const { data } = await api.get('/Servicios/secretaria/asignados');
  return data;
}

export async function secretariaListCompletados() {
  const { data } = await api.get('/Servicios/secretaria/completados');
  return data;
}

export async function getCostoTotalServicio(servicioId) {
  const { data } = await api.get(`/Servicios/${servicioId}/costo-total`);
  return data;
}

export async function secretariaAsignarMecanico(servicioId, empleadoId) {
  const { data } = await api.post(`/Servicios/secretaria/${servicioId}/asignar/${empleadoId}`);
  return data;
}

export async function getServicioById(id) {
  const { data } = await api.get(`/Servicios/${id}`);
  return data;
}

export async function updateServicio(id, servicioData) {
  const { data } = await api.put(`/Servicios/${id}`, servicioData);
  return data;
}

export async function updateServicioEstado(servicioId, estadoId) {
  const { data } = await api.put(`/Servicios/${servicioId}/estado`, { idEstado: estadoId });
  return data;
}

export async function desasignarEmpleado(servicioId) {
  const { data } = await api.put(`/Servicios/${servicioId}/desasignar`);
  return data;
}

export async function mecanicoListAsignados(empleadoId) {
  const { data } = await api.get(`/Servicios/mecanico/${empleadoId}/asignados`);
  return data;
}

export async function mecanicoListCompletados(empleadoId) {
  const { data } = await api.get(`/Servicios/mecanico/${empleadoId}/completados`);
  return data;
}

export async function listEmpleados() {
  const { data } = await api.get('/Empleado');
  return data;
}

export async function descargarReservaPdf(servicio) {
  const doc = new jsPDF({ unit: 'pt', format: 'a4' });

  const primary = '#0ea5e9';
  const textMuted = '#6b7280';
  const pageWidth = doc.internal.pageSize.getWidth();

  // Determinar tipo legible aunque no venga del backend
  const tipoRaw = servicio?.tipoServicio || (servicio?.detallesRevision ? 'Revision' : 'Reparacion');
  const tipoPretty = tipoRaw === 'Revision' ? 'Revisión' : 'Reparación';

  // Header band
  doc.setFillColor(primary);
  doc.rect(0, 0, pageWidth, 80, 'F');
  doc.setTextColor('#ffffff');
  doc.setFontSize(20);
  doc.text('Comprobante de Reserva', 40, 50);

  // Card container
  const cardX = 40;
  const cardY = 110;
  const cardW = pageWidth - 80;
  const cardH = 360;
  doc.setDrawColor('#e5e7eb');
  doc.setFillColor('#ffffff');
  doc.roundedRect(cardX, cardY, cardW, cardH, 8, 8, 'FD');

  // Title
  doc.setTextColor('#111827');
  doc.setFontSize(16);
  doc.text(`Reserva #${servicio.id ?? ''}`, cardX + 16, cardY + 28);
  doc.setFontSize(11);
  doc.setTextColor(textMuted);
  doc.text(`Fecha: ${new Date().toLocaleString()}`, cardX + 16, cardY + 46);

  // Info grid
  const label = (x, y, t) => { doc.setTextColor(textMuted); doc.text(t, x, y); };
  const value = (x, y, t) => { doc.setTextColor('#111827'); doc.text(String(t ?? ''), x, y); };

  let y = cardY + 80;
  label(cardX + 16, y, 'Tipo de servicio');
  value(cardX + 180, y, tipoPretty);
  y += 20;
  label(cardX + 16, y, 'Estado');
  value(cardX + 180, y, servicio.estadoDescripcion ?? '');
  y += 20;
  label(cardX + 16, y, 'Cliente');
  value(cardX + 180, y, servicio.clienteNombre ?? '');

  // Divider
  y += 24;
  doc.setDrawColor('#e5e7eb');
  doc.line(cardX + 16, y, cardX + cardW - 16, y);
  y += 24;

  // Notes
  doc.setTextColor('#111827');
  doc.setFontSize(12);
  doc.text('Notas', cardX + 16, y);
  doc.setFontSize(11);
  doc.setTextColor(textMuted);
  y += 18;
  const notes = servicio.tipoServicio === 'Revision'
    ? (servicio.detallesRevision || 'Inspección general del vehículo. Sujeto a diagnóstico del mecánico.')
    : 'El detalle de repuestos se agregará una vez el mecánico evalúe y registre la reparación.';
  const split = doc.splitTextToSize(notes, cardW - 32);
  doc.text(split, cardX + 16, y);

  // Footer signature area
  const footY = cardY + cardH - 60;
  doc.setDrawColor('#e5e7eb');
  doc.line(cardX + 16, footY, cardX + 200, footY);
  doc.setTextColor(textMuted);
  doc.text('Firma cliente', cardX + 16, footY + 16);

  return doc.output('blob');
}
