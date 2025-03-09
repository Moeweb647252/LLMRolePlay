<script setup lang="ts">
import { useMessage } from 'naive-ui'
import { ref } from 'vue'
import {
  NModal,
  NForm,
  NFormItem,
  NInput,
  NUpload,
  NSwitch,
  NDynamicInput,
  NSpace,
  NButton,
} from 'naive-ui'
import type { UploadFileInfo } from 'naive-ui'
import { api } from '@/api'
import type { AddCharacterForm } from '@/types/modal/character'

const show = defineModel<boolean>('show', {
  default: false,
})

const message = useMessage()

const emit = defineEmits(['cancel', 'confirm'])
const fileList = ref([] as UploadFileInfo[])

const form = ref<AddCharacterForm>({
  name: null,
  description: null,
  content: [],
  isPublic: false,
  settings: {},
  avatar: null,
})

const confirm = async () => {
  if (!validate()) return
  api.addCharacter(
    form.value.name!,
    form.value.description,
    form.value.content,
    {},
    form.value.isPublic,
    form.value.avatar,
  )
  emit('confirm', form.value)
}

const cancel = () => {
  emit('cancel')
}

const validate = () => {
  if (!form.value.name) {
    message.error('名称不能为空')
    return false
  }
  return true
}

const uploadAvatar = async () => {
  return true
}
</script>

<template>
  <n-modal
    v-model:show="show"
    title="添加角色"
    size="medium"
    preset="card"
    style="width: fit-content; min-width: 25em"
  >
    <n-form label-placement="left">
      <n-form-item label="名称">
        <n-input v-model:value="form.name" />
      </n-form-item>
      <n-form-item label="头像">
        <n-upload
          v-model:file-list="fileList"
          :multiple="false"
          list-type="image-card"
          :trigger-style="{
            display: fileList.length ? 'none' : 'block',
          }"
          @before-upload="uploadAvatar()"
        />
      </n-form-item>
      <n-form-item label="描述">
        <n-input v-model:value="form.description" />
      </n-form-item>
      <n-form-item label="公开">
        <n-switch v-model:value="form.isPublic" />
      </n-form-item>
      <n-form-item label="设置">
        <n-dynamic-input
          v-model:value="form.content"
          preset="pair"
          key-placeholder="设置名"
          value-placeholder="值"
        />
      </n-form-item>
    </n-form>
    <template #footer>
      <n-space justify="end">
        <n-button @click="cancel"> 取消 </n-button>
        <n-button type="primary" @click="confirm"> 保存 </n-button>
      </n-space>
    </template>
  </n-modal>
</template>
