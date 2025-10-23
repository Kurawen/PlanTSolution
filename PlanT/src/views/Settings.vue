<script>


</script>

<template>
    <MainTemplate @open-auth="(mode) => emit('open-auth', mode)">
        <!-- Основной контент -->
        <main id="main">
            <!-- работайте над задачами -->
            <section class="tasks">
                <div class="tasks-content">
                    <h1 class="tasks-title">Работайте над задачами</h1>
                    <p class="tasks-desc">
                        <strong>PlanT</strong> дает возможность студентам организовывать проекты и без особых 
                        усилий сотрудничать и достигать целей. При этом можно работать как одному, так и в команде.
                    </p>
                </div>
                <img src="./assets/plant-big.svg" alt="растение" class="tasks-plant-img">
            </section>


    


            <!-- готовы улучшить свою продуктивность? -->
            <section class="prod">
                <div class="prod-content">
                    <h2 class="prod-title">Готовы улучшить свою продуктивность?</h2>
                    <button class="prod-btn" @click="openAuth('login')">Начать сейчас</button>
                </div>
            </section>
        </main>

        <!-- модальное окно авторизации/регистрации -->
        <div v-if="showAuth" class="auth-modal-overlay" @click="closeAuth">
            <div class="auth-modal-content" @click.stop>
                <Auth 
                :mode="authMode"
                @submit="handleAuthSubmit"
                @switch-mode="handleSwitchMode"
                @guest-login="handleGuestLogin"
                />
            </div>
        </div>

        <!-- модальное окно в разработке -->
        <DevelopingModalWindow v-if="showModal" @close="closeModal"/>

        
    </MainTemplate>
</template>

<style>



</style>