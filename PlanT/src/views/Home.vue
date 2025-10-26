<script setup>
import { ref, computed } from 'vue'
import DevelopingModalWindow from '@/components/DevelopingModalWindow.vue'
import Auth from '@/components/Authentication.vue'

const showModal = ref(false)
const showAuth = ref(false)
const authMode = ref('login')

// Состояние авторизации
const isAuthenticated = ref(true)

const openModal = () => {
    showModal.value = true
}

const closeModal = () => {
    showModal.value = false
}

const openAuth = (mode = 'login') => {
    authMode.value = mode
    showAuth.value = true
}

const closeAuth = () => {
    showAuth.value = false
}

// Обработчики для авторизации
const handleAuthSubmit = (data) => {
    console.log('Данные формы:', data)
    // После успешной авторизации:
    // isAuthenticated.value = true
    // closeAuth()
}

const handleSwitchMode = (newMode) => {
    authMode.value = newMode
}

const handleGuestLogin = () => {
    console.log('Гостевой вход')
    // isAuthenticated.value = true
    closeAuth()
}

// Вычисляемое свойство для отображения кнопок
const showActionButtons = computed(() => isAuthenticated.value)

// Функция для демонстрации (уберите в продакшене)
const toggleAuth = () => {
    isAuthenticated.value = !isAuthenticated.value
}
</script>

<template>
    <!-- Основной контент -->
    <main id="main">
        <!-- работайте над задачами -->
        <section class="tasks">
            <div class="tasks-content">
                <h1 class="tasks-title">Работайте над задачами</h1>
                <p class="tasks-desc">
                    <strong>PlanT</strong> дает возможность студентам организовывать проекты и без особых 
                    усилий сотрудничать и достигать целей. Прищей можно работать как одному, так и в команде.
                </p>
            </div>
            <img src="../assets/plant-big.svg" alt="растение" class="tasks-plant-img">
        </section>

        <!-- ключевые особенности -->
        <section class="keys">
            <h2 class="keys-title">Ключевые особенности</h2>
            <div class="keys-grid">
                <div class="keys-card" :class="{ 'keys-card-compact': !showActionButtons }">
                    <div class="keys-name">
                        <img src="../assets/list-todo.svg" alt="растение в горшке" class="keys-icon">
                        <h3>Управление задачами</h3>
                    </div>
                    <p class="keys-desc">Управляйте задачами и достигайте высот.</p>
                    <button 
                        v-if="showActionButtons" 
                        class="keys-btn" 
                        @click="openModal"
                    >
                        Перейти к задачам
                    </button>
                </div>

                <div class="keys-card" :class="{ 'keys-card-compact': !showActionButtons }">
                    <div class="keys-name">
                        <img src="../assets/team.svg" alt="растение в горшке" class="keys-icon">
                        <h3>Работа в команде</h3>
                    </div>
                    <p class="keys-desc">Создавайте группы. Создавайте контент.</p>
                    <button 
                        v-if="showActionButtons" 
                        class="keys-btn" 
                        @click="openModal"
                    >
                        Перейти к группам
                    </button>
                </div>

                <div class="keys-card" :class="{ 'keys-card-compact': !showActionButtons }">
                    <div class="keys-name">
                        <img src="../assets/message.svg" alt="растение в горшке" class="keys-icon">
                        <h3>Коммуникация</h3>
                    </div>
                    <p class="keys-desc">Взаимодействуйте с вашей командой.</p>
                    <button 
                        v-if="showActionButtons" 
                        class="keys-btn" 
                        @click="openModal"
                    >
                        Перейти к сообщениям
                    </button>
                </div>
            </div>
        </section>

        <!-- готовы улучшить свою продуктивность? -->
        <section v-if="!isAuthenticated" class="prod" >
            <div class="prod-content">
                <h2 class="prod-title">Готовы улучшить свою продуктивность?</h2>
                <button class="prod-btn" @click="openAuth">Начать сейчас</button>
            </div>
        </section>
    </main>
    
    <DevelopingModalWindow v-if="showModal" @close="closeModal"/>

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
</template>

<style scoped>
/* работа над задачами */
.tasks {
    background-color: var(--bg-color);
    margin: 0 12rem;
    padding: 4rem 4rem;
    text-align: center;
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
    border-radius: 5px;
}

.tasks-content {
    max-width: 800px;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: start;
    gap: 30px;
    text-align: start;
}

.tasks-title {
    color: black;
    font-size: 3rem;
    font-family: var(--text-header);
    font-weight: 800;
}

.tasks-desc {
    font-size: 1.2rem;
    line-height: 2rem;
    color: var(--text-color);
}

.tasks-plant-img {
    max-width: 250px;
    height: auto;
}

/* ключевые особенности */
.keys {
    padding: 4rem 2rem;
    margin: 0 15rem;
}

.keys-name > h3 {
    font-weight: 700;
    font-family: var(--text-header);
    font-size: 1.5rem;
}

.keys-title {
    text-align: center;
    font-size: 2.5rem;
    font-weight: 600;
    font-family: var(--text-header);
    margin-bottom: 3rem;
}

.keys-grid {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    align-items: stretch;
    gap: 2rem;
}

.keys-card {
    padding: 2rem;
    border-radius: 12px;
    box-shadow: 0 4px 20px rgba(0,0,0,0.1);
    text-align: left;
    transition: all 0.3s ease;
    display: flex;
    flex-direction: column;
    gap: 2rem;
    flex: 1;
    min-height: 220px;
}

/* Компактный вид карточек когда кнопок нет */
.keys-card-compact {
    gap: 1.5rem;
    min-height: 180px;
    padding: 1.5rem;
}

.keys-icon {
    max-width: 30px;
    height: auto;
}

.keys-name {
    display: flex;
    flex-direction: row;
    justify-content: start;
    align-items: center;
    gap: 15px;
    font-size: 1rem;
}

.keys-desc {
    color: gray;
    line-height: 1.6;
    flex: 1;
}

.keys-btn {
    background: transparent;
    border: 2px solid var(--border-color);
    border-radius: 5px;
    padding: 0.75rem 1.5rem;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.3s;
    align-self: flex-start;
    margin-top: auto;
}

.keys-btn:hover {
    background-color: var(--bg-color);
}

/* улучшение продуктивности */
.prod {
    color: white;
    padding: 4rem 2rem;
    text-align: center;
}

.prod-content {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    margin: 0 auto;
    gap: 4rem;
}

.prod-title {
    font-size: 2.2rem;
    font-family: var(--text-header);
    font-weight: 600;
    color: black;
}

.prod-btn {
    color: white;
    background-color: black;
    border: 1px solid black;
    border-radius: 5px;
    padding: 1rem 2.5rem;
    font-size: 1.1rem;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.3s ease;
}

.prod-btn:hover {
    color: black;
    background-color: white;
}

/* Демо-кнопка (скрыта по умолчанию) */
.demo-auth-toggle {
    text-align: center;
    margin-top: 2rem;
    padding: 1rem;
    background-color: #f8f9fa;
    border-radius: 8px;
    border: 1px dashed #ccc;
}

.demo-btn {
    background-color: #6c757d;
    color: white;
    border: none;
    border-radius: 6px;
    padding: 0.5rem 1rem;
    font-size: 0.8rem;
    cursor: pointer;
    margin-right: 1rem;
}

.demo-btn:hover {
    background-color: #5a6268;
}

.demo-status {
    font-size: 0.9rem;
    color: #666;
}

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