<script setup>
import { ref } from 'vue'

const props = defineProps({
    squadData: {
        type: Object,
        required: true
    }
})

const emit = defineEmits(['close'])

// Состояния для модальных окон внутри SquadOtdel
const showInviteModal = ref(false)
const showUploadModal = ref(false)
const showEditModal = ref(false)

const closeModal = () => {
    emit('close')
}

const openInviteModal = () => showInviteModal.value = true
const openUploadModal = () => showUploadModal.value = true
const openEditModal = () => showEditModal.value = true

const closeInnerModals = () => {
    showInviteModal.value = false
    showUploadModal.value = false
    showEditModal.value = false
}
</script>

<template>
    <div class="modal-overlay">
        <div class="squad-container" @click.stop>
            <div class="squad-window">
                <!-- Заголовок -->
                <div class="squad-header">
                    <h1>{{ squadData.name }}</h1>
                    <button class="close-btn" @click="closeModal">×</button>
                </div>
                
                <!-- Описание -->
                <p class="squad-description">{{ squadData.description }}</p>

                <!-- Участники -->
                <div class="section">
                    <h2>Участники</h2>
                    <div class="members-grid">
                        <div 
                            v-for="member in squadData.members" 
                            :key="member.name" 
                            class="member-card"
                            >
                            <div class="member-avatar"><img src="../assets/kotya.jpg" alt="" class="member-avatar"></div>
                            <div class="member-info">
                                <h3>{{ member.name }}</h3>
                                <p>{{ member.role }}</p>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Файлы -->
                <div class="section">
                    <h2>Файлы</h2>
                    <div class="files-list">
                        <div 
                            v-for="file in squadData.files" 
                            :key="file.name" 
                            class="file-item"
                            >
                            <div class="file-info">
                                <h3>{{ file.name }}</h3>
                                <p>{{ file.type }} | Загружено: {{ file.date }}</p>
                            </div>
                            
                            <a href="../assets/hanna.jpg" download=""><button class="download-btn" download>↓</button></a>
                        </div>
                    </div>
                </div>

                <!-- Действия -->
                <div class="actions-section">
                    <h2>Действия</h2>
                    <div class="actions-grid">
                        <button class="btn-black btn-md" @click="openInviteModal">
                            <span>Пригласить участника</span>
                        </button>
                        <button class="btn-black btn-md" @click="openUploadModal">
                            <span>Загрузить файл</span>
                        </button>
                        <button class="btn-black btn-md" @click="openEditModal">
                            <span>Редактировать группу</span>
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>

  <!-- Модальные окна для действий -->
  <div v-if="showInviteModal" class="modal-overlay inner" @click="closeInnerModals">
        <div class="modal" @click.stop>
            <h3>Пригласить участника</h3>
            <input type="email" placeholder="Введите email участника" class="modal-input">
            <div class="modal-actions">
                <button class="btn secondary" @click="closeInnerModals">Отмена</button>
                <button class="btn primary">Отправить приглашение</button>
            </div>
        </div>
  </div>

  <div v-if="showUploadModal" class="modal-overlay inner" @click="closeInnerModals">
        <div class="modal" @click.stop>
            <h3>Загрузить файл</h3>
            <div class="upload-area">
                <span class="upload-icon">📁</span>
                <p>Перетащите файл сюда или нажмите для выбора</p>
            </div>
            <div class="modal-actions">
                <button class="btn secondary" @click="closeInnerModals">Отмена</button>
                <button class="btn primary">Загрузить</button>
            </div>
        </div>
  </div>

  <div v-if="showEditModal" class="modal-overlay inner" @click="closeInnerModals">
        <div class="modal" @click.stop>
            <h3>Редактировать группу</h3>
            <input type="text" placeholder="Название отдела" class="modal-input" :value="squadData.name">
            <textarea placeholder="Описание отдела" class="modal-textarea">{{ squadData.description }}</textarea>
            <div class="modal-actions">
                <button class="btn secondary" @click="closeInnerModals">Отмена</button>
                <button class="btn primary">Сохранить изменения</button>
            </div>
        </div>
    </div>
</template>

<style scoped>
.modal-overlay {
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

.modal-overlay.inner {
    z-index: 2000;
}

.squad-container {
    max-width: 90%;
    max-height: 90%;
    overflow-y: auto;
}

.squad-window {
    background: white;
    border-radius: 12px;
    padding: 30px;
    width: 1000px;
    max-width: 100%;
}

.squad-window > p {
    color: gray;
    font-size: 1.1rem;
    line-height: 1.6;
    margin-bottom: 30px;
}

.squad-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 20px;
}

.squad-header > h1 {
    color: black;
}

.close-btn {
    background: none;
    border: none;
    font-size: 2rem;
    cursor: pointer;
    padding: 5px;
}

.section {
    margin-bottom: 30px;
}

.section h2 {
    color: #2c3e50;
    font-size: 1.5rem;
    margin-bottom: 15px;
}

.members-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
    gap: 15px;
}

.member-card {
    display: flex;
    align-items: center;
    padding: 15px;
    background: #f8f9fa;
    border-radius: 8px;
    border: 1px solid #e9ecef;
}

.member-avatar {
    width: 40px;
    height: 40px;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    font-weight: bold;
    margin-right: 15px;
    font-size: 0.9rem;
}

.member-info h3 {
    margin: 0 0 5px 0;
    color: #2c3e50;
    font-size: 1rem;
}

.member-info p {
    margin: 0;
    color: #6c757d;
    font-size: 0.8rem;
}

.files-list {
    display: flex;
    flex-direction: column;
    gap: 10px;
}

.file-item {
    display: flex;
    align-items: center;
    padding: 12px;
    background: #f8f9fa;
    border-radius: 6px;
    border: 1px solid #e9ecef;
}

.file-icon {
    font-size: 1.2rem;
    margin-right: 12px;
}

.file-info {
    flex: 1;
}

.file-info h3 {
    margin: 0 0 4px 0;
    color: #2c3e50;
    font-size: 0.9rem;
}

.file-info p {
    margin: 0;
    color: #6c757d;
    font-size: 0.8rem;
}

.download-btn {
    background: none;
    border: none;
    font-size: 1.1rem;
    cursor: pointer;
    padding: 6px;
    border-radius: 4px;
}

.actions-section {
    border-top: 2px solid #e9ecef;
    padding-top: 20px;
}

.actions-section > h2 {
    margin-bottom: 15px;
}

.actions-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
    gap: 12px;
}

.action-btn {
    display: flex;
    flex-direction: column;
    align-items: center;
    padding: 15px;
    background: white;
    border: 2px solid black;
    border-radius: 8px;
    cursor: pointer;
    transition: all 0.3s ease;
    text-align: center;
}

.action-btn:hover {
    background-color: var(--bg-color);
}

.action-icon {
    font-size: 1.5rem;
    margin-bottom: 8px;
}

.action-btn span:last-child {
    font-weight: 500;
    font-size: 0.9rem;
}

/* Стили для внутренних модальных окон */
.modal {
    background: white;
    padding: 20px;
    border-radius: 8px;
    width: 450px;
    height: 270px;
    max-width: 90%;
}

.modal h3 {
    margin: 0 0 15px 0;
}

.modal-input,
.modal-textarea {
    width: 100%;
    padding: 10px;
    border: 1px solid #ddd;
    border-radius: 4px;
    margin-bottom: 15px;
}

.modal-textarea {
    padding-bottom: 50px;
}

.modal-actions {
    display: flex;
    gap: 10px;
    justify-content: flex-end;
}

.btn {
    padding: 8px 16px;
    border: none;
    border-radius: 4px;
    cursor: pointer;
}

.btn.primary {
    background: #667eea;
    color: white;
}

.btn.secondary {
    background: #6c757d;
    color: white;
}

.upload-area {
    border: 2px dashed #ddd;
    border-radius: 6px;
    padding: 30px;
    text-align: center;
    margin-bottom: 15px;
    cursor: pointer;
}
</style>