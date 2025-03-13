<script setup lang="ts">
import { Provider } from '@/types/provider'
import { ref } from 'vue'
import { useMessage, useModal } from 'naive-ui'
import { NButton, NList, NListItem, NSpace, NThing } from 'naive-ui'
import AddProviderModal from '../modals/AddProviderModal.vue'
import EditProviderModal from '../modals/EditProviderModal.vue'
import { api } from '@/api'

const providers = ref(await api.getProviders())
const message = useMessage()
const model = useModal()

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

const startAddProvider = () => {}
</script>
<template>
  <div style="padding: 2em">
    <div class="header">
      <h3>Providers</h3>
      <NButton type="primary" @click="startAddProvider"> 添加 </NButton>
    </div>
    <div>
      <NList>
        <NListItem v-for="provider in providers" :key="provider.id">
          <NThing
            :title="provider.name"
            :description="provider.description"
            :title-extra="provider.type"
          />

          <template #suffix>
            <NSpace :wrap="false">
              <NButton type="primary" @click="startEditProvider(provider)">
                编辑
              </NButton>
              <NButton type="error" @click="deleteProvider(provider)">
                删除
              </NButton>
            </NSpace>
          </template>
        </NListItem>
      </NList>
    </div>
  </div>
  <AddProviderModal></AddProviderModal>
  <EditProviderModal></EditProviderModal>
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
