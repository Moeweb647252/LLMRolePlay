<script setup lang="ts">
import { useMessage } from 'naive-ui'
import { ref } from 'vue'
import {
  NModal,
  NForm,
  NFormItem,
  NInput,
  NSwitch,
  NDynamicInput,
  NSpace,
  NButton,
} from 'naive-ui'
import { api } from '@/api'
import type { AddPresetForm } from '@/types/modal/preset'

const show = defineModel<boolean>('show', {
  default: false,
})

const message = useMessage()

const emit = defineEmits(['cancel', 'confirm'])

const form = ref<AddPresetForm>({
  name: null,
  description: null,
  settings: null,
  content: [],
  isPublic: false,
})

const confirm = async () => {
  if (!validate()) return

  try {
    await api.addPreset(
      form.value.name!,
      form.value.description!,
      form.value.content,
      form.value.settings,
      form.value.isPublic,
    )
    message.success('预设添加成功')
    emit('confirm', form.value)
  } catch (error) {
    message.error('添加预设失败')
    console.error(error)
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
</script>

<template>
  <NModal
    v-model:show="show"
    title="添加预设"
    size="medium"
    preset="card"
    style="width: fit-content; min-width: 25em"
  >
    <NForm label-placement="left">
      <NFormItem label="名称">
        <NInput v-model:value="form.name" placeholder="输入预设名称" />
      </NFormItem>
      <NFormItem label="描述">
        <NInput
          v-model:value="form.description"
          type="textarea"
          placeholder="输入预设描述"
        />
      </NFormItem>
      <NFormItem label="公开">
        <NSwitch v-model:value="form.isPublic" />
      </NFormItem>
      <NFormItem label="设置">
        <NDynamicInput
          v-model:value="form.content"
          preset="pair"
          key-placeholder="参数名"
          value-placeholder="参数值"
        />
      </NFormItem>
    </NForm>
    <template #footer>
      <NSpace justify="end">
        <NButton @click="cancel">取消</NButton>
        <NButton type="primary" @click="confirm">保存</NButton>
      </NSpace>
    </template>
  </NModal>
</template>
