import type { User } from '@/types/user'
import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useSettingsStore = defineStore(
  'settings',
  () => {
    const user = ref<User | null>(null)
    return {
      user,
    }
  },
  {
    persist: true,
  },
)
