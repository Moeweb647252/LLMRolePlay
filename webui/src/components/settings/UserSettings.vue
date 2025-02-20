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
  <n-form label-placement="left"> </n-form>
</template>
