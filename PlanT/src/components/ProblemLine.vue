<!-- ProblemLine.vue -->
<script setup>
defineProps({
    task: {
        type: Object,
        required: true
    },
    selected: {
        type: Boolean,
        default: false
    }
})

const emit = defineEmits(['toggle-selection', 'open-details'])

const getPriorityClass = (priority) => {
    switch (priority) {
        case 'высокий': return 'priority-high'
        case 'средний': return 'priority-medium'
        case 'низкий': return 'priority-low'
        default: return ''
    }
}

const getStatusClass = (status) => {
    switch (status) {
        case 'завершено': return 'status-completed'
        case 'в работе': return 'status-in-progress'
        case 'не начато': return 'status-todo'
        default: return ''
    }
}
</script>

<template>
    <div class="problem-line">
        <div class="checkbox-column">
            <input 
                type="checkbox" 
                :checked="selected"
                @change="$emit('toggle-selection')"
            />
        </div>
        <div class="task-column">
            <span :class="{ 'completed': task.completed }">
                {{ task.title }}
            </span>
        </div>
        <div class="deadline-column">{{ task.deadline }}</div>
        <div class="status-column">
            <span :class="getStatusClass(task.status)">
                {{ task.status }}
            </span>
        </div>
        <div class="priority-column">
            <span :class="getPriorityClass(task.priority)">
                {{ task.priority }}
            </span>
        </div>
        <div class="executor-column">{{ task.executor }}</div>
        <div class="actions-column">
            <button class="action-btn" @click="$emit('open-details')">⋮</button>
        </div>
    </div>
</template>

<style scoped>
.problem-line {
    display: grid;
    grid-template-columns: 50px 2fr 1fr 1fr 1fr 1.5fr 80px;
    gap: 1rem;
    padding: 1rem;
    border-bottom: 1px solid #f0f0f0;
    align-items: center;
    transition: background-color 0.2s;
}

.problem-line:hover {
    background: #f8f9fa;
}

.problem-line:last-child {
    border-bottom: none;
}

/* Колонки */
.checkbox-column {
    display: flex;
    justify-content: center;
}

.task-column {
    font-weight: 500;
}

.deadline-column,
.executor-column {
    color: #666;
}

.status-column span,
.priority-column span {
    padding: 10px 14px;
    border-radius: 5px;
    font-size: 0.9rem;
    font-weight: 600;
}

/* Статусы */
.status-completed,
.status-in-progress,
.status-todo {
    color: white;
}
.status-completed {
    background: #b5ed5a;
    background: linear-gradient(90deg, rgb(181, 252, 67) 0%, rgb(23, 204, 240) 100%);
}

.status-in-progress {
    background: #f50a0a;
    background: linear-gradient(90deg, rgb(245, 39, 39) 0%, rgb(255, 52, 241) 100%);
}

.status-todo {
    background: #2ea1ff;
    background: linear-gradient(90deg, rgb(32, 105, 164) 0%, rgb(194, 60, 221) 100%);
}

/* Приоритеты */
.priority-high,
.priority-medium,
.priority-low {
    color: white;
}

.priority-high {
    background-color: var(--prior-high);
}

.priority-medium {
    background-color: var(--prior-middle);
}

.priority-low {
    background-color: var(--prior-low);
}

/* Завершенные задачи */
.completed {
    text-decoration: line-through;
    color: #999;
}

/* Кнопка действий */
.action-btn {
    background: none;
    border: none;
    font-size: 1.2rem;
    cursor: pointer;
    color: #666;
    padding: 0.25rem;
    border-radius: 4px;
}

.action-btn:hover {
    background: #f0f0f0;
    color: #333;
}
</style>