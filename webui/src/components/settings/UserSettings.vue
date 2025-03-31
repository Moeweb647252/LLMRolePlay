<script setup lang="ts">
import { api } from '@/api'
import { User } from '@/types/user'
import { useMessage, useModal } from 'naive-ui'
import { ref } from 'vue'
import { NButton, NList, NListItem, NSpace, NTag } from 'naive-ui'
import type { EditUserForm } from '@/types/modal'
import AddUserModal from '../modals/AddUserModal.vue'
import EditUserModal from '../modals/EditUserModal.vue'

const users = ref(await api.getUsers())
const message = useMessage()
const model = useModal()

const deleteUser = async (user: User) => {
  await api.deleteUser(user.id!)
  users.value.splice(users.value.indexOf(user), 1)
  message.success('删除成功')
  model.create({
    title: '删除用户',
    content: `确定删除用户 ${user.username} ?`,
    preset: 'dialog',
    positiveText: '确定',
    negativeText: '取消',
    onPositiveClick: async () => {
      try {
        await api.deleteUser(user.id!)
        users.value.splice(users.value.indexOf(user), 1)
        message.success('删除成功')
      } catch (e) {
        console.log(e)
        message.error('删除失败')
      }
    },
  })
}

const addShow = ref(false)
const addKey = ref(0)
const editShow = ref(false)
const editKey = ref(0)
const editing = ref<EditUserForm | null>(null)

const onConfirmAdd = async (form: User) => {
  users.value.push(form)
  message.success('添加成功')
}

let onConfirmEdit = (_form: EditUserForm) => {}

const startAdd = () => {
  addKey.value++
  addShow.value = true
}

const startEdit = (user: User) => {
  editKey.value++
  editing.value = {
    id: user.id!,
    email: user.email,
    username: user.username,
    group: user.group,
    // Add other fields as necessary
  }
  editShow.value = true
  onConfirmEdit = async (form: EditUserForm) => {
    user.username = form.username
    user.email = form.email
    user.group = form.group
  }
}
</script>
<template>
  <div style="padding: 2em">
    <div class="header">
      <h3>用户</h3>
      <NButton type="primary" @click="startAdd"> 添加 </NButton>
    </div>
    <div>
      <NList>
        <NListItem v-for="user in users" :key="user.id!">
          {{ user.username }}
          <NTag v-if="user.group == 2" type="success"> 管理员 </NTag>
          <NTag v-if="user.group == 1" type="info"> 用户 </NTag>
          <template #suffix>
            <NSpace :wrap="false">
              <NButton type="primary" @click="startEdit(user)"> 编辑 </NButton>
              <NButton type="error" @click="deleteUser(user)"> 删除 </NButton>
            </NSpace>
          </template>
        </NListItem>
      </NList>
    </div>
  </div>
  <AddUserModal
    :key="addKey"
    v-model:show="addShow"
    @confirm="onConfirmAdd"
    @close="addShow = false"
  />
  <EditUserModal
    v-if="editing"
    :key="editKey"
    v-model:show="editShow"
    :value="editing"
    @confirm="onConfirmEdit"
    @close="editShow = false"
  ></EditUserModal>
</template>

<style scoped>
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1em;
}
</style>
