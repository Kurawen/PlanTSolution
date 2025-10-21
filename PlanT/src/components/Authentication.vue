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
            placeholder="students2@mail.ru"
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

        <div class="auth-divider"></div>

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
</template>

<style scoped>
.auth-container {
    max-width: 400px;
    margin: 0 auto;
    padding: 2rem;
    background: white;
    border-radius: 12px;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
}

.auth-header {
    text-align: center;
    margin-bottom: 2rem;
}

.auth-logo {
    font-size: 2.5rem;
    font-weight: 700;
    color: #2ecc71;
    margin-bottom: 0.5rem;
}

.auth-title {
    font-size: 1.5rem;
    color: #333;
    font-weight: 600;
}

.auth-form {
    margin-bottom: 1.5rem;
}

.form-group {
    margin-bottom: 1.5rem;
}

.form-label {
    display: block;
    margin-bottom: 0.5rem;
    font-weight: 500;
    color: #333;
}

.form-input {
    width: 100%;
    padding: 0.75rem;
    border: 2px solid #e1e5e9;
    border-radius: 8px;
    font-size: 1rem;
    transition: border-color 0.3s;
}

.form-input:focus {
    outline: none;
    border-color: #2ecc71;
}

.form-input::placeholder {
    color: #999;
}

.auth-button {
    width: 100%;
    background: #2ecc71;
    color: white;
    border: none;
    padding: 1rem;
    border-radius: 8px;
    font-size: 1.1rem;
    font-weight: 600;
    cursor: pointer;
    transition: background-color 0.3s;
}

.auth-button:hover:not(:disabled) {
    background: #27ae60;
}

.auth-button:disabled {
    background: #bdc3c7;
    cursor: not-allowed;
}

.auth-divider {
    height: 1px;
    background: #e1e5e9;
    margin: 1.5rem 0;
}

.switch-button {
    width: 100%;
    background: transparent;
    color: #2ecc71;
    border: 2px solid #2ecc71;
    padding: 0.75rem;
    border-radius: 8px;
    font-weight: 600;
    cursor: pointer;
    margin-bottom: 1rem;
    transition: all 0.3s;
}

.switch-button:hover {
    background: #2ecc71;
    color: white;
}

.guest-button {
    width: 100%;
    background: transparent;
    color: #666;
    border: 1px solid #ddd;
    padding: 0.75rem;
    border-radius: 8px;
    font-weight: 500;
    cursor: pointer;
    transition: all 0.3s;
}

.guest-button:hover {
    background: #f8f9fa;
    border-color: #999;
}
</style>