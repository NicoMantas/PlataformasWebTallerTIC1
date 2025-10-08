import api from './api';

export async function listOrdenes() {
  const { data } = await api.get('/OrdenesTrabajo');
  return data;
}

export async function getOrdenById(id) {
  const { data } = await api.get(`/OrdenesTrabajo/${id}`);
  return data;
}

export async function createOrden(ordenData) {
  const { data } = await api.post('/OrdenesTrabajo', {
    fechaCreacion: new Date().toISOString(),
    idTipoEstadoOrden: 1, // Pendiente
    idCliente: ordenData.clienteId,
    idVehiculo: ordenData.vehiculoId,
    serviciosIds: ordenData.serviciosIds,
    descripcion: ordenData.descripcion || 'Solicitud de servicio'
  });
  return data;
}

export async function updateOrden(id, ordenData) {
  const { data } = await api.put(`/OrdenesTrabajo/${id}`, ordenData);
  return data;
}

export async function deleteOrden(id) {
  await api.delete(`/OrdenesTrabajo/${id}`);
  return true;
}

export async function addServicioToOrden(ordenId, servicioId) {
  await api.post(`/OrdenesTrabajo/${ordenId}/servicios/${servicioId}`);
  return true;
}

export async function removeServicioFromOrden(ordenId, servicioId) {
  await api.delete(`/OrdenesTrabajo/${ordenId}/servicios/${servicioId}`);
  return true;
}
