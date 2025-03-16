<script setup lang="ts">
import { api } from '@/api'
import { Character } from '@/types/character'
import { useMessage, useModal } from 'naive-ui'
import { ref } from 'vue'
import { NButton, NList, NListItem, NSpace } from 'naive-ui'
const characters = ref(await api.getCharacters())
const message = useMessage()
const model = useModal()
import AddCharacterModal from '../modals/AddCharacterModal.vue'
import EditCharacterModal from '../modals/EditCharacterModal.vue'
import type { EditCharacterForm } from '@/types/modal/index'

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
        console.log(e)
        message.error('删除失败')
      }
    },
  })
}

const showAddModal = ref(false)
const addModalKey = ref(0)
const showEditModal = ref(false)
const editModalKey = ref(0)
const editingCharacter = ref<EditCharacterForm | null>(null)

const add = () => {
  addModalKey.value++
  showAddModal.value = true
}

const edit = (character: Character) => {
  editModalKey.value++
  editingCharacter.value = {
    id: character.id!,
    name: character.name,
    description: character.description,
    content: character.content,
    isPublic: character.isPublic,
    settings: character.settings,
    avatar: character.avatar,
  }
  showEditModal.value = true
  onEditConfirm = async (form: EditCharacterForm) => {
    character.name = form.name
    character.description = form.description
    character.content = form.content
    character.isPublic = form.isPublic
    character.settings = form.settings
    character.avatar = form.avatar
  }
}

const onAddConfirm = async (value: Character) => {
  characters.value.push(value)
}

let onEditConfirm = async (_form: EditCharacterForm) => {}
</script>
<template>
  <div style="padding: 2em">
    <div class="header">
      <h3>角色</h3>
      <NButton type="primary" @click="add"> 添加 </NButton>
    </div>
    <div>
      <NList>
        <NListItem v-for="character in characters" :key="character.id!">
          {{ character.name }}
          <template #suffix>
            <NSpace :wrap="false">
              <NButton type="primary" @click="edit(character)"> 编辑 </NButton>
              <NButton type="error" @click="deleteCharacter(character)">
                删除
              </NButton>
            </NSpace>
          </template>
        </NListItem>
      </NList>
    </div>
  </div>
  <AddCharacterModal
    v-model:show="showAddModal"
    @confirm="onAddConfirm"
  ></AddCharacterModal>
  <EditCharacterModal
    v-model:show="showEditModal"
    :value="editingCharacter!"
    @confirm="onEditConfirm"
  ></EditCharacterModal>
</template>

<style scoped>
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1em;
}
</style>
