<script setup>
const props = defineProps({
    title: {
        type: String,
        default: "Название группы"
    },
    description: {
        type: String,
        default: "Описание группы"
    },
    memberCount: {
        type: Number,
        default: 0
    }
})

const emit = defineEmits(['details', 'edit'])

const handleDetails = () => {
    emit('details', props.title)
}

const handleEdit = () => {
    emit('edit', props.title)
}
</script>

<template>
    <div class="squad-obj">
        <h2 class="squad-title">{{ title }}</h2>
        <hr class="hrr">
        <p class="squad-desc">{{ description }}</p>
        <div class="squad-count">
            <img src="../assets/team.svg" alt="команда" class="team-icon">
            <p class="squad-count-number">{{ memberCount }} участников</p>
        </div>
        <div class="squad-buttons">
            <button class="squad-but" @click="handleDetails">Подробнее</button>
            <button class="squad-but" @click="handleEdit">Редактировать</button>
        </div>
    </div>
</template>

<style scoped>
.hrr {
    color: var(--border-color);
    height: 1px;
    border: none;
    background-color: var(--border-color);
    width: 100%;
    margin: 0;
}

.squad-obj {
    display: flex;
    flex-direction: column;
    border: 1px solid var(--border-color);
    border-radius: 5px;
    padding: 15px;
    margin: 10px;
    max-width: 100%;
    width: max-content;
    background-color: white;
    gap: 12px;
}

.squad-title {
    font-size: 1.2rem;
    font-weight: bold;
    margin: 0;
}

.squad-desc {
    font-size: 0.9rem;
    color: black;
    margin: 0;
}

.squad-buttons {
    display: flex;
    align-items: center;
    justify-content: space-between;
}

.squad-but {
    background-color: white;
    color: black;
    font-size: 0.9rem;
    font-weight: 500;
    border: 1px solid var(--border-color);
    border-radius: 5px;
    padding: 12px 25px;
    cursor: pointer;
}

.squad-but:hover {
    background-color: #f5f5f5;
}

.squad-count {
    display: flex;
    align-items: center;
    justify-content: start;
    gap: 10px;
}

.squad-count-number {
    font-size: 0.9rem;
    margin: 0;
}

.team-icon {
    max-width: 20px;
    height: auto;
}
</style>