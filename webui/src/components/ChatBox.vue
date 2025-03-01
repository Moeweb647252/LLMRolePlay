<script setup lang="ts">
import { onMounted, reactive, ref, useTemplateRef, watch } from 'vue'
import { IosSend } from '@vicons/ionicons4'
import { Chat } from '@/types/chat'
import { api, generate } from '@/api'
import type { Message } from '@/types/chat'
import { NScrollbar } from 'naive-ui'

const participantIndex = ref(0)

const props = defineProps<{
  chat: Chat
}>()

const messages = reactive<Message[]>(await api.getMessages(props.chat.id!))
const input = ref('')
const generating = ref(false)
const messageScroll = useTemplateRef('messageScroll')

const addMessage = async () => {
  const msg = reactive({
    id: 0,
    content: input.value,
    role: 'user',
    participantId: null,
    createdAt: new Date().toISOString(),
  })
  input.value = ''
  messages.push(msg)
  let id = await api.addMessage(props.chat.id!, msg.content, msg.role)
  msg.id = id
  await generateMessage()
}

const generateMessage = async () => {
  generating.value = true
  const msg = reactive({
    id: 0,
    content: '',
    role: 'assistant',
    participantId: props.chat.participants[participantIndex.value].id!,
    createdAt: new Date().toISOString(),
  })
  messages.push(msg)
  let id = await generate(props.chat.participants[participantIndex.value].id!, (delta) => {
    msg.content += delta
  })
  msg.id = id
  generating.value = false
  participantIndex.value++
  if (participantIndex.value >= props.chat.participants.length) {
    participantIndex.value = 0
  }
}

const deleteMessage = async (message: Message) => {
  await api.deleteMessage(message.id!)
  messages.splice(messages.indexOf(message), 1)
}

onMounted(async () => {
  watch(
    messages,
    () => {
      messageScroll.value!.scrollTo(
        0,
        messageScroll.value?.scrollbarInstRef?.containerRef?.scrollHeight!,
      )
    },
    {
      immediate: true,
    },
  )
})
</script>

<template>
  <div class="container">
    <div style="height: 4em">
      <n-flex align="center" justify="space-between" style="height: 100%; width: 100%">
        <h2>{{ props.chat.name }}</h2>
      </n-flex>
    </div>
    <div style="height: calc(100% - 2em); padding-top: 2em; overflow: hidden">
      <div class="chat-box">
        <div class="messages">
          <n-scrollbar ref="messageScroll" style="height: 100%; width: calc(100% - 2em)">
            <Message
              v-for="(i, index) in messages"
              class="message"
              :key="i.id"
              :content="i.content"
              :reloadable="index == messages.length - 1"
              :name="
                i.participantId
                  ? chat.participants.find((c) => c.id === i.participantId)?.name
                  : '你'
              "
              @delete="deleteMessage(i)"
            ></Message>
          </n-scrollbar>
        </div>
        <div class="input">
          <n-grid style="width: 100%" x-gap="12" :cols="2">
            <n-gi>
              <n-input
                v-model:value="input"
                type="textarea"
                placeholder="Input Message"
                round
                :autosize="{
                  minRows: 1,
                  maxRows: 5,
                }"
              >
                <template #suffix>
                  <n-button
                    :disabled="generating"
                    type="primary"
                    size="small"
                    strong
                    secondary
                    @click="addMessage"
                  >
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
                <n-select
                  v-model:value="participantIndex"
                  :options="
                    chat.participants.map((c, index) => {
                      return {
                        label: c.name,
                        value: index,
                      }
                    })
                  "
                ></n-select>
                <n-button
                  :disabled="generating"
                  type="primary"
                  strong
                  secondary
                  @click="generateMessage"
                >
                  生成
                </n-button>
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
