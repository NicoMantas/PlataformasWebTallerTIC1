import api from './api';

export async function createCliente(clienteData) {
  console.log('📤 [clientesService] Enviando petición POST a /api/Cliente con datos:', clienteData);
  try {
    const { data } = await api.post('/Cliente', clienteData);
    console.log('📥 [clientesService] Respuesta exitosa del servidor:', data);
    return data;
  } catch (error) {
    console.error('❌ [clientesService] Error en la petición:', error);
    console.error('❌ [clientesService] Error response:', error.response);
    console.error('❌ [clientesService] Error status:', error.response?.status);
    console.error('❌ [clientesService] Error data:', error.response?.data);
    throw error;
  }
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
