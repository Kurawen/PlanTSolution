<script>
export default {
    name: 'NotificationsProblems',
    props: {
        title: {
            type: String,
            required: true
        },
        status: {
            type: String,
            default: ''
        },
        assignee: {
            type: String,
            default: ''
        },
        dueDate: {
            type: String,
            default: ''
        },
        priority: {
            type: String,
            default: 'medium'
        }
    },
    methods: {
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
    <div class="task-cont">
        <div class="task-main">
            <h6 class="task-title">{{ title }}</h6>
            <div class="task-details">
                <div class="task-meta">
                    <span class="task-assignee" v-if="assignee">{{ assignee }}</span>
                    <span class="task-date" v-if="dueDate">{{ dueDate }}</span>
                </div>
                <div class="task-status">
                    <span class="task-priority" :class="getPriorityClass(priority)">
                        {{ priority === 'high' ? 'Высокий' : priority === 'medium' ? 'Средний' : 'Низкий' }}
                    </span>
                    <span class="task-status-text" v-if="status">{{ status }}</span>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
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
</style>