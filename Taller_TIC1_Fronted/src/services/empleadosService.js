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

export async function desactivarEmpleado(id, detallesDesactivacion, fechaDesactivacion = null, fechaActivacion = null, adminId = null) {
  const url = adminId 
    ? `/Empleado/${id}/desactivar?adminId=${adminId}`
    : `/Empleado/${id}/desactivar`;
  
  const payload = {
    detallesDesactivacion
  };
  
  if (fechaDesactivacion) {
    payload.fechaDesactivacion = fechaDesactivacion;
  }
  
  if (fechaActivacion) {
    payload.fechaActivacion = fechaActivacion;
  }
  
  const { data } = await api.post(url, payload);
  return data;
}

export async function activarEmpleado(id, fechaActivacion = null, adminId = null) {
  const url = adminId 
    ? `/Empleado/${id}/activate?adminId=${adminId}`
    : `/Empleado/${id}/activate`;
  
  const payload = {};
  
  if (fechaActivacion) {
    payload.fechaActivacion = fechaActivacion;
  }
  
  const { data } = await api.patch(url, payload);
  return data;
}


