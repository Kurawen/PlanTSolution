<script>
export default {
    name: 'ProblemDelete',
    emits: ['close', 'confirm'],
    setup(props, { emit }) {
        const closeModal = () => {
            emit('close')
        }

        const confirmDelete = () => {
            emit('confirm')
        }

        return {
            closeModal,
            confirmDelete
        }
    }
}
</script>

<template>
<div class="delete-container">
    <div class="delete-window">
        <div class="delete-title">
            <h2>Удалить задачу</h2>
        </div>
        <p>Вы уверены?</p>
        <hr>
        <div class="down-buttons">
            <button class="btn-gray btn-md" @click="closeModal">Отмена</button>
            <button class="btn-black btn-md" @click="confirmDelete">Удалить</button>
        </div>
    </div>
</div>
</template>

<style scoped>

p {
    font-size: 1.2rem;
    color: black;
}

hr {
    height: 2px;
    border: none;
    background-color: var(--border-color);
    width: 100%;
}
.delete-container {
    position: fixed;
    display: flex;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    justify-content: center;
    align-items: center;
    z-index: 1010;
}

.delete-window {
    padding: 15px;
    border-radius: 5px;
    width: 300px;
    background-color: white;
    padding: 15px;
    display: flex;
    flex-direction: column;
    gap: 15px;
}

.delete-title {
    display: flex;
    justify-content: space-between;
}

.delete-title > h2 {
    font-family: var(--text-header);
    font-size: 1.5rem;
    font-weight: 800;
}

.down-buttons {
    display: flex;
    justify-content: space-between;

}

.button-otmena,
.button-delete {
    font-size: 1rem;
    border-radius: 5px;
    padding: 10px 15px;
    cursor: pointer;
}

.button-otmena {
    color: black;
    border: 2px solid var(--border-color);
    background-color: white;
}

.button-otmena:hover {
    background-color: var(--bg-color);
    transition: transform 0.2s ease;
}

.button-delete {
    background-color: black;
    color: white;
    border: 2px solid black;
}

.button-delete:hover {
    background-color: white;
    color: black;
    transition: all 0.2s ease;
}

</style>