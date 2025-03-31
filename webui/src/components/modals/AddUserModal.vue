<script setup lang="ts">
import { useMessage } from 'naive-ui'
import { NModal, NForm, NFormItem, NInput, NSelect, NButton } from 'naive-ui'
import { ref } from 'vue'
import type { AddUserForm } from '@/types/modal/user'
import type { User } from '@/types/user'
import { api } from '@/api'

const message = useMessage()

const show = defineModel<boolean>('show', {
  default: false,
})

const form = ref<AddUserForm>({
  username: null,
  password: null,
  email: null,
  group: 1,
})

const emit = defineEmits<{
  cancel: []
  confirm: [User]
}>()

const validate = () => {
  if (!form.value.username) {
    message.error('用户名不能为空')
    return false
  }
  if (!form.value.password) {
    message.error('密码不能为空')
    return false
  }
  if (!form.value.email) {
    message.error('邮箱不能为空')
    return false
  }
  return true
}

const cancel = () => {
  show.value = false // 关闭模态框
  emit('cancel')
}

const confirm = async () => {
  if (validate()) {
    let id = await api.addUser(
      form.value.username!,
      form.value.email!,
      form.value.password!, // 密码不能为空
      form.value.group!, // 使用默认值1，如果不传则为1
    )
    emit('confirm', {
      id, // 返回新创建的用户ID
      username: form.value.username!,
      email: form.value.email!,
      password: form.value.password!, // 密码
      group: form.value.group!, // 用户组
      token: null,
    } as User)
    show.value = false // 关闭模态框
  }
}
</script>

<template>
  <NModal
    v-model:show="show"
    title="添加模板"
    preset="card"
    style="width: fit-content; min-width: 25em"
    size="medium"
    :closable="true"
    :mask-closable="true"
    @close="cancel"
  >
    <NForm>
      <NFormItem label="用户名">
        <NInput v-model:value="form.username" />
      </NFormItem>
      <NFormItem label="密码">
        <NInput v-model:value="form.password" />
      </NFormItem>
      <NFormItem label="邮箱">
        <NInput v-model:value="form.email" />
      </NFormItem>
      <NFormItem label="组">
        <NSelect
          v-model:value="form.group"
          :options="[
            { label: '管理员', value: 2 },
            { label: '用户', value: 1 },
          ]"
        />
      </NFormItem>
    </NForm>
    <template #footer>
      <NButton @click="cancel">取消</NButton>
      <NButton type="primary" @click="confirm">确定</NButton>
    </template>
  </NModal>
</template>
