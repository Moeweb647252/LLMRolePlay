<script setup lang="ts">
import { useSettingsStore } from './stores/settings'
import { useProviderStore } from './stores/providers'
import { useChatStore } from './stores/chats'
import { usePresetStore } from './stores/presets'
import { useCharacterStore } from './stores/characters'
import { api, NoTokenError } from './api'
import { useRouter } from 'vue-router'

const router = useRouter()

const settings = useSettingsStore()
const providers = useProviderStore()
const chats = useChatStore()
const presets = usePresetStore()
const characters = useCharacterStore()

try {
  presets.set(await api.getPresets())
  providers.set(await api.getProviders())
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
