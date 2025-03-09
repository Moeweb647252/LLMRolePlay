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
  <n-modal
    v-model:show="show"
    title="添加模型"
    size="medium"
    preset="card"
    style="width: fit-content; min-width: 25em"
    @cancel="emit('cancel')"
  >
    <n-form label-placement="left">
      <n-form-item label="名称" required>
        <n-input v-model:value="form.name" placeholder="请输入模型名称" />
      </n-form-item>
      <n-form-item label="描述">
        <n-input
          v-model:value="form.description"
          type="textarea"
          placeholder="请输入模型描述"
        />
      </n-form-item>
      <n-form-item label="模型名称" required>
        <n-input
          v-model:value="form.modelName"
          placeholder="请输入模型的技术名称"
        />
      </n-form-item>
      <n-form-item label="是否公开">
        <n-switch v-model:value="form.isPublic" />
      </n-form-item>
      <n-form-item label="温度">
        <n-input-number
          v-model:value="form.settings.temperature"
          :min="0"
          :max="2"
          :step="0.1"
        />
      </n-form-item>
      <n-form-item label="Top P">
        <n-input-number
          v-model:value="form.settings.top_p"
          :min="0"
          :max="1"
          :step="0.05"
        />
      </n-form-item>
      <n-form-item label="最大Token数">
        <n-input-number
          v-model:value="form.settings.max_tokens"
          :min="1"
          :step="100"
        />
      </n-form-item>
    </n-form>
    <template #footer>
      <n-space justify="end">
        <n-button @click="emit('cancel')"> 取消 </n-button>
        <n-button type="primary" @click="confirm"> 保存 </n-button>
      </n-space>
    </template>
  </n-modal>
</template>
