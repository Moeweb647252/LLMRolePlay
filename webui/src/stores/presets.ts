import { ref } from 'vue'
import { defineStore } from 'pinia'

export interface Preset {
  id: number
  name: string
  description: string
  settings: {
    [key: string]: string
  }
}

export const usePresetStore = defineStore('presets', () => {
  const presets = ref([] as Preset[])
  const set = (value: Preset[]) => {
    presets.value = value
  }
  return {
    presets,
    set,
  }
})
