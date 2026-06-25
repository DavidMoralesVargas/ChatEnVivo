<template>
    <ChatHeader/>
  <div class="contenedor-principal">
    <h2>Usuarios Disponibles</h2>
    
    <div class="lista-usuarios">
      <div v-for="usuario in usuarios" :key="usuario.id" class="tarjeta-usuario">
        <div class="info-usuario">
          <strong>{{ usuario.nombre_Usuario }}</strong>
          <span>{{ usuario.email }}</span>
        </div>
        <button class="btn-chatear" @click="abrirChat(usuario)">Chatear</button>
      </div>
    </div>

    <div v-if="modalAbierto" class="modal-overlay">
      <div class="modal-chat">
        
        <div class="chat-header">
          <h3>Chat con {{ usuarioDestino.nombre_Usuario }}</h3>
          <button class="btn-cerrar" @click="cerrarChat">X</button>
        </div>

        <div class="chat-mensajes" ref="contenedorMensajes">
          <div 
            v-for="(msg, index) in mensajes" 
            :key="index"
            :class="['mensaje-burbuja', msg.emisor === miNombreUsuario ? 'mi-mensaje' : 'su-mensaje']"
          >
            <span class="emisor-tag" v-if="msg.emisor !== miNombreUsuario">{{ msg.emisor }}</span>
            <p>{{ msg.mensaje }}</p>
          </div>
        </div>

        <div class="chat-input">
          <input 
            type="text" 
            v-model="nuevoMensaje" 
            @keyup.enter="enviarMensaje" 
            placeholder="Escribe un mensaje..."
          />
          <button class="btn-enviar" @click="enviarMensaje">Enviar</button>
        </div>

      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted, nextTick } from 'vue';
import axios from 'axios';
import Swal from 'sweetalert2';
import * as signalR from '@microsoft/signalr';
import ChatHeader from '../../components/ChatHeader.vue';

// --- ESTADO ---
const usuarios = ref([]);
const modalAbierto = ref(false);
const usuarioDestino = ref(null);
const mensajes = ref([]);
const nuevoMensaje = ref("");
const contenedorMensajes = ref(null);
let connection = null;
const url_api = "https://localhost:7274"

// IMPORTANTE: Debes reemplazar esto con la lógica para obtener tu nombre de usuario real (ej. desde el JWT)
const miNombreUsuario = ref("MiUsuarioActual"); 

// --- CICLO DE VIDA ---
onMounted(async () => {
  await obtenerUsuarios();
  await iniciarSignalR();
});

onUnmounted(() => {
  if (connection) {
    connection.stop();
  }
});

// --- MÉTODOS ---

// 1. Obtener lista de usuarios
const obtenerUsuarios = async () => {
  try {
    const token = localStorage.getItem("Token");
    const respuesta = await axios.get(`${url_api}/api/usuarios`, {
      headers: { Authorization: `Bearer ${token}` }
    });
    // Filtramos para no mostrarnos a nosotros mismos en la lista
    usuarios.value = respuesta.data.filter(u => u.nombre_Usuario !== miNombreUsuario.value);
  } catch (error) {
    Swal.fire('Error', 'No se pudieron cargar los usuarios', 'error');
  }
};

// 2. Configurar SignalR
const iniciarSignalR = async () => {
  const token = localStorage.getItem("Token");
  
  // Reemplaza "/chatHub" con la URL real de tu Hub de SignalR en el backend
  connection = new signalR.HubConnectionBuilder()
    .withUrl(`${url_api}/chathub`, { 
      accessTokenFactory: () => token 
    })
    .withAutomaticReconnect()
    .build();

  // Escuchar el evento que configuraste en tu Hub/Caso de uso
  // Asumo que el backend envía (mensaje, nombreEmisor)
  connection.on("ChatIndivudual", (mensaje) => {
  // Si tenemos el modal abierto, asumimos ciegamente que el mensaje es de esa persona
  if (modalAbierto.value && usuarioDestino.value) {
    mensajes.value.push({ 
      mensaje: mensaje, 
      emisor: usuarioDestino.value.nombre_Usuario // Le asignamos el nombre localmente
    });
    hacerScrollAbajo();
  }
});

  try {
    await connection.start();
  } catch (error) {
    console.error("Error al conectar SignalR:", error);
  }
};

// 3. Lógica del Chat UI
const abrirChat = (usuario) => {
  usuarioDestino.value = usuario;
  mensajes.value = []; // Aquí podrías cargar el historial si tuvieras un endpoint para ello
  modalAbierto.value = true;
};

const cerrarChat = () => {
  modalAbierto.value = false;
  usuarioDestino.value = null;
};

const hacerScrollAbajo = async () => {
  await nextTick();
  if (contenedorMensajes.value) {
    contenedorMensajes.value.scrollTop = contenedorMensajes.value.scrollHeight;
  }
};

// 4. Enviar Mensaje vía Axios
const enviarMensaje = async () => {
  if (!nuevoMensaje.value.trim()) return;

  const payload = {
    Mensaje: nuevoMensaje.value,
    Usuario: usuarioDestino.value.nombre_Usuario, // El nombre de usuario que espera tu DTO
    Grupo: "",
    IdEmisor: miNombreUsuario.value 
  };

  try {
    const token = localStorage.getItem("Token");
    await axios.post(`${url_api}/api/chat/usuario`, payload, {
      headers: { Authorization: `Bearer ${token}` }
    });

    // Agregamos nuestro propio mensaje a la vista (si tu backend no hace "echo" de vuelta a ti mismo)
    mensajes.value.push({ mensaje: nuevoMensaje.value, emisor: miNombreUsuario.value });
    nuevoMensaje.value = "";
    hacerScrollAbajo();

  } catch (error) {
    Swal.fire('Error', 'No se pudo enviar el mensaje', 'error');
  }
};
</script>

<style scoped>
/* Estilos Generales y de la Lista */
.contenedor-principal {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}

.lista-usuarios {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.tarjeta-usuario {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 15px;
  background-color: #f9f9f9;
  border-radius: 8px;
  box-shadow: 0 1px 3px rgba(0,0,0,0.1);
}

.info-usuario {
  display: flex;
  flex-direction: column;
  color: black; /* <-- Agregamos esta línea para forzar el color negro */
}

.btn-chatear {
  background-color: #25D366; /* Verde WhatsApp */
  color: white;
  border: none;
  padding: 8px 15px;
  border-radius: 5px;
  cursor: pointer;
  font-weight: bold;
}

.btn-chatear:hover { background-color: #128C7E; }

/* Modal y Chat (Estilo WhatsApp) */
.modal-overlay {
  position: fixed;
  top: 0; left: 0; right: 0; bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-chat {
  background-color: #efeae2; /* Fondo característico de WhatsApp */
  width: 100%;
  max-width: 450px;
  height: 600px;
  border-radius: 10px;
  display: flex;
  flex-direction: column;
  overflow: hidden;
  box-shadow: 0 5px 15px rgba(0,0,0,0.3);
}

.chat-header {
  background-color: #075e54;
  color: white;
  padding: 15px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.chat-header h3 { margin: 0; }

.btn-cerrar {
  background: transparent;
  border: none;
  color: white;
  font-size: 1.2rem;
  cursor: pointer;
}

.chat-mensajes {
  flex: 1;
  padding: 20px;
  overflow-y: auto;
  display: flex;
  flex-direction: column;
  gap: 10px;
}

/* Burbujas de chat */
.mensaje-burbuja {
  max-width: 75%;
  padding: 10px;
  border-radius: 10px;
  position: relative;
  word-wrap: break-word;
}

.mensaje-burbuja p { 
  margin: 0; 
  color: #000000; /* Aseguramos que el texto del chat sea negro y visible */
}

.emisor-tag {
  font-size: 0.75rem;
  color: #075e54;
  font-weight: bold;
  margin-bottom: 4px;
  display: block;
}

.mi-mensaje {
  background-color: #dcf8c6; /* Verde claro WhatsApp */
  align-self: flex-end;
  border-top-right-radius: 0;
}

.su-mensaje {
  background-color: #ffffff;
  align-self: flex-start;
  border-top-left-radius: 0;
  box-shadow: 0 1px 2px rgba(0,0,0,0.1);
}

.chat-input {
  display: flex;
  padding: 10px;
  background-color: #f0f0f0;
  gap: 10px;
}

.chat-input input {
  flex: 1;
  padding: 10px;
  border: none;
  border-radius: 20px;
  outline: none;
}

.btn-enviar {
  background-color: #075e54;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 20px;
  cursor: pointer;
}
</style>