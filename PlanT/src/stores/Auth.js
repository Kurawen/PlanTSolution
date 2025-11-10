// import { ref, computed } from 'vue'
// import { defineStore } from 'pinia'

// export const useAuthStore = defineStore('auth', () => {
//     const token = ref(localStorage.getItem('authToken') || '')
//     const userData = ref({
//         email: localStorage.getItem('userEmail') || '',
//         userId: localStorage.getItem('userId') || ''
//     })

//     const isAuthenticated = computed(() => !!token.value)

//     const login = (newToken, email, userId) => {
//         token.value = newToken
//         userData.value = { email, userId }
//         localStorage.setItem('authToken', newToken)
//         localStorage.setItem('userEmail', email)
//         localStorage.setItem('userId', userId)
//     }

//     const logout = () => {
//         token.value = ''
//         userData.value = { email: '', userId: '' }
//         localStorage.removeItem('authToken')
//         localStorage.removeItem('userEmail')
//         localStorage.removeItem('userId')
//     }

//     return {
//         token,
//         userData,
//         isAuthenticated,
//         login,
//         logout
//     }
// })