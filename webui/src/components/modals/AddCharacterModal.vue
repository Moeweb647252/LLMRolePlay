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
import type { AddCharacterForm } from '@/types/modal/character'
import { api } from '@/api'
import type { Character } from '@/types/character'

const show = defineModel<boolean>('show', {
  default: false,
})

const message = useMessage()

const emit = defineEmits<{
  cancel: []
  confirm: [Character]
}>()
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
  show.value = false
  try {
    let id = await api.addCharacter(
      form.value.name!,
      form.value.description,
      form.value.content,
      form.value.settings,
      form.value.isPublic,
      form.value.avatar,
    )
    message.success('添加成功')
    emit('confirm', {
      id: id,
      name: form.value.name!,
      description: form.value.description,
      content: form.value.content,
      isPublic: form.value.isPublic,
      settings: form.value.settings,
      avatar: form.value.avatar,
    })
  } catch (e) {
    console.log(e)
    message.error('添加失败')
  }
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
  <NModal
    v-model:show="show"
    title="添加角色"
    size="medium"
    preset="card"
    style="width: fit-content; min-width: 25em"
  >
    <NForm label-placement="left">
      <NFormItem label="名称">
        <NInput v-model:value="form.name" />
      </NFormItem>
      <NFormItem label="头像">
        <NUpload
          v-model:file-list="fileList"
          :multiple="false"
          list-type="image-card"
          :trigger-style="{
            display: fileList.length ? 'none' : 'block',
          }"
          @before-upload="uploadAvatar()"
        />
      </NFormItem>
      <NFormItem label="描述">
        <NInput v-model:value="form.description" />
      </NFormItem>
      <NFormItem label="公开">
        <NSwitch v-model:value="form.isPublic" />
      </NFormItem>
      <NFormItem label="设置">
        <NDynamicInput
          v-model:value="form.content"
          preset="pair"
          key-placeholder="设置名"
          value-placeholder="值"
        />
      </NFormItem>
    </NForm>
    <template #footer>
      <NSpace justify="end">
        <NButton @click="cancel"> 取消 </NButton>
        <NButton type="primary" @click="confirm"> 保存 </NButton>
      </NSpace>
    </template>
  </NModal>
</template>
