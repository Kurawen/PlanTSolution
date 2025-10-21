<script>
export default {
    name: 'DevelopingModalWindow',
    emits: ['close'],
    methods: {
        CloseModalWindow() {
            this.$emit('close');
        }
    }
}
</script>

<template>
    <!-- стили -->
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Nunito+Sans:ital,opsz,wght@0,6..12,200..1000;1,6..12,200..1000&display=swap" rel="stylesheet">
    
    <!-- область окна -->
    <div class="modal-window">
        <!-- само модальное окно -->
        <div class="okno-dev">
            <img src="" alt="&times;" class="close-icon" @click="CloseModalWindow">
            <div class="in-dev">
                <p class="dev-title">В разработке</p>
                <img src="../assets/plant-logo.svg" alt="растение в горшке" class="plant-dev">
            </div>
            <div class="close-button">
                <button class="dev-but" @click="CloseModalWindow">Закрыть</button>
            </div>
        </div>
    </div>
</template>

<style scoped>
.modal-window {
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

.okno-dev {
    height: auto;
    width: 550px;
    display: flex;
    justify-content: space-between;
    align-items: center;
    flex-direction: column;
    background-color: white;
    border-radius: 5px;
    /* padding: 10px 0; */
}

.in-dev {
    text-align: center;
    justify-content: center;
    align-items: center;
    gap: 15px;
    display: flex;
    flex-direction: column;
}

.dev-but {
    background-color: white;
    border: 1px solid #DEE1E6FF;
    color: black;
    font-size: 1rem;
    font-weight: bold;
    padding: 10px 12px;
}

.dev-but:hover {
    background-color: #f5f5f5;
}

.dev-title {
    font-weight: 800;
    font-size: 2.5rem;
    font-family: "Nunito Sans", sans-serif;
    text-align: center;
    margin: 0;
    color: black;
}

.plant-dev {
    max-width: 150px;
    height: auto;
}

.close-icon {
    align-self: flex-end;
    cursor: pointer;
    font-size: 1.5rem;
    margin: 0;
    padding-right: 10px;
}

.close-button {
    display: flex;
    align-items: center;
    justify-content: end;
    flex-direction: row;
    width: 100%;
    border-top: 1px solid #DEE1E6FF;
    padding: 10px;
    margin-top: 25px;
}

</style>