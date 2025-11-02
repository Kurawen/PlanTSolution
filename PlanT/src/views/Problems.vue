<script setup>
import { ref, computed } from 'vue'
import ProblemCreate from '@/components/ProblemCreate.vue'
import ProblemDetails from '@/components/ProblemDetails.vue'
import ProblemLine from '@/components/ProblemLine.vue'

const showModal = ref(false)
const showModal1 = ref(false)
const selectedTaskId = ref(null)

const openModal = () => {
    showModal.value = true
}

const closeModal = () => {
    showModal.value = false
}

const openModal1 = (taskId) => {
    selectedTaskId.value = taskId
    showModal1.value = true
}

const closeModal1 = () => {
    showModal1.value = false
    selectedTaskId.value = null
}

// Функция добавления новой задачи
const addNewTask = (newTaskData) => {
    const newTask = {
        id: tasks.value.length + 1,
        title: newTaskData.title,
        deadline: newTaskData.deadline,
        status: newTaskData.status || 'не начато',
        priority: newTaskData.priority || 'средний',
        executor: newTaskData.executor,
        completed: false
    }
    
    tasks.value.push(newTask)
    closeModal()
}

// Функция удаления задачи
const deleteTask = (taskId) => {
    const taskIndex = tasks.value.findIndex(task => task.id === taskId)
    if (taskIndex !== -1) {
        tasks.value.splice(taskIndex, 1)
        // Если удаляем выбранную задачу, закрываем модальное окно
        if (selectedTaskId.value === taskId) {
            closeModal1()
        }
        // Удаляем задачу из выбранных
        selectedTasks.value.delete(taskId)
    }
}

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
        completed: false
    },
    {
        id: 3,
        title: 'Исследовать новые рыночные тенденции',
        deadline: '20.09.2025',
        status: 'не начато',
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

// Добавляем ref для сортировки
const sortBy = ref('deadline')
const sortDirection = ref('asc')

// Функции для сортировки
const sortTasks = (tasks) => {
    return [...tasks].sort((a, b) => {
        let valueA, valueB
        
        switch (sortBy.value) {
            case 'deadline':
                valueA = new Date(a.deadline.split('.').reverse().join('-'))
                valueB = new Date(b.deadline.split('.').reverse().join('-'))
                break
            case 'priority':
                const priorityOrder = { 'высокий': 3, 'средний': 2, 'низкий': 1 }
                valueA = priorityOrder[a.priority] || 0
                valueB = priorityOrder[b.priority] || 0
                break
            case 'status':
                const statusOrder = { 'к выполнению': 1, 'в работе': 2, 'завершено': 3 }
                valueA = statusOrder[a.status] || 0
                valueB = statusOrder[b.status] || 0
                break
            default:
                return 0
        }
        
        if (valueA < valueB) return sortDirection.value === 'asc' ? -1 : 1
        if (valueA > valueB) return sortDirection.value === 'asc' ? 1 : -1
        return 0
    })
}

// Функция для изменения сортировки
const changeSort = (field) => {
    if (sortBy.value === field) {
        sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc'
    } else {
        sortBy.value = field
        sortDirection.value = 'asc'
    }
}

// Вычисляемое свойство для отсортированных задач
const sortedTasks = computed(() => sortTasks(tasks.value))

// Функция для получения иконки сортировки
const getSortIcon = (field) => {
    if (sortBy.value !== field) return '↕️'
    return sortDirection.value === 'asc' ? '↑' : '↓'
}


</script>

<template>
    <div class="problems-container">
        <div class="problems">
            <!-- Заголовок -->
            <div class="problems-title">
                <h1>Мои задачи</h1>
                <button class="btn-black btn-md" @click="openModal">+ Новая задача</button>
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
                        <div class="deadline-column sortable" @click="changeSort('deadline')">
                            СРОК {{ getSortIcon('deadline') }}
                        </div>
                        <div class="status-column sortable" @click="changeSort('status')">
                            СТАТУС {{ getSortIcon('status') }}
                        </div>
                        <div class="priority-column sortable" @click="changeSort('priority')">
                            ПРИОРИТЕТ {{ getSortIcon('priority') }}
                        </div>
                        <div class="executor-column">ИСПОЛНИТЕЛЬ</div>
                        <div class="actions-column">ДЕЙСТВИЯ</div>
                    </div>

                    <!-- Используем компонент ProblemLine для каждой задачи -->
                    <ProblemLine 
                        v-for="task in sortedTasks" 
                        :key="task.id"
                        :task="task"
                        :selected="selectedTasks.has(task.id)"
                        @toggle-selection="toggleTaskSelection(task.id)"
                        @open-details="() => openModal1(task.id)"
                    />
                </div>
            </section>
        </div>
    </div>

    <!-- Модальное окно создания задачи -->
    <ProblemCreate 
        v-if="showModal" 
        @close="closeModal"
        @create-task="addNewTask"
    />
    
    <ProblemDetails 
        v-if="showModal1" 
        @close="closeModal1"
        @delete-task="deleteTask"
        :task-id="selectedTaskId"
        :tasks="tasks"
    />
</template>

<style scoped>
.problems-container {
    display: flex;
    justify-content: center;
}

.problems {
    width: 100%;
    max-width: 1200px;
    background: white;
    border-radius: 5px;
    padding: 2rem;
    margin-bottom: 1rem;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.problems-title {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 2rem;
}

.problems-title h1 {
    font-family: var(--text-header);
    font-weight: 800;
    font-size: 2rem;
    color: black;
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
    border: 1px solid var(--border-color);
    border-radius: 5px;
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
    background-color: white;
    border: none;
    border: 2px solid black;
    padding: 0.75rem 1.5rem;
    border-radius: 6px;
    cursor: pointer;
    font-weight: 500;
    transition: all 0.3s;
    color: black;
}

.problems-type button.active {
    background: black;
    color: white;
}

.problems-type button:not(.active):hover {
    background: var(--bg-color);
    color: #e21d1d;
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
</style>