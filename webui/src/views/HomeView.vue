<script setup lang="ts">
import ChatBox from '@/components/ChatBox.vue'
import { useCharacterStore } from '@/stores/characters'
import type { Chat, Participant } from '@/stores/chats'
import { usePresetStore } from '@/stores/presets'
import { useProviderStore } from '@/stores/providers'
import { useSettingsStore } from '@/stores/settings'
import { IosMenu, MdAdd, MdContact } from '@vicons/ionicons4'
import { NTag } from 'naive-ui'
import { computed, h, ref } from 'vue'
import { useRouter } from 'vue-router'

const settings = useSettingsStore()
const router = useRouter()
const showSider = ref(true)

const providers = useProviderStore().providers
const presets = usePresetStore().presets
const characters = useCharacterStore().characters

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
  participants: [],
})

const startAddChat = () => {
  addChatForm.value.visible = true
}

const addParticipantForm = ref({
  visible: false,
  name: '',
  settings: {},
  modelId: null as null | number,
  presetId: null as null | number,
  characterId: null as null | number,
  avatar: null as null | number,
  onConfirm: () => {},
})

const addChatAddParticipant = () => {
  addParticipantForm.value = {
    visible: true,
    name: '',
    settings: {},
    modelId: null,
    presetId: null,
    characterId: null,
    avatar: null,
    onConfirm: () => {},
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
      <n-form-item label="模型">
        <n-select v-model:value="addParticipantForm.modelId" :options="modelOptions" />
      </n-form-item>
      <n-form-item label="预设">
        <n-select
          v-model:value="addParticipantForm.presetId"
          :options="
            presets.map((p) => {
              return {
                label: p.name,
                value: p.id,
              }
            })
          "
        />
      </n-form-item>
      <n-form-item label="角色">
        <n-select
          v-model:value="addParticipantForm.characterId"
          :options="
            characters.map((c) => {
              return {
                label: c.name,
                value: c.id,
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
