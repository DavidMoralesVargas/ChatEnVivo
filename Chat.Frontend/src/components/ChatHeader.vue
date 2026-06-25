<template>
  <header class="chat-header">
    <div class="header-left">
      <h2>Chat Grupal</h2>
    </div>
    
    <div class="header-center">
      <button class="nav-btn">Chat con Usuarios</button>
      <button class="nav-btn">Chat con Grupos</button>
    </div>

    <div class="header-right">
      <button class="logout-btn" @click="cerrarSesion">Cerrar Sesión</button>
    </div>
  </header>
</template>

<script setup>
import { useRouter } from 'vue-router';
const router = useRouter();
const cerrarSesion = () => {
  // Borramos el token del local storage
  localStorage.removeItem("Token");
  
  // Aquí podrías agregar un redireccionamiento, por ejemplo:
  // window.location.href = '/login'; o router.push('/login');
  router.push('/Ingresar');
};
</script>

<style scoped>
.chat-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background-color: #075e54; /* Color estilo WhatsApp */
  color: white;
  padding: 10px 20px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.header-center {
  display: flex;
  gap: 10px;
}

.nav-btn, .logout-btn {
  background-color: transparent;
  color: white;
  border: 1px solid white;
  padding: 5px 15px;
  border-radius: 5px;
  cursor: pointer;
  transition: background 0.3s;
}

.nav-btn:hover, .logout-btn:hover {
  background-color: rgba(255, 255, 255, 0.2);
}

.logout-btn {
  background-color: #d9534f;
  border: none;
}

.logout-btn:hover {
  background-color: #c9302c;
}

h2 {
  margin: 0;
  font-size: 1.2rem;
}
</style>