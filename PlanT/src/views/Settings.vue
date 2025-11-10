<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { logout as authLogout } from '@/services/AuthService' // Импортируем функцию выхода
import defaultAvatar from '../assets/margo.jpg'

const router = useRouter()

// Данные пользователя
const user = ref({
    avatar: defaultAvatar,
    username: 'Анастейша Стил',
    email: 'nastya50@mail.ru',
    phone: '+7 (123) 456-78-90'
})

const password = ref({
    current: '',
    new: '',
    confirm: ''
})

const showModal = ref(false)
const fileInput = ref(null)

// Методы
const triggerFileInput = () => {
    fileInput.value?.click()
}

const handleFileSelect = (event) => {
    const file = event.target.files[0]
    if (file) {
        console.log('Выбран файл:', file.name)
        changeAvatar(file)
    }
}

const changeAvatar = (file) => {
    const reader = new FileReader()
    
    reader.onload = (e) => {
        user.value.avatar = e.target.result
        console.log('Аватар обновлен')
    }
    
    reader.readAsDataURL(file)
}

const saveChanges = () => {
    console.log('Сохранение изменений:', {
        user: user.value,
        password: password.value
    })
}

// Функция выхода из аккаунта
const logout = () => {
    console.log('Выход из аккаунта')
    
    // Вызываем функцию выхода из AuthService
    authLogout()
    
    // Перенаправляем на главную страницу
    window.location.href = '/'
}

const openModal = () => {
    showModal.value = true
}

const closeModal = () => {
    showModal.value = false
}
</script>

<template>
    <div class="settings-container">
        <div class="settings-window">
            <h1 class="settings-title">Настройки профиля</h1>

            <section class="settings-section">
                <h2 class="section-title">Основная информация</h2>
                
                <!-- Аватар -->
                <div class="avatar-section">
                    <div class="avatar-container">
                        <div class="avatar-preview">
                        <img 
                            :src="user.avatar" 
                            alt="Аватар пользователя" 
                            class="avatar-image"
                        >
                        </div>
                        <button type="button" class="btn-gray btn-md" @click="triggerFileInput">
                            Изменить аватар
                        </button>
                        <input 
                            type="file" 
                            ref="fileInput"
                            class="file-load" 
                            name="filename"
                            accept="image/*"
                            @change="handleFileSelect"
                            style="display: none"
                        >
                    </div>
                </div>

                <div class="form-grid">
                    <div class="form-group">
                        <label class="form-label">Имя пользователя</label>
                        <input 
                        type="text" 
                        class="form-input"
                        v-model="user.username"
                        placeholder="Фамилия Имя"
                        >
                    </div>

                    <div class="form-group">
                        <label class="form-label">Электронная почта</label>
                        <input 
                        type="email" 
                        class="form-input"
                        v-model="user.email"
                        placeholder="editor50@mail.ru"
                        >
                    </div>

                    <div class="form-group">
                        <label class="form-label">Номер телефона</label>
                        <input 
                            type="tel" 
                            class="form-input"
                            v-model="user.phone"
                            placeholder="+7 (456) 13-34-45"
                        >
                    </div>
                </div>
            </section>

            <div class="section-divider"></div>

            <!-- Смена пароля -->
            <section class="settings-section">
                <h2 class="section-title">Изменить пароль</h2>
                
                <div class="form-grid">
                    <div class="form-group">
                        <label class="form-label">Текущий пароль</label>
                        <input 
                            type="password" 
                            class="form-input"
                            v-model="password.current"
                            placeholder="Введите текущий пароль"
                        >
                    </div>

                    <div class="form-group">
                        <label class="form-label">Новый пароль</label>
                        <input 
                            type="password" 
                            class="form-input"
                            v-model="password.new"
                            placeholder="Введите новый пароль"
                        >
                    </div>

                    <div class="form-group">
                        <label class="form-label">Подтвердите новый пароль</label>
                        <input 
                            type="password" 
                            class="form-input"
                            v-model="password.confirm"
                            placeholder="Повторите новый пароль"
                        >
                    </div>
                </div>
            </section>

            <div class="action-buttons">
                <button class="btn-black btn-md" @click="saveChanges">
                    Сохранить изменения
                </button>
                <button class="btn-red btn-md" @click="logout">
                    Выйти из аккаунта
                </button>
            </div>
        </div>

        <DevelopingModalWindow v-if="showModal" @close="closeModal"/>
    </div>
</template>

<style scoped>
.settings-container {
    display: flex;
    justify-content: center;
    padding: 2rem;
    min-height: calc(100vh - 200px);
}

.settings-window {
    border-radius: 12px;
    padding: 2rem;
    width: 100%;
    max-width: 600px;
    background-color: white;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
}

.settings-title {
    font-size: 2rem;
    font-weight: 700;
    color: #1a1a1a;
    margin: 0 0 2rem 0;
    text-align: center;
}

.settings-section {
    margin-bottom: 2rem;
}

.section-title {
    font-size: 1.3rem;
    font-weight: 600;
    color: #333;
    margin: 0 0 1.5rem 0;
}

.section-divider {
    height: 2px;
    background-color: var(--border-color);
    margin: 2rem 0;
}

.avatar-section {
    margin-bottom: 2rem;
}

.avatar-container {
    display: flex;
    align-items: center;
    gap: 1.5rem;
}

.avatar-preview {
    width: 100px;
    height: 100px;
    border-radius: 50%;
    overflow: hidden;
    border: 2px solid white;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.avatar-image {
    width: 100%;
    height: 100%;
    object-fit: cover;
}

.form-grid {
    display: flex;
    flex-direction: column;
    gap: 1.5rem;
}

.form-group {
    display: flex;
    flex-direction: column;
    gap: 0.5rem;
}

.form-label {
    font-size: 0.9rem;
    font-weight: 600;
    color: black;
    margin-bottom: 0.25rem;
}

.form-input {
    padding: 0.75rem 1rem;
    border: 2px solid #e1e5e9;
    border-radius: 8px;
    font-size: 1rem;
    transition: all 0.3s ease;
    background-color: white;
}

.form-input:focus {
    outline: none;
    border-color: black;
}

.form-input::placeholder {
    color: #999;
}

/* Кнопки действий */
.action-buttons {
    display: flex;
    flex-direction: column;
    gap: 1rem;
    margin-top: 2rem;
}

</style>