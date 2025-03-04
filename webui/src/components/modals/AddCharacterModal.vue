<script setup lang="ts">
import { useMessage } from 'naive-ui'
import { ref } from 'vue'
import { NModal } from 'naive-ui'
import type { UploadFileInfo } from 'naive-ui'

const show = defineModel<boolean>('show', {
  default: false,
})

const message = useMessage()

const emit = defineEmits(['cancel', 'confirm'])

const form = ref({
  name: '',
  description: '',
  content: [],
  isPublic: false,
  avatarFileList: [] as UploadFileInfo[],
})

const confirm = () => {
  if (!validate()) return

  const formData = {
    name: form.value.name,
    description: form.value.description,
    content: form.value.content,
    isPublic: form.value.isPublic,
    avatarFileList: form.value.avatarFileList,
  }

  emit('confirm', formData)
  resetForm()
}

const cancel = () => {
  resetForm()
  emit('cancel')
}

const resetForm = () => {
  form.value = {
    name: '',
    description: '',
    content: [],
    isPublic: false,
    avatarFileList: [],
  }
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
    @cancel="cancel"
  >
    <n-form label-placement="left">
      <n-form-item label="名称">
        <n-input v-model:value="form.name" />
      </n-form-item>
      <n-form-item label="头像">
        <n-upload
          v-model:file-list="form.avatarFileList"
          :multiple="false"
          list-type="image-card"
          :trigger-style="{
            display: form.avatarFileList.length ? 'none' : 'block',
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
        <n-button @click="cancel">
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
</template>
