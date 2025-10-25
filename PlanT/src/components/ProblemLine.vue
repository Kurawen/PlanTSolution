<script>

</script>

<template>
    <div class="problema">
        <div>checkbox</div>
        <div class="problem-task">Подготовить квартальный отчет</div>
        <div class="problem-date">15.09.2025</div>
        <div class="problem-status">в процессе</div>
        <div class="problem-prior">высокий</div>
        <div class="problem-creator">Анна Иванова</div>
        <div class="problem-actions">...</div>
    </div>
</template>

<style>
.problema {
    display: flex;
    align-items: center;
    text-align: center;
    font-size: 1rem;
    padding: 15px;
    gap: 30px;
    border: 1px solid grey;
    width: max-content;
}

.problem-task {
    font-weight: bold;

}

.problem-date {
    
}

.problem-status {
    background-color: black;
    color: white;
        padding: 8px 12px;
}

.problem-prior {
    background-color: red;
    color: white;
    padding: 8px 12px;
}

.problem-creator {
    
}

.problem-actions {
    
}
</style>