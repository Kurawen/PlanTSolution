<script setup>
import { ref, reactive, computed } from 'vue'
import { RouterView } from 'vue-router'

// Данные профиля
const profile = reactive({
    name: 'Анастейша Стил',
    email: 'nastya50@mail.ru',
    phone: '+7 (123) 456-78-90'
})

// Редактируемая копия профиля
const editProfile = reactive({...profile})

// Группы пользователя
const groups = ref([
    { id: 1, name: 'Запуск продукта в 3 квартале', role: 'Старший менеджер проекта' },
    { id: 2, name: 'Маркетинговая стратегия 2025', role: 'Дизайнер' },
    { id: 3, name: 'Кросс-функциональная синхронизация', role: 'Проектировщик' },
    { id: 4, name: 'Инновационный центр НИОКР', role: 'Тестировщик' }
])

// Состояние редактирования
const isEditing = ref(false)

// Получение инициалов для аватара
const getInitials = computed(() => {
    return profile.name
        .split(' ')
        .map(word => word[0])
        .join('')
        .toUpperCase()
})

// Переключение режима редактирования
const toggleEditMode = () => {
    isEditing.value = !isEditing.value
    if (isEditing.value) {
        Object.assign(editProfile, profile)
    }
}

// Отмена редактирования
const cancelEdit = () => {
    isEditing.value = false
}

// Сохранение профиля
const saveProfile = () => {
    Object.assign(profile, editProfile)
    isEditing.value = false
    // В реальном приложении здесь был бы вызов API для сохранения данных
    alert('Профиль успешно обновлен!')
}
</script>

<template>
    <div class="profile-page">
        <div class="profile-card">
            <div class="profile-header">
                <div class="profile-avatar">
                    <img src="../assets/margo.jpg" alt="" class="profile-avatar">
                </div>
                <h1 class="profile-name">{{ profile.name }}</h1>
                <div class="contact-info">
                    <div class="contact-item">
                        <span>{{ profile.email }}</span>
                    </div>
                    <div class="contact-item">
                        <span>{{ profile.phone }}</span>
                    </div>
                </div>
            </div>
            
            <div class="profile-body">
                <div class="section">
                    <h2 class="section-title">Профиль</h2>
                    <router-link to="/settings" class="btn-black btn-md">Редактировать профиль</router-link>

                </div>
                
                <div class="section">
                    <h2 class="section-title">Группы</h2>
                    <div class="groups-list">
                        <div class="group-item" v-for="group in groups" :key="group.id">
                        <div class="group-name">{{ group.name }}</div>
                        <div class="group-role">{{ group.role }}</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>



<style scoped>
.profile-page {
    max-width: 600px;
    margin: 0 auto;
}

.profile-card {
    background: white;
    border-radius: 5px;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    overflow: hidden;
    margin-bottom: 24px;
}

.profile-header {
    background-color: white;
    padding: 30px;
    text-align: center;
    position: relative;
}

.profile-avatar {
    width: 120px;
    height: 120px;
    border-radius: 50%;
    margin: 0 auto 15px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 48px;
    font-weight: bold;
}

.profile-name {
    font-size: 28px;
    font-weight: 600;
    margin-bottom: 8px;
    color: black;
}

.contact-info {
    display: flex;
    justify-content: center;
    gap: 20px;
    flex-wrap: wrap;
}

.contact-item {
    display: flex;
    align-items: center;
    gap: 8px;
    font-size: 14px;
}

.contact-icon {
    width: 20px;
    height: 20px;
    display: flex;
    align-items: center;
    justify-content: center;
}

.profile-body {
    padding: 24px;
}

.section {
    margin-bottom: 28px;
}

.section-title {
    font-size: 18px;
    font-weight: 600;
    margin-bottom: 16px;
    color: black;
    display: flex;
    align-items: center;
    gap: 8px;
}

.groups-list {
    display: flex;
    flex-direction: column;
    gap: 12px;
}

.group-item {
    border: 2px solid black;
    border-radius: 5px;
    padding: 16px;
    transition: all 0.3s ease;
}

.group-item:hover {
    transform: translateX(5px);
}

.group-name {
    font-weight: 600;
    margin-bottom: 4px;
}

.group-role {
    color: grey;
    font-size: 14px;
}

</style>