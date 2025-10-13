<!-- 
<template> - для HTML разметки
<script> - для логики компонента(с использованием Composition API или Options API)
<style> - для стилей      
-->

<script setup lang="ts">
  import { RouterLink, RouterView } from 'vue-router'
  import HelloWorld from './components/HelloWorld.vue'


  import { ref } from 'vue'
  const count = ref(0)

  import { createApp } from 'vue'
  import App from './App.vue'
</script>

const app = createApp()

<template>
  <header id="shapka">
    <nav class="navbar">
      <a href="PlanT.ru"><img src="" alt="PlanT"></a>
      <a href="" class="nav_glav">Главная</a>
      <a href="" class="">Настройки</a>
    </nav>
    <div class="wrapper">
      <HelloWorld msg="Ah shit, here we go again." />
    </div>

    <button @click="count++">Кликер: {{ count }}</button>
  </header>
  <main id="main">
    <div>

    </div>
  </main>
  <footer id="dno">
    <div>

    </div>
  </footer>
 
  <!-- <RouterView /> -->
</template>

<style scoped>
header {
  line-height: 1.5;
  max-height: 100vh;
}

button {
  font-weight: bold;
}

.logo {
  display: block;
  margin: 0 auto 2rem;
}

nav {
  width: 100%;
  font-size: 12px;
  text-align: center;
  margin-top: 2rem;
}

nav a.router-link-exact-active {
  color: var(--color-text);
}

nav a.router-link-exact-active:hover {
  background-color: transparent;
}

nav a {
  display: inline-block;
  padding: 0 1rem;
  border-left: 1px solid var(--color-border);
}

nav a:first-of-type {
  border: 0;
}

@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
    padding-right: calc(var(--section-gap) / 2);
  }

  .logo {
    margin: 0 2rem 0 0;
  }

  header .wrapper {
    display: flex;
    place-items: flex-start;
    flex-wrap: wrap;
  }

  nav {
    text-align: left;
    margin-left: -1rem;
    font-size: 1rem;

    padding: 1rem 0;
    margin-top: 1rem;
  }
}
</style>
