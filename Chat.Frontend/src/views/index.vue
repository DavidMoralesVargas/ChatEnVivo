<template>
  <div class="chat-layout">
    <ChatHeader />

    <div class="chat-container">
      <div class="messages-area">
        <div 
          v-for="(msg, index) in mensajes" 
          :key="index" 
          :class="['message-bubble', msg.esMio ? 'mine' : 'theirs']"
        >
          <span class="sender-name">
            {{ msg.esMio ? 'Tú' : (msg.usuario || 'Anónimo') }}
          </span>
          
          <p class="message-text">{{ msg.texto }}</p>
        </div>
      </div>

      <div class="input-area">
        <input 
          type="text" 
          v-model="nuevoMensaje" 
          @keyup.enter="enviarMensaje"
          placeholder="Escribe un mensaje..."
        />
        <button @click="enviarMensaje" class="send-btn">
          >
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue';
import axios from 'axios';
import * as signalR from '@microsoft/signalr';
import ChatHeader from '../components/ChatHeader.vue';

const mensajes = ref([]);
const nuevoMensaje = ref("");
let connection = null;

const API_BASE_URL = "https://localhost:7274"; 

// Aquí obtienes el correo o nombre del usuario logueado
const miUsuario = localStorage.getItem("Username") || "Anónimo";

onMounted(async () => {
  connection = new signalR.HubConnectionBuilder()
    .withUrl(`${API_BASE_URL}/chatHub`) 
    .withAutomaticReconnect()
    .build();

  // 2. Escuchar el evento de SignalR
  connection.on("ChatGlobal", (mensajeRecibido) => {
    // Mapeamos el objeto recibido de tu backend. 
    // Evaluamos tanto PascalCase como camelCase por seguridad.
    const texto = mensajeRecibido.mensaje || mensajeRecibido.Mensaje;
    const remitente = mensajeRecibido.usuario || mensajeRecibido.Usuario;

    mensajes.value.push({
      texto: texto, 
      usuario: remitente,
      esMio: false // Al venir de SignalR (AllExcept), garantizamos que es de otro usuario
    });
  });

  try {
    await connection.start();
    console.log("Conectado a SignalR con ID:", connection.connectionId);
  } catch (err) {
    console.error("Error al conectar a SignalR: ", err);
  }
});

onUnmounted(() => {
  if (connection) {
    connection.stop();
  }
});

const enviarMensaje = async () => {
  if (!nuevoMensaje.value.trim()) return;

  const textoAEnviar = nuevoMensaje.value;
  
  // 1. Añadimos nuestro propio mensaje inmediatamente a la lista local
  mensajes.value.push({
    texto: textoAEnviar,
    usuario: miUsuario,
    esMio: true
  });

  nuevoMensaje.value = "";

  const idConexionActual = connection ? connection.connectionId : null;

  // 2. Envío de datos por Axios hacia tu controlador
  try {
    await axios.post(`${API_BASE_URL}/api/chat/todos`, {
      Mensaje: textoAEnviar,
      Usuario: miUsuario,
      Grupo: "", 
      IdEmisor: idConexionActual 
    }, {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('Token')}`
      }
    });
  } catch (error) {
    console.error("Error enviando el mensaje: ", error);
  }
};
</script>

<style scoped>
.chat-layout {
  display: flex;
  flex-direction: column;
  height: 100vh;
  background-color: #ece5dd; 
}

.chat-container {
  display: flex;
  flex-direction: column;
  flex: 1;
  overflow: hidden;
  max-width: 900px;
  margin: 0 auto;
  width: 100%;
  background-color: #e5ddd5;
}

.messages-area {
  flex: 1;
  padding: 20px;
  overflow-y: auto;
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.message-bubble {
  max-width: 60%;
  padding: 10px 15px;
  border-radius: 10px;
  font-family: sans-serif;
  box-shadow: 0 1px 1px rgba(0,0,0,0.1);
  word-wrap: break-word;
  color: #111111; /* <-- AQUÍ ESTÁ EL CAMBIO: Texto oscuro para que se lea bien */
}

/* A la derecha y color verde (Whatsapp) */
.message-bubble.mine {
  align-self: flex-end;
  background-color: #dcf8c6;
  border-bottom-right-radius: 0;
}

/* A la izquierda y color blanco */
.message-bubble.theirs {
  align-self: flex-start;
  background-color: #ffffff;
  border-bottom-left-radius: 0;
}

/* Estilo para el nombre del usuario que envía (opcional) */
.sender-name {
  display: block;
  font-size: 0.75rem;
  color: #128c7e;
  font-weight: bold;
  margin-bottom: 3px;
}

.input-area {
  display: flex;
  padding: 10px;
  background-color: #f0f0f0;
  gap: 10px;
}

.input-area input {
  flex: 1;
  padding: 15px;
  border-radius: 20px;
  border: none;
  outline: none;
  font-size: 1rem;
  color: #333; /* Texto del input oscuro también */
}

.send-btn {
  background-color: #128c7e;
  color: white;
  border: none;
  border-radius: 50%;
  width: 50px;
  height: 50px;
  font-size: 1.5rem;
  cursor: pointer;
  display: flex;
  justify-content: center;
  align-items: center;
  transition: background 0.3s;
}

.send-btn:hover {
  background-color: #075e54;
}
</style> 