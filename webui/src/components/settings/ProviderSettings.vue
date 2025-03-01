<script setup lang="ts">
import { Model, Provider } from '@/types/provider'
import { h, ref } from 'vue'
import { MdAdd, MdCreate } from '@vicons/ionicons4'
import { NButton, NIcon, NTag, NDynamicTags, useMessage, useModal } from 'naive-ui'
import { api } from '@/api'
import SettingsInput from '../SettingsInput.vue'
import SettingsNumberInput from '../SettingsNumberInput.vue'
import SettingsSwitch from '../SettingsSwitch.vue'

const providers = ref(await api.getProviders())
const message = useMessage()
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
  {
    label: 'openrouter',
    value: 'openrouter',
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

const renderProviderModelsTag = (_model: Model, index: number, onEdit: (model: Model) => void) => {
  return h(
    NTag,
    {
      closable: true,
      onClose: () => {
        model.create({
          title: '删除Model',
          content: `确定删除Model ${_model.name} ?`,
          preset: 'dialog',
          positiveText: '确定',
          negativeText: '取消',
          onPositiveClick: async () => {
            try {
              await api.deleteModel(_model.id!)
              editProviderForm.value.provider!.models.splice(index, 1)
            } catch (error) {
              console.error(error)
              message.error('Model删除失败.')
              return
            }
            message.success('Model删除成功.')
          },
        })
      },
    },
    {
      default: () =>
        h(
          'div',
          {
            style: {
              display: 'flex',
              'align-items': 'center',
            },
          },
          [
            _model.name,
            h(
              NButton,
              {
                type: 'default',
                text: true,
                style: {
                  'text-size': '1em',
                  'margin-left': '0.5em',
                  width: '1em',
                  height: '1em',
                },
                class: 'edit-icon',
                onClick: () => {
                  onEdit(_model)
                },
              },
              {
                icon: () =>
                  h(NIcon, {
                    component: MdCreate,
                    class: 'n-base-icon n-base-close',
                  }),
              },
            ),
          ],
        ),
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
        providers.value.splice(providers.value.indexOf(provider), 1)
      } catch (error) {
        console.error(error)
        message.error('Provider删除失败.')
        return
      }
      message.success('Provider删除成功.')
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
    providers.value.push(provider)
  } catch (error) {
    console.error(error)
    message.error('Provider添加失败.')
    return
  }

  message.success('Provider添加成功.')
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
        editProviderForm.value.provider!.id!,
        model.settings,
      )
      model.id = id
      editProviderForm.value.provider!.models.push(model)
    },
  }
}

const editModelForm = ref({
  visible: false,
  model: null as Model | null,
})

const editProviderEditModel = (model: Model) => {
  editModelForm.value = {
    visible: true,
    model: model,
  }
}

const startEditModel = (model: Model) => {
  editModelForm.value = {
    visible: true,
    model: model,
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
      <n-dynamic-tags
        v-model:value="addProviderForm.models as any"
        :render-tag="
          (a: any, b: any) =>
            h(
              NTag,
              { closable: true, onClose: () => addProviderForm.models.splice(b, 1) },
              { default: () => a.name },
            )
        "
      >
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
    <n-form label-placement="left" v-if="editProviderForm.provider">
      <n-form-item label="类型">
        <SettingsInput
          v-model:value="editProviderForm.provider.type"
          @confirm="
            async () =>
              await api.updateProvider(editProviderForm.provider!.id!, {
                type: editProviderForm.provider!.type!,
              })
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
              await api.updateProvider(editProviderForm.provider!.id!, {
                name: editProviderForm.provider!.name!,
              })
          "
        />
      </n-form-item>
      <n-form-item label="描述">
        <SettingsInput
          v-model:value="editProviderForm.provider!.description"
          @confirm="
            async () =>
              await api.updateProvider(editProviderForm.provider!.id!, {
                description: editProviderForm.provider!.description!,
              })
          "
        />
      </n-form-item>
      <n-form-item label="Base URL">
        <SettingsInput
          v-model:value="editProviderForm.provider!.url"
          @confirm="
            async () =>
              await api.updateProvider(editProviderForm.provider!.id!, {
                url: editProviderForm.provider!.url!,
              })
          "
        />
      </n-form-item>
      <n-form-item label="API Key">
        <SettingsInput
          v-model:value="editProviderForm.provider!.apiKey"
          @confirm="
            async () =>
              await api.updateProvider(editProviderForm.provider!.id!, {
                apiKey: editProviderForm.provider!.apiKey!,
              })
          "
        />
      </n-form-item>
      <n-form-item label="模型">
        <n-dynamic-tags
          v-model:value="editProviderForm.provider!.models as any"
          tag-class="model-tag"
          :render-tag="(a: any, b: any) => renderProviderModelsTag(a, b, editProviderEditModel)"
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
  <n-modal
    title="编辑Modal"
    preset="card"
    v-model:show="editModelForm.visible"
    size="medium"
    style="width: fit-content; min-width: 25em"
  >
    <n-form label-placement="left" v-if="editModelForm.model">
      <n-form-item label="名称">
        <SettingsInput
          :value="editModelForm.model.name"
          @confirm="
            async () => {
              await api.updateModel(editModelForm.model!.id!, {
                name: editModelForm.model!.name!,
              })
              editModelForm.model!.name = editModelForm.model!.name!
            }
          "
        />
      </n-form-item>
      <n-form-item label="模型名">
        <SettingsInput
          :value="editModelForm.model.modelName"
          @confirm="
            async () => {
              await api.updateModel(editModelForm.model!.id!, {
                modelName: editModelForm.model!.modelName!,
              })
              editModelForm.model!.modelName = editModelForm.model!.modelName!
            }
          "
        />
      </n-form-item>
      <n-form-item label="描述">
        <SettingsInput
          value="editModelForm.model.description"
          @confirm="
            async () => {
              await api.updateModel(editModelForm.model!.id!, {
                description: editModelForm.model!.description!,
              })
              editModelForm.model!.description = editModelForm.model!.description!
            }
          "
        />
      </n-form-item>
      <n-form-item label="公开">
        <SettingsSwitch
          :value="editModelForm.model!.isPublic"
          @confirm="
            async (isPublic: boolean) => {
              await api.updateModel(editModelForm.model!.id!, {
                isPublic: isPublic,
              })
              editModelForm.model!.isPublic = isPublic
            }
          "
        ></SettingsSwitch>
      </n-form-item>
      <n-form-item label="Temperture">
        <SettingsNumberInput
          v-model:value="editModelForm.model!.settings.temperture"
          @confirm="
            async () => {
              await api.updateModel(editModelForm.model!.id!, {
                settings: {
                  temperture: editModelForm.model!.settings.temperture,
                },
              })
            }
          "
        />
      </n-form-item>
      <n-form-item label="Top P">
        <SettingsNumberInput
          v-model:value="editModelForm.model!.settings.top_p"
          @confirm="
            async () => {
              await api.updateModel(editModelForm.model!.id!, {
                settings: {
                  top_p: editModelForm.model!.settings.top_p,
                },
              })
            }
          "
        />
      </n-form-item>
      <n-form-item label="Max Tokens">
        <SettingsNumberInput
          v-model:value="editModelForm.model!.settings.max_tokens"
          @confirm="
            async () => {
              await api.updateModel(editModelForm.model!.id!, {
                settings: {
                  max_tokens: editModelForm.model!.settings.max_tokens,
                },
              })
            }
          "
        />
      </n-form-item>
    </n-form>
  </n-modal>
</template>

<style>
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.n-tag .edit-icon {
  display: none;
}

.n-tag:hover .edit-icon {
  display: flex;
}
</style>
