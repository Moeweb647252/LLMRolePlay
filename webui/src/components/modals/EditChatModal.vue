<script setup lang="ts">
import {
  NModal,
  NForm,
  NFormItem,
  NButton,
  NDynamicTags,
  NIcon,
  NTag,
  useModal,
  useMessage,
} from 'naive-ui'
import { MdAdd } from '@vicons/ionicons4'
import SettingsInput from '../SettingsInput.vue'
import { ref, h, toRef } from 'vue'
import type {
  AddParticipantForm,
  EditChatForm,
  EditParticipantForm,
  Options,
} from '@/types/modal'
import { api } from '@/api'
import EditParticipantModal from './EditParticipantModal.vue'
import AddParticipantModal from './AddParticipantModal.vue'

const modal = useModal()
const message = useMessage()

const props = defineProps<{
  models: Options
  presets: Options
  characters: Options
  templates: Options
  value: EditChatForm
}>()

const renderParticipantTags = (participant: EditParticipantForm) => {
  return h(
    NTag,
    {
      style: {
        'user-select': 'none',
      },
      closable: true,
      onClose: () => {
        modal.create({
          title: '删除参与者',
          content: `确定删除参与者 ${participant.name} ?`,
          preset: 'dialog',
          positiveText: '确定',
          negativeText: '取消',
          onPositiveClick: async () => {
            try {
              await api.deleteParticipant(participant.id!)
              form.value!.participants.splice(
                form.value!.participants.indexOf(participant),
                1,
              )
              message.success('删除成功')
            } catch (e) {
              console.log(e)
              message.error('删除失败')
            }
          },
        })
      },
      onClick: () => {
        console.log('click')
        startEditParticipant(participant)
      },
    },
    {
      default: () => participant.name,
    },
  )
}

const editingParticipant = ref(null as EditParticipantForm | null)

const startEditParticipant = (participant: EditParticipantForm) => {
  EditParticipantModalKey.value++
  editingParticipant.value = participant
  showEditParticipantModal.value = true
}

const startAddParticipant = () => {
  AddParticipantModalKey.value++
  showAddParticipantModal.value = true
}

const show = defineModel<boolean>('show', {
  default: false,
})
const onEditParticipantConfirm = () => {
  form.value!.participants[
    form.value!.participants.indexOf(editingParticipant.value!)
  ] = editingParticipant.value!
}
const onAddParticipantConfirm = async (participant: AddParticipantForm) => {
  let pId = await api.addParticipant(
    form.value!.id!,
    participant.character!,
    participant.presets,
    participant.template!,
    participant.model!,
    participant.name!,
    participant.settings,
  )
  let newParticipant = {
    id: pId,
    ...participant,
  }
  form.value!.participants.push(newParticipant as EditParticipantForm)
}
const showEditParticipantModal = ref(false)
const EditParticipantModalKey = ref(0)
const showAddParticipantModal = ref(false)
const AddParticipantModalKey = ref(0)

const form = toRef(props, 'value')

const emit = defineEmits(['confirm'])
</script>

<template>
  <NModal
    v-model:show="show"
    title="编辑聊天"
    preset="card"
    style="width: fit-content; min-width: 25em"
    size="medium"
  >
    <NForm v-if="form" label-placement="left">
      <NFormItem label="名称">
        <SettingsInput
          :value="form.name"
          @confirm="
            async (name) => {
              await api.updateChat(form!.id, {
                name: name,
              })
              form!.name = name
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="描述">
        <SettingsInput
          :value="form!.description"
          @confirm="
            async (description) => {
              if (description) {
                await api.updateChat(form!.id!, {
                  description: description,
                })
                form!.description = description
              }
            }
          "
        />
      </NFormItem>
      <NFormItem label="参与者">
        <NDynamicTags
          v-model:value="form!.participants as any[]"
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
  </NModal>
  <AddParticipantModal
    :key="AddParticipantModalKey"
    v-model:show="showAddParticipantModal"
    :characters="characters"
    :models="models"
    :presets="presets"
    :templates="templates"
    @confirm="onAddParticipantConfirm"
  />
  <EditParticipantModal
    :key="EditParticipantModalKey"
    v-model:show="showEditParticipantModal"
    :characters="characters"
    :models="models"
    :presets="presets"
    :templates="templates"
    :value="editingParticipant!"
    @confirm="
      (value) => {
        editingParticipant = value
        onEditParticipantConfirm()
      }
    "
  />
</template>
