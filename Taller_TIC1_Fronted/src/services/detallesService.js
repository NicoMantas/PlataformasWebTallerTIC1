import api from './api';

// Detalle Revision services
export async function getDetalleRevision(servicioId) {
  const { data } = await api.get(`/DetalleRevision/${servicioId}`);
  return data;
}

export async function createDetalleRevision(detalleData) {
  const { data } = await api.post('/DetalleRevision', detalleData);
  return data;
}

export async function updateDetalleRevision(servicioId, detalleData) {
  const { data } = await api.put(`/DetalleRevision/${servicioId}`, detalleData);
  return data;
}

export async function deleteDetalleRevision(servicioId) {
  const { data } = await api.delete(`/DetalleRevision/${servicioId}`);
  return data;
}

// Detalle Reparacion services
export async function getDetalleReparacion(servicioId) {
  const { data } = await api.get(`/DetalleReparacion/${servicioId}`);
  return data;
}

export async function createDetalleReparacion(detalleData) {
  const { data } = await api.post('/DetalleReparacion', detalleData);
  return data;
}

export async function updateDetalleReparacion(servicioId, detalleData) {
  const { data } = await api.put(`/DetalleReparacion/${servicioId}`, detalleData);
  return data;
}

export async function deleteDetalleReparacion(servicioId) {
  const { data } = await api.delete(`/DetalleReparacion/${servicioId}`);
  return data;
}

// Detalle Reparacion Repuesto services
export async function getRepuestosByDetalleReparacion(servicioId) {
  const { data } = await api.get(`/DetalleReparacion/${servicioId}/repuestos`);
  return data;
}

export async function addRepuestoToReparacion(servicioId, repuestoData) {
  const { data } = await api.post(`/DetalleReparacion/${servicioId}/repuestos`, repuestoData);
  return data;
}

export async function updateRepuestoInReparacion(servicioId, repuestoId, repuestoData) {
  const { data } = await api.put(`/DetalleReparacion/${servicioId}/repuestos/${repuestoId}`, repuestoData);
  return data;
}

export async function removeRepuestoFromReparacion(servicioId, repuestoId) {
  const { data } = await api.delete(`/DetalleReparacion/${servicioId}/repuestos/${repuestoId}`);
  return data;
}
