<script setup lang="ts">
import { api } from '@/api'
import { useCharacterStore } from '@/stores/characters'
import { useMessage } from 'naive-ui'
import { ref } from 'vue'
import SettingsInput from '../SettingsInput.vue'

const characters = useCharacterStore().characters
const message = useMessage()

const addCharacterForm = ref({
  visible: false,
  name: '',
  description: '',
  settings: [],
})

const addCharacter = async () => {
  let data = JSON.parse(JSON.stringify(addCharacterForm.value))
  addCharacterForm.value = {
    name: '',
    description: '',
    settings: [],
    visible: false,
  }
  let id = await api.addCharacter(data.name, data.description, data.settings)
  characters.push({
    id,
    name: data.name,
    description: data.description,
    settings: data.settings,
  })
  message.success('添加成功')
}

const cancelAddCharacter = () => {
  addCharacterForm.value = {
    name: '',
    description: '',
    settings: [],
    visible: false,
  }
}

const editCharacterForm = ref({
  visible: false,
  name: '',
  description: '',
  settings: [],
})

const editCharacter = async () => {}

const cancelEditCharacter = () => {
  editCharacterForm.value = {
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
      <h3>角色</h3>
      <n-space>
        <n-button type="primary" @click="addCharacterForm.visible = true">添加</n-button>
      </n-space>
    </div>
    <div>
      <n-list>
        <n-list-item v-for="character in characters" :key="character.id">
          <n-thing :title="character.name" :description="character.description"></n-thing>
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
    title="添加角色"
    v-modal:show="addCharacterForm.visible"
    preset="card"
    style="height: fit-content; min-height: 25em"
  >
    <n-form label-placement="left">
      <n-form-item label="名称">
        <n-input v-model:value="addCharacterForm.name" />
      </n-form-item>
      <n-form-item label="描述">
        <n-input v-model:value="addCharacterForm.description" />
      </n-form-item>
      <n-form-item label="设置">
        <n-dynamic-input
          v-model:value="addCharacterForm.settings"
          preset="pair"
          key-placeholder="设置名"
          value-placeholder="值"
        />
      </n-form-item>
    </n-form>
    <template #footer>
      <n-space justify="end">
        <n-button type="primary" @click="addCharacter">添加</n-button>
        <n-button @click="cancelAddCharacter">取消</n-button>
      </n-space>
    </template>
  </n-modal>
  <n-modal v-model:show="editCharacterForm.visible" title="编辑角色" preset="card">
    <n-form label-placement="left">
      <n-form-item label="名称">
        <SettingsInput v-model:value="editCharacterForm.name" />
      </n-form-item>
      <n-form-item label="描述">
        <SettingsInput v-model:value="editCharacterForm.description" />
      </n-form-item>
      <n-form-item label="设置">
        <n-dynamic-input
          v-model:value="editCharacterForm.settings"
          preset="pair"
          key-placeholder="设置名"
          value-placeholder="值"
        />
        <n-button type="primary">确定</n-button>
      </n-form-item>
    </n-form>
    <template #footer>
      <n-space justify="end">
        <n-button type="primary" @click="editCharacter">确定</n-button>
      </n-space>
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
