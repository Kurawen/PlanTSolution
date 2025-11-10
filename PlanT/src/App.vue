<script setup lang="ts">
import { ref } from 'vue'
import { RouterView } from 'vue-router'
import MainTemplate from './components/MainTemplate.vue'
import DevelopingModalWindow from './components/DevelopingModalWindow.vue'
import Auth from './components/Authentication.vue'
import Notifications from './components/Notifications.vue'

const showModal = ref(false)
const showAuth = ref(false)
const authMode = ref('login')
const notificationsData = ref(null)

const openModal = () => {
    showModal.value = true
}

const closeModal = () => {
    showModal.value = false
    notificationsData.value = null
}

const openAuth = (mode = 'login') => {
    authMode.value = mode
    showAuth.value = true
}

const closeAuth = () => {
    showAuth.value = false
}

const handleAuthSubmit = (data) => {
    console.log('Данные формы:', data)
    // Здесь будет логика авторизации/регистрации
    // После успешной авторизации:
    // closeAuth()
}

const handleSwitchMode = (newMode) => {
    authMode.value = newMode
}

const handleGuestLogin = () => {
    console.log('Гостевой вход')
    closeAuth()
    // Логика для гостевого режима
}

const handleOpenNotifications = (data) => {
    notificationsData.value = data
    showModal.value = true
}

</script>

<template>
    <MainTemplate 
        @open-auth="openAuth" 
        @open-notifications="handleOpenNotifications"
    >
        
        <RouterView/>

        <!-- Отладочная информация -->
        <!-- <div style="position: fixed; top: 10px; right: 10px; background: red; color: white; padding: 10px; z-index: 9999">
            RouterView: {{ $route.path }}
        </div> -->

        <!-- модальное окно авторизации/регистрации -->
        <div v-if="showAuth" class="auth-modal-overlay" @click="closeAuth">
            <div class="auth-modal-content" @click.stop>
                <Auth 
                :mode="authMode"
                @submit="handleAuthSubmit"
                @switch-mode="handleSwitchMode"
                @guest-login="handleGuestLogin"
                />
            </div>
        </div>

        <!-- модальное окно в разработке -->
        <!-- <DevelopingModalWindow v-if="showModal" @close="closeModal"/> -->

        <!-- модальное окно уведомлений -->
        <Notifications 
            v-if="showModal && notificationsData" 
            @close="closeModal"
            :chat-notifications="notificationsData.chatNotifications"
            :task-notifications="notificationsData.taskNotifications"
        />
        
    </MainTemplate>
</template>

<style>
.auth-modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 1000;
}

.auth-modal-content {
    animation: slideIn 0.3s ease-out;
}

@keyframes slideIn {
    from {
        opacity: 0;
        transform: scale(0.9);
    }
    to {
        opacity: 1;
        transform: scale(1);
    }
}

</style>