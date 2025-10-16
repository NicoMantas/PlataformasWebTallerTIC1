import api from './api';

export async function listProveedores() {
  const { data } = await api.get('/Proveedor');
  return data;
}

export async function getProveedorById(id) {
  const { data } = await api.get(`/Proveedor/${id}`);
  return data;
}

export async function createProveedor(proveedorData) {
  const { data } = await api.post('/Proveedor', proveedorData);
  return data;
}

export async function updateProveedor(id, proveedorData) {
  const { data } = await api.put(`/Proveedor/${id}`, proveedorData);
  return data;
}

export async function deleteProveedor(id) {
  const { data } = await api.delete(`/Proveedor/${id}`);
  return data;
}

export async function searchProveedoresByName(name) {
  const { data } = await api.get(`/Proveedor/search?name=${encodeURIComponent(name)}`);
  return data;
}
