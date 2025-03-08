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
  <n-modal
    v-model:show="show"
    title="编辑角色"
    preset="card"
    style="width: fit-content; min-width: 25em"
    size="medium"
  >
    <n-form v-if="form" label-placement="left">
      <n-form-item label="名称">
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
      </n-form-item>
      <n-form-item label="描述">
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
      </n-form-item>
      <n-form-item label="公开">
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
      </n-form-item>
      <n-form-item label="头像">
        <n-upload
          action=""
          :max="1"
          accept="image/*"
          :custom-request="uploadAvatar"
        >
          <n-button>上传头像</n-button>
        </n-upload>
      </n-form-item>
      <n-form-item label="内容">
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
      </n-form-item>
    </n-form>
  </n-modal>
</template>
