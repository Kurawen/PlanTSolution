<script setup>
import { ref, onMounted } from 'vue'
import DevelopingModalWindow from '@/components/DevelopingModalWindow.vue'
import ProblemLine from '@/components/ProblemLine.vue'
import SquadObj from '@/components/SquadObj.vue'
import SquadCreate from '@/components/SquadCreate.vue'
import SquadOtdel from '@/components/SquadOtdel.vue'

const showGroupModal = ref(false)


const openGroupModal = () => {
    showGroupModal.value = true
}

const closeGroupModal = () => {
    showGroupModal.value = false
}



const handleGroupCreated = (newGroup) => {
    console.log('Новая группа создана:', newGroup)
    // Здесь можно добавить логику для обновления списка групп
    // Например: squads.value.push(newGroup)
}

// Реактивные данные
const squads = ref([
    {
        id: 1,
        title: "Отдел маркетинга",
        description: "Официальная группа для команды маркетинга, координирования компаний и отслеживания эффективности. Обсуждение идей и стратегий.",
        memberCount: 15
    },
    {
        id: 2,
        title: "Разработчики",
        description: "Группа для разработчиков компании. Обмен опытом, код-ревью и планирование спринтов.",
        memberCount: 8
    },
    {
        id: 3,
        title: "Дизайнеры",
        description: "Сообщество дизайнеров для обсуждения трендов, проектов и инструментов дизайна.",
        memberCount: 6
    },
    {
        id: 4,
        title: "Аналитики",
        description: "Группа аналитиков для работы с данными, построения отчетов и анализа метрик.",
        memberCount: 12
    }
])

const isLoading = ref(false)
const showModal = ref(false)

// Функции для обработки событий
const handleSquadDetails = (squadTitle) => {
    console.log('Подробнее о:', squadTitle)
    // Здесь может быть навигация или открытие модального окна
}

const handleSquadEdit = (squadTitle) => {
    console.log('Редактирование:', squadTitle)
    // Логика редактирования группы
}

const handleCreateInvite = () => {
    openModal()
}

const loadSquads = async () => {
    isLoading.value = true
    try {
        // const response = await fetch('/api/squads')
        // squads.value = await response.json()
    } catch (error) {
        console.error('Ошибка загрузки данных:', error)
    } finally {
        isLoading.value = false
    }
}

const openModal = () => {
    showModal.value = true
}

const closeModal = () => {
    showModal.value = false
}

// Загрузка данных при монтировании
onMounted(() => {
    // loadSquads() // раскомментировать для загрузки с API
})
</script>

<template>
    <div class="squad-container">
        <main class="squad-window">
            <div class="squad-header">
                <h1>Ваши группы</h1>
                <p>Просматривайте и управляйте всеми группами, в которых вы состоите.</p>
            </div>
            
            <!-- Блок с группами -->
            <div class="squad-content">
                <div v-if="isLoading" class="loading-state">
                    Загрузка групп...
                </div>
                
                <div v-else-if="squads.length === 0" class="empty-state">
                    <p>У вас пока нет групп. Создайте первую!</p>
                </div>
                
                <div v-else class="squads-grid">
                    <SquadObj 
                        v-for="squad in squads" 
                        :key="squad.id"
                        :title="squad.title"
                        :description="squad.description"
                        :member-count="squad.memberCount"
                        @details="handleSquadDetails"
                        @edit="handleSquadEdit"
                    />
                </div>
            </div>

            <div class="squad-actions">
                <button class="squad-but-join" @click="openGroupModal">
                    Создать или пригласить
                </button>
            </div>
        </main>
    </div>

    <DevelopingModalWindow v-if="showModal" @close="closeModal"/>

    <SquadCreate
        v-if="showGroupModal"
        @close="closeGroupModal"
        @group-created="handleGroupCreated"
    />
</template>

<style scoped>
.squad-container {
    display: flex;
    justify-content: center;
}

.squad-window {
    border-radius: 8px;
    display: flex;
    flex-direction: column;
    width: 100%;
    max-width: 1200px;
    padding: 30px;
    gap: 25px;
    background-color: white;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.squad-header {
    text-align: center;
    margin-bottom: 10px;
}

.squad-header h1 {
    margin-bottom: 10px;
    font-size: 2rem;
    color: black;
}

.squad-header p {
    margin: 0;
    color: #666;
    font-size: 1.1rem;
}

.squad-content {
    min-height: 300px;
}

.squads-grid {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(340px, 1fr));
    gap: 20px;
    justify-items: center;
}

.empty-state {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 200px;
    text-align: center;
}

.empty-state p {
    font-size: 1.1rem;
    color: #666;
    margin: 0;
}

.squad-actions {
    display: flex;
    justify-content: center;
    margin-top: 20px;
}

.squad-but-join {
    border: 1px solid black;
    background-color: black;
    color: white;
    border-radius: 6px;
    padding: 0.75rem 2rem;
    font-weight: 600;
    font-size: 1rem;
    cursor: pointer;
    transition: all 0.3s ease;
}

.squad-but-join:hover {
    background-color: white;
    color: black;
}

</style>