<script setup lang="ts">
import { api } from '@/api'
import { usePresetStore, type Preset } from '@/stores/presets'
import { useMessage, useModal } from 'naive-ui'
import { ref } from 'vue'
import SettingsInput from '../SettingsInput.vue'
import SettingsSwitch from '../SettingsSwitch.vue'
import SettingsDynamicInput from '../SettingsDynamicInput.vue'

const presets = usePresetStore().presets
const message = useMessage()
const model = useModal()

const addPresetForm = ref({
  visible: false,
  name: '',
  description: '',
  settings: [],
  isPublic: false,
})

const addPreset = async () => {
  let data = JSON.parse(JSON.stringify(addPresetForm.value))
  addPresetForm.value = {
    name: '',
    description: '',
    settings: [],
    visible: false,
    isPublic: false,
  }
  let id = await api.addPreset(data.name, data.description, data.settings, data.isPublic)
  presets.push({
    id,
    name: data.name,
    description: data.description,
    settings: data.settings,
    isPublic: data.isPublic,
  })
  message.success('添加成功')
}

const cancelAddPreset = () => {
  addPresetForm.value = {
    name: '',
    description: '',
    settings: [],
    visible: false,
    isPublic: false,
  }
}

const editPresetForm = ref({
  visible: false,
  preset: null as Preset | null,
})

const editPreset = (preset: Preset) => {
  editPresetForm.value = {
    visible: true,
    preset,
  }
}

const deletePreset = async (preset: Preset) => {
  model.create({
    title: '删除预设',
    content: `确定删除预设 ${preset.name} ?`,
    preset: 'dialog',
    positiveText: '确定',
    negativeText: '取消',
    onPositiveClick: async () => {
      try {
        await api.deletePreset(preset.id)
        presets.splice(presets.indexOf(preset), 1)
        message.success('删除成功')
      } catch (e) {
        message.error('删除失败')
      }
    },
  })
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
              <n-button type="primary" @click="editPreset(preset)">编辑</n-button>
              <n-button type="error" @click="deletePreset(preset)">删除</n-button>
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
      <n-form-item label="公开">
        <n-switch v-model:value="addPresetForm.isPublic" />
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
  <n-modal
    title="编辑预设"
    size="medium"
    v-model:show="editPresetForm.visible"
    preset="card"
    style="width: fit-content; min-width: 25em"
  >
    <n-form label-placement="left">
      <n-form-item label="名称">
        <SettingsInput
          :value="editPresetForm.preset!.name"
          @confirm="
            async () =>
              await api.updatePreset(editPresetForm.preset!.id, {
                name: editPresetForm.preset!.name,
              })
          "
        ></SettingsInput>
      </n-form-item>
      <n-form-item label="描述">
        <SettingsInput
          :value="editPresetForm.preset!.description"
          @confirm="
            async () =>
              await api.updatePreset(editPresetForm.preset!.id, {
                description: editPresetForm.preset!.description,
              })
          "
        ></SettingsInput>
      </n-form-item>
      <n-form-item label="公开">
        <SettingsSwitch
          :value="editPresetForm.preset!.isPublic"
          @confirm="
            async (isPublic) => {
              await api.updatePreset(editPresetForm.preset!.id, {
                isPublic: isPublic,
              })
              editPresetForm.preset!.isPublic = isPublic
            }
          "
        ></SettingsSwitch>
      </n-form-item>
      <n-form-item label="设置">
        <SettingsDynamicInput
          :value="editPresetForm.preset!.settings"
          @confirm="
            async (settings: any) => {
              await api.updatePreset(editPresetForm.preset!.id, {
                settings: settings,
              })
              editPresetForm.preset!.settings = settings
            }
          "
        ></SettingsDynamicInput>
      </n-form-item>
    </n-form>
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
