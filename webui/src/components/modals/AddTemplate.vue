<script setup lang="ts">
import type { AddTemplateForm } from '@/types/modal/template'
import { NModal, NForm, NFormItem, NInput, NButton, NSwitch } from 'naive-ui'
import { ref } from 'vue'
import { useMessage } from 'naive-ui'

const message = useMessage()

const show = defineModel<boolean>('show', {
  default: false,
})

const form = ref<AddTemplateForm>({
  name: null,
  description: null,
  content: null,
  isPublic: false,
  settings: {},
})

const emit = defineEmits(['cancel', 'confirm'])

const validate = () => {
  if (!form.value.name) {
    message.error('名称不能为空')
    return false
  }
  if (!form.value.content) {
    message.error('内容不能为空')
    return false
  }
  return true
}

const confirm = () => {
  if (validate()) {
    emit('confirm', form.value)
  }
}
const cancel = () => {
  emit('cancel')
}
</script>

<template>
  <n-modal v-model:show="show" title="添加模板" size="medium">
    <n-form>
      <n-form-item label="名称">
        <n-input v-model:value="form.name" />
      </n-form-item>
      <n-form-item label="描述">
        <n-input v-model:value="form.description" />
      </n-form-item>
      <n-form-item label="内容">
        <n-input v-model:value="form.content" />
      </n-form-item>
      <n-form-item label="是否公开">
        <n-switch v-model:value="form.isPublic" />
      </n-form-item>
      <n-form-item>
        <n-button type="primary" @click="confirm">确定</n-button>
        <n-button @click="cancel">取消</n-button>
      </n-form-item>
    </n-form>
  </n-modal>
</template>
