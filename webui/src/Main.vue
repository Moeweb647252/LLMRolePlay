<script setup lang="ts">
import { useSettingsStore } from './stores/settings'
import { useProviderStore } from './stores/providers'
</script>

<template>
  <n-message-provider>
    <RouterView />
  </n-message-provider>
</template>
