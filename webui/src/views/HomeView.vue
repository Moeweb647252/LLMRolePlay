<script setup lang="ts">
import { api } from '@/api'
import ChatBox from '@/components/ChatBox.vue'
import { useSettingsStore } from '@/stores/settings'
import { IosMenu, MdAdd, MdContact, MdCreate, MdClose } from '@vicons/ionicons4'
import { useMessage, useModal } from 'naive-ui'
import {
  NButton,
  NDropdown,
  NIcon,
  NLayout,
  NLayoutContent,
  NLayoutHeader,
  NLayoutSider,
  NList,
  NListItem,
  NSpace,
  NSpin,
  NScrollbar,
} from 'naive-ui'
import { computed, onMounted, reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { Chat } from '@/types/chat'
import AddChatModal from '@/components/modals/AddChatModal.vue'
import EditChatModal from '@/components/modals/EditChatModal.vue'
import type { EditChatForm } from '@/types/modal'

const settings = useSettingsStore()
const router = useRouter()
const showSider = ref(true)
const message = useMessage()
const modal = useModal()

const providers = ref(await api.getProviders())
const userPresets = ref(await api.getPresets())
const userCharacters = ref(await api.getCharacters())
const userTemplates = ref(await api.getTemplates())
const publicPresets = ref(await api.getPublicPresets())
const publicCharacters = ref(await api.getPublicCharacters())
const publicTemplates = ref(await api.getPublicTemplates())
const publicModels = ref(await api.getPublicModels())
const chats = ref(await api.getChats())

const presets = computed(() => {
  return userPresets.value.concat(publicPresets.value).map((p) => {
    return {
      label: p.name + (p.isPublic ? '(公共)' : ''),
      value: p.id,
    }
  })
})
const characters = computed(() => {
  return userCharacters.value.concat(publicCharacters.value).map((c) => {
    return {
      label: c.name! + (c.isPublic ? '(公共)' : ''),
      value: c.id!,
    }
  })
})
const templates = computed(() => {
  return userTemplates.value.concat(publicTemplates.value).map((t) => {
    return {
      label: t.name! + (t.isPublic ? '(公共)' : ''),
      value: t.id!,
    }
  })
})
const models = computed(() => {
  return providers.value
    .map((p) => p.models)
    .flat()
    .concat(publicModels.value)
    .map((m) => {
      return {
        label: m.name + (m.provider ? `(${m.provider.name})` : '(公共)'),
        value: m.id,
      }
    })
})

const currentChat = ref(null as Chat | null)
const chatBoxKey = ref(0)
const modalsShow = reactive({
  addChat: false,
  editChat: false,
  editingChat: null as EditChatForm | null,
})

const modalsKey = reactive({
  addChat: 0,
  editChat: 0,
})

if (!settings.user) {
  router.push('/login')
}

const dropdownOptions = [
  {
    label: '设置',
    key: 'settings',
  },
]

const onDropdownSelect = (key: string) => {
  if (key === 'settings') {
    router.push('/main/settings')
  }
}

const setChat = (chat: Chat) => {
  settings.currentChatId = chat.id
  currentChat.value = chat
  chatBoxKey.value++
}

const deleteChat = async (chat: Chat) => {
  modal.create({
    title: '删除聊天',
    content: `确定删除聊天 ${chat.name} ?`,
    preset: 'dialog',
    positiveText: '确定',
    negativeText: '取消',
    onPositiveClick: async () => {
      try {
        await api.deleteChat(chat.id!)
        chats.value = chats.value.filter((c) => c.id !== chat.id)
        if (settings.currentChatId === chat.id) {
          settings.currentChatId = void 0
          currentChat.value = null
        }
        message.success('删除成功')
      } catch (e) {
        console.log(e)
        message.error('删除失败')
      }
    },
  })
}

const onConfirmAddChat = async (chat: Chat) => {
  chats.value.push(chat)
}
let onConfirmEditChat = (_form: EditChatForm) => {}

const startAddChat = () => {
  modalsShow.addChat = true
  modalsKey.addChat++
}

const startEditChat = (chat: Chat) => {
  modalsShow.editingChat = {
    name: chat.name!,
    description: chat.description,
    id: chat.id!,
    settings: chat.settings,
    participants: chat.participants.map((p) => {
      return {
        id: p.id!,
        name: p.name!,
        model: p.model!.id,
        presets: p.presets.map((p) => p.id),
        character: p.character!.id,
        template: p.template!.id,
        settings: null,
      }
    }),
  }
  modalsShow.editChat = true
  modalsKey.editChat++
  onConfirmEditChat = (_form: EditChatForm) => {
    console.log(_form)
    chat.name = _form.name
    chat.description = _form.description
    chat.participants = _form.participants.map((p) => {
      return {
        id: p.id,
        name: p.name,
        model: providers.value
          .map((p) => p.models)
          .flat()
          .find((m) => m.id === p.model)!,
        presets: p.presets.map((p) => {
          return userPresets.value.find((up) => up.id === p)!
        })!,
        character: userCharacters.value.find((c) => c.id === p.character)!,
        template: userTemplates.value.find((t) => t.id === p.template)!,
      }
    })
    chat.settings = _form.settings
    chatBoxKey.value++
  }
}

onMounted(async () => {
  if (settings.currentChatId) {
    const chat = chats.value.find((chat) => chat.id === settings.currentChatId)
    if (chat) {
      setChat(chat)
    }
  }
})
</script>
<template>
  <NLayout :has-sider="showSider" class="full bfc">
    <NLayoutSider v-if="showSider" width="200" class="bfc">
      <div class="sider bfc">
        <div
          style="
            margin: 2px 4px;
            height: 3.5em;
            display: flex;
            align-items: center;
            justify-content: center;
          "
        >
          <h2>LLMRolePlay</h2>
        </div>
        <div style="margin: 2px 4px; height: 2.5em">
          <NButton
            type="default"
            style="width: 100%; height: 100%"
            @click="startAddChat"
          >
            <template #icon>
              <NIcon>
                <MdAdd />
              </NIcon>
            </template>
          </NButton>
        </div>
        <NScrollbar style="height: 100%" content-style="margin: 0 4px">
          <NList :show-divider="false">
            <NListItem
              v-for="(chat, index) in chats"
              :key="index"
              style="height: 2.5em; margin-left: 1em; margin-right: 1em"
              class="chat-list-item"
            >
              <NSpace
                style="width: 100%"
                align="center"
                justify="space-between"
              >
                <div @click="setChat(chat)">
                  {{ chat.name }}
                </div>
                <NSpace>
                  <NButton
                    text
                    size="small"
                    class="chat-button"
                    @click="startEditChat(chat)"
                  >
                    <template #icon>
                      <NIcon>
                        <MdCreate />
                      </NIcon>
                    </template>
                  </NButton>
                  <NButton
                    text
                    size="small"
                    class="chat-button"
                    @click="deleteChat(chat)"
                  >
                    <template #icon>
                      <NIcon>
                        <MdClose />
                      </NIcon>
                    </template>
                  </NButton>
                </NSpace>
              </NSpace>
            </NListItem>
          </NList>
        </NScrollbar>
      </div>
    </NLayoutSider>
    <NLayout class="full bfc">
      <NLayoutHeader
        style="
          display: flex;
          align-items: center;
          justify-content: space-between;
          padding-top: 0.5em;
          height: 3em;
          margin-left: 0.5em;
        "
      >
        <NButton
          type="default"
          strong
          secondary
          @click="showSider = !showSider"
        >
          <NIcon size="2em">
            <IosMenu />
          </NIcon>
        </NButton>
        <NSpace>
          <NButton @click="router.push('/main/translate')"> 翻译模式 </NButton>
          <NDropdown :options="dropdownOptions" @select="onDropdownSelect">
            <NIcon size="2.5em" style="padding-right: 0.5em">
              <MdContact />
            </NIcon>
          </NDropdown>
        </NSpace>
      </NLayoutHeader>
      <NLayoutContent class="bfc" style="height: calc(100% - 3.5em)">
        <Suspense>
          <ChatBox v-if="currentChat" :key="chatBoxKey" :chat="currentChat" />
          <template #fallback>
            <div
              style="
                width: 100%;
                height: 100%;
                display: flex;
                align-items: center;
                justify-content: center;
              "
            >
              <NSpin tip="Loading..." />
            </div>
          </template>
        </Suspense>
      </NLayoutContent>
    </NLayout>
  </NLayout>
  <AddChatModal
    :key="modalsKey.addChat"
    :providers="providers"
    :presets="presets"
    :characters="characters"
    :templates="templates"
    :models="models"
    :show="modalsShow.addChat"
    @confirm="onConfirmAddChat"
  ></AddChatModal>
  <EditChatModal
    v-if="modalsShow.editingChat"
    :key="modalsKey.editChat"
    :value="modalsShow.editingChat!"
    :providers="providers"
    :presets="presets"
    :characters="characters"
    :templates="templates"
    :models="models"
    :show="modalsShow.editChat"
    @confirm="onConfirmEditChat"
  ></EditChatModal>
</template>

<style scoped>
.full {
  height: 100%;
  width: 100%;
}

.sider {
  height: 100%;
  width: 100%;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.bfc {
  overflow: hidden;
}

.chat-list-item {
  cursor: pointer;
}

.chat-list-item .chat-button {
  display: none;
}

.chat-list-item:hover .chat-button {
  display: flex;
}
</style>
