<script setup lang="ts">
import { useMessage } from 'naive-ui'
import { NModal, NForm, NFormItem, NInput, NSelect, NButton } from 'naive-ui'
import { ref } from 'vue'
import type { AddUserForm } from '@/types/modal/user'
import { api } from '@/api'

const message = useMessage()

const show = defineModel<boolean>('show', {
  default: false,
})

const form = ref<AddUserForm>({
  username: null,
  password: null,
  email: null,
  role: 'user',
})

const emit = defineEmits(['cancel', 'confirm'])

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
  emit('cancel')
}

const confirm = async () => {
  if (validate()) {
    await api.addUser(
      form.value.username!,
      form.value.email!,
      form.value.password!,
      form.value.role!,
    )
    emit('confirm', form.value)
  }
}
</script>

<template>
  <n-modal v-model:show="show" title="添加模板">
    <n-form>
      <n-form-item label="用户名">
        <n-input v-model:value="form.username" />
      </n-form-item>
      <n-form-item label="密码">
        <n-input v-model:value="form.password" />
      </n-form-item>
      <n-form-item label="邮箱">
        <n-input v-model:value="form.email" />
      </n-form-item>
      <n-form-item label="组">
        <n-select
          v-model:value="form.role"
          :options="[
            { label: '管理员', value: 'admin' },
            { label: '用户', value: 'user' },
          ]"
        />
      </n-form-item>
    </n-form>
    <template #footer>
      <n-button @click="cancel">取消</n-button>
      <n-button type="primary" @click="confirm">确定</n-button>
    </template>
  </n-modal>
</template>
