<script setup lang="ts">
import { api } from '@/api'
import { Character } from '@/types/character'
import { useMessage, useModal, type UploadFileInfo } from 'naive-ui'
import { ref } from 'vue'
import SettingsInput from '../SettingsInput.vue'
import SettingsSwitch from '../SettingsSwitch.vue'
const characters = ref(await api.getCharacters())
const message = useMessage()
const model = useModal()

const addCharacterForm = ref({
  visible: false,
  name: '',
  description: '',
  settings: [],
  isPublic: false,
  avatarFileList: [] as UploadFileInfo[],
})

const addCharacter = async () => {
  let data: typeof addCharacterForm.value = JSON.parse(JSON.stringify(addCharacterForm.value))
  addCharacterForm.value = {
    name: '',
    description: '',
    settings: [],
    visible: false,
    isPublic: false,
    avatarFileList: [],
  }
  let fileId = null
  if (data.avatarFileList.length > 0) {
    const buffer = await data.avatarFileList[0].file?.arrayBuffer()
    if (buffer) {
      fileId = await api.uploadFile(buffer)
    }
  }
  let id = await api.addCharacter(data.name, data.description, data.settings, data.isPublic)
  characters.value.push(
    new Character(id, data.name, data.settings, data.description, data.isPublic, fileId),
  )
  message.success('添加成功')
}

const cancelAddCharacter = () => {
  addCharacterForm.value = {
    name: '',
    description: '',
    settings: [],
    visible: false,
    isPublic: false,
    avatarFileList: [],
  }
}

const editCharacterForm = ref({
  visible: false,
  character: null as Character | null,
})

const editCharacter = (character: Character) => {
  editCharacterForm.value = {
    visible: true,
    character,
  }
}

const deleteCharacter = async (character: Character) => {
  model.create({
    title: '删除角色',
    content: `确定删除角色 ${character.name} ?`,
    preset: 'dialog',
    positiveText: '确定',
    negativeText: '取消',
    onPositiveClick: async () => {
      try {
        await api.deleteCharacter(character.id!)
        characters.value.splice(characters.value.indexOf(character), 1)
        message.success('删除成功')
      } catch (e) {
        message.error('删除失败')
      }
    },
  })
}

const uploadAvatar = async () => {
  return true
}
</script>
<template>
  <div style="padding: 2em">
    <div class="header">
      <h3>角色</h3>
      <n-button type="primary" @click="addCharacterForm.visible = true">添加</n-button>
    </div>
    <div>
      <n-list>
        <n-list-item v-for="character in characters" :key="character.id">
          {{ character.name }}
          <template #suffix>
            <n-space :wrap="false">
              <n-button type="primary" @click="editCharacter(character)">编辑</n-button>
              <n-button type="error" @click="deleteCharacter(character)">删除</n-button>
            </n-space>
          </template>
        </n-list-item>
      </n-list>
    </div>
  </div>
  <n-modal
    title="添加角色"
    size="medium"
    v-model:show="addCharacterForm.visible"
    preset="card"
    style="width: fit-content; min-width: 25em"
  >
    <n-form label-placement="left">
      <n-form-item label="名称">
        <n-input v-model:value="addCharacterForm.name" />
      </n-form-item>
      <n-form-item label="头像">
        <n-upload
          v-model:file-list="addCharacterForm.avatarFileList"
          @before-upload="uploadAvatar()"
          :multiple="false"
          list-type="image-card"
          :trigger-style="{ display: addCharacterForm.avatarFileList.length ? 'none' : 'block' }"
        />
      </n-form-item>
      <n-form-item label="描述">
        <n-input v-model:value="addCharacterForm.description" />
      </n-form-item>
      <n-form-item label="公开">
        <n-switch v-model:value="addCharacterForm.isPublic" />
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
        <n-button @click="cancelAddCharacter">取消</n-button>
        <n-button type="primary" @click="addCharacter">保存</n-button>
      </n-space>
    </template>
  </n-modal>
  <n-modal
    title="编辑角色"
    size="medium"
    v-model:show="editCharacterForm.visible"
    preset="card"
    style="width: fit-content; min-width: 25em"
  >
    <n-form label-placement="left">
      <n-form-item label="名称">
        <SettingsInput
          :value="editCharacterForm.character!.name"
          @confirm="
            async () =>
              await api.updateCharacter(editCharacterForm.character!.id!, {
                name: editCharacterForm.character!.name!,
              })
          "
        ></SettingsInput>
      </n-form-item>
      <n-form-item label="描述">
        <SettingsInput
          :value="editCharacterForm.character!.description"
          @confirm="
            async () =>
              await api.updateCharacter(editCharacterForm.character!.id!, {
                description: editCharacterForm.character!.description!,
              })
          "
        ></SettingsInput>
      </n-form-item>
      <n-form-item label="公开">
        <SettingsSwitch
          :value="editCharacterForm.character!.isPublic"
          @confirm="
            async (isPublic: boolean) => {
              await api.updatePreset(editCharacterForm.character!.id!, {
                isPublic: isPublic,
              })
              editCharacterForm.character!.isPublic = isPublic
            }
          "
        ></SettingsSwitch>
      </n-form-item>
      <n-form-item label="设置">
        <SettingsDynamicInput
          :value="editCharacterForm.character!.settings"
          @confirm="
            async (settings: any) => {
              await api.updateCharacter(editCharacterForm.character!.id!, {
                settings: settings,
              })
              editCharacterForm.character!.settings = settings
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
