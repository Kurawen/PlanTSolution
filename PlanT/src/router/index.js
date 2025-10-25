import { createRouter, createWebHistory } from 'vue-router'

import Home from '../views/Home.vue'
import Settings from '../views/Settings.vue'
import UserProfile from '../views/UserProfile.vue'
import Squads from '../views/Squads.vue'
import Messages from '../views/Messages.vue'
import Problems from '../views/Problems.vue'

const routes = [
    { path: '/', name: 'Home', component: Home },
    { path: '/settings', name: 'Settings', component: Settings },
    { path: '/profile', name: 'UserProfile', component: UserProfile },
    { path: '/squads', name: 'Squads', component: Squads },
    { path: '/messages', name: 'Messages', component: Messages },
    { path: '/problems', name: 'Problems', component: Problems },
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router
