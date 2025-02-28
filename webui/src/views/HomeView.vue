<script setup lang="ts">
import { api } from '@/api'
import ChatBox from '@/components/ChatBox.vue'
import { type Character } from '@/types/character'
import { useSettingsStore } from '@/stores/settings'
import { IosMenu, MdAdd, MdContact, MdCreate, MdClose } from '@vicons/ionicons4'
import { NTag, useMessage, type UploadFileInfo, useModal } from 'naive-ui'
import { computed, h, ref } from 'vue'
import { useRouter } from 'vue-router'
import { Model, Provider } from '@/types/provider'
import { Preset } from '@/types/preset'
import { Template } from '@/types/template'
import { Chat, Participant } from '@/types/chat'

const settings = useSettingsStore()
const router = useRouter()
const showSider = ref(true)
const message = useMessage()
const modal = useModal()

const providers = ref([] as Provider[])
const presets = ref([] as Preset[])
const characters = ref([] as Character[])
const templates = ref([] as Template[])
const chats = ref(await api.getChats())

const currentChat = ref(null as Chat | null)
const chatBoxKey = ref(0)

const renderAddChatParticipant = (paticipant: Participant, index: number) => {
  return h(
    NTag,
    {
      closable: true,
      onClose: () => {},
      onClick: () => {},
    },
    {
      default: () => paticipant.name,
    },
  )
}

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

const addChatForm = ref({
  visible: false,
  name: '',
  description: '',
  settings: [],
  participants: [] as any[],
})

const startAddChat = async () => {
  addChatForm.value = {
    visible: true,
    name: '',
    description: '',
    settings: [],
    participants: [],
  }
  addChatForm.value.visible = true
  providers.value = await api.getProviders()
  presets.value = await api.getPresets()
  characters.value = await api.getCharacters()
  templates.value = await api.getTemplates()
}

const addParticipantForm = ref({
  visible: false,
  name: '',
  settings: {},
  model: null as null | Model,
  presets: null as
    | null
    | {
        data: Preset | null
      }[],
  character: null as null | Character,
  template: null as null | Template,
  avatarFileList: [] as UploadFileInfo[],
  onConfirm: () => {},
})

const addChatAddParticipant = () => {
  addParticipantForm.value = {
    visible: true,
    name: '',
    settings: {},
    model: null,
    presets: [],
    character: null,
    template: null,
    avatarFileList: [] as UploadFileInfo[],
    onConfirm: () => {
      addChatForm.value.participants.push({
        name: addParticipantForm.value.name,
        settings: addParticipantForm.value.settings,
        model: addParticipantForm.value.model,
        presets: addParticipantForm.value.presets,
        character: addParticipantForm.value.character,
        template: addParticipantForm.value.template,
      })
      addParticipantForm.value.visible = false
    },
  }
}

const modelOptions = computed(() => {
  return providers.value
    .map((providers) => providers.models)
    .flat()
    .map((model) => ({
      label: model.name + `(${model.provider!.name})`,
      value: model,
    }))
})

const addChat = async () => {
  const chatId = await api.addChat(addChatForm.value.name, addChatForm.value.description, {})
  const participants = []
  for (const participant of addChatForm.value.participants) {
    const participantId = await api.addParticipant(
      chatId,
      participant.character.id,
      participant.presets.map((p: any) => p.data!.id),
      participant.template.id,
      participant.model.id,
      participant.name,
      {},
    )
    participants.push({
      id: participantId,
      name: participant.name,
      settings: participant.settings,
      model: {
        id: participant.model.id,
        name: participant.model.name,
      },
      presets: participant.presets.map((p: any) => {
        return {
          id: p.data!.id,
          name: p.data!.name,
        }
      }),
      character: {
        id: participant.character.id,
        name: participant.character.name,
      },
      template: {
        id: participant.template.id,
        name: participant.template.name,
      },
    })
  }
  chats.value.push(new Chat(chatId, addChatForm.value.name, participants))
  currentChat.value = chats.value[chats.value.length - 1]
  addChatForm.value.visible = false
  message.success('添加成功')
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
        chats.value.splice(chats.value.indexOf(chat), 1)
        message.success('删除成功')
      } catch (e) {
        message.error('删除失败')
      }
    },
  })
}

const setChat = (chat: Chat) => {
  currentChat.value = chat
  chatBoxKey.value++
}
</script>
<template>
  <n-layout :has-sider="showSider" class="full bfc">
    <n-layout-sider width="200" theme="dark" class="bfc" v-if="showSider">
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
          <n-button type="default" style="width: 100%; height: 100%" @click="startAddChat">
            <template #icon>
              <n-icon>
                <MdAdd />
              </n-icon>
            </template>
          </n-button>
        </div>
        <n-scrollbar style="height: 100%" content-style="margin: 0 4px">
          <n-list :show-divider="false">
            <n-list-item
              v-for="(chat, index) in chats"
              :key="index"
              style="height: 2.5em; margin-left: 1em; margin-right: 1em"
              class="chat-list-item"
            >
              <n-space style="width: 100%" align="center" justify="space-between">
                <div @click="setChat(chat)">{{ chat.name }}</div>
                <n-space>
                  <n-button text size="small" class="chat-button">
                    <template #icon>
                      <n-icon>
                        <MdCreate />
                      </n-icon>
                    </template>
                  </n-button>
                  <n-button text size="small" class="chat-button" @click="deleteChat(chat)">
                    <template #icon>
                      <n-icon>
                        <MdClose />
                      </n-icon>
                    </template>
                  </n-button>
                </n-space>
              </n-space>
            </n-list-item>
          </n-list>
        </n-scrollbar>
      </div>
    </n-layout-sider>
    <n-layout class="full bfc">
      <n-layout-header
        style="
          display: flex;
          align-items: center;
          justify-content: space-between;
          padding-top: 0.5em;
          height: 3em;
          margin-left: 0.5em;
        "
      >
        <n-button type="default" @click="showSider = !showSider" strong secondary>
          <n-icon size="2em">
            <IosMenu />
          </n-icon>
        </n-button>
        <n-dropdown :options="dropdownOptions" @select="onDropdownSelect">
          <n-icon size="2.5em" style="padding-right: 0.5em">
            <MdContact />
          </n-icon>
        </n-dropdown>
      </n-layout-header>
      <n-layout-content class="bfc" style="height: calc(100% - 3.5em)">
        <Suspense>
          <ChatBox v-if="currentChat" :chat="currentChat" :key="chatBoxKey"></ChatBox>
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
              <n-spin tip="Loading..." />
            </div>
          </template>
        </Suspense>
      </n-layout-content>
    </n-layout>
  </n-layout>
  <n-modal
    v-model:show="addChatForm.visible"
    preset="card"
    title="添加聊天"
    size="medium"
    style="width: fit-content; min-width: 25em"
  >
    <n-form label-placement="left">
      <n-form-item label="名称">
        <n-input v-model:value="addChatForm.name" />
      </n-form-item>
      <n-form-item label="描述">
        <n-input v-model:value="addChatForm.description" />
      </n-form-item>
      <n-form-item label="参与者">
        <n-dynamic-tags
          v-model:value="addChatForm.participants"
          :render-tag="renderAddChatParticipant"
        >
          <template #trigger>
            <n-button size="small" type="primary" dashed @click="addChatAddParticipant">
              <template #icon>
                <n-icon>
                  <MdAdd />
                </n-icon>
              </template>
              添加参与者
            </n-button>
          </template>
        </n-dynamic-tags>
      </n-form-item>
    </n-form>
    <template #footer>
      <n-space justify="end">
        <n-button @click="addChatForm.visible = false">取消</n-button>
        <n-button type="primary" @click="addChat">保存</n-button>
      </n-space>
    </template>
  </n-modal>
  <n-modal
    title="添加参与者"
    size="medium"
    v-model:show="addParticipantForm.visible"
    preset="card"
    style="width: fit-content; min-width: 25em"
  >
    <n-form label-placement="left">
      <n-form-item label="姓名">
        <n-input v-model:value="addParticipantForm.name" />
      </n-form-item>
      <n-form-item label="模型">
        <n-select v-model:value="addParticipantForm.model" filterable :options="modelOptions" />
      </n-form-item>
      <n-form-item label="预设">
        <n-dynamic-input
          v-model:value="addParticipantForm.presets"
          @create="
            () => {
              let obj = { data: null }
              addParticipantForm.presets!.push(obj)
              return obj
            }
          "
        >
          <template #default="obj">
            <n-select
              @update:value="(v: any) => (obj.value.data = v)"
              filterable
              :options="
                presets.map((p: Preset) => {
                  return {
                    label: p.name,
                    value: p,
                  }
                })
              "
            />
          </template>
        </n-dynamic-input>
      </n-form-item>
      <n-form-item label="角色">
        <n-select
          v-model:value="addParticipantForm.character"
          filterable
          :options="
            characters.map((c: Character) => {
              return {
                label: c.name,
                value: c,
              }
            })
          "
        />
      </n-form-item>
      <n-form-item label="模板">
        <n-select
          v-model:value="addParticipantForm.template"
          filterable
          :options="
            templates.map((t: Template) => {
              return {
                label: t.name,
                value: t,
              }
            })
          "
        />
      </n-form-item>
    </n-form>
    <template #footer>
      <n-space justify="end">
        <n-button @click="addParticipantForm.visible = false">取消</n-button>
        <n-button type="primary" @click="addParticipantForm.onConfirm">保存</n-button>
      </n-space>
    </template>
  </n-modal>
  <n-modal></n-modal>
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
