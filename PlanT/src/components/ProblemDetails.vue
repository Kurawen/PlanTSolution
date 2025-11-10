<script>
import { ref, computed } from 'vue'
import ProblemDelete from './ProblemDelete.vue';

export default {
    name: 'ProblemDetails',
    components: {
        ProblemDelete
    },
    props: {
        taskId: {
            type: Number,
            required: true
        },
        tasks: {
            type: Array,
            default: () => []
        }
    },
    setup(props, { emit }) {
        const showDeleteModal = ref(false)

        // Находим текущую задачу по ID
        const currentTask = computed(() => {
            return props.tasks.find(task => task.id === props.taskId) || {}
        })

        // Функции для смены статуса
        const updateStatus = (newStatus) => {
            emit('update-task', {
                taskId: props.taskId,
                field: 'status',
                value: newStatus
            })
        }

        // Функции для смены приоритета
        const updatePriority = (newPriority) => {
            emit('update-task', {
                taskId: props.taskId,
                field: 'priority',
                value: newPriority
            })
        }

        const openDeleteModal = () => {
            showDeleteModal.value = true
        }

        const closeDeleteModal = () => {
            showDeleteModal.value = false
        }

        const closeMainModal = () => {
            emit('close')
        }

        const confirmDelete = () => {
            emit('delete-task', props.taskId)
            closeDeleteModal()
            closeMainModal()
        }

        return {
            showDeleteModal,
            openDeleteModal,
            closeDeleteModal,
            closeMainModal,
            confirmDelete,
            currentTask,
            updateStatus,
            updatePriority
        }
    }
}
</script>

<template>
    <div class="detail-container">
        <div class="detail-window">
            <div class="detail-title">
                <h2>Детали задачи</h2>
                <button class="close-btn" @click="closeMainModal">×</button>
            </div>
            <hr>
            
            <!-- Заголовок -->
            <div class="detail-header">
                <h6>Заголовок</h6>
                <p>{{ currentTask.title || 'Название задачи' }}</p>
            </div>

            <!-- Описание -->
            <div class="detail-desc">
                <h6>Описание</h6>
                <p>{{ currentTask.description || 'Описание отсутствует' }}</p>
            </div>

            <!-- Статус с кнопками -->
            <div class="detail-section">
                <h6>Статус</h6>
                <div class="status-buttons">
                    <button 
                        class="btn-gray btn-md" 
                        :class="{ active: currentTask.status === 'не начато' }"
                        @click="updateStatus('не начато')"
                    >
                        не начато
                    </button>
                    <button 
                        class="btn-gray btn-md" 
                        :class="{ active: currentTask.status === 'в работе' }"
                        @click="updateStatus('в работе')"
                    >
                        В работе
                    </button>
                    <button 
                        class="btn-gray btn-md" 
                        :class="{ active: currentTask.status === 'завершено' }"
                        @click="updateStatus('завершено')"
                    >
                        Завершено
                    </button>
                </div>
            </div>

            <!-- Приоритет с кнопками -->
            <div class="detail-section">
                <h6>Приоритет</h6>
                <div class="priority-buttons">
                    <button 
                        class="priority-btn priority-low" 
                        :class="{ active: currentTask.priority === 'низкий' }"
                        @click="updatePriority('низкий')"
                    >
                        Низкий
                    </button>
                    <button 
                        class="priority-btn priority-medium" 
                        :class="{ active: currentTask.priority === 'средний' }"
                        @click="updatePriority('средний')"
                    >
                        Средний
                    </button>
                    <button 
                        class="priority-btn priority-high" 
                        :class="{ active: currentTask.priority === 'высокий' }"
                        @click="updatePriority('высокий')"
                    >
                        Высокий
                    </button>
                </div>
            </div>

            <!-- Срок и исполнитель -->
            <div class="detail-row">
                <div class="detail-item">
                    <h6>Крайний срок</h6>
                    <p class="deadline-text">{{ currentTask.deadline || 'Срок не установлен' }}</p>
                </div>
                <div class="detail-item">
                    <h6>Исполнитель</h6>
                    <p class="executor-text">{{ currentTask.executor || 'Не назначен' }}</p>
                </div>
            </div>

            <!-- Кнопки действий -->
            <div class="detail-buttons">
                <button class="btn-gray btn-md" @click="closeMainModal">Закрыть</button>
                <button class="btn-black btn-md" @click="openDeleteModal">Удалить</button>
            </div>
        </div>
    </div>

    <ProblemDelete 
        v-if="showDeleteModal" 
        @close="closeDeleteModal"
        @confirm="confirmDelete"
    />
</template>

<style scoped>
hr {
    height: 2px;
    border: none;
    background-color: var(--border-color);
    width: 100%;
}

.detail-container {
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

.detail-window {
    width: 700px;
    padding: 1.5rem;
    border-radius: 5px;
    background-color: white;
    display: flex;
    flex-direction: column;
    gap: 20px;
}

h6 {
    font-size: 0.8rem;
    color: var(--text-color);
    margin-bottom: 0.5rem;
    font-weight: 500;
}

.detail-title {
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.detail-title h2 {
    font-size: 1.5rem;
    font-weight: 700;
    color: black;
    margin: 0;
}

.detail-header {
    margin-bottom: 10px;
}

.detail-header > p {
    font-size: 1.4rem;
    font-weight: 600;
    color: black;
    margin: 0;
}

.detail-desc > p {
    color: #666;
    line-height: 1.5;
    margin: 0;
}

.detail-section {
    display: flex;
    flex-direction: column;
    gap: 10px;
}

.detail-row {
    display: flex;
    gap: 50px;
}

.detail-item {
    flex: 1;
}

.deadline-text,
.executor-text {
    font-weight: 500;
    color: black;
    margin: 0;
}

/* Кнопки статуса */
.status-buttons {
    display: flex;
    gap: 10px;
}

.status-btn.active {
    background: black;
    color: white;
    border-color: black;
}

/* Кнопки приоритета */
.priority-buttons {
    display: flex;
    gap: 10px;
}

.priority-btn {
    padding: 8px 16px;
    border: 2px solid;
    border-radius: 6px;
    background: white;
    font-size: 0.9rem;
    font-weight: 500;
    cursor: pointer;
    transition: all 0.3s ease;
    flex: 1;
}

.priority-low {
    border-color: #28a745;
    color: #28a745;
}

.priority-medium {
    border-color: #ffc107;
    color: #ffc107;
}

.priority-high {
    border-color: #dc3545;
    color: #dc3545;
}

.priority-btn:hover {
    opacity: 0.8;
}

.priority-btn.active {
    color: white;
}

.priority-low.active {
    background: #28a745;
    border-color: #28a745;
}

.priority-medium.active {
    background: #ffc107;
    border-color: #ffc107;
    color: black;
}

.priority-high.active {
    background: #dc3545;
    border-color: #dc3545;
}

/* Кнопки действий */
.detail-buttons {
    display: flex;
    justify-content: end;
    align-items: center;
    border-top: 2px solid var(--border-color);
    padding-top: 15px;
    gap: 15px;
}

.btn-delete, 
.btn-close {
    font-size: 1rem;
    border-radius: 5px;
    padding: 10px 14px;
    transition: all 0.2s ease;
    cursor: pointer;
}

.btn-delete {
    color: white;
    background-color: black;
    border: 2px solid black;
}

.btn-delete:hover {
    background-color: white;
    color: black;
}

.btn-close {
    background-color: white;
    border: 2px solid var(--border-color);
}

.btn-close:hover {
    background-color: var(--bg-color);
}

.close-btn {
    background: none;
    border: none;
    font-size: 1.5rem;
    color: black;
    cursor: pointer;
    padding: 0;
    display: flex;
    align-items: center;
    justify-content: center;
}
</style>