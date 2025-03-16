<script setup lang="ts">
import type { AddModelForm, AddProviderForm } from '@/types/modal'
import { useMessage } from 'naive-ui'
import {
  NModal,
  NForm,
  NFormItem,
  NInput,
  NSpace,
  NButton,
  NSelect,
  NDynamicTags,
  NIcon,
  NTag,
} from 'naive-ui'
import { MdAdd } from '@vicons/ionicons4'

import { h, ref } from 'vue'
import { api } from '@/api'
import type { Model, Provider } from '@/types/provider'

const renderModelTag = (model: AddModelForm, index: number) => {
  return h(
    NTag,
    {
      closable: true,
      onClose: () => {
        form.value.models.splice(index, 1)
      },
      onClick: () => {
        console.log('click')
      },
    },
    {
      default: () => model.name,
    },
  )
}

const message = useMessage()

const show = defineModel<boolean>('show', {
  default: false,
})

const types = [
  {
    label: 'OpenAI',
    value: 'openai',
  },
  {
    label: 'Azure OpenAI',
    value: 'azure',
  },
  {
    label: 'Local LLM',
    value: 'local',
  },
]

const form = ref<AddProviderForm>({
  name: null,
  description: null,
  type: 'openai',
  baseUrl: null,
  apiKey: null,
  models: [],
  settings: null,
})

const emit = defineEmits<{
  cancel: []
  confirm: [Provider]
}>()

const validate = () => {
  if (!form.value.name) {
    message.error('名称不能为空')
    return false
  }
  if (!form.value.type) {
    message.error('类型不能为空')
    return false
  }
  if (form.value.type === 'openai' && !form.value.apiKey) {
    message.error('API Key不能为空')
    return false
  }
  if (form.value.type === 'azure' && !form.value.baseUrl) {
    message.error('Base URL不能为空')
    return false
  }
  return true
}

const confirm = async () => {
  if (!validate()) return
  show.value = false
  try {
    let providerId = await api.addProvider(
      form.value.name!,
      form.value.description,
      form.value.type,
      form.value.baseUrl!,
      form.value.apiKey!,
      null,
    )
    let newProvider = {
      id: providerId,
      name: form.value.name!,
      description: form.value.description,
      type: form.value.type,
      baseUrl: form.value.baseUrl!,
      apiKey: form.value.apiKey!,
      models: [] as Model[],
      settings: null,
    }
    for (const model of form.value.models) {
      let id = await api.addModel(
        model.name!,
        model.modelName!,
        model.description,
        providerId,
        model.isPublic,
        model.settings,
      )
      newProvider.models.push({
        name: model.name!,
        modelName: model.modelName!,
        description: model.description,
        isPublic: model.isPublic,
        settings: model.settings,
        provider: newProvider,
        id,
      })
    }
    emit('confirm', newProvider)
    message.success('Provider添加成功.')
  } catch (error) {
    console.error(error)
    message.error('Provider添加失败.')
  }
}

const cancel = () => {}
</script>

<template>
  <NModal
    v-model:show="show"
    title="添加提供者"
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
      <NFormItem label="类型">
        <NSelect v-model:value="form.type" :options="types" />
      </NFormItem>
      <NFormItem label="模型">
        <NDynamicTags
          v-model:value="form.models as any[]"
          :render-tag="renderModelTag as any"
        >
          <template #trigger>
            <NButton size="small" type="primary" dashed>
              <template #icon>
                <NIcon>
                  <MdAdd />
                </NIcon>
              </template>
              添加模型
            </NButton>
          </template>
        </NDynamicTags>
      </NFormItem>
      <NFormItem>
        <NSpace justify="end">
          <NButton @click="cancel">取消</NButton>
          <NButton type="primary" @click="confirm">确定</NButton>
        </NSpace>
      </NFormItem>
    </NForm>
  </NModal>
</template>
