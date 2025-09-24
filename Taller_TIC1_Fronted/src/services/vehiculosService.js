import api from './api';

export async function listVehiculos() {
  const { data } = await api.get('/Vehiculos');
  return data;
}

export async function createVehiculo(vehiculoData) {
  const { data } = await api.post('/Vehiculos', vehiculoData);
  return data;
}

export async function updateVehiculo(id, vehiculoData) {
  const { data } = await api.put(`/Vehiculos/${id}`, vehiculoData);
  return data;
}

export async function deleteVehiculo(id) {
  const { data } = await api.delete(`/Vehiculos/${id}`);
  return data;
}

export async function getVehiculoById(id) {
  const { data } = await api.get(`/Vehiculos/${id}`);
  return data;
}
