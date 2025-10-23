<script>
    
</script>
<template>
<div id="problem-back">
    <div id="problem-window">
        <div class="porblem-title">
            <div>
                <h1>Создать задачу</h1>
                <p>Заполните детали новой задачи</p>
            </div>
            <img src="" alt="крестик">
        </div>
        <div class="problem-forms">
            <div class="problem-name">
                <form action="">
                    <label for="name">Название задачи</label><br>
                    <input type="text" id="name" name="name" required>
                </form>
            </div>
            <div class="problem-desc">
                <form action="">
                    <label for="desc">Описание</label><br>
                    <input type="textarea" id="desc" name="desc" required>
                </form>
            </div>
            <div class="problem-date">
                <form action="">
                    <label for="date">Срок выполнения</label><br>
                    <input type="text" id="date" name="date" required>
                </form>
            </div>
            <div class="problem-exec">
                <form action="">
                    <label for="exec">Исполнитель</label><br>
                    <input type="text" id="exec" name="exec" required>
                </form>
            </div>
            <div class="problem-prior">
                <form action="">
                    <label for="prior">Приоритет</label><br>
                    <input type="text" id="prior" name="prior" required>
                </form>
            </div>
        </div>
    </div>
</div>
</template>

<style>
#problem-back {
  	position: fixed;
    display: flex;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    justify-content: center;
    align-items: center;
    z-index: 1000;
}

#problem-window {
    border: 1px solid black;
    border-radius: 5px;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
	max-width: 100%;
}
</style>