import api from './api';

export async function listFacturas() {
  const { data } = await api.get('/Factura');
  return data;
}

export async function getFacturaById(id) {
  const { data } = await api.get(`/Factura/${id}`);
  return data;
}

export async function getFacturaByOrden(ordenId) {
  const { data } = await api.get(`/Factura/orden/${ordenId}`);
  return data;
}

export async function createFactura(facturaData) {
  const { data } = await api.post('/Factura', facturaData);
  return data;
}

export async function updateFactura(id, facturaData) {
  const { data } = await api.put(`/Factura/${id}`, facturaData);
  return data;
}

export async function deleteFactura(id) {
  await api.delete(`/Factura/${id}`);
  return true;
}
