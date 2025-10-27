<script>
export default {
    name: 'NotificationsChat',
    props: {
        title: {
            type: String,
            required: true
        },
        message: {
            type: String,
            required: true
        },
        time: {
            type: String,
            required: true
        },
        avatar: {
            type: String,
            default: ''
        }
    }
}
</script>

<template>
    <div class="chat-cont">
        <div class="chat-avatar">
            <img :src="avatar" :alt="title" class="chat-img" v-if="avatar">
            <div class="avatar-placeholder" v-else>{{ title.charAt(0) }}</div>
        </div>
        <div class="chat-content">
            <div class="chat-header">
                <h6 class="chat-title">{{ title }}</h6>
                <span class="chat-time">{{ time }}</span>
            </div>
            <p class="chat-message">{{ message }}</p>
        </div>
    </div>
</template>

<style scoped>
.chat-cont {
    display: flex;
    align-items: flex-start;
    gap: 12px;
    padding: 16px;
    border-radius: 8px;
    transition: background-color 0.2s ease;
    cursor: pointer;
}

.chat-cont:hover {
    background-color: #f8f9fa;
}

.chat-cont:not(:last-child) {
    border-bottom: 1px solid #e1e5e9;
    margin-bottom: 8px;
}

.chat-avatar {
    flex-shrink: 0;
    width: 40px;
    height: 40px;
}

.chat-img {
    width: 100%;
    height: 100%;
    border-radius: 50%;
    object-fit: cover;
}

.avatar-placeholder {
    width: 100%;
    height: 100%;
    border-radius: 50%;
    background-color: #007bff;
    color: white;
    display: flex;
    align-items: center;
    justify-content: center;
    font-weight: 600;
    font-size: 0.9rem;
}

.chat-content {
    flex: 1;
    min-width: 0;
}

.chat-header {
    display: flex;
    justify-content: space-between;
    align-items: flex-start;
    margin-bottom: 4px;
    gap: 12px;
}

.chat-title {
    font-family: 'Inter', sans-serif;
    font-size: 0.9rem;
    font-weight: 600;
    color: #1a1a1a;
    margin: 0;
    line-height: 1.3;
}

.chat-time {
    font-family: 'Inter', sans-serif;
    font-size: 0.75rem;
    color: #666;
    white-space: nowrap;
    flex-shrink: 0;
}

.chat-message {
    font-family: 'Inter', sans-serif;
    font-size: 0.85rem;
    color: #666;
    margin: 0;
    line-height: 1.4;
    display: -webkit-box;
    -webkit-line-clamp: 2;
    -webkit-box-orient: vertical;
    overflow: hidden;
}
</style>