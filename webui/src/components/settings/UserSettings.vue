<script setup lang="ts">
import { useSettingsStore } from '@/stores/settings'
import { ref } from 'vue'

const settings = useSettingsStore()

const username = ref(settings.user!.username)
const email = ref(settings.user!.email)
const password = ref('')
const passwordConfirmation = ref('')
</script>

<template>
  <n-form label-placement="left">
    <n-form-item label="Username">
      <n-input v-model:value="username" />
    </n-form-item>
    <n-form-item label="Email">
      <n-input v-model:value="email" />
    </n-form-item>
    <n-form-item label="Password">
      <n-input v-model:value="password" type="password" />
    </n-form-item>
    <n-form-item label="Confirm Password">
      <n-input v-model:value="passwordConfirmation" type="password" />
    </n-form-item>
    <n-form-item>
      <n-button type="primary">Save</n-button>
    </n-form-item>
  </n-form>
</template>
