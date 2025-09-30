import api from './api';

export async function listVehiculos() {
  const { data } = await api.get('/Vehiculo');
  return data;
}

export async function createVehiculo(vehiculoData) {
  const { data } = await api.post('/Vehiculo', vehiculoData);
  return data;
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
