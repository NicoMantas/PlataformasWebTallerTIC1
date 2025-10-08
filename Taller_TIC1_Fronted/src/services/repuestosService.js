import api from './api';

export async function listRepuestos() {
  const { data } = await api.get('/Repuestos');
  return data;
}

export async function getRepuestoById(id) {
  const { data } = await api.get(`/Repuestos/${id}`);
  return data;
}

export async function createRepuesto(repuestoData) {
  const { data } = await api.post('/Repuestos', repuestoData);
  return data;
}

export async function updateRepuesto(id, repuestoData) {
  const { data } = await api.put(`/Repuestos/${id}`, repuestoData);
  return data;
}

export async function deleteRepuesto(id) {
  const { data } = await api.delete(`/Repuestos/${id}`);
  return data;
}
