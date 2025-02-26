import { defineStore } from 'pinia'
import type { Character } from './characters'
import type { Preset } from './presets'
import { ref } from 'vue'
import type { Model } from './providers'
export interface Participant {
  id: number
  model: Model
  preset: Preset
  character: Character
  name: string
  settings: {}
  avatar: number | null
}

export interface Message {
  id: number
  content: string
  role: 'user' | 'ai'
  cretedAt: string
}

export interface Chat {
  id: number
  name: string
  participants: Participant[]
  messages: Message[]
}

export const useChatStore = defineStore('chats', () => {
  const chats = ref([] as Chat[])
  const set = (value: Chat[]) => {
    chats.value = value
  }
  return {
    chats,
    set,
  }
})
