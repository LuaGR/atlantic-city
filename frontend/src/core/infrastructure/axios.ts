import axios from 'axios';

// Instancia configurada apuntando a la url del backend
export const api = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5097',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Interceptor para inyectar token en cada petición automáticamente
api.interceptors.request.use((config) => {
  if (typeof window !== 'undefined') {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
  }
  return config;
}, (error) => {
  return Promise.reject(error);
});

// Interceptor global para errores (como 401 Unauthorized)
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response && error.response.status === 401) {
      // Opcional: Desloguear al usuario en caso de token expirado
      if (typeof window !== 'undefined') {
        localStorage.removeItem('token');
        // window.location.href = '/'; 
      }
    }
    return Promise.reject(error);
  }
);
