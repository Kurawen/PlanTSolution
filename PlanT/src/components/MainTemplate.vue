<script setup>
import { ref, computed, onMounted } from 'vue'
import { isAuthenticated, getUserData } from '@/services/AuthService'
// import { useAuthStore } from '@/stores/auth'

// const authStore = useAuthStore()

// const authState = computed(() => authStore.isAuthenticated)
// const userData = computed(() => authStore.userData)

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

// Реальное состояние авторизации
const authState = ref(false)
const userData = ref(null)

// Данные уведомлений
const notificationsData = ref({
    chatNotifications: [
        {
            id: 1,
            title: 'Команда маркетинга',
            message: 'Отчет за Q3 готов к проверке.',
            time: '10 минут назад',
            avatar: ''
        },
        {
            id: 2,
            title: 'Проект Alpha',
            message: 'Обновление статуса: Встреча в 15.00.',
            time: '2 часа назад',
            avatar: ''
        },
        {
            id: 3,
            title: 'Отдел продаж',
            message: 'Новый клиент: Свяжитесь с Эмили Браун.',
            time: 'Вчера',
            avatar: ''
        }
    ],
    taskNotifications: [
        {
            id: 1,
            title: 'Разработать фичу авторизации',
            status: 'Приближается срок',
            assignee: 'Анна Смирнова',
            priority: 'high',
            dueDate: '2024-07-30'
        },
        {
            id: 2,
            title: 'Обзор дизайна домашней страницы',
            assignee: 'Дмитрий Иванов',
            dueDate: '30.07.2024',
            priority: 'medium'
        },
        {
            id: 3,
            title: 'Исправить ошибку #BUG-456',
            assignee: 'Сергей Петров',
            dueDate: '15.07.2024',
            priority: 'high'
        },
        {
            id: 4,
            title: 'Подготовить презентацию для клиентов',
            assignee: 'Елена Кузнецова',
            dueDate: '28.07.2024',
            status: 'Продолжается срок',
            priority: 'medium'
        }
    ]
})

// Вычисляемые свойства для разных состояний
const showAuthLinks = computed(() => !authState.value)
const showUserLinks = computed(() => authState.value)

onMounted(() => {
    checkAuth()
})

// Функция проверки авторизации
const checkAuth = () => {
    authState.value = isAuthenticated()
    if (authState.value) {
        userData.value = getUserData()
    }
}

// Функция для открытия уведомлений
const openNotifications = () => {
    emit('open-notifications', notificationsData.value)
}
</script>

<template>
    <div id="app">
        <!-- шапка -->
        <header id="shapka" v-if="!hideHeader">
            <nav class="navbar">
                <div class="nav-title">
                    <router-link to="/">
                        <img src="../assets/plant-logo.svg" alt="растение" class="plant-dev">
                    </router-link>
                    <router-link to="/" style="text-decoration: none;">
                        <p class="nav-logo">PlanT</p>
                    </router-link>
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
                            @click="openNotifications"
                        >
                        <router-link to="/profile">
                            <img src="../assets/hanna.jpg" alt="личный кабинет" class="photo-link">
                        </router-link>
                    </template>

                    <!-- Для неавторизованных пользователей -->
                    <template v-if="showAuthLinks">
                        <button class="login-btn" @click="$emit('open-auth', 'login')">
                            Войти в аккаунт
                        </button>
                    </template>
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
    max-width: 35px;
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