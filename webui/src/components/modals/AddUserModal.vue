<script setup lang="ts">
import { useMessage } from 'naive-ui'
import { NModal, NForm, NFormItem, NInput, NSelect, NButton } from 'naive-ui'
import { ref } from 'vue'
import type { AddUserForm } from '@/types/modal/user'

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
    emit('confirm', form.value)
  }
}
</script>

<template>
  <NModal v-model:show="show" title="添加模板">
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
