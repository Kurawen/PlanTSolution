<!-- ProblemCreate.vue -->
<script setup>
import { ref } from 'vue'

const emit = defineEmits(['close', 'create-task'])

const newTask = ref({
    title: '',
    deadline: '',
    status: 'не начато',
    priority: 'средний',
    executor: ''
})

const createTask = () => {
    if (!newTask.value.title.trim()) {
        alert('Введите название задачи')
        return
    }

    emit('create-task', { ...newTask.value })
}

const resetForm = () => {
    newTask.value = {
        title: '',
        deadline: '',
        status: 'к выполнению',
        priority: 'средний',
        executor: ''
    }
}

const closeModal = () => {
    resetForm()
    emit('close')
}
</script>

<template>
    <div class="modal-overlay">
        <div class="modal-content" @click.stop>
            <div class="modal-header">
                <h2>Создание новой задачи</h2>
                <button class="close-btn" @click="closeModal">×</button>
            </div>
            
            <div class="modal-body">
                <form @submit.prevent="createTask">
                    <div class="form-group">
                        <label for="title">Название задачи *</label>
                        <input
                            id="title"
                            v-model="newTask.title"
                            type="text"
                            placeholder="Введите название задачи"
                            required
                        />
                    </div>
                    
                    <div class="form-group">
                        <label for="deadline">Срок выполнения</label>
                        <input
                            id="deadline"
                            v-model="newTask.deadline"
                            type="date"
                        />
                    </div>
                    
                    <div class="form-group">
                        <label for="status">Статус</label>
                        <select id="status" v-model="newTask.status">
                            <option value="не начато">Не начато</option>
                            <option value="в работе">В работе</option>
                            <option value="завершено">Завершено</option>
                        </select>
                    </div>
                    
                    <div class="form-group">
                        <label for="priority">Приоритет</label>
                        <select id="priority" v-model="newTask.priority">
                            <option value="низкий">Низкий</option>
                            <option value="средний">Средний</option>
                            <option value="высокий">Высокий</option>
                        </select>
                    </div>
                    
                    <div class="form-group">
                        <label for="executor">Исполнитель</label>
                        <input
                            id="executor"
                            v-model="newTask.executor"
                            type="text"
                            placeholder="Введите имя исполнителя"
                        />
                    </div>
                    
                    <div class="form-actions">
                        <button type="button" class="btn-gray btn-md" @click="closeModal">
                            Отмена
                        </button>
                        <button type="submit" class="btn-black btn-md">
                            Создать задачу
                        </button>
                    </div>
                </form>
            </div>
        </div>
    </div>
</template>

<style scoped>
.modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 1000;
}

.modal-content {
    background: white;
    border-radius: 8px;
    width: 90%;
    max-width: 500px;
    max-height: 90vh;
    overflow-y: auto;
}

.modal-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 1.5rem;
    border-bottom: 1px solid var(--border-color);
}

.modal-header h2 {
    font-size: 1.5rem;
}

.close-btn {
    background: none;
    border: none;
    font-size: 1.5rem;
    cursor: pointer;
    color: #666;
    padding: 0;
    width: 30px;
    height: 30px;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
}

.close-btn:hover {
    background: var(--bg-color);
}

.modal-body {
    padding: 1.5rem;
}

.form-group {
    margin-bottom: 1.5rem;
}

.form-group label {
    display: block;
    margin-bottom: 0.5rem;
    font-weight: 500;
    color: black;
}

.form-group input,
.form-group select {
    width: 100%;
    padding: 0.75rem;
    border: 1px solid var(--border-color);
    border-radius: 4px;
    font-size: 1rem;
}

.form-group input:focus,
.form-group select:focus {
    outline: none;
    border-color: black;
}

.form-actions {
    display: flex;
    gap: 1rem;
    justify-content: flex-end;
    margin-top: 2rem;
}

.cancel-btn {
    padding: 0.75rem 1.5rem;
    border: 2px solid var(--border-color);
    border-radius: 5px;
    background: white;
    color: #333;
    cursor: pointer;
    font-size: 1rem;
}

.cancel-btn:hover {
    background: var(--bg-color);
}

.create-btn {
    padding: 0.75rem 1.5rem;
    border: 2px solid black;
    border-radius: 4px;
    background: black;
    color: white;
    cursor: pointer;
    font-size: 1rem;
    transition: ease-in-out 0.3s;
}

.create-btn:hover {
    background-color: white;
    color: black;
}
</style>