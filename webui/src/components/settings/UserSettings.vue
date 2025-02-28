<script setup lang="ts">
import { api } from '@/api'
import { User } from '@/types/user'
import { useMessage, useModal } from 'naive-ui'
import { onMounted, ref } from 'vue'
import SettingsInput from '../SettingsInput.vue'

const message = useMessage()
const model = useModal()

const users = ref<User[]>([])

const userTypeOptions = [
  { label: '用户', value: '1' },
  { label: '管理员', value: '2' },
]

const addUserForm = ref({
  visible: false,
  username: '',
  email: '',
  password: '',
  group: '1',
})

const addUser = async () => {
  let data = JSON.parse(JSON.stringify(addUserForm.value))
  addUserForm.value = {
    visible: false,
    username: '',
    email: '',
    password: '',
    group: '1',
  }
  let id = await api.addUser(data.username, data.email, data.password, data.group)
  users.value.push({
    id,
    username: data.username,
    email: data.email,
    token: '',
    group: data.group,
  })
  message.success('添加成功')
}

const cancelAddUser = () => {
  addUserForm.value = {
    visible: false,
    username: '',
    email: '',
    password: '',
    group: '1',
  }
}

const editUserForm = ref({
  visible: false,
  user: null as User | null,
})

const editUser = (user: User) => {
  editUserForm.value = {
    visible: true,
    user,
  }
}

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
        message.error('删除失败')
      }
    },
  })
}

onMounted(async () => {
  users.value = await api.getUsers()
})
</script>
<template>
  <div style="padding: 2em">
    <div class="header">
      <h3>用户</h3>
      <n-button type="primary" @click="addUserForm.visible = true">添加</n-button>
    </div>
    <div>
      <n-list>
        <n-list-item v-for="user in users" :key="user.id">
          {{ user.username }}
          <n-tag type="success" v-if="user.group == '2'">管理员</n-tag>
          <n-tag type="info" v-if="user.group == '1'">用户</n-tag>
          <template #suffix>
            <n-space :wrap="false">
              <n-button type="primary" @click="editUser(user)">编辑</n-button>
              <n-button type="error" @click="deleteUser(user)">删除</n-button>
            </n-space>
          </template>
        </n-list-item>
      </n-list>
    </div>
  </div>
  <n-modal
    title="添加用户"
    size="medium"
    v-model:show="addUserForm.visible"
    preset="card"
    style="width: fit-content; min-width: 25em"
  >
    <n-form label-placement="left">
      <n-form-item label="用户名">
        <n-input v-model:value="addUserForm.username" />
      </n-form-item>
      <n-form-item label="邮箱">
        <n-input v-model:value="addUserForm.email" />
      </n-form-item>
      <n-form-item label="密码">
        <n-input v-model:value="addUserForm.password" />
      </n-form-item>
      <n-form-item label="组">
        <n-select v-model:value="addUserForm.group" :options="userTypeOptions"> </n-select>
      </n-form-item>
    </n-form>
    <template #footer>
      <n-space justify="end">
        <n-button @click="cancelAddUser">取消</n-button>
        <n-button type="primary" @click="addUser">保存</n-button>
      </n-space>
    </template>
  </n-modal>
  <n-modal
    title="编辑用户"
    size="medium"
    v-model:show="editUserForm.visible"
    preset="card"
    style="width: fit-content; min-width: 25em"
  >
    <n-form label-placement="left">
      <n-form-item label="用户名">
        <SettingsInput v-model:value="editUserForm.user!.username" />
      </n-form-item>
      <n-form-item label="邮箱">
        <SettingsInput v-model:value="editUserForm.user!.email" />
      </n-form-item>
      <n-form-item label="组">
        <SettingsInput
          type="select"
          v-model:value="editUserForm.user!.group"
          :options="userTypeOptions"
          @confirm="async () => {}"
        >
        </SettingsInput>
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
