<script setup>
import { ref, computed } from 'vue'
import SquadOtdel from './SquadOtdel.vue'
import SquadRoleCreate from './SquadRoleCreate.vue'

const showSquadOtdel = ref(false)
const showAssignRoles = ref(false)

const openSquadOtdelModal = () => {
    showSquadOtdel.value = true
}

const closeSquadOtdelModal = () => {
    showSquadOtdel.value = false
}

const openRoleCreateModal = () => {
    showAssignRoles.value = true
}

const closeRoleCreateModal = () => {
    showAssignRoles.value = false
}

const props = defineProps({
    title: {
        type: String,
        default: "Название группы"
    },
    description: {
        type: String,
        default: "Описание группы"
    },
    memberCount: {
        type: Number,
        default: 0
    },
})

const squadDetails = {
    name: props.title,
    description: props.description,
    memberCount: props.memberCount,
    members: [
        { name: 'Иван Петров', role: 'Руководитель проекта', avatar: 'ИП' },
        { name: 'Мария Смирнова', role: 'Ведущий аналитик', avatar: 'МС' },
        { name: 'Алексей Волков', role: 'Разработчик', avatar: 'АВ' },
        { name: 'Елена Новикова', role: 'Дизайнер', avatar: 'ЕН' }
    ],
    files: [
        { name: 'Проектная документация v2.0', type: 'DOCX', date: '15 марта 2024 г.', icon: '📄' },
        { name: 'План маркетинговой кампании Q2', type: 'PDF', date: '10 марта 2024 г.', icon: '📊' },
        { name: 'Отчет о прогрессе разработки', type: 'XLSX', date: '08 марта 2024 г.', icon: '📈' }
    ]
}
</script>

<template>
    <div class="squad-obj">
        <h2 class="squad-title">{{ title }}</h2>
        <hr class="hrr">
        <p class="squad-desc">{{ description }}</p>
        <div class="squad-count">
            <img src="../assets/team.svg" alt="команда" class="team-icon">
            <p class="squad-count-number">{{ memberCount }} участников</p>
        </div>
        <div class="squad-buttons">
            <button class="btn-gray btn-gray-md" @click="openSquadOtdelModal">Подробнее</button>
            <button class="btn-gray btn-gray-md" @click="openRoleCreateModal">Назначить роли</button>
        </div>
    </div>

    <SquadOtdel 
        v-if="showSquadOtdel" 
        @close="closeSquadOtdelModal"
        :squad-data="squadDetails"
    />

    <SquadRoleCreate
        v-if="showAssignRoles" 
        @close="closeRoleCreateModal"
    />
</template>

<style scoped>
.hrr {
    color: var(--border-color);
    height: 1px;
    border: none;
    background-color: var(--border-color);
    width: 100%;
    margin: 0;
}

.squad-obj {
    display: flex;
    flex-direction: column;
    border: 1px solid var(--border-color);
    border-radius: 5px;
    padding: 15px;
    margin: 10px;
    max-width: 100%;
    width: max-content;
    background-color: white;
    gap: 12px;
}

.squad-title {
    font-size: 1.2rem;
    font-weight: bold;
    margin: 0;
}

.squad-desc {
    font-size: 0.9rem;
    color: black;
    margin: 0;
}

.squad-buttons {
    display: flex;
    align-items: center;
    justify-content: space-between;
}

.squad-but {
    background-color: white;
    color: black;
    font-size: 0.9rem;
    font-weight: 500;
    border: 1px solid var(--border-color);
    border-radius: 5px;
    padding: 12px 25px;
    cursor: pointer;
}

.squad-but:hover {
    background-color: #f5f5f5;
}

.squad-count {
    display: flex;
    align-items: center;
    justify-content: start;
    gap: 10px;
}

.squad-count-number {
    font-size: 0.9rem;
    margin: 0;
}

.team-icon {
    max-width: 20px;
    height: auto;
}
</style>