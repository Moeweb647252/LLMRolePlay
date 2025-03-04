<script setup lang="ts">
import { NTag, useMessage } from 'naive-ui'
import { h, ref } from 'vue'
import AddParticipantModal from './AddParticipantModal.vue'
import type { AddParticipantForm, AddChatForm, Options } from '@/types/modals'

defineProps<{
  models: Options[]
  presets: Options[]
  characters: Options[]
  templates: Options[]
}>()

const message = useMessage()

const renderParticipantTags = (participant: AddParticipantForm, index: number) => {
  return h(
    NTag,
    {
      closable: true,
      onClose: () => {
        form.value.participants.splice(index, 1)
      },
      onClick: () => {},
    },
    {
      default: () => participant.name,
    },
  )
}

const form = ref<AddChatForm>({
  name: '',
  description: null,
  settings: {},
  participants: [],
})
const isShowAddParticipantModal = ref(false)
const show = defineModel<boolean>('show', {
  default: false,
})

const startAddParticipant = () => {
  form.value.participants.push()
}

const confirm = () => {
  if (!validate()) return
  emit('confirm', form.value)
}

const validate = () => {
  if (!form.value.name) {
    message.error('名称不能为空')
    return false
  }

  return true
}

const emit = defineEmits(['cancel', 'confirm'])
</script>

<template>
  <n-modal
    v-model:show="show"
    preset="card"
    title="添加聊天"
    size="medium"
    style="width: fit-content; min-width: 25em"
  >
    <n-form label-placement="left">
      <n-form-item label="名称">
        <n-input v-model:value="form.name" />
      </n-form-item>
      <n-form-item label="描述">
        <n-input v-model:value="form.description" />
      </n-form-item>
      <n-form-item label="参与者">
        <n-dynamic-tags
          v-model:value="form.participants"
          :render-tag="renderParticipantTags"
        >
          <template #trigger>
            <n-button
              size="small"
              type="primary"
              dashed
              @click="startAddParticipant"
            >
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
        <n-button @click="emit('cancel')">
          取消
        </n-button>
        <n-button
          type="primary"
          @click="confirm"
        >
          保存
        </n-button>
      </n-space>
    </template>
  </n-modal>
  <AddParticipantModal
    :models="models"
    :characters="characters"
    :presets="presets"
    :templates="templates"
    :show="isShowAddParticipantModal"
  />
</template>
