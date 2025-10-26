<script>
export default {
    name: 'CreateProblem',
    props: {
        isVisible: Boolean
    },
    data() {
        return {
            task: {
                title: '',
                description: '',
                dueDate: '',
                assignee: '',
                priority: ''
            }
        }
    },
    methods: {
        close() {
            this.$emit('close');
        },
        saveTask() {
            this.$emit('save', { ...this.task });
            this.close();
        },
        cancel() {
            this.close();
        }
    }
}
</script>

<template>
    <div id="problem-container">
        <div id="problem-window">
            <div class="problem-top">
                <div class="problem-title">
                    <h2>Создать задачу</h2>
                    <p>Заполните детали новой задачи</p>
                </div>
                <button class="close-btn" @click="close">×</button>
            </div>
            
            <div class="problem-forms">
                <div class="form-group">
                    <label for="name" class="problem-label">Название задачи</label>
                    <input 
                        class="problem-input" 
                        placeholder="Введите название задачи" 
                        type="text" 
                        id="name" 
                        v-model="task.title" 
                        required
                    >
                </div>
                
                <div class="form-group">
                    <label for="desc" class="problem-label">Описание</label>
                    <textarea 
                        class="problem-textarea" 
                        placeholder="Опишите задачу подробно" 
                        id="desc" 
                        v-model="task.description" 
                        required
                        rows="4"
                    ></textarea>
                </div>
                
                <div class="form-group">
                    <label for="date" class="problem-label">Срок выполнения</label>
                    <input 
                        class="problem-input" 
                        placeholder="Выберите дату" 
                        type="date" 
                        id="date" 
                        v-model="task.dueDate" 
                        required
                    >
                </div>
                
                <div class="form-group">
                    <label for="exec" class="problem-label">Исполнитель</label>
                    <input 
                        class="problem-input" 
                        placeholder="Назначить исполнителя" 
                        type="text" 
                        id="exec" 
                        v-model="task.assignee" 
                        required
                    >
                </div>
                
                <div class="form-group">
                    <label for="prior" class="problem-label">Приоритет</label>
                    <select 
                        class="problem-select" 
                        id="prior" 
                        v-model="task.priority" 
                        required
                    >
                        <option value="" disabled selected>Установить приоритет</option>
                        <option value="low">Низкий</option>
                        <option value="medium">Средний</option>
                        <option value="high">Высокий</option>
                    </select>
                </div>
            </div>
            
            <div class="problem-actions">
                <button class="btn-cancel" @click="cancel">Отмена</button>
                <button class="btn-save" @click="saveTask">Сохранить</button>
            </div>
        </div>
    </div>
</template>

<style scoped>
#problem-container {
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

#problem-window {
    padding: 30px;
    background-color: white;
    border-radius: 5px;
    display: flex;
    flex-direction: column;
    max-width: 100%;
    width: 480px;
    gap: 24px;
    box-shadow: 0 10px 25px rgba(0, 0, 0, 0.2);
}

.problem-top {
    display: flex;
    justify-content: space-between;
    align-items: flex-start;
    gap: 20px;
    padding-bottom: 16px;
    border-bottom: 1px solid #e1e5e9;
}

.problem-title h2 {
    font-weight: 700;
    font-size: 1.5rem;
    color: #1a1a1a;
    margin: 0 0 4px 0;
}

.problem-title p {
    font-size: 0.9rem;
    color: #666;
    margin: 0;
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

.problem-forms {
    display: flex;
    flex-direction: column;
    gap: 20px;
}

.form-group {
    display: flex;
    flex-direction: column;
    gap: 8px;
}

.problem-label {
    font-size: 0.9rem;
    font-weight: 600;
    color: #333;
    margin-bottom: 4px;
}

.problem-input,
.problem-textarea,
.problem-select {
    width: 100%;
    padding: 12px 16px;
    border: 2px solid #e1e5e9;
    border-radius: 8px;
    font-size: 0.9rem;
    transition: all 0.2s ease;
    box-sizing: border-box;
}

.problem-input:focus,
.problem-textarea:focus,
.problem-select:focus {
    outline: none;
    border-color: #007bff;
    box-shadow: 0 0 0 3px rgba(0, 123, 255, 0.1);
}

.problem-textarea {
    resize: vertical;
    min-height: 80px;
    line-height: 1.4;
}

.problem-select {
    background-color: white;
    cursor: pointer;
}

.problem-actions {
    display: flex;
    gap: 12px;
    justify-content: flex-end;
    padding-top: 16px;
    border-top: 1px solid #e1e5e9;
}

.btn-cancel,
.btn-save {
    padding: 12px 24px;
    border: none;
    border-radius: 5px;
    font-size: 0.9rem;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.2s ease;
}

.btn-cancel {
    background-color: white;
    color: #333;
    border: 2px solid var(--border-color);
}

.btn-cancel:hover {
    background-color: var(--bg-color);
}

.btn-save {
    background-color: black;
    color: white;
    border: 2px solid black;
}

.btn-save:hover {
    background-color: white;
    color: black;
    transform: translateY(-1px);
}

.btn-save:active {
    transform: translateY(0);
}

.problem-input::placeholder,
.problem-textarea::placeholder {
    color: #999;
}

.problem-select option[disabled] {
    color: #999;
}
</style>