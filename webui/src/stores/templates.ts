import { defineStore } from 'pinia'
import { ref } from 'vue'

export interface Template {
  id: number
  name: string
  content: string
  description: string
  isPublic: boolean
}

export const useTemplateStore = defineStore('templates', () => {
  const templates = ref([] as Template[])
  const set = (value: Template[]) => {
    templates.value = value
  }
  return {
    templates,
    set,
  }
})
