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
  <NModal v-model:show="show" title="添加模板" size="medium">
    <NForm>
      <NFormItem label="名称">
        <NInput v-model:value="form.name" />
      </NFormItem>
      <NFormItem label="描述">
        <NInput v-model:value="form.description" />
      </NFormItem>
      <NFormItem label="内容">
        <NInput v-model:value="form.content" />
      </NFormItem>
      <NFormItem label="是否公开">
        <NSwitch v-model:value="form.isPublic" />
      </NFormItem>
      <NFormItem>
        <NButton type="primary" @click="confirm">确定</NButton>
        <NButton @click="cancel">取消</NButton>
      </NFormItem>
    </NForm>
  </NModal>
</template>
