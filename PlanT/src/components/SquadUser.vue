<script setup>
defineProps({
    user: {
        type: Object,
        required: true
    }
})

defineEmits(['update:role'])
</script>

<template>
    <div class="role-item">
        <div class="user-info">
            <div class="user-name">{{ user.name }}</div>
            <div class="user-email">{{ user.email }}</div>
        </div>
        <select 
            class="role-select" 
            :value="user.role" 
            @change="$emit('update:role', $event.target.value)"
            >
            <option value="Руководитель проекта">Руководитель проекта</option>
            <option value="Ведущий аналитик">Ведущий аналитик</option>
            <option value="Разработчик">Разработчик</option>
            <option value="Дизайнер">Дизайнер</option>
            <option value="Тестировщик">Тестировщик</option>
        </select>
    </div>
</template>

<style scoped>
.role-item {
    display: flex;
    justify-content: space-between;
    align-items: flex-start;
    padding: 0;
}

.user-info {
    flex: 1;
}

.user-name {
    font-size: 1rem;
    font-weight: 500;
    color: #333;
    margin-bottom: 4px;
}

.user-email {
    font-size: 0.875rem;
    color: #666;
}

.role-select {
    padding: 8px 12px;
    border: 1px solid #d0d0d0;
    border-radius: 4px;
    font-size: 1rem;
    color: #333;
    background: white;
    min-width: 200px;
}
</style>