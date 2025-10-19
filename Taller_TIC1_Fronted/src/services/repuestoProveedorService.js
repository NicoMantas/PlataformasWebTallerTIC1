import api from './api';

export async function listRepuestoProveedores() {
  const { data } = await api.get('/RepuestoProveedor');
  return data;
}

export async function createRepuestoProveedor(repuestoProveedorData) {
  const { data } = await api.post('/RepuestoProveedor', repuestoProveedorData);
  return data;
}

export async function getRepuestoProveedorByIds(idRepuesto, idProveedor) {
  const { data } = await api.get(`/RepuestoProveedor/repuesto/${idRepuesto}/proveedor/${idProveedor}`);
  return data;
}

export async function deleteRepuestoProveedor(idRepuesto, idProveedor) {
  const { data } = await api.delete(`/RepuestoProveedor/repuesto/${idRepuesto}/proveedor/${idProveedor}`);
  return data;
}

export async function getRepuestosByProveedor(idProveedor) {
  const { data } = await api.get(`/RepuestoProveedor/proveedor/${idProveedor}`);
  return data;
}

export async function getProveedoresByRepuesto(idRepuesto) {
  const { data } = await api.get(`/RepuestoProveedor/repuesto/${idRepuesto}`);
  return data;
}
