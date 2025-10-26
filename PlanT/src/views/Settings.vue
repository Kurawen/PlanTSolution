<script>
export default {
    name: 'SettingsPage',
    data() {
        return {
            user: {
                avatar: '',
                username: 'Анастейша Стил',
                email: 'editor50@mail.ru',
                phone: '+7 (456) 13-34-45'
            },
            password: {
                current: '',
                new: '',
                confirm: ''
            },
            showModal: false
        }
    },
    methods: {
        changeAvatar() {
            // Логика изменения аватара
            console.log('Смена аватара')
        },
        saveChanges() {
            // Логика сохранения изменений
            console.log('Сохранение изменений:', {
                user: this.user,
                password: this.password
            })
        },
        logout() {
            // Логика выхода из аккаунта
            console.log('Выход из аккаунта')
        },
        openModal() {
            this.showModal = true
        },
        closeModal() {
            this.showModal = false
        }
    }
}
</script>

<template>
    <div class="settings-container">
        <div class="settings-window">
            <!-- Заголовок -->
            <h1 class="settings-title">Настройки профиля</h1>

            <!-- Основная информация -->
            <section class="settings-section">
                <h2 class="section-title">Основная информация</h2>
                
                <!-- Аватар -->
                <div class="avatar-section">
                    <div class="avatar-container">
                        <div class="avatar-placeholder">
                            <span class="avatar-text">{{ user.username.charAt(0) }}</span>
                        </div>
                        <button class="avatar-change-btn" @click="changeAvatar">
                            Изменить аватар
                        </button>
                    </div>
                </div>

                <!-- Формы основной информации -->
                <div class="form-grid">
                    <div class="form-group">
                        <label class="form-label">Имя пользователя</label>
                        <div class="input-with-value">
                            <span class="value-text">{{ user.username }}</span>
                        </div>
                    </div>

                    <div class="form-group">
                        <label class="form-label">Электронная почта</label>
                        <input 
                            type="email" 
                            class="form-input"
                            v-model="user.email"
                            placeholder="editor50@mail.ru"
                        >
                    </div>

                    <div class="form-group">
                        <label class="form-label">Номер телефона</label>
                        <input 
                            type="tel" 
                            class="form-input"
                            v-model="user.phone"
                            placeholder="+7 (456) 13-34-45"
                        >
                    </div>
                </div>
            </section>

            <!-- Разделитель -->
            <div class="section-divider"></div>

            <!-- Смена пароля -->
            <section class="settings-section">
                <h2 class="section-title">Изменить пароль</h2>
                
                <div class="form-grid">
                    <div class="form-group">
                        <label class="form-label">Текущий пароль</label>
                        <input 
                            type="password" 
                            class="form-input"
                            v-model="password.current"
                            placeholder="Введите текущий пароль"
                        >
                    </div>

                    <div class="form-group">
                        <label class="form-label">Новый пароль</label>
                        <input 
                            type="password" 
                            class="form-input"
                            v-model="password.new"
                            placeholder="Введите новый пароль"
                        >
                    </div>

                    <div class="form-group">
                        <label class="form-label">Подтвердите новый пароль</label>
                        <input 
                            type="password" 
                            class="form-input"
                            v-model="password.confirm"
                            placeholder="Повторите новый пароль"
                        >
                    </div>
                </div>
            </section>

            <!-- Кнопки действий -->
            <div class="action-buttons">
                <button class="btn-save" @click="saveChanges">
                    Сохранить изменения
                </button>
                <button class="btn-logout" @click="logout">
                    Выйти из аккаунта
                </button>
            </div>
        </div>

        <!-- Модальное окно в разработке -->
        <DevelopingModalWindow v-if="showModal" @close="closeModal"/>
    </div>
</template>

<style scoped>
.settings-container {
    display: flex;
    justify-content: center;
    padding: 2rem;
    min-height: calc(100vh - 200px);
}

.settings-window {
    border-radius: 12px;
    padding: 2rem;
    width: 100%;
    max-width: 600px;
    background-color: white;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
}

.settings-title {
    font-size: 2rem;
    font-weight: 700;
    color: #1a1a1a;
    margin: 0 0 2rem 0;
    text-align: center;
}

.settings-section {
    margin-bottom: 2rem;
}

.section-title {
    font-size: 1.3rem;
    font-weight: 600;
    color: #333;
    margin: 0 0 1.5rem 0;
}

.section-divider {
    height: 1px;
    background-color: #e1e5e9;
    margin: 2rem 0;
}

/* Аватар */
.avatar-section {
    margin-bottom: 2rem;
}

.avatar-container {
    display: flex;
    align-items: center;
    gap: 1.5rem;
}

.avatar-placeholder {
    width: 80px;
    height: 80px;
    border-radius: 50%;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    display: flex;
    align-items: center;
    justify-content: center;
    border: 3px solid white;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.avatar-text {
    font-size: 2rem;
    font-weight: 600;
    color: white;
}

.avatar-change-btn {
    background: none;
    border: 2px solid #007bff;
    color: #007bff;
    padding: 0.5rem 1rem;
    border-radius: 8px;
    font-size: 0.9rem;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.3s ease;
}

.avatar-change-btn:hover {
    background-color: #007bff;
    color: white;
}

/* Формы */
.form-grid {
    display: flex;
    flex-direction: column;
    gap: 1.5rem;
}

.form-group {
    display: flex;
    flex-direction: column;
    gap: 0.5rem;
}

.form-label {
    font-size: 0.9rem;
    font-weight: 600;
    color: #333;
    margin-bottom: 0.25rem;
}

.form-input {
    padding: 0.75rem 1rem;
    border: 2px solid #e1e5e9;
    border-radius: 8px;
    font-size: 1rem;
    transition: all 0.3s ease;
    background-color: white;
}

.form-input:focus {
    outline: none;
    border-color: #007bff;
    box-shadow: 0 0 0 3px rgba(0, 123, 255, 0.1);
}

.form-input::placeholder {
    color: #999;
}

/* Поле с текстом вместо инпута */
.input-with-value {
    padding: 0.75rem 1rem;
    border: 2px solid #e1e5e9;
    border-radius: 8px;
    background-color: #f8f9fa;
    cursor: not-allowed;
}

.value-text {
    font-size: 1rem;
    color: #333;
    font-weight: 500;
}

/* Кнопки действий */
.action-buttons {
    display: flex;
    flex-direction: column;
    gap: 1rem;
    margin-top: 2rem;
}

.btn-save {
    background-color: #007bff;
    color: white;
    border: none;
    padding: 1rem 2rem;
    border-radius: 8px;
    font-size: 1rem;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.3s ease;
}

.btn-save:hover {
    background-color: #0056b3;
    box-shadow: 0 4px 12px rgba(0, 123, 255, 0.3);
}

.btn-logout {
    background-color: white;
    color: #dc3545;
    border: 2px solid #dc3545;
    padding: 1rem 2rem;
    border-radius: 8px;
    font-size: 1rem;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.3s ease;
}

.btn-logout:hover {
    background-color: #dc3545;
    color: white;
    box-shadow: 0 4px 12px rgba(220, 53, 69, 0.3);
}

</style>