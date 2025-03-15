<script setup lang="ts">
import { api } from '@/api'
import { Preset } from '@/types/preset'
import { useMessage, useModal } from 'naive-ui'
import { ref } from 'vue'
import { NButton, NList, NListItem, NSpace } from 'naive-ui'
import AddPresetModal from '../modals/AddPresetModal.vue'
import EditPresetModal from '../modals/EditPresetModal.vue'
import type { EditPresetForm } from '@/types/modal'

const presets = ref(await api.getPresets())
const message = useMessage()
const model = useModal()

const deletePreset = async (preset: Preset) => {
  model.create({
    title: '删除预设',
    content: `确定删除预设 ${preset.name} ?`,
    preset: 'dialog',
    positiveText: '确定',
    negativeText: '取消',
    onPositiveClick: async () => {
      try {
        await api.deletePreset(preset.id!)
        presets.value.splice(presets.value.indexOf(preset), 1)
        message.success('删除成功')
      } catch (e) {
        console.log(e)
        message.error('删除失败')
      }
    },
  })
}

const showAddModal = ref(false)
const showEditModal = ref(false)
const editingPreset = ref<EditPresetForm | null>(null)

const add = () => {
  showAddModal.value = true
}

const edit = (preset: Preset) => {
  editingPreset.value = {
    id: preset.id!,
    name: preset.name,
    description: preset.description,
    content: preset.content,
    isPublic: preset.isPublic,
    settings: preset.settings,
  }
  showEditModal.value = true
  onEditConfirm = (form: EditPresetForm) => {
    preset.name = form.name
    preset.description = form.description
    preset.content = form.content
    preset.isPublic = form.isPublic
    preset.settings = form.settings
  }
}

const sharePreset = (preset: Preset) => {
  // 实现分享预设的功能
  message.info(`分享预设: ${preset.name}`)
}

const onAddConfirm = async () => {
  showAddModal.value = false
  presets.value = await api.getPresets()
}

let onEditConfirm = (_form: EditPresetForm) => {}
</script>

<template>
  <div style="padding: 2em">
    <div class="header">
      <h3>预设</h3>
      <NButton type="primary" @click="add"> 添加 </NButton>
    </div>
    <div>
      <NList>
        <NListItem v-for="preset in presets" :key="preset.id!">
          {{ preset.name }}
          <template #suffix>
            <NSpace :wrap="false">
              <NButton type="primary" @click="sharePreset(preset)">
                分享
              </NButton>
              <NButton type="primary" @click="edit(preset)"> 编辑 </NButton>
              <NButton type="error" @click="deletePreset(preset)">
                删除
              </NButton>
            </NSpace>
          </template>
        </NListItem>
      </NList>
    </div>
  </div>

  <AddPresetModal
    v-model:show="showAddModal"
    @confirm="onAddConfirm"
  ></AddPresetModal>

  <EditPresetModal
    v-model:show="showEditModal"
    :value="editingPreset!"
    @confirm="onEditConfirm"
  ></EditPresetModal>
</template>

<style scoped>
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1em;
}
</style>
