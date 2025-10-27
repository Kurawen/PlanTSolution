<script>
export default {
    name: 'Notifications',
    props: {
        chatNotifications: {
            type: Array,
            default: () => []
        },
        taskNotifications: {
            type: Array,
            default: () => []
        }
    },
    methods: {
        close() {
            this.$emit('close');
        },
        getPriorityClass(priority) {
            const classes = {
                'high': 'priority-high',
                'medium': 'priority-medium',
                'low': 'priority-low'
            };
            return classes[priority] || 'priority-medium';
        }
    }
}
</script>

<template>
    <div class="notifications-section">
        <div class="notification-window">
            <!-- Заголовок -->
            <div class="top-title">
                <h2 class="notif-title">Уведомления</h2>
                <button class="close-btn" @click="close">×</button>
            </div>
            
            <hr>
            
            <!-- Описание -->
            <p class="notif-desc">Просматривайте уведомления о чатах и задачах.</p>

            <!-- Уведомления чата -->
            <section class="notif-chat" v-if="chatNotifications.length > 0">
                <h3 class="section-title">Уведомления чата</h3>
                
                <div class="chat-notifications">
                    <div 
                        v-for="notification in chatNotifications"
                        :key="notification.id"
                        class="chat-cont"
                    >
                        <div class="chat-avatar">
                            <img :src="notification.avatar" :alt="notification.title" class="chat-img" v-if="notification.avatar">
                            <div class="avatar-placeholder" v-else>{{ notification.title.charAt(0) }}</div>
                        </div>
                        <div class="chat-content">
                            <div class="chat-header">
                                <h6 class="chat-title">{{ notification.title }}</h6>
                                <span class="chat-time">{{ notification.time }}</span>
                            </div>
                            <p class="chat-message">{{ notification.message }}</p>
                        </div>
                    </div>
                </div>
            </section>

            <!-- Уведомления задач -->
            <section class="notif-task" v-if="taskNotifications.length > 0">
                <h3 class="section-title">Уведомления по задачам</h3>
                
                <div class="task-notifications">
                    <div 
                        v-for="task in taskNotifications"
                        :key="task.id"
                        class="task-cont"
                    >
                        <div class="task-main">
                            <h6 class="task-title">{{ task.title }}</h6>
                            <div class="task-details">
                                <div class="task-meta">
                                    <span class="task-assignee" v-if="task.assignee">{{ task.assignee }}</span>
                                    <span class="task-date" v-if="task.dueDate">{{ task.dueDate }}</span>
                                </div>
                                <div class="task-status">
                                    <span class="task-priority" :class="getPriorityClass(task.priority)">
                                        {{ task.priority === 'high' ? 'Высокий' : task.priority === 'medium' ? 'Средний' : 'Низкий' }}
                                    </span>
                                    <span class="task-status-text" v-if="task.status">{{ task.status }}</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            <!-- Сообщение если уведомлений нет -->
            <div v-if="chatNotifications.length === 0 && taskNotifications.length === 0" class="no-notifications">
                <p>Нет новых уведомлений</p>
            </div>
        </div>
    </div>
</template>

<style scoped>
.notifications-section {
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

.notification-window {
    display: flex;
    flex-direction: column;
    background-color: white;
    border-radius: 12px;
    padding: 24px;
    width: 600px;
    max-width: 90vw;
    max-height: 80vh;
    overflow-y: auto;
    gap: 16px;
}

.top-title {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 8px;
}

.notif-title {
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

hr {
    height: 1px;
    border: none;
    background-color: #e1e5e9;
    width: 100%;
    margin: 8px 0;
}

.notif-desc {
    font-family: 'Inter', sans-serif;
    font-size: 0.9rem;
    color: #666;
    margin: 0;
    line-height: 1.4;
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
    display: flex;
    flex-direction: column;
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
    border: 1px solid #e1e5e9;
}

.chat-cont:hover {
    background-color: #f8f9fa;
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
    background-color: #007bff;
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
    border: 1px solid #e1e5e9;
    background-color: #f8f9fa;
    transition: all 0.2s ease;
    cursor: pointer;
}

.task-cont:hover {
    border-color: #007bff;
    background-color: white;
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

/* Адаптивность */
@media (max-width: 640px) {
    .notification-window {
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
.notification-window::-webkit-scrollbar {
    width: 6px;
}

.notification-window::-webkit-scrollbar-track {
    background: #f1f1f1;
    border-radius: 3px;
}

.notification-window::-webkit-scrollbar-thumb {
    background: #c1c1c1;
    border-radius: 3px;
}
</style>