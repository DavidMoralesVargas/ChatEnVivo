import axios from 'axios';
import Swal from 'sweetalert2';

// 1. Creamos la instancia de Axios
const apiClient = axios.create({
  baseURL: 'https://localhost:7274',
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json'
  }
});

// ==========================================
// INTERCEPTORES DE PETICIÓN (SALIDA)
// ==========================================
apiClient.interceptors.request.use(
    (config) => {
    // Obtenemos el token del localStorage
        const token = localStorage.getItem('Token');
    
        if (token) {
            // Inyectamos el token con el formato estándar Bearer
            config.headers['Authorization'] = `Bearer ${token}`;
        }
    
        return config;
    },
    (error) => {
        return Promise.reject(error);
    } 
);

// ==========================================
// INTERCEPTORES DE RESPUESTA (LLEGADA)
// ==========================================
apiClient.interceptors.response.use(
  (response) => {
    // Si la petición fue exitosa (Códigos 2xx), dejamos pasar la respuesta tal cual
    return response;
  },
  (error) => {
    // Si el backend devuelve un error, lo atrapamos aquí
    if (error.response) {
      const status = error.response.status;
      const data = error.response.data;
      
      // Intentamos extraer el mensaje del backend.
      // Dependiendo de cómo lo envíe tu API en C#, puede venir en 'data.message', 'data.title' o directamente en 'data'.
      const backendMessage = data.message || data.title || (typeof data === 'string' ? data : 'Los Antiguos han rechazado la petición.');

      // Evaluamos el código de estado
      if (status === 400) {
        // Bad Request
        Swal.fire({
          icon: 'warning',
          title: 'Petición Incorrecta',
          text: backendMessage,
          confirmButtonColor: '#8a0303', // Un rojo sangre acorde al diseño de tu header
          background: '#1c1c1e',
          color: '#d4d4d4'
        });
      } else if (status === 403) {
        // Forbidden
        Swal.fire({
          icon: 'error',
          title: 'Acceso Denegado',
          text: 'No tienes los privilegios necesarios. ¿Seguro que eres el Príncipe de esta ciudad?',
          confirmButtonColor: '#8a0303',
          background: '#1c1c1e',
          color: '#d4d4d4'
        });
      } else if (status === 401) {
        // Unauthorized (No logueado o token expirado)
        Swal.fire({
          icon: 'error',
          title: 'No Autorizado',
          text: 'Tu sesión ha expirado o no posees el Vínculo de Sangre necesario.',
          confirmButtonColor: '#8a0303',
          background: '#1c1c1e',
          color: '#d4d4d4'
        });
        // Aquí podrías agregar lógica para forzar el cierre de sesión:
        // localStorage.removeItem('Token');
        // window.location.href = '/login';
      } else {
        // Otros errores (500 Internal Server Error, 404 Not Found, etc.)
        Swal.fire({
          icon: 'error',
          title: `Error del Servidor (${status})`,
          text: backendMessage,
          confirmButtonColor: '#8a0303',
          background: '#1c1c1e',
          color: '#d4d4d4'
        });
      }
    } else {
      // Errores de red (el servidor está apagado o hay un problema de CORS)
      Swal.fire({
        icon: 'error',
        title: 'Error de Conexión',
        text: 'No se pudo contactar con el dominio. El servidor podría estar descansando de día.',
        confirmButtonColor: '#8a0303',
        background: '#1c1c1e',
        color: '#d4d4d4'
      });
    }

    // Rechazamos la promesa para que el componente también sepa que hubo un error 
    // y no siga ejecutando código de éxito.
    return Promise.reject(error);
  }
);

// ==========================================
// MÉTODOS HTTP EXPUESTOS
// ==========================================

export const api = {
    async get(endpoint) {
        const response = await apiClient.get(endpoint);
        return response.data;
    },

    async post(endpoint, data) {
        const response = await apiClient.post(endpoint, data);
        return response.data;
    },

    async put(endpoint, data) {
        const response = await apiClient.put(endpoint, data);
        return response.data;
    },

    async remove(endpoint) {
        const response = await apiClient.delete(endpoint);
        return response.data;
    }
};