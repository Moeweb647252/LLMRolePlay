<script setup lang="ts">
import { useMessage } from 'naive-ui'
import {
  NModal,
  NFormItem,
  NForm,
  NInput,
  NButton,
  NDynamicTags,
  NSpace,
  NTag,
  NIcon,
} from 'naive-ui'
import { MdAdd } from '@vicons/ionicons4'
import { h, ref } from 'vue'
import AddParticipantModal from './AddParticipantModal.vue'
import type { AddParticipantForm, AddChatForm, Options } from '@/types/modal'
import { api } from '@/api'

defineProps<{
  models: Options
  presets: Options
  characters: Options
  templates: Options
}>()

const message = useMessage()

const renderParticipantTags = (
  participant: AddParticipantForm,
  index: number,
) => {
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
  name: null,
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
  api.addChat(form.value.name!, form.value.description, form.value.participants)
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
  <NModal
    v-model:show="show"
    preset="card"
    title="添加聊天"
    size="medium"
    style="width: fit-content; min-width: 25em"
  >
    <NForm label-placement="left">
      <NFormItem label="名称">
        <NInput v-model:value="form.name" />
      </NFormItem>
      <NFormItem label="描述">
        <NInput v-model:value="form.description" />
      </NFormItem>
      <NFormItem label="参与者">
        <NDynamicTags
          v-model:value="form.participants as any[]"
          :render-tag="renderParticipantTags as any"
        >
          <template #trigger>
            <NButton
              size="small"
              type="primary"
              dashed
              @click="startAddParticipant"
            >
              <template #icon>
                <NIcon>
                  <MdAdd />
                </NIcon>
              </template>
              添加参与者
            </NButton>
          </template>
        </NDynamicTags>
      </NFormItem>
    </NForm>
    <template #footer>
      <NSpace justify="end">
        <NButton @click="emit('cancel')"> 取消 </NButton>
        <NButton type="primary" @click="confirm"> 保存 </NButton>
      </NSpace>
    </template>
  </NModal>
  <AddParticipantModal
    :models="models"
    :characters="characters"
    :presets="presets"
    :templates="templates"
    :show="isShowAddParticipantModal"
  />
</template>
