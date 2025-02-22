<script setup lang="ts">
import { usePresetStore } from '@/stores/presets'

const presets = usePresetStore().presets
</script>
<template>
  <div style="padding: 2em">
    <div class="header">
      <h3>Providers</h3>
      <n-button type="primary" @click="true">添加</n-button>
    </div>
    <div>
      <n-list>
        <n-list-item v-for="preset in presets" :key="preset.id">
          {{ preset.name }}
          <template #extra>
            <n-button type="primary">编辑</n-button>
            <n-button type="danger">删除</n-button>
          </template>
        </n-list-item>
      </n-list>
    </div>
  </div>
  <n-modal title="添加预设" size="medium"> </n-modal>
</template>

<style scoped>
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1em;
}
</style>
