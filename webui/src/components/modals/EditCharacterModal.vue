<script setup lang="ts">
import {
  NModal,
  NForm,
  NFormItem,
  NUpload,
  NButton,
  type UploadCustomRequestOptions,
} from 'naive-ui'
import SettingsInput from '../SettingsInput.vue'
import SettingsSwitch from '../SettingsSwitch.vue'
import SettingsDynamicInput from '../SettingsDynamicInput.vue'
import { toRef } from 'vue'
import type { EditCharacterForm } from '@/types/modal/character'
import { api } from '@/api'

const props = defineProps<{
  character: EditCharacterForm
}>()

const form = toRef(props, 'character')
const show = defineModel('show', {
  type: Boolean,
  default: false,
})
const emit = defineEmits(['confirm'])

const uploadAvatar = async ({ file }: UploadCustomRequestOptions) => {
  if (!file.file) {
    return false
  }
  try {
    const buffer = await file.file.arrayBuffer()
    const fileId = await api.uploadFile(buffer)

    await api.updateCharacter(form.value.id, {
      avatar: fileId,
    })

    emit('confirm', form)
    return true
  } catch (e) {
    console.error(e)
    return false
  }
}
</script>

<template>
  <NModal
    v-model:show="show"
    title="编辑角色"
    preset="card"
    style="width: fit-content; min-width: 25em"
    size="medium"
  >
    <NForm v-if="form" label-placement="left">
      <NFormItem label="名称">
        <SettingsInput
          :value="form!.name"
          @confirm="
            async (name) => {
              await api.updateCharacter(form!.id, {
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
              await api.updateCharacter(form!.id, {
                description: description,
              })
              form!.description = description
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="公开">
        <SettingsSwitch
          :value="form!.isPublic"
          @confirm="
            async (isPublic) => {
              await api.updateCharacter(form!.id, {
                isPublic: isPublic,
              })
              form!.isPublic = isPublic
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="头像">
        <NUpload
          action=""
          :max="1"
          accept="image/*"
          :custom-request="uploadAvatar"
        >
          <NButton>上传头像</NButton>
        </NUpload>
      </NFormItem>
      <NFormItem label="内容">
        <SettingsDynamicInput
          :value="form!.content"
          @confirm="
            async (content) => {
              await api.updateCharacter(form!.id, {
                content: content,
              })
              form!.content = content
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
    </NForm>
  </NModal>
</template>
