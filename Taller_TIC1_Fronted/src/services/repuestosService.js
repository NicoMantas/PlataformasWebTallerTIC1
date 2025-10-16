import api from './api';

export async function listRepuestos() {
  const { data } = await api.get('/Repuesto');
  return data;
}

export async function getRepuestoById(id) {
  const { data } = await api.get(`/Repuesto/${id}`);
  return data;
}

export async function createRepuesto(repuestoData) {
  const { data } = await api.post('/Repuesto', repuestoData);
  return data;
}

export async function updateRepuesto(id, repuestoData) {
  const { data } = await api.put(`/Repuesto/${id}`, repuestoData);
  return data;
}

export async function deleteRepuesto(id) {
  const { data } = await api.delete(`/Repuesto/${id}`);
  return data;
}

export async function searchRepuestosByName(name) {
  const { data } = await api.get(`/Repuesto/search?name=${encodeURIComponent(name)}`);
  return data;
}

export async function getRepuestosByStock(minStock) {
  const { data } = await api.get(`/Repuesto/stock/${minStock}`);
  return data;
}

export async function getRepuestoByNumeroSerie(numeroSerie) {
  const { data } = await api.get(`/Repuesto/serie/${numeroSerie}`);
  return data;
}
