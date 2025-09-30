import api from './api';

export async function createCliente(clienteData) {
  const { data } = await api.post('/Cliente', clienteData);
  return data;
}

export async function getClienteById(id) {
  const { data } = await api.get(`/Cliente/${id}`);
  return data;
}

export async function updateCliente(id, clienteData) {
  const { data } = await api.put(`/Cliente/${id}`, clienteData);
  return data;
}

export async function deleteCliente(id) {
  const { data } = await api.delete(`/Cliente/${id}`);
  return data;
}

export async function getAllClientes() {
  const { data } = await api.get('/Cliente');
  return data;
}
