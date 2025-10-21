<script setup lang="ts">
import { ref } from 'vue'
import DevelopingModalWindow from './components/DevelopingModalWindow.vue'
import Auth from './components/Authentication.vue'
import Notifications from './components/Notifications.vue'
import MainTemplate from './components/MainTemplate.vue'

const showModal = ref(false)
const showAuth = ref(false)
const authMode = ref('login')

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

</script>



<template>
    <div id="app">
        <!-- шапка -->
        <header id="shapka">
            <nav class="navbar">
                <div class="nav-title">
                    <img src="./assets/plant-logo.svg" alt="растение" class="plant-dev">
                    <a href="#" class="nav-logo">PlanT</a>
                </div>
                <a href="#" class="nav-link">Главная</a>
                <button class="login-btn" @click="openAuth('login')">Войти в аккаунт</button>
            </nav>
        </header>

        <!-- Основной контент -->
        <main id="main">
            <!-- работайте над задачами -->
            <section class="tasks">
                <div class="tasks-content">
                    <h1 class="tasks-title">Работайте над задачами</h1>
                    <p class="tasks-desc">
                        <strong>PlanT</strong> дает возможность студентам организовывать проекты и без особых 
                        усилий сотрудничать и достигать целей. При этом можно работать как одному, так и в команде.
                    </p>
                </div>
                <img src="./assets/plant-big.svg" alt="растение" class="tasks-plant-img">
            </section>


            <!-- ключевые особенности -->
            <section class="keys">
                <h2 class="keys-title">Ключевые особенности</h2>
                <div class="keys-grid">
                    <div class="keys-card">
                        <div class="keys-name">
                            <img src="./assets/list-todo.svg" alt="растение в горшке" class="keys-icon">
                            <h3>Управление задачами</h3>
                        </div>
                        <p class="keys-desc">Управляйте задачами и достигайте высот.</p>
                        <button class="keys-btn" @click="openModal">Перейти к задачам</button>
                    </div>

                    <div class="keys-card">
                        <div class="keys-name">
                            <img src="./assets/team.svg" alt="растение в горшке" class="keys-icon">
                            <h3>Работа в команде</h3>
                        </div>
                        <p class="keys-desc">Создавайте группы. Создавайте контент.</p>
                        <button class="keys-btn" @click="openModal">Перейти к группам</button>
                    </div>

                    <div class="keys-card">
                        <div class="keys-name">
                            <img src="../assets/message.svg" alt="растение в горшке" class="keys-icon">
                            <h3>Коммуникация</h3>
                        </div>
                        <p class="keys-desc">Взаимодействуйте с вашей командой.</p>
                        <button class="keys-btn" @click="openModal">Перейти к сообщениям</button>
                    </div>
                </div>
            </section>


            <!-- готовы улучшить свою продуктивность? -->
            <section class="prod">
                <div class="prod-content">
                    <h2 class="prod-title">Готовы улучшить свою продуктивность?</h2>
                    <button class="prod-btn" @click="openAuth('login')">Начать сейчас</button>
                </div>
            </section>
        </main>

        <!-- подвал -->
        <footer class="dno">
            <p>2025</p>
            <p>© PlanT</p>
            <a href="https://t.me/myfavoritejumoreski" target="_blank"><img src="./assets/telegram.svg" alt="телеграм" id="telegram-icon"></a>
        </footer>






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
        <DevelopingModalWindow v-if="showModal" @close="closeModal"/>
    </div>
</template>

<style>
.plant-dev {
    max-width: 50px;
    height: auto;
}

.plant-dev-main {
    max-width: 150px;
    height: auto;
}

#app {
    /* min-height: 100vh; */
    display: flex;
    flex-direction: column;
}

/* шапка */
#shapka {
    box-shadow: 0 2px 10px rgba(0,0,0,0.1);
    position: sticky;
    top: 0;
    z-index: 100;
}

.navbar {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 1rem 2rem;
    margin: 0 auto;
    font-size: 1.4rem;
    background-color: white;
}

.nav-title {
    display: flex;
    align-items: center;
    gap: 10px;
}

.nav-logo {
    font-size: 1.8rem;
    font-weight: 700;
    font-family: var(--text-header);
    text-decoration: none;
}

.nav-link {
    text-decoration: none;
    font-weight: 500;
    transition: color 0.3s;
}

.login-btn {
    background-color: white;
    border: 1px solid black;
    border-radius: 5px;
    padding: 0.75rem 1.5rem;
    font-weight: 600;
    /* font-size: 1rem; */
    cursor: pointer;
    transition: background 0.3s;
}

.login-btn:hover {
    background: #f5f5f5;
}

/* основной контент */
#main {
    flex: 1;
}

/* работа над задачами */
.tasks {
    background-color: var(--bg-color);
    margin: 24px 12rem;
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
    align-items: center;
    gap: 2rem;
}

.keys-card {
    padding: 2rem;
    border-radius: 12px;
    box-shadow: 0 4px 20px rgba(0,0,0,0.1);
    text-align: left;
    transition: transform 0.3s, box-shadow 0.3s;
    display: flex;
    flex-direction: column;
    gap: 3rem;
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
}

.keys-btn {
    background: transparent;
    border: 1px solid black;
    border-radius: 5px;
    padding: 0.75rem 1.5rem;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.3s;
    align-self: flex-start;
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
    margin: 0 auto;
}

.prod-title {
    font-size: 2.2rem;
    margin-bottom: 2rem;
    font-family: var(--text-header);
    font-weight: 600;
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
    transition: background 0.4s;
}

.prod-btn:hover {
    color: black;
    background-color: white;
    border: 1px solid black;
}

/* подвал */
.dno {
    color: black;
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: space-between;
    font-weight: 500;
    font-size: 1.2rem;
    padding: 1rem 2rem;
}

#telegram-icon {
    max-width: 40px;
    height: auto;
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