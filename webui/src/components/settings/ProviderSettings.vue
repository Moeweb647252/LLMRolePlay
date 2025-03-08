<script setup lang="ts">
import { ref } from 'vue'
import { NButton, NList, NListItem, NSpace } from 'naive-ui'

import { api } from '@/api'

const providers = ref(await api.getProviders())
</script>
<template>
  <div style="padding: 2em">
    <div class="header">
      <h3>Providers</h3>
      <n-button type="primary" @click="addProviderForm.visible = true">
        添加
      </n-button>
    </div>
    <div>
      <n-list>
        <n-list-item v-for="provider in providers" :key="provider.id">
          <n-thing
            :title="provider.name"
            :description="provider.description"
            :title-extra="provider.type"
          />

          <template #suffix>
            <n-space :wrap="false">
              <n-button type="primary" @click="startEditProvider(provider)">
                编辑
              </n-button>
              <n-button type="error" @click="deleteProvider(provider)">
                删除
              </n-button>
            </n-space>
          </template>
        </n-list-item>
      </n-list>
    </div>
  </div>
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
