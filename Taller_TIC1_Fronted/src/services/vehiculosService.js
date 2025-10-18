import api from './api';

export async function listVehiculos() {
  const { data } = await api.get('/Vehiculo');
  return data;
}

export async function createVehiculo(vehiculoData) {
  console.log('📤 [vehiculosService] Enviando petición POST a /api/Vehiculo con datos:', vehiculoData);
  try {
    const { data } = await api.post('/Vehiculo', vehiculoData);
    console.log('📥 [vehiculosService] Respuesta exitosa del servidor:', data);
    return data;
  } catch (error) {
    console.error('❌ [vehiculosService] Error en la petición:', error);
    console.error('❌ [vehiculosService] Error response:', error.response);
    console.error('❌ [vehiculosService] Error status:', error.response?.status);
    console.error('❌ [vehiculosService] Error data:', error.response?.data);
    throw error;
  }
}

export async function updateVehiculo(id, vehiculoData) {
  const { data } = await api.put(`/Vehiculo/${id}`, vehiculoData);
  return data;
}

export async function deleteVehiculo(id) {
  const { data } = await api.delete(`/Vehiculo/${id}`);
  return data;
}

export async function getVehiculoById(id) {
  const { data } = await api.get(`/Vehiculo/${id}`);
  return data;
}
