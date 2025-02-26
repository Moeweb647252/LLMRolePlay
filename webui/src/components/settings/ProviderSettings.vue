<script setup lang="ts">
import { useProviderStore, type Model, type Provider } from '@/stores/providers'
import { h, ref } from 'vue'
import { MdAdd } from '@vicons/ionicons4'
import { NTag, useMessage, useModal } from 'naive-ui'
import { api } from '@/api'
import SettingsInput from '../SettingsInput.vue'

const providers = useProviderStore().providers
const messgae = useMessage()
const model = useModal()

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
  settings: [] as { key: string; value: string }[],
  type: 'openai' as 'openai' | 'google' | 'azure',
})

const renderProviderModelsTag = (model: Model, index: number) => {
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

const deleteProvider = async (provider: any) => {
  model.create({
    title: '删除Provider',
    content: `确定删除Provider ${provider.name} ?`,
    preset: 'dialog',
    positiveText: '确定',
    negativeText: '取消',
    onPositiveClick: async () => {
      try {
        await api.deleteProvider(provider.id)
        providers.splice(providers.indexOf(provider), 1)
      } catch (error) {
        console.error(error)
        messgae.error('Provider删除失败.')
        return
      }
      messgae.success('Provider删除成功.')
    },
  })
}

const addProviderAddModel = () => {
  addModelForm.value = {
    visible: true,
    name: '',
    modelName: '',
    description: '',
    isPublic: false,
    settings: {
      temperture: null,
      top_p: null,
      max_tokens: null,
    },
    onAdd: (data: any) => {
      addProviderForm.value.models.push(data)
    },
  }
}

const addModelConfirm = () => {
  let data = {
    id: null,
    name: addModelForm.value.name,
    modelName: addModelForm.value.modelName,
    description: addModelForm.value.description,
    provider: null,
    settings: addModelForm.value.settings,
  }
  addModelForm.value.onAdd!(data)
  addModelForm.value.visible = false
}

const addModelForm = ref({
  visible: false,
  name: '',
  modelName: '',
  description: '',
  isPublic: false,
  settings: {
    temperture: null,
    top_p: null,
    max_tokens: null,
  },
  onAdd: null as ((data: any) => void) | null,
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
    settings: [] as { key: string; value: string }[],
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
      console.log(id)
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

const editProviderForm = ref({
  visible: false,
  provider: null as Provider | null,
})

const startEditProvider = (provider: any) => {
  editProviderForm.value = {
    visible: true,
    provider: provider,
  }
}

const editProviderAddModel = () => {
  addModelForm.value = {
    isPublic: false,
    visible: true,
    name: '',
    modelName: '',
    description: '',
    settings: {
      temperture: null,
      top_p: null,
      max_tokens: null,
    },
    onAdd: async (model: any) => {
      let id = await api.addModel(
        model.name,
        model.modelName,
        model.description,
        editProviderForm.value.provider!.id,
        model.settings,
      )
      model.id = id
      editProviderForm.value.provider!.models.push(model)
    },
  }
}

const editModelForm = ref({
  visible: false,
  id: null as number | null,
  name: '',
  modelName: '',
  description: '',
  settings: {
    temperture: null,
    top_p: null,
    max_tokens: null,
  },
})

const editProviderEditModel = () => {}

const startEditModel = (model: any) => {
  editModelForm.value = {
    id: model.id,
    visible: true,
    name: model.name,
    modelName: model.modelName,
    description: model.description,
    settings: model.settings,
  }
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

          <template #suffix>
            <n-space :wrap="false">
              <n-button type="primary" @click="startEditProvider(provider)">编辑</n-button>
              <n-button type="error" @click="deleteProvider(provider)">删除</n-button>
            </n-space>
          </template>
        </n-list-item>
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
      <n-dynamic-tags v-model:value="addProviderForm.models" :render-tag="renderProviderModelsTag">
        <template #trigger>
          <n-button size="small" type="primary" dashed @click="addProviderAddModel">
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
      <n-space justify="end">
        <n-button @click="addProviderForm.visible = false">取消</n-button>
        <n-button type="primary" @click="addProvider">添加</n-button>
      </n-space>
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
      <n-form-item label="公开">
        <n-switch v-model:value="addModelForm.isPublic"></n-switch>
      </n-form-item>
      <n-form-item label="Temperture">
        <n-input v-model:value="addModelForm.settings.temperture" type="number"></n-input>
      </n-form-item>
      <n-form-item label="Top P">
        <n-input v-model:value="addModelForm.settings.top_p" type="number"></n-input>
      </n-form-item>
      <n-form-item label="Max Tokens">
        <n-input v-model:value="addModelForm.settings.max_tokens" type="number"></n-input>
      </n-form-item>
    </n-form>
    <template #footer>
      <n-space justify="end">
        <n-button type="primary" @click="addModelConfirm">添加</n-button>
        <n-button @click="addModelForm.visible = false">取消</n-button>
      </n-space>
    </template>
  </n-modal>
  <n-modal
    title="编辑Provider"
    v-model:show="editProviderForm.visible"
    preset="card"
    size="medium"
    style="width: fit-content; min-width: 25em"
  >
    <n-form label-placement="left">
      <n-form-item label="类型">
        <SettingsInput
          v-model:value="editProviderForm.provider!.type"
          @confirm="
            async () =>
              await api.updateProvider(
                editProviderForm.provider!.id,
                null,
                null,
                null,
                null,
                editProviderForm.provider!.type,
              )
          "
          type="select"
          :options="providerTypeOptions"
        >
        </SettingsInput>
      </n-form-item>
      <n-form-item label="名称">
        <SettingsInput
          v-model:value="editProviderForm.provider!.name"
          @confirm="
            async () =>
              await api.updateProvider(
                editProviderForm.provider!.id,
                editProviderForm.provider!.name,
              )
          "
        />
      </n-form-item>
      <n-form-item label="描述">
        <SettingsInput
          v-model:value="editProviderForm.provider!.description"
          @confirm="
            async () =>
              await api.updateProvider(
                editProviderForm.provider!.id,
                null,
                null,
                null,
                editProviderForm.provider!.description,
              )
          "
        />
      </n-form-item>
      <n-form-item label="Base URL">
        <SettingsInput
          v-model:value="editProviderForm.provider!.url"
          @confirm="
            async () =>
              await api.updateProvider(
                editProviderForm.provider!.id,
                null,
                editProviderForm.provider!.url,
              )
          "
        />
      </n-form-item>
      <n-form-item label="API Key">
        <SettingsInput
          v-model:value="editProviderForm.provider!.apiKey"
          @confirm="
            async () =>
              await api.updateProvider(
                editProviderForm.provider!.id,
                null,
                null,
                editProviderForm.provider!.apiKey,
              )
          "
        />
      </n-form-item>
      <n-form-item label="模型">
        <n-dynamic-tags
          v-model:value="editProviderForm.provider!.models"
          :render-tag="renderProviderModelsTag"
        >
          <template #trigger>
            <n-button size="small" type="primary" dashed @click="editProviderAddModel">
              <template #icon>
                <n-icon>
                  <MdAdd />
                </n-icon>
              </template>
              添加模型
            </n-button>
          </template>
        </n-dynamic-tags>
      </n-form-item>
    </n-form>
  </n-modal>
  <n-modal title="编辑Modal">
    <n-form label-placement="left">
      <n-form-item label="名称">
        <SettingsInput
          v-model:value="editModelForm.name"
          @confirm="
            async () => {
              await api.updateModel(editModelForm.id!, editModelForm.name)
            }
          "
        />
      </n-form-item>
      <n-form-item label="模型名">
        <SettingsInput v-model:value="editModelForm.modelName" />
      </n-form-item>
      <n-form-item label="描述">
        <SettingsInput v-model:value="editModelForm.description" />
      </n-form-item>
      <n-form-item label="Temperture">
        <SettingsInput v-model:value="editModelForm.settings.temperture" />
      </n-form-item>
      <n-form-item label="Top P">
        <SettingsInput v-model:value="editModelForm.settings.top_p" />
      </n-form-item>
      <n-form-item label="Max Tokens">
        <SettingsInput v-model:value="editModelForm.settings.max_tokens" />
      </n-form-item>
    </n-form>
  </n-modal>
</template>

<style scoped>
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
</style>
