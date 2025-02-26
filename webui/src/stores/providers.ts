import { defineStore } from 'pinia'
import { ref } from 'vue'
import { OpenAI } from 'openai'

export interface Model {
  id: number | null
  name: string
  modelName: string
  description: string
  isPublic: boolean
  settings: {
    temperture: number | null
    top_p: number | null
    max_tokne: number | null
    [key: string]: any | null
  }
  provider: Provider | null
}

export interface Provider {
  id: number
  name: string
  url: string
  apiKey: string
  description: string
  settings: { key: string; value: string }[]
  type: 'openai' | 'google' | 'azure'
  models: Model[]
}

export const useProviderStore = defineStore('providers', () => {
  const providers = ref([] as Provider[])
  const set = (value: Provider[]) => {
    providers.value = value
  }
  return {
    providers,
    set,
  }
})
