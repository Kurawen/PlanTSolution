<script setup>
import { ref, computed } from 'vue'

const emit = defineEmits(['close', 'group-created'])

const activeTab = ref('create')
const groupName = ref('')
const groupDescription = ref('')
const selectedGroup = ref('')

// Моковые данные для демонстрации
const availableGroups = ref([
    { id: 1, name: 'Отдел проектирования' },
    { id: 2, name: 'Отдел маркетинга' },
    { id: 3, name: 'Разработчики' },
    { id: 4, name: 'Дизайнеры' }
])

const inviteLink = computed(() => {
    if (!selectedGroup.value) return 'link.ai'
    const group = availableGroups.value.find(g => g.id === selectedGroup.value)
    return `link.ai/group/${group?.name.toLowerCase().replace(/\s+/g, '-')}`
})

const createGroup = () => {
    if (!groupName.value.trim()) {
        alert('Введите название группы')
        return
    }

    const newGroup = {
        id: Date.now(),
        name: groupName.value,
        description: groupDescription.value,
        memberCount: 1
    }

    emit('group-created', newGroup)
    emit('close')
}

const copyLink = async () => {
    try {
        await navigator.clipboard.writeText(inviteLink.value)
        alert('Ссылка скопирована в буфер обмена!')
    } catch (err) {
        // Fallback для старых браузеров
        const textArea = document.createElement('textarea')
        textArea.value = inviteLink.value
        document.body.appendChild(textArea)
        textArea.select()
        document.execCommand('copy')
        document.body.removeChild(textArea)
        alert('Ссылка скопирована!')
    }
}
</script>

<template>
    <div class="modal-overlay" @click.self="$emit('close')">
        <div class="modal-content">
        <div class="modal-header">
            <h1>Управление группами</h1>
            <p>Создайте новую группу или пригласите пользователей в существующую.</p>
        </div>

        <div class="tabs">
            <button 
            class="tab-button" 
            :class="{ active: activeTab === 'create' }"
            @click="activeTab = 'create'"
            >
            Создать группу
            </button>
            <button 
            class="tab-button" 
            :class="{ active: activeTab === 'invite' }"
            @click="activeTab = 'invite'"
            >
            Пригласить пользователей
            </button>
        </div>

        <div v-if="activeTab === 'create'" class="create-group-form">
            <div class="form-group">
                <label for="groupName" class="form-label">Название группы</label>
                <input
                    id="groupName"
                    v-model="groupName"
                    type="text"
                    class="form-input"
                    placeholder="Отдел проектирования"
                />
            </div>

            <div class="form-group">
                <label for="groupDescription" class="form-label">Описание</label>
                <textarea
                    id="groupDescription"
                    v-model="groupDescription"
                    class="form-textarea"
                    placeholder="Краткое описание группы"
                    rows="3"
                ></textarea>
            </div>

            <div class="form-actions">
                <button class="cancel-button" @click="$emit('close')">Отмена</button>
                <button class="create-button" @click="createGroup">Создать</button>
            </div>
        </div>

        <div v-if="activeTab === 'invite'" class="invite-form">
            <div class="form-group">
            <label class="form-label">Группа</label>
            <select v-model="selectedGroup" class="form-select">
                <option value="">Выберите группу</option>
                <option 
                v-for="group in availableGroups" 
                :key="group.id" 
                :value="group.id"
                >
                {{ group.name }}
                </option>
            </select>
            </div>

            <div class="form-group">
            <label class="form-label">Ссылка</label>
            <div class="link-container">
                <input
                :value="inviteLink"
                type="text"
                class="form-input link-input"
                readonly
                />
            </div>
            </div>

            <div class="form-actions">
                <button class="cancel-button" @click="$emit('close')">Отмена</button>
                <button class="copy-main-button" @click="copyLink">Скопировать</button>
            </div>
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
    border-radius: 12px;
    padding: 2rem;
    width: 90%;
    max-width: 500px;
    max-height: 90vh;
    overflow-y: auto;
    box-shadow: 0 10px 30px rgba(0, 0, 0, 0.3);
}

.modal-header {
    text-align: center;
    margin-bottom: 1.5rem;
}

.modal-header h1 {
    margin: 0 0 0.5rem 0;
    font-size: 1.5rem;
    color: #333;
}

.modal-header p {
    margin: 0;
    color: #666;
    font-size: 0.9rem;
}

.tabs {
    display: flex;
    border-bottom: 1px solid #e0e0e0;
    margin-bottom: 1.5rem;
}

.tab-button {
    flex: 1;
    padding: 0.75rem 1rem;
    background: none;
    border: none;
    border-bottom: 2px solid transparent;
    font-weight: 500;
    cursor: pointer;
    transition: all 0.3s;
}

.tab-button.active {
    border-bottom-color: #000;
    color: #000;
}

.tab-button:not(.active) {
    color: #666;
}

.tab-button:hover:not(.active) {
    color: #333;
    background: #f5f5f5;
}

.form-group {
    margin-bottom: 1.5rem;
}

.form-label {
    display: block;
    margin-bottom: 0.5rem;
    font-weight: 600;
    color: #333;
}

.form-input,
.form-textarea,
.form-select {
    width: 100%;
    padding: 0.75rem;
    border: 1px solid #ddd;
    border-radius: 6px;
    font-size: 1rem;
    box-sizing: border-box;
}

.form-input:focus,
.form-textarea:focus,
.form-select:focus {
    outline: none;
    border-color: #000;
}

.form-textarea {
    resize: vertical;
    min-height: 80px;
    font-family: inherit;
}

.link-container {
    display: flex;
    gap: 0.5rem;
}

.link-input {
    flex: 1;
}

.copy-button {
    padding: 0.75rem 1rem;
    background: #f8f9fa;
    border: 1px solid #ddd;
    border-radius: 6px;
    cursor: pointer;
    white-space: nowrap;
}

.copy-button:hover {
    background: #e9ecef;
}

.form-actions {
    display: flex;
    gap: 1rem;
    justify-content: flex-end;
    margin-top: 2rem;
}

.cancel-button {
    padding: 0.75rem 1.5rem;
    background: white;
    border: 1px solid #ddd;
    border-radius: 6px;
    cursor: pointer;
    font-weight: 500;
}

.cancel-button:hover {
    background: #f8f9fa;
}

.create-button {
    padding: 0.75rem 1.5rem;
    background: #000;
    color: white;
    border: 1px solid #000;
    border-radius: 6px;
    cursor: pointer;
    font-weight: 500;
}

.create-button:hover {
    background: #333;
}

.copy-main-button {
    padding: 0.75rem 1.5rem;
    background: #000;
    color: white;
    border: 1px solid #000;
    border-radius: 6px;
    cursor: pointer;
    font-weight: 500;
}

.copy-main-button:hover {
    background: #333;
}
</style>