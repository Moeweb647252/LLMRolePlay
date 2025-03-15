<script setup lang="ts">
import { Model, Provider } from '@/types/provider'
import { reactive, ref } from 'vue'
import { useMessage, useModal } from 'naive-ui'
import { NButton, NList, NListItem, NSpace, NThing } from 'naive-ui'
import AddProviderModal from '../modals/AddProviderModal.vue'
import EditProviderModal from '../modals/EditProviderModal.vue'
import { api } from '@/api'
import type { AddProviderForm, EditProviderForm } from '@/types/modal'

const providers = ref(await api.getProviders())
const message = useMessage()
const model = useModal()

const showAddModal = ref(false)
const showEditModal = ref(false)
const addModalKey = ref(0)
const editModalKey = ref(0)
const editingProvider = ref<EditProviderForm | null>(null)

const deleteProvider = async (provider: Provider) => {
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

const add = () => {}
const edit = (provider: Provider) => {
  editModalKey.value++
  editingProvider.value = {
    id: provider.id,
    name: provider.name,
    description: provider.description,
    type: provider.type,
    baseUrl: provider.baseUrl,
    apiKey: provider.apiKey,
    models: provider.models,
    settings: null,
  }
  showEditModal.value = true
  onEditConfirm = (form: EditProviderForm) => {
    provider.name = form.name
    provider.description = form.description
    provider.type = form.type
    provider.models = form.models.map((model) => {
      return { ...model, provider: provider }
    })
  }
}

const onAddConfirm = async (value: AddProviderForm) => {
  showAddModal.value = false
  try {
    let providerId = await api.addProvider(
      value.name!,
      value.description,
      value.type,
      value.baseUrl!,
      value.apiKey!,
      null,
    )
    let newProvider = reactive({
      id: providerId,
      name: value.name!,
      description: value.description,
      type: value.type,
      baseUrl: value.baseUrl!,
      apiKey: value.apiKey!,
      models: [] as Model[],
      settings: null,
    })
    providers.value.push(newProvider)
    for (const model of value.models) {
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
    message.success('Provider添加成功.')
  } catch (error) {
    console.error(error)
    message.error('Provider添加失败.')
  }
}

let onEditConfirm = (_form: EditProviderForm) => {}
</script>
<template>
  <div style="padding: 2em">
    <div class="header">
      <h3>Providers</h3>
      <NButton type="primary" @click="add"> 添加 </NButton>
    </div>
    <div>
      <NList>
        <NListItem v-for="provider in providers" :key="provider.id">
          <NThing
            :title="provider.name"
            :description="provider.description ?? ''"
            :title-extra="provider.type"
          />

          <template #suffix>
            <NSpace :wrap="false">
              <NButton type="primary" @click="edit(provider)"> 编辑 </NButton>
              <NButton type="error" @click="deleteProvider(provider)">
                删除
              </NButton>
            </NSpace>
          </template>
        </NListItem>
      </NList>
    </div>
  </div>
  <AddProviderModal
    :key="addModalKey"
    v-model:show="showAddModal"
    @confirm="onAddConfirm"
  ></AddProviderModal>
  <EditProviderModal
    v-if="editingProvider"
    :key="editModalKey"
    v-model:show="showEditModal"
    :value="editingProvider!"
    @confirm="onEditConfirm"
  ></EditProviderModal>
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
