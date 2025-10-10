import api from './api';

export async function listEmpleados() {
  const { data } = await api.get('/Empleado');
  return data;
}

export async function createEmpleado({ nombre, apellido, cedula, salario, idTipoEmpleado }) {
  const { data } = await api.post('/Empleado', {
    nombre,
    apellido,
    cedula,
    salario,
    idTipoEmpleado
  });
  return data; // EmpleadoResponseDto
}

export async function updateEmpleado(id, payload) {
  const { data } = await api.put(`/Empleado/${id}`, payload);
  return data;
}

export async function deleteEmpleado(id) {
  await api.delete(`/Empleado/${id}`);
  return true;
}

export async function desactivarEmpleado(id, detallesDesactivacion, adminId = null) {
  const url = adminId 
    ? `/Empleado/${id}/desactivar?adminId=${adminId}`
    : `/Empleado/${id}/desactivar`;
    
  const { data } = await api.post(url, {
    detallesDesactivacion
  });
  return data;
}

export async function activarEmpleado(id, adminId = null) {
  const url = adminId 
    ? `/Empleado/${id}/activate?adminId=${adminId}`
    : `/Empleado/${id}/activate`;
    
  const { data } = await api.patch(url);
  return data;
}


