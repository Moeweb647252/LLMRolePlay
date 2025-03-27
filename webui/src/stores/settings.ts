import type { User } from '@/types/user'
import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useSettingsStore = defineStore(
  'settings',
  () => {
    const user = ref<User>()
    const currentChatId = ref<number>()
    return {
      user,
      currentChatId,
    }
  },
  {
    persist: true,
  },
)
