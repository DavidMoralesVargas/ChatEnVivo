<template>
  <ChatHeader/>
  <div class="chat-container">
    <h2>Lista de Chats Grupales</h2>
    
    <button @click="crearChat" class="btn-create">Crear Nuevo Chat</button>

    <div v-if="chats.length > 0" class="chat-list">
      <ul>
        <li v-for="chat in chats" :key="chat.codigoChat">
          <div class="chat-item-info">
            <strong>Código:</strong> {{ chat.codigoChat }} | 
            <strong>Fecha de Creación:</strong> {{ formatearFecha(chat.fechaCreacion) }}
          </div>
          <button @click="abrirChat(chat.codigoChat)" class="btn-open">Abrir Chat</button>
        </li>
      </ul>
    </div>
    <div v-else>
      <p>Cargando chats o no hay chats disponibles...</p>
    </div>

    <div v-if="chatActivo" class="modal-overlay" @click.self="cerrarChat">
      <div class="modal-content">
        <div class="modal-header">
          <h3>Chat Grupo: {{ chatActivo }}</h3>
          <button @click="cerrarChat" class="btn-close">X</button>
        </div>
        
        <div class="modal-body" ref="chatBody">
          <div v-if="mensajes.length === 0" class="no-messages">
            No hay mensajes aún. ¡Escribe el primero!
          </div>
          
          <div v-for="(msg, index) in mensajes" :key="index" 
               :class="['mensaje-wrapper', esMiMensaje(msg.Usuario) ? 'mio' : 'otro']">
            <span class="mensaje-autor">{{ esMiMensaje(msg.Usuario) ? 'Tú' : msg.Usuario }}</span>
            <div class="mensaje-burbuja">
              {{ msg.Mensaje }}
            </div>
          </div>
        </div>

        <div class="modal-footer">
          <input 
            v-model="nuevoMensaje" 
            @keyup.enter="enviarMensaje"
            type="text" 
            placeholder="Escribe un mensaje..." 
            class="input-mensaje"
          />
          <button @click="enviarMensaje" class="btn-send">Enviar</button>
        </div>
      </div>
    </div>

  </div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount, nextTick } from 'vue';
import axios from 'axios';
import * as signalR from '@microsoft/signalr';
import Swal from 'sweetalert2';
import ChatHeader from '../../components/ChatHeader.vue';

// --- CONSTANTES ---
const BASE_URL = 'https://localhost:7274'; 
const API_URL = `${BASE_URL}/api/chat`;
const HUB_URL = `${BASE_URL}/chathub`;

// --- ESTADO REACTIVO ---
const chats = ref([]);
let connection = null;

// Estado del Modal y Mensajes
const chatActivo = ref(null);
const mensajes = ref([]);
const nuevoMensaje = ref("");
const miUsuarioId = ref("");
const chatBody = ref(null);

// --- UTILIDAD: Obtener mi ID del JWT ---
// Extraemos el NameIdentifier (o equivalente) del token para saber si el mensaje es nuestro
const decodificarToken = () => {
  const token = localStorage.getItem('Token');
  if (!token) return;
  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    // Normalmente ClaimTypes.NameIdentifier en .NET se mapea a esta URL larga o a "nameid" / "sub"
    miUsuarioId.value = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || payload.nameid || payload.sub;
  } catch (error) {
    console.error("Error al decodificar el token:", error);
  }
};

// --- MÉTODOS ---

const fetchChats = async () => {
  try {
    const response = await axios.get(API_URL);
    chats.value = response.data;
  } catch (error) {
    console.error("Error al obtener la lista de chats:", error);
    Swal.fire('Error', 'No se pudo cargar la lista de chats.', 'error');
  }
};

const initSignalR = async () => {
  connection = new signalR.HubConnectionBuilder()
    .withUrl(HUB_URL)
    .withAutomaticReconnect()
    .build();

  connection.on("CreacionChat", () => {
    fetchChats();
  });

  try {
    await connection.start();
    console.log("Conectado exitosamente a SignalR");
  } catch (error) {
    console.error("Error al conectar con SignalR:", error);
  }
};

const crearChat = async () => {
  const token = localStorage.getItem('Token');
  if (!token) return Swal.fire('No autorizado', 'No se encontró un token.', 'warning');

  try {
    await axios.post(`${API_URL}/crearCodigo`, {}, {
      headers: { 'Authorization': `Bearer ${token}` }
    });
    Swal.fire({ icon: 'success', title: '¡Éxito!', text: 'Código creado.', timer: 1500, showConfirmButton: false });
  } catch (error) {
    Swal.fire('Error', 'Hubo un problema al crear el chat.', 'error');
  }
};

// --- LÓGICA DEL MODAL ---

const abrirChat = async (codigoGrupo) => {
  chatActivo.value = codigoGrupo;
  mensajes.value = []; 

  // 1. Escuchar el evento específico y agregar logs de depuración
  connection.on("ChatGrupal", (data) => {

    // Normalizamos las propiedades por si el backend las envió en minúsculas
    const mensajeProcesado = {
      Mensaje: data.Mensaje || data.mensaje,
      Usuario: data.Usuario || data.usuario
    };

    mensajes.value.push(mensajeProcesado);
    hacerScrollAbajo();
  });

  // 2. Unirse al grupo
  if (connection.state === signalR.HubConnectionState.Connected) {
    try {
      console.log(`Intentando unirse al grupo/partida: ${codigoGrupo}`);
      await connection.invoke("UnirsePartida", String(codigoGrupo));
    } catch (err) {
      console.warn("No se pudo invocar UnirsePartida en el Hub:", err);
    }
  }
};

const cerrarChat = async () => {
  if (!chatActivo.value) return;

  // 1. Dejar de escuchar el evento para el modal actual
  connection.off("ChatGrupal");

  // 2. Notificar al backend que salimos del grupo (¡AQUÍ ESTÁ LA CORRECCIÓN!)
  if (connection && connection.state === signalR.HubConnectionState.Connected) {
    try {
      console.log(`Saliendo del grupo/partida: ${chatActivo.value}`);
      // Forzamos el String() para que el backend C# lo acepte
      await connection.invoke("SalirPartida", String(chatActivo.value));
    } catch (err) {
      console.warn("No se pudo invocar SalirPartida en el Hub:", err);
    }
  }

  // 3. Limpiar el estado
  chatActivo.value = null;
  mensajes.value = [];
};

const enviarMensaje = async () => {
  if (!nuevoMensaje.value.trim() || !chatActivo.value) return;

  const token = localStorage.getItem('Token');
  const payload = {
    Mensaje: nuevoMensaje.value,
    Grupo: String(chatActivo.value)
    // Si el error indica que faltan Usuario o IdEmisor, los agregaremos aquí luego
  };

  const mensajeEnviado = nuevoMensaje.value; // Guardamos copia por si falla
  nuevoMensaje.value = ""; // Limpiamos el input de inmediato para mejor UX

  try {
    await axios.post(`${API_URL}/grupo`, payload, {
      headers: { 'Authorization': `Bearer ${token}` }
    });
    // Si llega aquí, el mensaje se envió con éxito.
    // El SignalR se encargará de pintarlo en pantalla.

  } catch (error) {
    console.error("Error al enviar mensaje:", error);
    nuevoMensaje.value = mensajeEnviado; // Restauramos el texto en el input
    
    // --- MEJOR MANEJO DE ERRORES ---
    let tituloError = 'Error al enviar';
    let detalleError = 'No se pudo enviar el mensaje por un problema desconocido.';

    if (error.response) {
      // El servidor respondió con un código de estado fuera del rango 2xx
      const status = error.response.status;
      const data = error.response.data;

      if (status === 400) {
        tituloError = 'Solicitud Inválida (400)';
        
        // ASP.NET Core suele enviar los errores de validación en la propiedad "errors"
        if (data.errors) {
          // Extraemos todos los mensajes de error y los unimos en un solo texto
          const mensajesValidacion = Object.values(data.errors).flat().join('\n');
          detalleError = `El servidor rechazó los datos:\n${mensajesValidacion}`;
        } 
        // Si no hay "errors", a veces envía el mensaje en "title" o directamente como texto
        else if (data.title) {
          detalleError = data.title;
        } else if (typeof data === 'string') {
          detalleError = data;
        }
      } else if (status === 401) {
        tituloError = 'No Autorizado (401)';
        detalleError = 'Tu sesión ha expirado o el token es inválido.';
      } else {
        tituloError = `Error del Servidor (${status})`;
        detalleError = data.title || 'Hubo un problema procesando la solicitud.';
      }
    } else if (error.request) {
      // La petición se hizo pero no hubo respuesta del servidor (ej. backend apagado)
      tituloError = 'Sin conexión';
      detalleError = 'No se pudo conectar con el servidor. Verifica si está encendido.';
    }

    // Mostramos el error exacto al usuario
    Swal.fire({
      icon: 'error',
      title: tituloError,
      text: detalleError,
    });
  }
};

const esMiMensaje = (usuarioDelMensaje) => {
  return usuarioDelMensaje === miUsuarioId.value;
};

const hacerScrollAbajo = async () => {
  await nextTick();
  if (chatBody.value) {
    chatBody.value.scrollTop = chatBody.value.scrollHeight;
  }
};

const formatearFecha = (fecha) => {
  if (!fecha) return '';
  return new Date(fecha).toLocaleString();
};

// --- CICLO DE VIDA ---
onMounted(() => {
  decodificarToken();
  fetchChats();
  initSignalR();
});

onBeforeUnmount(() => {
  if (connection) {
    connection.off("ChatGrupal");
    connection.stop();
  }
});
</script>

<style scoped>
/* --- Estilos Anteriores --- */
.chat-container {
  max-width: 600px;
  margin: 0 auto;
  font-family: Arial, sans-serif;
}
.btn-create {
  background-color: #42b983;
  color: white;
  padding: 10px 15px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  margin-bottom: 15px;
}
ul { list-style-type: none; padding: 0; }
li {
  background: #f9f9f9; margin-bottom: 10px; padding: 15px;
  border-radius: 5px; border-left: 5px solid #42b983; color: black;
  display: flex; justify-content: space-between; align-items: center;
}
.btn-open {
  background-color: #2b8a3e; color: white; border: none;
  padding: 8px 12px; border-radius: 4px; cursor: pointer;
}

/* --- ESTILOS DEL MODAL (WHATSAPP STYLE) --- */
.modal-overlay {
  position: fixed; top: 0; left: 0; width: 100vw; height: 100vh;
  background: rgba(0,0,0,0.6); display: flex;
  justify-content: center; align-items: center; z-index: 1000;
}
.modal-content {
  background: #e5ddd5; /* Fondo clásico de WhatsApp */
  width: 90%; max-width: 500px; height: 80vh;
  border-radius: 10px; display: flex; flex-direction: column;
  overflow: hidden; box-shadow: 0 4px 10px rgba(0,0,0,0.3);
}
.modal-header {
  background: #075e54; color: white; padding: 15px;
  display: flex; justify-content: space-between; align-items: center;
}
.modal-header h3 { margin: 0; font-size: 1.1rem; }
.btn-close {
  background: transparent; border: none; color: white;
  font-size: 1.2rem; cursor: pointer; font-weight: bold;
}
.modal-body {
  flex: 1; padding: 15px; overflow-y: auto;
  display: flex; flex-direction: column; gap: 10px;
}
.no-messages {
  text-align: center; color: #555; font-size: 0.9rem;
  background: rgba(255,255,255,0.5); padding: 10px;
  border-radius: 10px; align-self: center; margin-top: auto; margin-bottom: auto;
}

/* --- BURBUJAS DE CHAT --- */
.mensaje-wrapper {
  display: flex; flex-direction: column; max-width: 75%;
}
.mensaje-wrapper.mio {
  align-self: flex-end; /* Alineado a la derecha */
}
.mensaje-wrapper.otro {
  align-self: flex-start; /* Alineado a la izquierda */
}
.mensaje-autor {
  font-size: 0.75rem; color: #666; margin-bottom: 2px;
  padding: 0 5px;
}
.mensaje-wrapper.mio .mensaje-autor {
  text-align: right; font-weight: bold; color: #075e54;
}
.mensaje-wrapper.otro .mensaje-autor {
  text-align: left; font-weight: bold; color: #128C7E;
}
.mensaje-burbuja {
  padding: 10px 15px; border-radius: 15px;
  font-size: 0.95rem; word-wrap: break-word; color: black;
}
.mensaje-wrapper.mio .mensaje-burbuja {
  background-color: #dcf8c6;
  border-bottom-right-radius: 0;
}
.mensaje-wrapper.otro .mensaje-burbuja {
  background-color: #ffffff;
  border-bottom-left-radius: 0;
}

/* --- FOOTER / INPUT --- */
.modal-footer {
  background: #f0f0f0; padding: 10px; display: flex; gap: 10px;
}
.input-mensaje {
  flex: 1; padding: 12px; border: none; border-radius: 20px;
  outline: none; font-size: 0.95rem;
}
.btn-send {
  background: #128C7E; color: white; border: none;
  padding: 0 20px; border-radius: 20px; cursor: pointer;
  font-weight: bold;
}
</style>