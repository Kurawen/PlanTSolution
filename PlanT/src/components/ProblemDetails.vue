<script>
import { ref } from 'vue'
import ProblemDelete from './ProblemDelete.vue';

export default {
    name: 'ProblemDetails',
    components: {
        ProblemDelete
    },
    setup(props, { emit }) {
        const showDeleteModal = ref(false)

        const openDeleteModal = () => {
            showDeleteModal.value = true
        }

        const closeDeleteModal = () => {
            showDeleteModal.value = false
        }

        const closeMainModal = () => {
            emit('close')
        }

        return {
            showDeleteModal,
            openDeleteModal,
            closeDeleteModal,
            closeMainModal
        }
    }
}
</script>

<template>
    <div class="detail-container">
        <div class="detail-window">
            <div class="detail-title">
                <h2>Детали задачи</h2>
                <button class="close-btn" @click="closeMainModal">×</button>
            </div>
            <hr>
            <div class="detail-header">
                <h6>Заголовок</h6>
                <p>Подготовить квартальный отчет</p>
            </div>
            <div class="detail-desc">
                <h6>Описание</h6>
                <p>Заполни сделай и туда-сюда</p>
            </div>
            <div class="detail-stat">
                <div class="detail-date">
                    <h6>Крайний срок</h6>
                    <p>завтра в 7</p>
                </div>
                <div class="detail-status">
                    <h6>Приоритет</h6>
                    <p>высокий</p>
                </div>
            </div>
            <div class="detail-creator">
                <h6>Исполнитель</h6>
                <p>Анастейша стил</p>
            </div>
            <div class="detail-buttons">
                <button class="btn-delete" @click="openDeleteModal">Удалить</button>
                <button class="btn-close" @click="closeMainModal">Закрыть</button>
            </div>
        </div>
    </div>

    <ProblemDelete 
        v-if="showDeleteModal" 
        @close="closeDeleteModal"
        @confirm="closeDeleteModal"
    />
</template>

<style scoped>

hr {
    height: 2px;
    border: none;
    background-color: var(--border-color);
    width: 100%;
}
.detail-container {
    position: fixed;
    display: flex;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    justify-content: center;
    align-items: center;
    z-index: 1000;
}

.detail-window {
    width: 700px;
    padding: 15px;
    border-radius: 5px;
    background-color: white;
    display: flex;
    flex-direction: column;
    gap: 20px;
}

h6 {
    font-size: 0.8rem;
    color: var(--text-color);
    margin-bottom: 0.5rem;
    font-weight: 500;
}

.detail-title {
    display: flex;
    justify-content: space-between;
}

.detail-header {
    margin-bottom: 20px;
}

.detail-header > p {
    font-size: 1.4rem;
}

.detail-desc {

}

.detail-stat {
    display: flex;
    gap: 50px;
}

.detail-date {

}

.detail-status {

}

.detail-status > p {
    color: white;
    padding: 5px 10px;
    background-color: var(--status-overdue);
} 

.detail-creator {

}

.detail-buttons {
    display: flex;
    justify-content: end;
    align-items: center;
    border-top: 2px solid var(--border-color);
    padding-top: 15px;
    gap: 15px;
}

.btn-delete, 
.btn-close {
    font-size: 1rem;
    border-radius: 5px;
    padding: 10px 14px;
    transition: all 0.2s ease;
}

.btn-delete {
    color: white;
    background-color: black;
    border: 2px solid black;
    
}

.btn-delete:hover {
    background-color: white;
    color: black;
}

.btn-close {
    background-color: white;
    border: 2px solid var(--border-color);
}

.btn-close:hover {
    background-color: var(--bg-color);
}

.close-btn {
    background: none;
    border: none;
    font-size: 1.5rem;
    color: black;
    cursor: pointer;
    padding: 0;
    display: flex;
    align-items: center;
    justify-content: center;
}


</style>