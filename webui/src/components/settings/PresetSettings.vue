<script setup lang="ts">
import { api } from '@/api'
import { usePresetStore } from '@/stores/presets'
import { useMessage } from 'naive-ui'
import { ref } from 'vue'

const presets = usePresetStore().presets
const message = useMessage()

const addPresetForm = ref({
  visible: false,
  name: '',
  description: '',
  settings: [],
})

const addPreset = async () => {
  let data = JSON.parse(JSON.stringify(addPresetForm.value))
  addPresetForm.value = {
    name: '',
    description: '',
    settings: [],
    visible: false,
  }
  let id = await api.addPreset(data.name, data.description, data.settings)
  presets.push({
    id,
    name: data.name,
    description: data.description,
    settings: data.settings,
  })
  message.success('添加成功')
}

const cancelAddPreset = () => {
  addPresetForm.value = {
    name: '',
    description: '',
    settings: [],
    visible: false,
  }
}
</script>
<template>
  <div style="padding: 2em">
    <div class="header">
      <h3>预设</h3>
      <n-button type="primary" @click="addPresetForm.visible = true">添加</n-button>
    </div>
    <div>
      <n-list>
        <n-list-item v-for="preset in presets" :key="preset.id">
          {{ preset.name }}
          <template #suffix>
            <n-space :wrap="false">
              <n-button type="primary">编辑</n-button>
              <n-button type="error">删除</n-button>
            </n-space>
          </template>
        </n-list-item>
      </n-list>
    </div>
  </div>
  <n-modal
    title="添加预设"
    size="medium"
    v-model:show="addPresetForm.visible"
    preset="card"
    style="width: fit-content; min-width: 25em"
  >
    <n-form label-placement="left">
      <n-form-item label="名称">
        <n-input v-model:value="addPresetForm.name" />
      </n-form-item>
      <n-form-item label="描述">
        <n-input v-model:value="addPresetForm.description" />
      </n-form-item>
      <n-form-item label="设置">
        <n-dynamic-input
          v-model:value="addPresetForm.settings"
          preset="pair"
          key-placeholder="设置名"
          value-placeholder="值"
        />
      </n-form-item>
    </n-form>
    <template #footer>
      <n-space justify="end">
        <n-button @click="cancelAddPreset">取消</n-button>
        <n-button type="primary" @click="addPreset">保存</n-button>
      </n-space>
    </template>
  </n-modal>
</template>

<style scoped>
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1em;
}
</style>
