<script setup>
import { ref, computed } from 'vue'

const props = defineProps({
    hideHeader: {
        type: Boolean,
        default: false
    },
    hideFooter: {
        type: Boolean,
        default: false
    }
})

const emit = defineEmits(['open-auth', 'open-notifications'])

// Состояние авторизации
const isAuthenticated = ref(true)

// Вычисляемые свойства для разных состояний
const showAuthLinks = computed(() => !isAuthenticated.value)
const showUserLinks = computed(() => isAuthenticated.value)

// Функция для переключения состояния (для демонстрации)
const toggleAuth = () => {
    isAuthenticated.value = !isAuthenticated.value
}
</script>

<template>
    <div id="app">
        <!-- шапка -->
        <header id="shapka" v-if="!hideHeader">
            <nav class="navbar">
                <div class="nav-title">
                    <img src="../assets/plant-logo.svg" alt="растение" class="plant-dev">
                    <p class="nav-logo">PlanT</p>
                </div>
                
                <!-- Навигационные ссылки -->
                <div class="nav-links">
                    <router-link to="/" class="nav-link">Главная</router-link>
                    
                    <!-- Ссылки для авторизованных пользователей -->
                    <template v-if="showUserLinks">
                        <router-link to="/problems" class="nav-link">Задачи</router-link> 
                        <router-link to="/squads" class="nav-link">Группы</router-link> 
                        <router-link to="/messages" class="nav-link">Сообщения</router-link> 
                    </template>
                </div>

                <!-- Правая часть шапки -->
                <div class="notif-account">
                    <!-- Для авторизованных пользователей -->
                    <template v-if="showUserLinks">
                        <router-link to="/settings">
                            <img src="../assets/gear4.svg" alt="настройки" class="main-icon">
                        </router-link>
                        <img 
                            src="../assets/bell2.svg" 
                            alt="уведомления" 
                            class="main-icon" 
                            @click="emit('open-notifications')"
                        >
                        <router-link to="/profile">
                            <img src="../assets/hanna.jpg" alt="личный кабинет" class="photo-link">
                        </router-link>
                    </template>

                    <!-- Для неавторизованных пользователей -->
                    <template v-if="showAuthLinks">
                        <button class="login-btn" @click="emit('open-auth', 'login')">
                            Войти в аккаунт
                        </button>
                    </template>

                    <!-- Кнопка для демонстрации (уберите в продакшене) -->
                    <button v-if="false" class="demo-btn" @click="toggleAuth">
                        {{ isAuthenticated ? 'Выйти' : 'Войти (демо)' }}
                    </button>
                </div>
            </nav>
        </header>

        <main id="main">
            <slot></slot>
        </main>

        <!-- подвал -->
        <footer class="dno" v-if="!hideFooter">
            <p>2025</p>
            <p>© PlanT</p>
            <a href="https://t.me/myfavoritejumoreski" target="_blank">
                <img src="../assets/telegram.svg" alt="телеграм" class="main-icon">
            </a>
        </footer>
    </div>
</template>

<style scoped>
.plant-dev {
    max-width: 50px;
    height: auto;
}

#app {
    min-height: 100vh;
    display: flex;
    flex-direction: column;
    /* align-items: center; */
}

/* шапка */
#shapka {
    box-shadow: 0 2px 10px rgba(0,0,0,0.1);
    position: sticky;
    top: 0;
    z-index: 100;
    margin-bottom: 2rem;
}

.navbar {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 1rem 2rem;
    margin: 0 auto;
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
    color: black;
}

.nav-links {
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 30px;
    font-size: 1.2rem;
}

.nav-link {
    text-decoration: none;
    color: black;
    font-weight: 500;
    transition: color 0.3s;
    position: relative;
}

.nav-link:hover {
    color: #0000009f;
}

/* .nav-link.router-link-active::after {
    content: '';
    position: absolute;
    bottom: -5px;
    left: 0;
    width: 100%;
    height: 2px;
    background-color: #007bff;
} */

.notif-account {
    display: flex;
    gap: 20px;
    align-items: center;
    justify-content: center;
}

.login-btn {
    background-color: white;
    border: 1px solid black;
    border-radius: 8px;
    padding: 0.75rem 1.5rem;
    font-weight: 600;
    font-size: 1rem;
    cursor: pointer;
    transition: all 0.3s ease;
    min-width: fit-content;
}

.login-btn:hover {
    background: var(--bg-color);
}

/* Кнопка для демонстрации (скрыта по умолчанию) */
.demo-btn {
    background-color: #ff6b6b;
    color: white;
    border: none;
    border-radius: 6px;
    padding: 0.5rem 1rem;
    font-size: 0.8rem;
    cursor: pointer;
    opacity: 0.7;
}

.demo-btn:hover {
    opacity: 1;
}

#main {
    flex: 1;
    padding: 0;
}

/* подвал */
.dno {
    color: black;
    display: flex;
    align-items: center;
    justify-content: space-between;
    font-weight: 500;
    font-size: 1.2rem;
    padding: 1rem 2rem;
    background-color: white;
}

.dno > p {
    font-family: var(--text-header);
    font-weight: 700;
}

.main-icon {
    max-width: 40px;
    height: auto;
    cursor: pointer;
    transition: transform 0.2s ease-in-out;
}

.main-icon:hover, .photo-link:hover {
    transform: scale(1.1);
}

.photo-link {
    height: 50px;
    width: 50px;
    border-radius: 50%;
    object-fit: cover;
    border: 2px solid transparent;
    transition: transform 0.2s ease-in-out;
}

/* Адаптивность */
@media (max-width: 768px) {
    .navbar {
        padding: 1rem;
        flex-wrap: wrap;
        gap: 15px;
    }
    
    .nav-links {
        gap: 15px;
        font-size: 1rem;
    }
    
    .notif-account {
        gap: 15px;
    }
    
    .login-btn {
        padding: 0.5rem 1rem;
        font-size: 0.9rem;
    }
}
</style>