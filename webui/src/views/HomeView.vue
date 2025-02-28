<script setup lang="ts">
import { api } from '@/api'
import ChatBox from '@/components/ChatBox.vue'
import { useCharacterStore, type Character } from '@/stores/characters'
import type { Chat, Participant } from '@/stores/chats'
import { usePresetStore, type Preset } from '@/stores/presets'
import { useProviderStore } from '@/stores/providers'
import { useSettingsStore } from '@/stores/settings'
import { useTemplateStore, type Template } from '@/stores/templates'
import { IosMenu, MdAdd, MdContact } from '@vicons/ionicons4'
import { NTag, type UploadFileInfo } from 'naive-ui'
import { computed, h, ref } from 'vue'
import { useRouter } from 'vue-router'

const settings = useSettingsStore()
const router = useRouter()
const showSider = ref(true)

const providers = useProviderStore().providers
const presets = usePresetStore().presets
const characters = useCharacterStore().characters
const templates = useTemplateStore().templates

const currentChat = ref(null as Chat | null)

const renderAddChatParticipant = (paticipant: Participant, index: number) => {
  return h(
    NTag,
    {
      closable: true,
      onClose: () => {},
      onClick: () => {},
    },
    {},
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

const startAddChat = () => {
  addChatForm.value.visible = true
}

const addParticipantForm = ref({
  visible: false,
  name: '',
  settings: {},
  modelId: null as null | number,
  presetIds: null as null | { id: number | null }[],
  characterId: null as null | number,
  templateId: null as null | number,
  avatarFileList: [] as UploadFileInfo[],
  onConfirm: () => {},
})

const addChatAddParticipant = () => {
  addParticipantForm.value = {
    visible: true,
    name: '',
    settings: {},
    modelId: null,
    presetIds: [],
    characterId: null,
    templateId: null,
    avatarFileList: [] as UploadFileInfo[],
    onConfirm: () => {
      addChatForm.value.participants.push({
        name: addParticipantForm.value.name,
        settings: addParticipantForm.value.settings,
        modelId: addParticipantForm.value.modelId,
        presetIds: addParticipantForm.value.presetIds,
        characterId: addParticipantForm.value.characterId,
        templateId: addParticipantForm.value.templateId,
      })
      addParticipantForm.value.visible = false
    },
  }
}

const modelOptions = computed(() => {
  return providers
    .map((providers) => providers.models)
    .flat()
    .map((model) => ({
      label: model.name + `(${model.provider!.name})`,
      value: model.id,
    }))
})

const uploadAvatar = async (
  file: {
    file: UploadFileInfo
    fileList: UploadFileInfo[]
  },
  value: any,
) => {
  return true
}

const addChat = async () => {
  const chatId = await api.addChat(addChatForm.value.name, addChatForm.value.description)
  const participants = []
  for (const participant of addChatForm.value.participants) {
    if (!participant) continue
    let fileId = null
    if (participant.avatarFileList.length > 0) {
      const buffer = participant.avatarFileList[0].file.file?.arrayBuffer()
      if (buffer) {
        fileId = await api.uploadFile(buffer)
      }
    }
    const participantId = await api.addParticipant(
      chatId,
      participant.characterId,
      participant.presetIds,
      participant.templateId,
      participant.modelId,
      participant.name,
      fileId,
      settings,
    )
    participants.push({
      id: participantId,
      name: participant.name,
      settings: participant.settings,
      modelId: participant.modelId,
    })
  }
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
            <n-list-item v-for="item in 10" :key="item" style="height: 2.5em; margin-left: 1em">
              Chat {{ item }}
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
        <ChatBox v-if="currentChat" :chat="currentChat"></ChatBox>
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
      <n-form-item label="头像">
        <n-upload
          v-model:file-list="addParticipantForm.avatarFileList"
          @before-upload="uploadAvatar($event, addParticipantForm)"
          :multiple="false"
          list-type="image-card"
          :trigger-style="{ display: addParticipantForm.avatarFileList.length ? 'none' : 'block' }"
        />
      </n-form-item>
      <n-form-item label="模型">
        <n-select v-model:value="addParticipantForm.modelId" filterable :options="modelOptions" />
      </n-form-item>
      <n-form-item label="预设">
        <n-dynamic-input
          v-model:value="addParticipantForm.presetIds"
          @create="
            () => {
              let obj = { id: null }
              addParticipantForm.presetIds!.push(obj)
              return obj
            }
          "
        >
          <template #default="obj">
            <n-select
              @update:value="(v: any) => (obj.value.id = v)"
              filterable
              :options="
                presets.map((p: Preset) => {
                  return {
                    label: p.name,
                    value: p.id,
                  }
                })
              "
            />
          </template>
        </n-dynamic-input>
      </n-form-item>
      <n-form-item label="角色">
        <n-select
          v-model:value="addParticipantForm.characterId"
          filterable
          :options="
            characters.map((c: Character) => {
              return {
                label: c.name,
                value: c.id,
              }
            })
          "
        />
      </n-form-item>
      <n-form-item label="模板">
        <n-select
          v-model:value="addParticipantForm.templateId"
          filterable
          :options="
            templates.map((t: Template) => {
              return {
                label: t.name,
                value: t.id,
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
</style>
