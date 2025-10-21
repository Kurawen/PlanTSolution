<script setup>
const props = defineProps({
hideHeader: {
    type: Boolean,
    default: false
},
hideFooter: {
    type: Boolean,
    default: false
}
})

const emit = defineEmits(['open-auth'])
</script>

<template>
    <div id="app">
        <!-- шапка -->
        <header id="shapka">
            <nav class="navbar">
                <div class="nav-title">
                    <img src="../assets/plant-logo.svg" alt="растение" class="plant-dev">
                    <a href="#" class="nav-logo">PlanT</a>
                </div>
                <a href="#" class="nav-link">Главная</a>
                <div class="notif-account">
                    <img src="../assets/notification.svg" alt="телеграм" id="main-icon" @click="emit()">
                    <button class="login-btn" @click="emit('open-auth', 'login')">Войти в аккаунт</button>
                </div>
            </nav>
        </header>

        <main id="main">
            <slot></slot>
        </main>

        <!-- подвал -->
        <footer class="dno">
            <p>2025</p>
            <p>© PlanT</p>
            <a href="https://t.me/myfavoritejumoreski" target="_blank">
                <img src="../assets/telegram.svg" alt="телеграм" id="main-icon">
            </a>
        </footer>

    </div>
</template>

<style>
* {
    border: 1px solid red;
}

.plant-dev {
    max-width: 50px;
    height: auto;
}

#app {
    min-height: 100vh;
    display: flex;
    flex-direction: column;
}

/* шапка */
#shapka {
    box-shadow: 0 2px 10px rgba(0,0,0,0.1);
    position: sticky;
    top: 0;
    z-index: 100;
}

.navbar {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 1rem 2rem;
    margin: 0 auto;
    font-size: 1.4rem;
    background-color: white;
}

.nav-title {
    display: flex;
    align-items: center;
    gap: 10px;
}

.nav-logo {
    font-size: 1.8rem;
    font-weight: 700;
    font-family: var(--text-header);
    text-decoration: none;
    color: black;
}

.nav-link {
    text-decoration: none;
    color: black;
    font-weight: 500;
    transition: color 0.3s;
}

.notif-account {
    display: flex;
    flex-direction: row;
    gap: 20px;
    align-items: center;
    justify-content: space-between;
}

#notif-icon {
    max-width: 50px;
    height: auto;
}

.login-btn {
    background-color: white;
    border: 1px solid black;
    border-radius: 5px;
    padding: 0.75rem 4rem;
    font-weight: 600;
    font-size: 1rem;
    cursor: pointer;
    transition: background 0.3s;
}

.login-btn:hover {
    background: #f5f5f5;
}

#main {
    flex: 1;
    padding: 0;
}

/* подвал */
.dno {
    color: black;
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: space-between;
    font-weight: 500;
    font-size: 1.2rem;
    padding: 1rem 2rem;
    background-color: white;
    border-top: 1px solid gray;
}

#main-icon {
    max-width: 40px;
    height: auto;
}

#main-icon:hover {
    transform: scale(1.1);
}

</style>