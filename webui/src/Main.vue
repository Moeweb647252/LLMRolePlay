<script setup lang="ts">
import { useSettingsStore } from './stores/settings'
import { useProviderStore } from './stores/providers'
import { useChatStore } from './stores/chats'
import { usePresetStore } from './stores/presets'
import { useCharacterStore } from './stores/characters'
import { api } from './api'

const settings = useSettingsStore()
const providers = useProviderStore()
const chats = useChatStore()
const presets = usePresetStore()
const characters = useCharacterStore()

presets.set(await api.getPresets())
</script>

<template>
  <n-message-provider>
    <RouterView />
  </n-message-provider>
</template>
