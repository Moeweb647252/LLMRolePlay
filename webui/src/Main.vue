<script setup lang="ts">
import { useSettingsStore } from './stores/settings'
import { api, NoTokenError } from './api'
import { useRouter } from 'vue-router'

const router = useRouter()

const settings = useSettingsStore()

try {
  await api.check()
} catch (e) {
  if (e instanceof NoTokenError) {
    router.push('/login')
  } else {
    console.error(e)
  }
}
</script>

<template>
  <RouterView />
</template>
