import { createApp } from 'vue'
import App from './App.vue'
import router from './router/index.js'

// const App = createApp(App)
// App.mount('#app')

createApp(App).use(router).mount('#app')