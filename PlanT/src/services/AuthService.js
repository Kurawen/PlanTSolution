import { ref } from 'vue'

const API = 'http://localhost:5047'
const token = ref(localStorage.getItem('authToken') || '')

// Функция регистрации
export const register = async (email, password) => {
    try {
        const response = await fetch(`${API}/register`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            email: email,
            password: password
        })
        })

        if (!response.ok) {
        const errorData = await response.json()
        throw new Error(errorData.detail || 'Ошибка регистрации')
        }

        const data = await response.json()
        return { success: true, data }
        
    } catch (error) {
        return { success: false, error: error.message }
    }
}

// Функция входа
export const login = async (email, password) => {
    try {
        const response = await fetch(`${API}/login`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            email: email,
            password: password
        })
        })

        if (!response.ok) {
        const errorData = await response.json()
        throw new Error(errorData.detail || 'Ошибка входа')
        }

        const data = await response.json()
        
        // Сохраняем токен
        token.value = data.token
        localStorage.setItem('authToken', data.token)
        localStorage.setItem('userEmail', data.email)
        localStorage.setItem('userId', data.userId)
        
        return { success: true, data }
        
    } catch (error) {
        return { success: false, error: error.message }
    }
}

// Функция выхода
export const logout = () => {
    token.value = ''
    localStorage.removeItem('authToken')
    localStorage.removeItem('userEmail')
    localStorage.removeItem('userId')
}

// Функция проверки авторизации
export const isAuthenticated = () => {
    return !!token.value
}

// Получить токен
export const getToken = () => {
    return token.value
}

// Получить данные пользователя
export const getUserData = () => {
    return {
        email: localStorage.getItem('userEmail'),
        userId: localStorage.getItem('userId')
    }
}

// Функция для авторизованных запросов
export const authFetch = async (url, options = {}) => {
    const headers = {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${getToken()}`,
        ...options.headers
    }

    const response = await fetch(`${API_BASE_URL}${url}`, {
        ...options,
        headers
    })

    if (response.status === 401) {
        logout()
        throw new Error('Требуется авторизация')
    }

    return response
}