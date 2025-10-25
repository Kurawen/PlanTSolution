<script setup>
import { ref } from 'vue'

// Данные для календаря
const currentDate = ref(new Date())
const daysOfWeek = ['Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб', 'Вс']

// Данные для задач
const tasks = ref([
  {
    id: 1,
    title: 'Подготовить квартальный отчет',
    deadline: '15.09.2025',
    status: 'в работе',
    priority: 'высокий',
    executor: 'Анна Иванова',
    completed: false
  },
  {
    id: 2,
    title: 'Провести еженедельную встречу команды',
    deadline: '10.09.2025',
    status: 'завершено',
    priority: 'средний',
    executor: 'Дмитрий Петров',
    completed: true
  },
  {
    id: 3,
    title: 'Исследовать новые рыночные тенденции',
    deadline: '20.09.2025',
    status: 'к выполнению',
    priority: 'низкий',
    executor: 'Елена Смирнова',
    completed: false
  },
  {
    id: 4,
    title: 'Обновить спецификации продукта',
    deadline: '12.09.2025',
    status: 'в работе',
    priority: 'высокий',
    executor: 'Сергей Козлов',
    completed: false
  },
  {
    id: 5,
    title: 'Подготовить презентацию для клиента',
    deadline: '18.09.2025',
    status: 'в работе',
    priority: 'средний',
    executor: 'Мария Николаева',
    completed: false
  }
])

const activeTab = ref('all')
const selectedTasks = ref(new Set())

// Функции для календаря
const getDaysInMonth = (date) => {
  return new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate()
}

const getFirstDayOfMonth = (date) => {
  return new Date(date.getFullYear(), date.getMonth(), 1).getDay()
}

const generateCalendarDays = () => {
  const daysInMonth = getDaysInMonth(currentDate.value)
  const firstDay = getFirstDayOfMonth(currentDate.value)
  const days = []
  
  // Пустые ячейки для начала месяца
  for (let i = 0; i < (firstDay === 0 ? 6 : firstDay - 1); i++) {
    days.push(null)
  }
  
  // Дни месяца
  for (let i = 1; i <= daysInMonth; i++) {
    days.push(i)
  }
  
  return days
}

const calendarDays = ref(generateCalendarDays())

const prevMonth = () => {
  currentDate.value = new Date(currentDate.value.getFullYear(), currentDate.value.getMonth() - 1, 1)
  calendarDays.value = generateCalendarDays()
}

const nextMonth = () => {
  currentDate.value = new Date(currentDate.value.getFullYear(), currentDate.value.getMonth() + 1, 1)
  calendarDays.value = generateCalendarDays()
}

const getMonthName = () => {
  const months = [
    'Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь',
    'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'
  ]
  return months[currentDate.value.getMonth()]
}

// Функции для задач
const toggleTaskSelection = (taskId) => {
  if (selectedTasks.value.has(taskId)) {
    selectedTasks.value.delete(taskId)
  } else {
    selectedTasks.value.add(taskId)
  }
}

const toggleAllTasks = () => {
  if (selectedTasks.value.size === tasks.value.length) {
    selectedTasks.value.clear()
  } else {
    tasks.value.forEach(task => selectedTasks.value.add(task.id))
  }
}

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
    case 'к выполнению': return 'status-todo'
    default: return ''
  }
}
</script>

<template>
    <div class="problems-container">
        <div class="problems">
            <!-- Заголовок -->
            <div class="problems-title">
                <h1>Мои задачи</h1>
                <button class="problem-create">+ Новая задача</button>
            </div>

            <!-- Календарь -->
            <section class="problems-calendar">
                <div class="calendar-header">
                    <button class="calendar-nav" @click="prevMonth">&lt;</button>
                    <h2 class="calendar-month">{{ getMonthName() }}</h2>
                    <button class="calendar-nav" @click="nextMonth">&gt;</button>
                </div>
                
                <div class="calendar-grid">
                    <!-- Дни недели -->
                    <div 
                        v-for="day in daysOfWeek" 
                        :key="day" 
                        class="calendar-day-header"
                    >
                        {{ day }}
                    </div>
                    
                    <!-- Дни месяца -->
                    <div 
                        v-for="(day, index) in calendarDays" 
                        :key="index" 
                        class="calendar-day"
                        :class="{ 'empty': day === null }"
                    >
                        {{ day }}
                    </div>
                </div>
            </section>

            <!-- Список задач -->
            <section class="problems-list">
                <div class="problems-type">
                    <button 
                        :class="{ active: activeTab === 'all' }"
                        @click="activeTab = 'all'"
                    >
                        Все задачи
                    </button>
                    <button 
                        :class="{ active: activeTab === 'personal' }"
                        @click="activeTab = 'personal'"
                    >
                        Личные задачи
                    </button>
                    <button 
                        :class="{ active: activeTab === 'group' }"
                        @click="activeTab = 'group'"
                    >
                        Задачи по группам
                    </button>
                </div>

                <div class="problems-table">
                    <!-- Заголовок таблицы -->
                    <div class="problems-bar">
                        <div class="checkbox-column">
                            <input 
                                type="checkbox" 
                                :checked="selectedTasks.size === tasks.length"
                                @change="toggleAllTasks"
                            />
                        </div>
                        <div class="task-column">ЗАДАЧА</div>
                        <div class="deadline-column">СРОК</div>
                        <div class="status-column">СТАТУС</div>
                        <div class="priority-column">ПРИОРИТЕТ</div>
                        <div class="executor-column">ИСПОЛНИТЕЛЬ</div>
                        <div class="actions-column">ДЕЙСТВИЯ</div>
                    </div>

                    <!-- Строки с задачами -->
                    <div 
                        v-for="task in tasks" 
                        :key="task.id" 
                        class="task-row"
                    >
                        <div class="checkbox-column">
                            <input 
                                type="checkbox" 
                                :checked="selectedTasks.has(task.id)"
                                @change="toggleTaskSelection(task.id)"
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
                            <button class="action-btn">⋮</button>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</template>

<style scoped>
.problems-container {
    display: flex;
    justify-content: center;
    min-height: 100vh;
    background-color: #f8f9fa;
    padding: 20px;
}

.problems {
    width: 100%;
    max-width: 1200px;
    background: white;
    border-radius: 12px;
    padding: 2rem;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.problems-title {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 2rem;
}

.problems-title h1 {
    font-size: 2rem;
    color: #333;
    margin: 0;
}

.problem-create {
    background-color: black;
    color: white;
    padding: 12px 24px;
    border: 1px solid black;
    border-radius: 6px;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.3s;
}

.problem-create:hover {
    background-color: #333;
}

/* Календарь */
.problems-calendar {
    margin-bottom: 2rem;
    border: 1px solid #e0e0e0;
    border-radius: 8px;
    padding: 1rem;
}

.calendar-header {
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin-bottom: 1rem;
}

.calendar-month {
    margin: 0;
    font-size: 1.2rem;
    font-weight: 600;
    color: #333;
}

.calendar-nav {
    background: none;
    border: 1px solid #ddd;
    border-radius: 4px;
    padding: 0.5rem 1rem;
    cursor: pointer;
    font-size: 1rem;
}

.calendar-nav:hover {
    background: #f5f5f5;
}

.calendar-grid {
    display: grid;
    grid-template-columns: repeat(7, 1fr);
    gap: 4px;
}

.calendar-day-header {
    text-align: center;
    font-weight: 600;
    color: #666;
    padding: 0.5rem;
    font-size: 0.9rem;
}

.calendar-day {
    text-align: center;
    padding: 0.75rem 0.5rem;
    border-radius: 4px;
    cursor: pointer;
    transition: background-color 0.2s;
}

.calendar-day:hover:not(.empty) {
    background: #f0f0f0;
}

.calendar-day.empty {
    background: transparent;
    cursor: default;
}

/* Табы */
.problems-type {
    display: flex;
    gap: 1rem;
    margin-bottom: 1.5rem;
    border-bottom: 1px solid #e0e0e0;
    padding-bottom: 1rem;
}

.problems-type button {
    background: none;
    border: none;
    padding: 0.75rem 1.5rem;
    border-radius: 6px;
    cursor: pointer;
    font-weight: 500;
    transition: all 0.3s;
    color: #666;
}

.problems-type button.active {
    background: black;
    color: white;
}

.problems-type button:not(.active):hover {
    background: #f5f5f5;
    color: #333;
}

/* Таблица задач */
.problems-table {
    border: 1px solid #e0e0e0;
    border-radius: 8px;
    overflow: hidden;
}

.problems-bar {
    display: grid;
    grid-template-columns: 50px 2fr 1fr 1fr 1fr 1.5fr 80px;
    gap: 1rem;
    padding: 1rem;
    background: #f8f9fa;
    font-weight: 600;
    color: #333;
    border-bottom: 1px solid #e0e0e0;
    align-items: center;
}

.task-row {
    display: grid;
    grid-template-columns: 50px 2fr 1fr 1fr 1fr 1.5fr 80px;
    gap: 1rem;
    padding: 1rem;
    border-bottom: 1px solid #f0f0f0;
    align-items: center;
    transition: background-color 0.2s;
}

.task-row:hover {
    background: #f8f9fa;
}

.task-row:last-child {
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
    padding: 0.25rem 0.75rem;
    border-radius: 20px;
    font-size: 0.85rem;
    font-weight: 500;
}

/* Статусы */
.status-completed {
    background: #e8f5e8;
    color: #2e7d32;
}

.status-in-progress {
    background: #e3f2fd;
    color: #1565c0;
}

.status-todo {
    background: #fff3e0;
    color: #ef6c00;
}

/* Приоритеты */
.priority-high {
    background: #ffebee;
    color: #c62828;
}

.priority-medium {
    background: #fff3e0;
    color: #ef6c00;
}

.priority-low {
    background: #e8f5e8;
    color: #2e7d32;
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