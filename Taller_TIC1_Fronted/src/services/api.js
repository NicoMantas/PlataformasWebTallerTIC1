import axios from 'axios';

// Base API client
const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'https://localhost:7295/api',
  headers: {
    'Content-Type': 'application/json'
  }
});

// Attach token if present
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('authToken');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Basic error normalization
api.interceptors.response.use(
  (response) => response,
  (error) => {
    const message = error?.response?.data?.message || error.message || 'Error de red';
    return Promise.reject({
      status: error?.response?.status,
      data: error?.response?.data,
      message
    });
  }
);

export default api;

