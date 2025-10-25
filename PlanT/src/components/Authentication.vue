<script>
export default {
    name: 'AuthForm',
    props: {
        mode: {
            type: String,
            default: 'login', // 'login' или 'register'
            validator: (value) => ['login', 'register'].includes(value)
        }
    },
    emits: ['submit', 'switch-mode'],
    data() {
        return {
            email: '',
            password: '',
            loading: false
        }
    },
    computed: {
        title() {
            return this.mode === 'login' ? 'Вход' : 'Регистрация'
        },
        buttonText() {
            return this.mode === 'login' ? 'Войти в аккаунт' : 'Создать аккаунт'
        },
        switchText() {
            return this.mode === 'login' 
                ? 'Нет аккаунта? Зарегистрироваться' 
                : 'Уже есть аккаунт? Войти'
        },
        switchMode() {
            return this.mode === 'login' ? 'register' : 'login'
        }
    },
    methods: {
        submitForm() {
        this.loading = true
        this.$emit('submit', {
            email: this.email,
            password: this.password,
            mode: this.mode
        })
        // Сброс loading будет происходить в родительском компоненте
        },
        switchForm() {
        this.$emit('switch-mode', this.switchMode)
        }
    }
}
</script>

<template>
    <div class="auth-container">

        <div class="auth-fields">
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
                    >
                </div>
        
                <button 
                    type="submit" 
                    class="auth-button"
                    :disabled="loading"
                >
                    {{ loading ? 'Загрузка...' : buttonText }}
                </button>
            </form>
    
            <button 
                @click="switchForm" 
                class="switch-button"
                type="button"
                >
                {{ switchText }}
            </button>
    
            <button class="guest-button" @click="$emit('guest-login')">
                Продолжить без регистрации
            </button>
        </div>
        <div class="img-cont">
            <img src="../assets/flower-log-reg.png" alt="" class="img-flower">
        </div>
    </div>
</template>

<style scoped>
* {
    /* border: 1px solid red; */

}

.auth-container {
    display: flex;
    justify-content: space-around;
    max-width: 100%;
    width: 800px;
    margin: 0 auto;
    padding: 2rem;
    background: white;
    border-radius: 12px;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
    /* gap: 80px; */
}

.auth-fields {
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

.auth-logo {
    font-size: 2.5rem;
    font-weight: 700;
    color: black;
    font-family: var(--text-header);
    margin-bottom: 0.5rem;
}

.auth-title {
    font-size: 1.5rem;
    color: #333;
    font-weight: 600;
}

.auth-form {
    display: flex;
    flex-direction: column;
    width: 100%;
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

.form-input::placeholder {
    color: #999;
}

.auth-button {
    max-width: 100%;
    background-color: black;
    color: white;
    border: none;
    padding: 0.9rem;
    border: 2px solid black;
    border-radius: 5px;
    font-size: 1.1rem;
    font-weight: 600;
    cursor: pointer;
    margin-bottom: 1rem;
    transition: background-color 0.3s;
}

.auth-button:hover {
    background-color: white;
    color: black;
}

.auth-button:disabled {
    background: #bdc3c7;
    cursor: not-allowed;
}

.switch-button {
    width: 100%;
    background: transparent;
    color: black;
    border: 2px solid black;
    padding: 0.75rem;
    border-radius: 5px;
    font-weight: 600;
    cursor: pointer;
    margin-bottom: 1rem;
    transition: all 0.3s;
}

.switch-button:hover {
    background: var(--bg-color);
}

.guest-button {
    width: 100%;
    background: transparent;
    color: #666;
    border: 2px solid #ddd;
    padding: 0.75rem;
    border-radius: 5px;
    font-weight: 500;
    cursor: pointer;
    transition: all 0.3s;
}

.guest-button:hover {
    background: #f8f9fa;
    border-color: #999;
}
</style>