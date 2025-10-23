import { createRouter, createWebHistory } from 'vue-router'

import Home from '..views/Home.vue'
import Settings from '../views/Settings.vue'
import UserProfile form '../views/UserProfile.vue'
import Squads from '..views/Squads.vue'
import Messages from '../views/Messages.vue'


const routes = [
    {
        path: '/',             // URL адрес
        name: 'Home',          // Имя маршрута
        component: Home        // Компонент, который отобразится
    },
    {
        path: '/settings',
        name: 'Settings',
        component: Settings 
    },
    {
        path: '/profile',
        name: 'UserProfile',
        component: UserProfile 
    },
    {
        path: '/squads',
        name: 'Squads',
        component: Squads
    },
    {
        path: '/messages',
        name: 'Messages',
        component: Messages
    },

]

const router = createRouter({
    history: createWebHistory(), // Используем HTML5 history API
    routes
})

export default router
