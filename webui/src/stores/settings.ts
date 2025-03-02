import type { User } from '@/types/user'
import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useSettingsStore = defineStore(
  'settings',
  () => {
    const user = ref<User | null>(null)
    const currentChatId = ref<number | null>(null)
    return {
      user,
      currentChatId,
    }
  },
  {
    persist: true,
  },
)
