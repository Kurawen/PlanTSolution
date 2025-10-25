<script setup>
import { ref } from 'vue'

// Данные для чатов
const chats = ref([
  {
    id: 1,
    name: "Проектный отдел 'А'",
    lastMessage: "Осталось разобрать rc",
    time: "10:30",
    unread: true
  },
  {
    id: 2,
    name: "Сара",
    lastMessage: "Отличная работа! Мож",
    time: "5:06",
    unread: false
  },
  {
    id: 3,
    name: "Скамеры",
    lastMessage: "ХОЧЕШЬ СТАТЬ МИЛЛИОР",
    time: "Вчера",
    unread: true
  },
  {
    id: 4,
    name: "Магнус Карлсен",
    lastMessage: "Го раз на раз",
    time: "Вчера",
    unread: false
  }
])

// Данные для активного чата
const activeChat = ref({
  id: 1,
  name: "Проектный отдел 'Альфа'",
  messages: [
    {
      id: 1,
      text: "Доброе утро, всем! Просто напоминание о нашем сроке отчета в 3 -м квартале. Давайте проверим, что все разделы готовы.",
      time: "10:00",
      isUser: false
    },
    {
      id: 2,
      text: "Доброе утро, Сара! Я только что закончил собирать данные для финансовых прогнозов. Готов к обзору.",
      time: "10:05",
      isUser: true
    },
    {
      id: 3,
      text: "Я тоже готов! Вскоре я отправлю свой раздел по анализу рынка. Он включает в себя последние конкурентные идеи.",
      time: "10:15",
      isUser: false
    },
    {
      id: 4,
      text: "Отлично. Давайте будем стремиться к тому, чтобы все было объединено в основной документ к концу дня, чтобы завтра мы могли провести окончательную проверку.",
      time: "10:20",
      isUser: false
    },
    {
      id: 5,
      text: "Звучит как план, спасибо, команда!",
      time: "10:30",
      isUser: false
    }
  ]
})

const newMessage = ref('')

const sendMessage = () => {
  if (newMessage.value.trim()) {
    activeChat.value.messages.push({
      id: Date.now(),
      text: newMessage.value,
      time: new Date().toLocaleTimeString('ru-RU', { hour: '2-digit', minute: '2-digit' }),
      isUser: true
    })
    newMessage.value = ''
  }
}
</script>

<template>
    <div class="chats-container">
        <div class="messages">
            <!-- Список чатов -->
            <div class="chats-list">
                <h2>Сообщения</h2>
                <form class="search-form">
                    <img src="../assets/search-icon.svg" alt="поиск" class="search-icon">
                    <input type="text" placeholder="Найти чат..." class="search-input">
                </form>
                <div class="chats">
                    <div 
                        v-for="chat in chats" 
                        :key="chat.id" 
                        class="chat-item"
                        :class="{ active: activeChat.id === chat.id }"
                    >
                        <div class="chat-avatar">
                            <img src="../assets/kotya.jpg" alt="" class="chat-avatar">
                        </div>
                        <div class="chat-content">
                            <div class="chat-header">
                                <p class="chat-name">{{ chat.name }}</p>
                                <p class="chat-time">{{ chat.time }}</p>
                            </div>
                            <p class="chat-last-message">{{ chat.lastMessage }}</p>
                        </div>
                        <div v-if="chat.unread" class="unread-indicator"></div>
                    </div>
                </div>
            </div>

            <!-- Активный чат -->
            <div class="cur-chat">
                <div class="chat-header">
                    <h3>{{ activeChat.name }}</h3>
                </div>
                
                <div class="messages-container">
                    <div 
                        v-for="message in activeChat.messages" 
                        :key="message.id" 
                        class="message"
                        :class="{ 'user-message': message.isUser }"
                    >
                        <div class="message-bubble">
                            <p class="message-text">{{ message.text }}</p>
                            <span class="message-time">{{ message.time }}</span>
                        </div>
                    </div>
                </div>

                <div class="message-input-container">
                    <input 
                        v-model="newMessage"
                        type="text" 
                        placeholder="Введите сообщение..." 
                        class="message-input"
                        @keypress.enter="sendMessage"
                    >
                    <button @click="sendMessage" class="send-button">
                        Отправить
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.chats-container {
    display: flex;
    justify-content: center;
    min-height: 100vh;
    background-color: #f8f9fa;
    padding: 20px;
}

.messages {
    border: 1px solid #e0e0e0;
    border-radius: 12px;
    display: flex;
    width: 100%;
    max-width: 1200px;
    height: 80vh;
    background: white;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    overflow: hidden;
}

/* Список чатов */
.chats-list {
    width: 350px;
    border-right: 1px solid #e0e0e0;
    display: flex;
    flex-direction: column;
}

.chats-list h2 {
    padding: 1.5rem;
    margin: 0;
    border-bottom: 1px solid #e0e0e0;
    font-size: 1.5rem;
    color: #333;
}

.search-form {
    position: relative;
    padding: 1rem 1.5rem;
    border-bottom: 1px solid #e0e0e0;
}

.search-input {
    width: 100%;
    padding: 0.75rem 0.75rem 0.75rem 2.5rem;
    border: 1px solid #ddd;
    border-radius: 20px;
    font-size: 0.9rem;
    background: #f8f9fa;
}

.search-input:focus {
    outline: none;
    border-color: #000;
    background: white;
}

.search-icon {
    position: absolute;
    left: 2rem;
    top: 50%;
    transform: translateY(-50%);
    width: 16px;
    height: 16px;
    background: #999;
}

.chats {
    flex: 1;
    overflow-y: auto;
}

.chat-item {
    display: flex;
    align-items: center;
    padding: 1rem 1.5rem;
    border-bottom: 1px solid #f0f0f0;
    cursor: pointer;
    transition: background-color 0.2s;
    position: relative;
}

.chat-item:hover {
    background: #f8f9fa;
}

.chat-item.active {
    background: #e3f2fd;
}

.chat-avatar {
    width: 50px;
    height: 50px;
    border-radius: 50%;
    background: #ddd;
    margin-right: 1rem;
    flex-shrink: 0;
}

.chat-content {
    flex: 1;
    min-width: 0;
}

.chat-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 0.25rem;
}

.chat-name {
    font-weight: 600;
    color: #333;
    margin: 0;
    font-size: 0.95rem;
}

.chat-time {
    font-size: 0.8rem;
    color: #999;
    margin: 0;
}

.chat-last-message {
    font-size: 0.85rem;
    color: #666;
    margin: 0;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
}

.unread-indicator {
    width: 8px;
    height: 8px;
    border-radius: 50%;
    background: #007bff;
    position: absolute;
    right: 1rem;
    top: 50%;
    transform: translateY(-50%);
}

/* Активный чат */
.cur-chat {
    flex: 1;
    display: flex;
    flex-direction: column;
}

.cur-chat .chat-header {
    padding: 1.5rem;
    border-bottom: 1px solid #e0e0e0;
    background: white;
}

.cur-chat .chat-header h3 {
    margin: 0;
    color: #333;
    font-size: 1.2rem;
}

.messages-container {
    flex: 1;
    padding: 1.5rem;
    overflow-y: auto;
    background: #f8f9fa;
}

.message {
    display: flex;
    margin-bottom: 1rem;
}

.message.user-message {
    justify-content: flex-end;
}

.message-bubble {
    max-width: 70%;
    padding: 0.75rem 1rem;
    border-radius: 18px;
    background: white;
    border: 1px solid #e0e0e0;
    position: relative;
}

.message.user-message .message-bubble {
    background: #007bff;
    color: white;
    border: none;
}

.message-text {
    margin: 0 0 0.25rem 0;
    font-size: 0.95rem;
    line-height: 1.4;
}

.message-time {
    font-size: 0.75rem;
    opacity: 0.7;
    display: block;
    text-align: right;
}

.message-input-container {
    display: flex;
    padding: 1rem 1.5rem;
    border-top: 1px solid #e0e0e0;
    background: white;
    gap: 0.75rem;
}

.message-input {
    flex: 1;
    padding: 0.75rem 1rem;
    border: 1px solid #ddd;
    border-radius: 20px;
    font-size: 0.95rem;
}

.message-input:focus {
    outline: none;
    border-color: #007bff;
}

.send-button {
    padding: 0.75rem 1.5rem;
    background: #007bff;
    color: white;
    border: none;
    border-radius: 20px;
    font-weight: 500;
    cursor: pointer;
    transition: background-color 0.2s;
}

.send-button:hover {
    background: #0056b3;
}

/* Адаптивность */
@media (max-width: 768px) {
    .messages {
        flex-direction: column;
        height: 90vh;
    }
    
    .chats-list {
        width: 100%;
        height: 40%;
    }
    
    .cur-chat {
        height: 60%;
    }
}
</style>