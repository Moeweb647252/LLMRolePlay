<script setup lang="ts">
import { NModal, NForm, NFormItem } from 'naive-ui'
import SettingsInput from '../SettingsInput.vue'
import { toRef } from 'vue'
import type { EditUserForm } from '@/types/modal/user'
import { api } from '@/api'

const props = defineProps<{
  user: EditUserForm
}>()

const form = toRef(props, 'user')
const show = defineModel('show', {
  type: Boolean,
  default: false,
})
const emit = defineEmits(['confirm'])

const userTypeOptions = [
  { label: '用户', value: '1' },
  { label: '管理员', value: '2' },
]
</script>

<template>
  <NModal
    v-model:show="show"
    title="编辑用户"
    preset="card"
    style="width: fit-content; min-width: 25em"
    size="medium"
  >
    <NForm v-if="form" label-placement="left">
      <NFormItem label="用户名">
        <SettingsInput
          :value="form!.username"
          @confirm="
            async (username) => {
              await api.updateUser(form!.id, {
                username: username,
              })
              form!.username = username
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="邮箱">
        <SettingsInput
          :value="form!.email"
          @confirm="
            async (email) => {
              await api.updateUser(form!.id, {
                email: email,
              })
              form!.email = email
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="角色">
        <SettingsInput
          type="select"
          :options="userTypeOptions"
          :value="form!.role"
          @confirm="
            async (role) => {
              await api.updateUser(form!.id, {
                group: parseInt(role),
              })
              form!.role = role
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
    </NForm>
  </NModal>
</template>
