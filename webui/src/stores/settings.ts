import { defineStore } from 'pinia'
import { ref } from 'vue'

export interface User {
  id: number
  username: string
  email: string
  token: string
  group: '1' | '2'
}

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
