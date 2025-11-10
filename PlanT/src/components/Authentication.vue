<script setup>
import { ref, computed } from 'vue';
import { useRouter } from 'vue-router';
import { register, login } from '@/services/AuthService'

const props = defineProps({
    mode: {
        type: String,
        default: 'login',
        validator: (value) => ['login', 'register'].includes(value)
    }
})

const emit = defineEmits(['submit', 'switch-mode', 'guest-login', 'close'])

const email = ref('')
const password = ref('')
const confirmPassword = ref('')
const loading = ref(false)
const error = ref('')
const router = useRouter()

const title = computed(() => props.mode === 'login' ? 'Вход' : 'Регистрация')
const buttonText = computed(() => props.mode === 'login' ? 'Войти в аккаунт' : 'Создать аккаунт')
const switchText = computed(() => props.mode === 'login' ? 'Нет аккаунта? Зарегистрироваться' : 'Уже есть аккаунт? Войти')
const switchMode = computed(() => props.mode === 'login' ? 'register' : 'login')
const passwordsMatch = computed(() => password.value === confirmPassword.value)

const submitForm = async () => {
    // Валидация для регистрации
    if (props.mode === 'register' && !passwordsMatch.value) {
        error.value = 'Пароли не совпадают'
        return
    }

    if (props.mode === 'register' && password.value.length < 6) {
        error.value = 'Пароль должен быть не менее 6 символов'
        return
    }

    loading.value = true
    error.value = ''

    try {
        if (props.mode === 'register') {
            // Регистрация
            const result = await register(email.value, password.value)
            if (result.success) {
                emit('submit', { 
                    email: email.value, 
                    password: password.value, 
                    mode: props.mode,
                    userData: result.data 
                })
                // Закрываем модальное окно после успешной регистрации
                emit('close')
                router.push('/?message=Регистрация успешна')
            } else {
                error.value = result.error
            }
        } else {
            // Вход
            const result = await login(email.value, password.value)
            if (result.success) {
                emit('submit', { 
                    email: email.value, 
                    password: password.value, 
                    mode: props.mode,
                    userData: result.data 
                })
                // Закрываем модальное окно после успешного входа
                emit('close')
                // Обновляем страницу чтобы шапка обновилась
                window.location.reload()
            } else {
                error.value = result.error
            }
        }
    } catch (err) {
        error.value = 'Произошла ошибка при подключении к серверу'
        console.error('Auth error:', err)
    } finally {
        loading.value = false
    }
}

const switchForm = () => {
    error.value = ''
    email.value = ''
    password.value = ''
    confirmPassword.value = ''
    emit('switch-mode', switchMode.value)
}

const handleGuestLogin = () => {
    emit('guest-login')
    emit('close')
}
</script>

<template>
    <div class="auth-container">
        <div class="auth-window">
            <div class="auth-header">
                <h1 class="auth-logo">PlanT</h1>
                <h2 class="auth-title">{{ title }}</h2>
            </div>
    
            <form @submit.prevent="submitForm" class="auth-form">
                <div class="form-group">
                    <label for="email" class="form-label">Email</label>
                    <input
                    id="email"
                    v-model="email"
                    type="email"
                    class="form-input"
                    placeholder="student74@mail.ru"
                    required
                    :disabled="loading"
                    >
                </div>
        
                <div class="form-group">
                    <label for="password" class="form-label">Пароль</label>
                    <input
                    id="password"
                    v-model="password"
                    type="password"
                    class="form-input"
                    placeholder="Не менее 8 символов"
                    minlength="8"
                    required
                    :disabled="loading"
                    >
                </div>
        
                <div v-if="mode === 'register'" class="form-group">
                    <label for="confirmPassword" class="form-label">Подтвердите пароль</label>
                    <input
                    id="confirmPassword"
                    v-model="confirmPassword"
                    type="password"
                    class="form-input"
                    placeholder="Повторите пароль"
                    minlength="8"
                    required
                    :disabled="loading"
                    >
                </div>

                <div v-if="error" class="error-message">
                    {{ error }}
                </div>

                <button 
                    type="submit" 
                    class="btn-black btn-md full-width"
                    :disabled="loading || (mode === 'register' && !passwordsMatch)"
                >
                    {{ loading ? 'Загрузка...' : buttonText }}
                </button>
            </form>

            <div class="auth-buttons">
                <button 
                    @click="switchForm" 
                    class="btn-gray btn-md full-width"
                    type="button"
                    :disabled="loading"
                >
                    {{ switchText }}
                </button>
        
                <button 
                    class="btn-black btn-md full-width guest-btn" 
                    @click="handleGuestLogin"
                    :disabled="loading"
                >
                    Продолжить без регистрации
                </button>
            </div>
        </div>
        <div class="img-cont">
            <img src="../assets/flower-log-reg.png" alt="" class="img-flower">
        </div>
    </div>
</template>

<style scoped>
.error-message {
    color: #dc3545;
    background: #f8d7da;
    border: 1px solid #f5c6cb;
    padding: 0.75rem;
    border-radius: 4px;
    margin-bottom: 1rem;
    text-align: center;
}

.auth-container {
    display: flex;
    justify-content: space-around;
    max-width: 100%;
    width: 800px;
    margin: 0 auto;
    padding: 2rem;
    background: white;
    border-radius: 5px;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
}

.auth-window {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    width: 350px;
}

.img-cont {
    display: flex;
    max-width: 100%;
    align-items: center;
    justify-content: center;
}

.img-flower {
    width: auto;
    height: 400px;
}

.auth-header {
    text-align: center;
    margin-bottom: 2rem;
}

.auth-header > h1 {
    font-size: 2.5rem;
    font-weight: 700;
    color: black;
    font-family: var(--text-header);
    margin-bottom: 0.5rem;
}

.auth-header > h2 {
    font-size: 1.5rem;
    color: #333;
    font-weight: 600;
}

.auth-form {
    display: flex;
    flex-direction: column;
    width: 100%;
    margin-bottom: 1rem;
}

.auth-buttons {
    display: flex;
    flex-direction: column;
    width: 100%;
    gap: 1rem;
}

.form-group {
    padding: 10px;
    border-radius: 5px;
    margin-bottom: 1rem;
    background-color: #f3f4f6;
    width: 100%;
}

.form-label {
    display: block;
    margin-bottom: 0.5rem;
    margin-left: 0.5rem;
    font-weight: 500;
    color: black;
}

.form-input {
    width: 100%;
    padding: 0.5rem;
    border: none;
    font-size: 1rem;
    background-color: #f3f4f6;
}

.form-input:disabled {
    opacity: 0.6;
    cursor: not-allowed;
}

.form-input::placeholder {
    color: #999;
}

.full-width {
    width: 100%;
    display: block;
}

</style>