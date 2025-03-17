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
import type { Chat } from '@/types/chat'
import type { SelectBaseOption } from 'naive-ui/es/select/src/interface'
const props = defineProps<{
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
  settings: {
    nameOfUser: null,
    currentParticipantId: null,
  },
  participants: [],
})

const show = defineModel<boolean>('show', {
  default: false,
})

const addParticipantShow = ref(false)
const addParticipantKey = ref(0)

const startAddParticipant = () => {
  addParticipantKey.value += 1
  addParticipantShow.value = true
}

const mapValue = (p: SelectBaseOption) => {
  return {
    id: p.value as number,
    name: p.label as string,
  }
}

const confirm = async () => {
  if (!validate()) return
  let id = await api.addChat(
    form.value.name!,
    form.value.description,
    form.value.settings,
  )
  let newChat: Chat = {
    id: id,
    name: form.value.name!,
    description: form.value.description,
    settings: form.value.settings,
    participants: [],
  }
  for (let participant of form.value.participants) {
    let pId = await api.addParticipant(
      id,
      participant.character!,
      participant.presets,
      participant.template!,
      participant.model!,
      participant.name!,
      participant.settings,
    )
    newChat.participants.push({
      id: pId,
      name: participant.name!,
      model: participant.model!,
      presets: props.presets
        .filter((p) => participant.presets.includes(p.value as number))
        .map(mapValue),
      character: [
        props.characters.find((c) => c.value === participant.character)!,
      ].map(mapValue)[0],
      template: [
        props.templates.find((t) => t.value === participant.template)!,
      ].map(mapValue)[0],
    })
  }
  emit('confirm', newChat)
  show.value = false
}

const validate = () => {
  if (!form.value.name) {
    message.error('名称不能为空')
    return false
  }

  return true
}

const emit = defineEmits<{
  cancel: []
  confirm: [Chat]
}>()

const onAddParticipantConfirm = (value: AddParticipantForm) => {
  form.value.participants.push(value)
}
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
    :key="addParticipantKey"
    v-model:show="addParticipantShow"
    :models="models"
    :characters="characters"
    :presets="presets"
    :templates="templates"
    @confirm="onAddParticipantConfirm"
  />
</template>
