<script setup lang="ts">
import { onMounted, reactive, ref, watch } from 'vue'
import { IosSend } from '@vicons/ionicons4'
import type { Chat } from '@/stores/chats'
import { api, generate } from '@/api'

type Message = {
  id: number
  content: string
  role: string
  createdAt: string
}

const participantIndex = ref(0)

const props = defineProps<{
  chat: Chat
}>()

const messages = ref<Message[]>([])

onMounted(async () => {
  messages.value = await api.getMessages(props.chat.id)
})

const generateMessage = async () => {
  const msg = reactive({
    id: 0,
    content: '',
    role: 'assistant',
    createdAt: new Date().toISOString(),
  })
  messages.value.push(msg)
  await generate(props.chat.id, props.chat.participants[participantIndex.value].id, (delta) => {
    msg.content += delta
  })
}
</script>

<template>
  <div class="container">
    <div style="height: 4em">
      <n-flex align="center" justify="space-between" style="height: 100%; width: 100%">
        <h2>Chat Name</h2>
      </n-flex>
    </div>
    <div style="height: calc(100% - 2em); padding-top: 2em; overflow: hidden">
      <div class="chat-box">
        <div class="messages">
          <n-scrollbar style="height: 100%; width: calc(100% - 2em)">
            <Message v-for="i in messages" class="message" :key="i.id"></Message>
          </n-scrollbar>
        </div>
        <div class="input">
          <n-grid style="width: 100%" x-gap="12" :cols="2">
            <n-gi>
              <n-input
                type="textarea"
                placeholder="Input Message"
                round
                :autosize="{
                  minRows: 1,
                  maxRows: 5,
                }"
              >
                <template #suffix>
                  <n-button type="primary" size="small" strong secondary>
                    <template #icon>
                      <n-icon>
                        <IosSend />
                      </n-icon>
                    </template>
                  </n-button>
                </template>
              </n-input>
            </n-gi>
            <n-gi>
              <n-input-group>
                <n-select></n-select>
                <n-button type="primary" strong secondary> 生成 </n-button>
              </n-input-group>
            </n-gi>
          </n-grid>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.chat-box {
  display: flex;
  flex-direction: column;
  height: 100%;
  overflow: hidden;
}

.messages {
  overflow: hidden;
  width: 100%;
  height: 100%;
  padding: 10px;
}
.input {
  width: 100%;
  padding-bottom: 1em;
}
.container {
  height: 100%;
  width: calc(100% - 4em);
  overflow: hidden;
  display: flex;
  flex-direction: column;
  margin-left: 2em;
  margin-right: 2em;
}

.message {
  padding-top: 1em;
}
</style>
