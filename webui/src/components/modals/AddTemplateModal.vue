<script setup lang="ts">
import type { AddTemplateForm } from '@/types/modal/template'
import { NModal, NForm, NFormItem, NInput, NButton, NSwitch } from 'naive-ui'
import { ref } from 'vue'
import { useMessage } from 'naive-ui'
import { api } from '@/api'
import type { Template } from '@/types/template'

const message = useMessage()

const show = defineModel<boolean>('show', {
  default: false,
})

const form = ref<AddTemplateForm>({
  name: null,
  description: null,
  content: '',
  isPublic: false,
  settings: {},
})

const emit = defineEmits<{
  cancel: []
  confirm: [Template]
}>()

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

const confirm = async () => {
  if (!validate()) return
  try {
    let id = await api.addTemplate(
      form.value.name!,
      form.value.content!,
      form.value.description,
      form.value.isPublic,
      form.value.settings,
    )
    let newTemplate = {
      id: id,
      name: form.value.name!,
      description: form.value.description,
      content: form.value.content!,
      isPublic: form.value.isPublic,
      settings: form.value.settings,
    }
    message.success('添加成功')
    emit('confirm', newTemplate)
    show.value = false
  } catch (e) {
    console.log(e)
    message.error('添加失败')
  }
}
const cancel = () => {
  emit('cancel')
}
</script>

<template>
  <NModal
    v-model:show="show"
    title="添加模板"
    size="medium"
    preset="card"
    style="width: fit-content; min-width: 25em"
  >
    <NForm>
      <NFormItem label="名称">
        <NInput v-model:value="form.name" />
      </NFormItem>
      <NFormItem label="描述">
        <NInput v-model:value="form.description" />
      </NFormItem>
      <NFormItem label="内容">
        <NInput v-model:value="form.content" type="textarea" />
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
