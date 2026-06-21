import { createRouter, createWebHistory } from 'vue-router'

import Index from '../views/index.vue'
import Ingreso from '../views/Auth/Ingreso.vue'

const router = createRouter({
    history: createWebHistory(),
    routes: [
        {
            path: "/",
            name: "Inicio",
            component: Index,
            meta: { requiresAuth: true }
        },
        {
            path: "/ingresar",
            name: "Ingresar",
            component: Ingreso
        }
    ]
})

// 🔐 GUARDIA GLOBAL
router.beforeEach((to, from, next) => {
    const token = localStorage.getItem("Token")

    // si la ruta requiere auth y no hay token
    if (to.meta.requiresAuth && !token) {
        next({ name: "Ingresar" })
    } 
    else {
        next()
    }
})

export default router