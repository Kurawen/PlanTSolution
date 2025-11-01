<script setup>
import { defineProps, defineEmits } from 'vue'

const props = defineProps({
    chatNotifications: {
        type: Array,
        default: () => []
    },
    taskNotifications: {
        type: Array,
        default: () => []
    }
})

const emit = defineEmits(['close'])

const getPriorityClass = (priority) => {
    const classes = {
        'high': 'priority-high',
        'medium': 'priority-medium',
        'low': 'priority-low'
    };
    return classes[priority] || 'priority-medium';
}

const close = () => {
    emit('close');
}
</script>

<template>
    <div class="notifications-container">
        <div class="notifications-content" @click.stop>
            <div class="notifications-header">
                <h2>Уведомления</h2>
                <button class="close-btn" @click="close">×</button>
            </div>
            
            <div class="notifications-body">
                <!-- Секция чат-уведомлений -->
                <div class="notif-chat" v-if="chatNotifications.length > 0">
                    <h3 class="section-title">Сообщения</h3>
                    <div class="chat-notifications">
                        <div 
                            v-for="notification in chatNotifications" 
                            :key="notification.id" 
                            class="chat-cont"
                        >
                            <div class="chat-avatar">
                                <div v-if="notification.avatar" class="chat-img">
                                    <img :src="notification.avatar" :alt="notification.title">
                                </div>
                                <div v-else class="avatar-placeholder">
                                    {{ notification.title.charAt(0) }}
                                </div>
                            </div>
                            <div class="chat-content">
                                <div class="chat-header">
                                    <h4 class="chat-title">{{ notification.title }}</h4>
                                    <span class="chat-time">{{ notification.time }}</span>
                                </div>
                                <p class="chat-message">{{ notification.message }}</p>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Секция уведомлений о задачах -->
                <div class="notif-task" v-if="taskNotifications.length > 0">
                    <h3 class="section-title">Задачи</h3>
                    <div class="task-notifications">
                        <div 
                            v-for="task in taskNotifications" 
                            :key="task.id" 
                            class="task-cont"
                        >
                            <div class="task-main">
                                <h4 class="task-title">{{ task.title }}</h4>
                                <div class="task-details">
                                    <div class="task-meta">
                                        <span class="task-assignee">{{ task.assignee }}</span>
                                        <span class="task-date">Срок: {{ task.dueDate }}</span>
                                    </div>
                                    <div class="task-status">
                                        <span 
                                            class="task-priority" 
                                            :class="getPriorityClass(task.priority)"
                                        >
                                            {{ task.priority }}
                                        </span>
                                        <span v-if="task.status" class="task-status-text">
                                            {{ task.status }}
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Если уведомлений нет -->
                <div v-if="chatNotifications.length === 0 && taskNotifications.length === 0" class="no-notifications">
                    <p>У вас пока нет уведомлений</p>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.notifications-container {
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

.notifications-content {
    background-color: white;
    border-radius: 12px;
    width: 600px;
    max-width: 90vw;
    max-height: 80vh;
    overflow-y: auto;
    animation: slideIn 0.3s ease-out;
}

.notifications-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 24px 24px 16px 24px;
    border-bottom: 1px solid #e1e5e9;
}

.notifications-header h2 {
    font-family: 'Inter', sans-serif;
    font-size: 1.5rem;
    font-weight: 700;
    color: #1a1a1a;
    margin: 0;
}

.close-btn {
    background: none;
    border: none;
    font-size: 1.5rem;
    color: #666;
    cursor: pointer;
    padding: 4px;
    width: 32px;
    height: 32px;
    display: flex;
    align-items: center;
    justify-content: center;
    border-radius: 6px;
    transition: all 0.2s ease;
}

.close-btn:hover {
    background-color: #f5f5f5;
    color: #333;
}

.notifications-body {
    padding: 16px 24px 24px 24px;
}

.section-title {
    font-family: 'Inter', sans-serif;
    font-size: 1.1rem;
    font-weight: 600;
    color: #333;
    margin: 0 0 16px 0;
}

.notif-chat,
.notif-task {
    margin-bottom: 24px;
}

.chat-notifications {
    display: flex;
    flex-direction: column;
    gap: 12px;
}

.task-notifications {
    display: flex;
    flex-direction: column;
    gap: 12px;
}

/* Стили для чат уведомлений */
.chat-cont {
    display: flex;
    align-items: flex-start;
    gap: 12px;
    padding: 16px;
    border-radius: 8px;
    transition: background-color 0.2s ease;
    cursor: pointer;
    border: 1px solid var(--border-color);
}

.chat-cont:hover {
    background-color: var(--bg2-color);
}

.chat-avatar {
    flex-shrink: 0;
    width: 40px;
    height: 40px;
}

.chat-img {
    width: 100%;
    height: 100%;
    border-radius: 50%;
    object-fit: cover;
}

.avatar-placeholder {
    width: 100%;
    height: 100%;
    border-radius: 50%;
    background-color: black;
    color: white;
    display: flex;
    align-items: center;
    justify-content: center;
    font-weight: 600;
    font-size: 0.9rem;
}

.chat-content {
    flex: 1;
    min-width: 0;
}

.chat-header {
    display: flex;
    justify-content: space-between;
    align-items: flex-start;
    margin-bottom: 4px;
    gap: 12px;
}

.chat-title {
    font-family: 'Inter', sans-serif;
    font-size: 0.9rem;
    font-weight: 600;
    color: #1a1a1a;
    margin: 0;
    line-height: 1.3;
}

.chat-time {
    font-family: 'Inter', sans-serif;
    font-size: 0.75rem;
    color: #666;
    white-space: nowrap;
    flex-shrink: 0;
}

.chat-message {
    font-family: 'Inter', sans-serif;
    font-size: 0.85rem;
    color: #666;
    margin: 0;
    line-height: 1.4;
}

/* Стили для задач */
.task-cont {
    padding: 16px;
    border-radius: 8px;
    border: 1px solid var(--border-color);
    background-color: white;
    transition: all 0.2s ease;
    cursor: pointer;
}

.task-cont:hover {
    background-color: var(--bg2-color);
    box-shadow: 0 2px 8px rgba(0, 123, 255, 0.1);
}

.task-main {
    display: flex;
    flex-direction: column;
    gap: 8px;
}

.task-title {
    font-family: 'Inter', sans-serif;
    font-size: 0.9rem;
    font-weight: 600;
    color: #1a1a1a;
    margin: 0;
    line-height: 1.3;
}

.task-details {
    display: flex;
    justify-content: space-between;
    align-items: center;
    gap: 12px;
}

.task-meta {
    display: flex;
    flex-direction: column;
    gap: 4px;
    flex: 1;
}

.task-assignee {
    font-family: 'Inter', sans-serif;
    font-size: 0.8rem;
    color: #666;
    font-weight: 500;
}

.task-date {
    font-family: 'Inter', sans-serif;
    font-size: 0.75rem;
    color: #999;
}

.task-status {
    display: flex;
    flex-direction: column;
    align-items: flex-end;
    gap: 4px;
    flex-shrink: 0;
}

.task-priority {
    padding: 4px 8px;
    border-radius: 12px;
    font-family: 'Inter', sans-serif;
    font-size: 0.7rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.5px;
}

.priority-high {
    background-color: #dc3545;
    color: white;
}

.priority-medium {
    background-color: #ffc107;
    color: #212529;
}

.priority-low {
    background-color: #28a745;
    color: white;
}

.task-status-text {
    font-family: 'Inter', sans-serif;
    font-size: 0.75rem;
    color: #666;
    font-style: italic;
}

.no-notifications {
    text-align: center;
    padding: 40px;
    color: #666;
}

.no-notifications p {
    font-family: 'Inter', sans-serif;
    font-size: 1rem;
    margin: 0;
}

/* Анимация */
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

/* Адаптивность */
@media (max-width: 640px) {
    .modal-content {
        padding: 20px;
        margin: 20px;
        width: calc(100% - 40px);
    }
    
    .task-details {
        flex-direction: column;
        align-items: flex-start;
        gap: 8px;
    }
    
    .task-status {
        align-items: flex-start;
    }
}

/* Скроллбар */
.modal-content::-webkit-scrollbar {
    width: 6px;
}

.modal-content::-webkit-scrollbar-track {
    background: #f1f1f1;
    border-radius: 3px;
}

.modal-content::-webkit-scrollbar-thumb {
    background: #c1c1c1;
    border-radius: 3px;
}
</style>