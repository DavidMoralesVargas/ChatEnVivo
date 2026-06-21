<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import Swal from 'sweetalert2';
import { api } from '../../services/api';

const router = useRouter();

const esLogin = ref(true);
const cargando = ref(false);

const loginForm = ref({
    Nombre_Usuario: '',
    Contrasena: ''
});

const registroForm = ref({
    Nombre_Usuario: '',
    Email: '',
    Contrasena: ''
});

const ingresar = async () => {
    try {

        cargando.value = true;

        const respuesta = await api.post(
            '/api/usuarios/Ingresar',
            loginForm.value
        );

        if (respuesta) {
            localStorage.setItem(
                'Token',
                respuesta
            );
        }

        router.push('/');

    } catch (error) {
        console.error(error);
    }
    finally {
        cargando.value = false;
    }
};

const registrar = async () => {
    try {

        cargando.value = true;

        await api.post(
            '/api/usuarios/Registrar',
            registroForm.value
        );

        await Swal.fire({
            icon: 'success',
            title: 'Cuenta creada',
            text: 'Ahora puedes iniciar sesión'
        });

        esLogin.value = true;

    } catch (error) {
        console.error(error);
    }
    finally {
        cargando.value = false;
    }
};
</script>

<template>
<div class="login-page">

    <div class="left-panel">

        <div class="brand">

            <h1>ChatEnVivo</h1>

            <p>
                Conecta con personas en tiempo real,
                crea salas y comparte mensajes.
            </p>

        </div>

    </div>

    <div class="right-panel">

        <div class="auth-card">

            <div class="tabs">

                <button
                    @click="esLogin = true"
                    :class="{ active: esLogin }"
                >
                    Iniciar Sesión
                </button>

                <button
                    @click="esLogin = false"
                    :class="{ active: !esLogin }"
                >
                    Registrarse
                </button>

            </div>

            <form
                v-if="esLogin"
                @submit.prevent="ingresar"
                class="formulario"
            >

                <h2>Bienvenido</h2>

                <input
                    v-model="loginForm.Nombre_Usuario"
                    placeholder="Usuario"
                >

                <input
                    v-model="loginForm.Contrasena"
                    type="password"
                    placeholder="Contraseña"
                >

                <button
                    type="submit"
                    class="btn-primary"
                >
                    {{ cargando ? 'Ingresando...' : 'Ingresar' }}
                </button>

            </form>

            <form
                v-else
                @submit.prevent="registrar"
                class="formulario"
            >

                <h2>Crear Cuenta</h2>

                <input
                    v-model="registroForm.Nombre_Usuario"
                    placeholder="Usuario"
                >

                <input
                    v-model="registroForm.Email"
                    placeholder="Correo"
                >

                <input
                    v-model="registroForm.Contrasena"
                    type="password"
                    placeholder="Contraseña"
                >

                <button
                    type="submit"
                    class="btn-primary"
                >
                    {{ cargando ? 'Registrando...' : 'Registrarse' }}
                </button>

            </form>

        </div>

    </div>

</div>
</template>

<style scoped>
.login-page{
    width:100%;
    min-height:100vh;
    display:flex;
    overflow:hidden;
}

/* IZQUIERDA */

.left-panel{
    flex:1.2;

    display:flex;
    align-items:center;
    justify-content:center;

    background:
        radial-gradient(
            circle at top left,
            #3b82f6,
            transparent 40%
        ),
        radial-gradient(
            circle at bottom left,
            #8b5cf6,
            transparent 40%
        ),
        #0f172a;
}

.brand{
    max-width:500px;
    padding:40px;
}

.brand h1{
    color:white;
    font-size:5rem;
    margin-bottom:20px;
}

.brand p{
    color:#cbd5e1;
    font-size:1.4rem;
    line-height:1.8;
}

/* DERECHA */

.right-panel{
    flex:0.9;

    background:#111827;

    display:flex;
    justify-content:center;
    align-items:center;

    padding:40px;
}

.auth-card{
    width:100%;
    max-width:500px;

    background:#1e293b;

    border-radius:20px;

    padding:40px;

    box-shadow:
        0 20px 50px rgba(0,0,0,.3);
}

/* TABS */

.tabs{
    display:flex;
    margin-bottom:30px;
}

.tabs button{
    flex:1;

    border:none;

    padding:15px;

    cursor:pointer;

    color:white;

    background:#334155;
}

.tabs button.active{
    background:#2563eb;
}

/* FORM */

.formulario{
    display:flex;
    flex-direction:column;
    gap:15px;
}

.formulario h2{
    color:white;
    text-align:center;
    margin-bottom:15px;
    font-size:2rem;
}

.formulario input{
    padding:16px;

    border-radius:10px;

    border:1px solid #475569;

    background:#0f172a;

    color:white;

    font-size:1rem;
}

.formulario input:focus{
    outline:none;
    border-color:#3b82f6;
}

.btn-primary{
    margin-top:10px;

    padding:16px;

    border:none;

    border-radius:10px;

    cursor:pointer;

    color:white;

    font-size:1rem;

    font-weight:bold;

    background:#2563eb;
}

.btn-primary:hover{
    background:#1d4ed8;
}

/* MÓVIL */

@media(max-width:900px){

    .login-page{
        flex-direction:column;
    }

    .left-panel{
        min-height:250px;
    }

    .brand h1{
        font-size:3rem;
    }
}
</style>