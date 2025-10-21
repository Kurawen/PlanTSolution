<script>

</script>
<template>
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Nunito+Sans:ital,opsz,wght@0,6..12,200..1000;1,6..12,200..1000&family=Rubik:ital,wght@0,300..900;1,300..900&display=swap" rel="stylesheet">

    <main id="main">
        <img src="" alt="&times;" class="close-icon" @click="CloseModalWindow">
        <h2 class="notif-title">Уведомления</h2>
        <p class="notif-desc">Просматривайте уведомления о чатах и задачах.</p>
        <section class="notif-chat">
            <h3>Уведомления чата</h3>
            <div class="notif-chat-sect">
                <img src="../assets/kotya.jpg" alt="" class="chat-img">
                <div class="notif-obj-col">
                    <h4>Команда маркетинга</h4>
                    <p>Отчет T2 готов к проверке</p>
                    <p>10 минут назад</p>
                </div>
            </div>
            <div class="notif-chat-sect">
                <img src="../assets/kotya.jpg" alt="" class="chat-img">
                <div class="notif-obj-col">
                    <h4>Проект Alpha</h4>
                    <p>Обновление статуса: встреча в 10:15</p>
                    <p>2 часа назад</p>
                </div>
            </div>
            <div class="notif-chat-sect">
                <img src="../assets/kotya.jpg" alt="" class="chat-img">
                <div class="notif-obj-col">
                    <h4>Отдел продаж</h4>
                    <p>Новый клиент. Свяжитесь с Райаном Гослингом</p>
                    <p>Вчера</p>
                </div>
            </div>
        </section>
        <section class="notif-task">


        </section>
    </main>

</template>
<style>
:root {
    --border-color: #DEE1E6FF;
    --text-color: #565D6DFF;
    --bg-color: #f8f8f8;
    --text-header: "Nunito Sans", sans-serif;
    --text-font: "Rubik", sans-serif;
}

* {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
    font-family: var(--text-font);
    color: black;
    /* border: 1px solid red; */
}

body {

}

#main {

}

.close-icon {
    align-self: flex-end;
    cursor: pointer;
    font-size: 1.5rem;
    margin: 0;
    padding-right: 10px;
}

.notif-title {

}

.notif-desc {

}

.notif-chat {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: left;

}

.notif-chat-sect {
    display: flex;
    flex-direction: row;
    justify-content: left;
    align-items: center;
    gap: 10px;

}

.notif-obj-col {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: left;
}

.chat-img {
    max-width: 40px;
    height: auto;
}



.notif-task {

}

</style>