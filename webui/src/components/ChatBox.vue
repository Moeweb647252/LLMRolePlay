<script setup lang="ts">
import { onMounted, reactive, ref, useTemplateRef, watch } from 'vue'
import { IosSend } from '@vicons/ionicons4'
import { Chat } from '@/types/chat'
import { api, generate } from '@/api'
import type { Message as MessageT } from '@/types/chat'
import { NScrollbar } from 'naive-ui'
import Message from './ChatMessage.vue'
import {
  NFlex,
  NGrid,
  NInput,
  NButton,
  NInputGroup,
  NSelect,
  NGridItem,
  NIcon,
} from 'naive-ui'

const participantIndex = ref(0)

const props = defineProps<{
  chat: Chat
}>()

const messages = reactive<MessageT[]>(await api.getMessages(props.chat.id!))
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
  await generateMessage(props.chat.participants[participantIndex.value].id!)
  participantIndex.value++
  if (participantIndex.value >= props.chat.participants.length) {
    participantIndex.value = 0
  }
}

const generateMessage = async (participantId: number) => {
  generating.value = true
  const msg = reactive({
    id: 0,
    content: '',
    role: 'assistant',
    participantId: participantId,
    createdAt: new Date().toISOString(),
  })
  messages.push(msg)
  let id = await generate(participantId, (delta) => {
    msg.content += delta
  })
  msg.id = id
  generating.value = false
  let settings = JSON.parse(JSON.stringify(props.chat.settings))
  settings.currentParticipantId =
    props.chat.participants[participantIndex.value].id!
  await api.updateChat(props.chat.id!, {
    settings: settings,
  })
}

const regenerateMessage = async (message: MessageT) => {
  await api.deleteMessage(message.id!)
  messages.splice(messages.indexOf(message), 1)
  await generateMessage(message.participantId!)
}

const deleteMessage = async (message: MessageT) => {
  await api.deleteMessage(message.id!)
  messages.splice(messages.indexOf(message), 1)
  participantIndex.value -= 1
  if (participantIndex.value < 0) {
    participantIndex.value = 0
  }
}

onMounted(async () => {
  watch(
    messages,
    () => {
      messageScroll.value!.scrollTo(
        0,
        messageScroll.value?.scrollbarInstRef?.containerRef
          ?.scrollHeight as number,
      )
    },
    {
      immediate: true,
    },
  )
  if (props.chat.settings.currentParticipantId) {
    const index = props.chat.participants.findIndex(
      (c) => c.id === props.chat.settings.currentParticipantId,
    )
    if (index !== -1) {
      participantIndex.value = index
    }
  }
})
</script>

<template>
  <div class="container">
    <div style="height: 4em">
      <NFlex
        align="center"
        justify="space-between"
        style="height: 100%; width: 100%"
      >
        <h2>{{ props.chat.name }}</h2>
      </NFlex>
    </div>
    <div style="height: calc(100% - 2em); padding-top: 2em; overflow: hidden">
      <div class="chat-box">
        <div class="messages">
          <NScrollbar
            ref="messageScroll"
            style="height: 100%; width: calc(100% - 2em)"
          >
            <Message
              v-for="(i, index) in messages"
              :key="i.id!"
              class="message"
              :content="i.content"
              :avatar="null"
              :direction="'left'"
              :reloadable="index == messages.length - 1"
              :name="
                i.participantId
                  ? chat.participants.find((c) => c.id === i.participantId)
                      ?.name!
                  : '你'
              "
              @delete="deleteMessage(i)"
              @regenerate="regenerateMessage(i)"
            />
          </NScrollbar>
        </div>
        <div class="input">
          <NGrid style="width: 100%" x-gap="12" :cols="2">
            <NGridItem>
              <NInput
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
                  <NButton
                    :disabled="generating"
                    type="primary"
                    size="small"
                    strong
                    secondary
                    @click="addMessage"
                  >
                    <template #icon>
                      <NIcon>
                        <IosSend />
                      </NIcon>
                    </template>
                  </NButton>
                </template>
              </NInput>
            </NGridItem>
            <NGridItem>
              <NInputGroup>
                <NSelect
                  v-model:value="participantIndex"
                  :options="
                    chat.participants.map((c, index) => {
                      return {
                        label: c.name,
                        value: index,
                      }
                    })
                  "
                />
                <NButton
                  :disabled="generating"
                  type="primary"
                  strong
                  secondary
                  @click="
                    generateMessage(chat.participants[participantIndex].id!)
                  "
                >
                  生成
                </NButton>
              </NInputGroup>
            </NGridItem>
          </NGrid>
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
