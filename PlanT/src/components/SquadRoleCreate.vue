<script setup>
import { ref } from 'vue'
import SquadUser from './SquadUser.vue'

const emit = defineEmits(['close'])

// Данные пользователей
const users = ref([
    {
        id: 1,
        name: 'Алексей Иванов',
        email: 'alexey.ivanov@taskflow.com',
        role: 'Руководитель проекта'
    },
    {
        id: 2,
        name: 'Мария Смирнова',
        email: 'maria.smirnova@taskflow.com',
        role: 'Ведущий аналитик'
    },
    {
        id: 3,
        name: 'Дмитрий Кузнецов',
        email: 'dmitry.kuznetsov@taskflow.com',
        role: 'Разработчик'
    },
    {
        id: 4,
        name: 'Елена Попова',
        email: 'elena.popova@taskflow.com',
        role: 'Дизайнер'
    },
    {
        id: 5,
        name: 'Михаил Васильев',
        email: 'mikhail.vasilyev@taskflow.com',
        role: 'Тестировщик'
    }
])

// Обновление роли пользователя
const updateUserRole = (userId, newRole) => {
    const user = users.value.find(u => u.id === userId)
    if (user) {
        user.role = newRole
    }
}

// Обработчики кнопок
const handleCancel = () => {
    console.log('Отмена изменений')
    emit('close')
}

const handleSave = () => {
    console.log('Сохранение изменений:', users.value)
    emit('close')
}
</script>

<template>
    <div class="assign-roles-container">
        <div class="assign-roles-window">
            <h1>Назначить роли</h1>
            
            <div class="roles-list">
                <SquadUser
                v-for="user in users"
                :key="user.id"
                :user="user"
                @update:role="updateUserRole(user.id, $event)"
                />
            </div>

            <div class="action-buttons">
                <button class="btn-cancel" @click="handleCancel">Отмена</button>
                <button class="btn-save" @click="handleSave">Сохранить изменения</button>
            </div>
        </div>
    </div>
</template>


<style scoped>
.assign-roles-container {
    position: fixed;
    display: flex;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    justify-content: center;
    align-items: center;
    z-index: 1000;
}

.assign-roles-window {
    background: white;
    border-radius: 8px;
    padding: 30px;
    width: 600px;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.assign-roles-window h1 {
    font-size: 1.5rem;
    font-weight: 600;
    margin-bottom: 25px;
    color: #333;
    text-align: left;
}

.roles-list {
    display: flex;
    flex-direction: column;
    gap: 20px;
    margin-bottom: 30px;
}

.action-buttons {
    display: flex;
    gap: 15px;
    justify-content: flex-end;
    border-top: 1px solid #e0e0e0;
    padding-top: 20px;
}

.btn-cancel,
.btn-save {
    padding: 10px 20px;
    border: 1px solid #d0d0d0;
    border-radius: 4px;
    font-size: 0.875rem;
    cursor: pointer;
    transition: all 0.2s ease;
}

.btn-cancel {
    background: white;
    color: #333;
}

.btn-cancel:hover {
    background: #f5f5f5;
}

.btn-save {
    background: #000;
    color: white;
    border-color: #000;
}

.btn-save:hover {
    background: #333;
    border-color: #333;
}
</style>