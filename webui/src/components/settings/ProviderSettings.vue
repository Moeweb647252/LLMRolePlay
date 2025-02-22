<script setup lang="ts">
import { useProviderStore, type Model } from '@/stores/providers'
import { h, ref } from 'vue'
import { MdAdd } from '@vicons/ionicons4'
import { NTag, useMessage } from 'naive-ui'
import { api } from '@/api'

const providers = useProviderStore().providers
const messgae = useMessage()

const providerTypeOptions = [
  { label: 'Openai-Compatible', value: 'openai' },
  {
    label: 'Google',
    value: 'google',
  },
  {
    label: 'Azure',
    value: 'azure',
  },
]

const addProviderForm = ref({
  visible: false,
  name: '',
  description: '',
  url: '',
  apiKey: '',
  models: [] as Model[],
  settings: {},
  type: 'openai' as 'openai' | 'google' | 'azure',
})

const renderProviderTag = (model: Model, index: number) => {
  return h(
    NTag,
    {
      closable: true,
      onClose: () => {
        addProviderForm.value.models.splice(index, 1)
      },
      onClick: () => {
        console.log(model)
      },
    },
    {
      default: () => model.name,
    },
  )
}

const addModel = () => {
  addProviderForm.value.models.push({
    id: null,
    name: addModelForm.value.name,
    modelName: addModelForm.value.modelName,
    description: addModelForm.value.description,
    provider: null,
    settings: addModelForm.value.settings,
  })
  addModelForm.value = {
    visible: false,
    name: '',
    modelName: '',
    description: '',
    settings: {},
  }
}

const addModelForm = ref({
  visible: false,
  name: '',
  modelName: '',
  description: '',
  settings: {},
})

const addProvider = async () => {
  addProviderForm.value.visible = false
  let data = JSON.parse(JSON.stringify(addProviderForm.value))
  addProviderForm.value = {
    visible: false,
    name: '',
    description: '',
    url: '',
    apiKey: '',
    models: [] as Model[],
    settings: {},
    type: 'openai' as 'openai' | 'google' | 'azure',
  }
  try {
    let providerId = await api.addProvider(
      data.name,
      data.url,
      data.apiKey,
      data.description,
      data.settings,
      data.type,
    )
    let models = []
    for (let model of data.models) {
      let id = await api.addModel(
        model.name,
        model.modelName,
        model.description,
        providerId,
        model.settings,
      )
      model.id = id
      models.push(model)
    }
    let provider = {
      id: providerId,
      name: data.name,
      description: data.description,
      url: data.url,
      apiKey: data.apiKey,
      models: models,
      settings: data.settings,
      type: data.type,
    }
    provider.models.forEach((model) => {
      model.provider = provider
    })
    providers.push(provider)
  } catch (error) {
    console.error(error)
    messgae.error('Provider添加失败.')
    return
  }

  messgae.success('Provider添加成功.')
}
</script>
<template>
  <div style="padding: 2em">
    <div class="header">
      <h3>Providers</h3>
      <n-button type="primary" @click="addProviderForm.visible = true">添加</n-button>
    </div>
    <div>
      <n-list>
        <n-list-item v-for="provider in providers" :key="provider.id">
          <n-thing
            :title="provider.name"
            :description="provider.description"
            :title-extra="provider.type"
          >
          </n-thing>
        </n-list-item>
        <template #suffix>
          <n-button type="text">编辑</n-button>
          <n-button type="text">删除</n-button>
        </template>
      </n-list>
    </div>
  </div>
  <n-modal
    v-model:show="addProviderForm.visible"
    preset="card"
    title="添加Provider"
    size="medium"
    style="width: fit-content; min-width: 25em"
  >
    <n-form label-placement="left">
      <n-form-item label="类型">
        <n-select :options="providerTypeOptions" v-model:value="addProviderForm.type"></n-select>
      </n-form-item>
      <n-form-item label="名称">
        <n-input v-model:value="addProviderForm.name"></n-input>
      </n-form-item>
      <n-form-item label="描述">
        <n-input v-model:value="addProviderForm.description"></n-input>
      </n-form-item>
      <n-form-item label="Base URL">
        <n-input v-model:value="addProviderForm.url"></n-input>
      </n-form-item>
      <n-form-item label="API Key">
        <n-input v-model:value="addProviderForm.apiKey"></n-input>
      </n-form-item>
      <n-dynamic-tags v-model:value="addProviderForm.models" :render-tag="renderProviderTag">
        <template #trigger>
          <n-button size="small" type="primary" dashed @click="addModelForm.visible = true">
            <template #icon>
              <n-icon>
                <MdAdd />
              </n-icon>
            </template>
            添加模型
          </n-button>
        </template>
      </n-dynamic-tags>
    </n-form>
    <template #footer>
      <n-button type="primary" @click="addProvider">添加</n-button>
      <n-button @click="addProviderForm.visible = false">取消</n-button>
    </template>
  </n-modal>
  <n-modal
    v-model:show="addModelForm.visible"
    preset="card"
    title="添加模型"
    size="medium"
    style="width: fit-content; min-width: 25em"
  >
    <n-form label-placement="left">
      <n-form-item label="名称">
        <n-input v-model:value="addModelForm.name"></n-input>
      </n-form-item>
      <n-form-item label="模型名">
        <n-input v-model:value="addModelForm.modelName"></n-input>
      </n-form-item>
      <n-form-item label="描述">
        <n-input v-model:value="addModelForm.description"></n-input>
      </n-form-item>
    </n-form>
    <template #footer>
      <n-button type="primary" @click="addModel">添加</n-button>
      <n-button>取消</n-button>
    </template>
  </n-modal>
</template>

<style scoped>
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
</style>
