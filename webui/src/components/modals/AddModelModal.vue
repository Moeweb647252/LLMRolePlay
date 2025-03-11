<script setup lang="ts">
import { useMessage } from 'naive-ui'
import { ref } from 'vue'
import {
  NModal,
  NFormItem,
  NForm,
  NInput,
  NSpace,
  NButton,
  NInputNumber,
  NSwitch,
} from 'naive-ui'
import type { AddModelForm } from '@/types/modal'

const show = defineModel<boolean>('show', {
  default: false,
})

const message = useMessage()

const emit = defineEmits(['cancel', 'confirm'])

const form = ref<AddModelForm>({
  name: null,
  description: null,
  modelName: null,
  isPublic: false,
  settings: {
    temperature: null,
    top_p: null,
    max_tokens: null,
  },
})

const confirm = () => {
  if (!validate()) return
  emit('confirm', form.value)
}

const validate = () => {
  if (!form.value.name) {
    message.error('名称不能为空')
    return false
  }
  if (!form.value.modelName) {
    message.error('模型名称不能为空')
    return false
  }
  return true
}
</script>

<template>
  <NModal
    v-model:show="show"
    title="添加模型"
    size="medium"
    preset="card"
    style="width: fit-content; min-width: 25em"
    @cancel="emit('cancel')"
  >
    <NForm label-placement="left">
      <NFormItem label="名称" required>
        <NInput v-model:value="form.name" placeholder="请输入模型名称" />
      </NFormItem>
      <NFormItem label="描述">
        <NInput
          v-model:value="form.description"
          type="textarea"
          placeholder="请输入模型描述"
        />
      </NFormItem>
      <NFormItem label="模型名称" required>
        <NInput
          v-model:value="form.modelName"
          placeholder="请输入模型的技术名称"
        />
      </NFormItem>
      <NFormItem label="是否公开">
        <NSwitch v-model:value="form.isPublic" />
      </NFormItem>
      <NFormItem label="温度">
        <NInputNumber
          v-model:value="form.settings.temperature"
          :min="0"
          :max="2"
          :step="0.1"
        />
      </NFormItem>
      <NFormItem label="Top P">
        <NInputNumber
          v-model:value="form.settings.top_p"
          :min="0"
          :max="1"
          :step="0.05"
        />
      </NFormItem>
      <NFormItem label="最大Token数">
        <NInputNumber
          v-model:value="form.settings.max_tokens"
          :min="1"
          :step="100"
        />
      </NFormItem>
    </NForm>
    <template #footer>
      <NSpace justify="end">
        <NButton @click="emit('cancel')"> 取消 </NButton>
        <NButton type="primary" @click="confirm"> 保存 </NButton>
      </NSpace>
    </template>
  </NModal>
</template>
