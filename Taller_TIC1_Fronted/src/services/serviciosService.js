export { }

export async function listServicios(api) {
  const { data } = await api.get('/Servicios');
  return data;
}

export async function crearOrden(api, payload) {
  // payload should match OrdenTrabajoCreateDTO
  const { data } = await api.post('/OrdenesTrabajo', payload);
  return data;
}
