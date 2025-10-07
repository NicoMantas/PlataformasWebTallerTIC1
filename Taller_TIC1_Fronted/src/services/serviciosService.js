import api from './api';
import { jsPDF } from 'jspdf';

export async function listServicios() {
  const { data } = await api.get('/Servicios');
  return data;
}

export async function createServicio({ tipoServicio, idCliente, idVehiculo, detallesRevision, repuestosReparacion }) {
  const payload = {
    costo: 0,
    idCliente,
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

export async function cancelarServicio(servicioId) {
  const { data } = await api.post(`/Servicios/${servicioId}/cancelar`);
  return data;
}

export async function crearOrden(payload) {
  const { data } = await api.post('/OrdenesTrabajo', payload);
  return data;
}

export async function descargarReservaPdf(servicio) {
  const doc = new jsPDF();
  const line = (y, text) => doc.text(String(text ?? ''), 14, y);
  doc.setFontSize(16);
  line(20, 'Comprobante de Reserva');
  doc.setFontSize(12);
  line(30, `ID Servicio: ${servicio.id ?? ''}`);
  line(38, `Tipo: ${servicio.tipoServicio ?? ''}`);
  line(46, `Estado: ${servicio.estadoDescripcion ?? ''}`);
  line(54, `Cliente: ${servicio.clienteNombre ?? ''}`);
  line(62, `Fecha: ${new Date().toLocaleString()}`);
  return doc.output('blob');
}
