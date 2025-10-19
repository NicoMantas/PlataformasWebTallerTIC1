import api from './api';

export async function login({ email, password }) {
  const { data } = await api.post('/Auth/login', {
    email,
    password
  });

  // Expected AuthResponseDto { success, message, user }
  if (!data?.success) {
    throw new Error(data?.message || 'Credenciales inválidas');
  }

  // If backend later adds token, store it; for now we store minimal session
  if (data?.token) {
    localStorage.setItem('authToken', data.token);
  }
  localStorage.setItem('user', JSON.stringify(data.user));

  return data;
}

export async function registerCliente({ email, password, idCliente, idTaller }) {
  const authData = {
    email,
    password,
    idTaller,
    idCliente
  };
  
  console.log('📤 [authService] Enviando petición POST a /api/Auth/register/cliente con datos:', authData);
  try {
    const { data } = await api.post('/Auth/register/cliente', authData);
    console.log('📥 [authService] Respuesta exitosa del servidor:', data);
    
    if (!data?.success) {
      console.error('❌ [authService] Error en la respuesta del servidor:', data?.message);
      throw new Error(data?.message || 'Error al registrar cliente');
    }
    return data;
  } catch (error) {
    console.error('❌ [authService] Error en la petición:', error);
    console.error('❌ [authService] Error response:', error.response);
    console.error('❌ [authService] Error status:', error.response?.status);
    console.error('❌ [authService] Error data:', error.response?.data);
    throw error;
  }
}

export async function registerEmpleado({ email, password, idEmpleado, idTaller }) {
  const { data } = await api.post('/Auth/register/empleado', {
    email,
    password,
    idTaller,
    idEmpleado
  });
  if (!data?.success) {
    throw new Error(data?.message || 'Error al registrar empleado');
  }
  return data;
}

export function logout() {
  localStorage.removeItem('authToken');
  localStorage.removeItem('user');
}

export function getCurrentUser() {
  const raw = localStorage.getItem('user');
  return raw ? JSON.parse(raw) : null;
}

