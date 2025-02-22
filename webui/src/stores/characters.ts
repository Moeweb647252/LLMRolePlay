import { defineStore } from 'pinia'
import { ref } from 'vue'

export interface Character {
  id: number
  name: string
  modelName: string
  settings: {
    [key: string]: string
  }
  description: string
}

export const useCharacterStore = defineStore('characters', () => {
  const characters = ref([] as Character[])
  const set = (value: Character[]) => {
    characters.value = value
  }
  return {
    characters,
    set,
  }
})
