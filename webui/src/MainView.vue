<script setup lang="ts">
import { ref } from 'vue'
import { api, NoTokenError } from './api'
import { useRouter } from 'vue-router'
import { RouterView } from 'vue-router'

const router = useRouter()
const inited = ref(false)

try {
  await api.check()
  inited.value = true
} catch (e) {
  if (e instanceof NoTokenError) {
    router.push('/login')
  } else {
    console.error(e)
  }
}
</script>

<template>
  <RouterView v-if="inited" />
</template>
