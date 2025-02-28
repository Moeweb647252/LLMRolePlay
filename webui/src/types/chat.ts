import { Character } from './character'
import { Preset } from './preset'
import { Model } from './provider'

export class Participant {
  id: number | null
  model: Model
  presets: Preset[]
  character: Character
  name: string
  settings: Record<string, any>

  constructor(
    id: number | null = null,
    model: Model = new Model(),
    presets: Preset[] = [],
    character: Character = new Character(),
    name: string = '',
    settings: Record<string, any> = {},
  ) {
    this.id = id
    this.model = model
    this.presets = presets
    this.character = character
    this.name = name
    this.settings = settings
  }
}

export class Message {
  id: number | null
  content: string
  role: 'user' | 'ai'
  createdAt: string // 修正拼写错误: cretedAt -> createdAt

  constructor(
    id: number | null = null,
    content: string = '',
    role: 'user' | 'ai' = 'user',
    createdAt: string = new Date().toISOString(),
  ) {
    this.id = id
    this.content = content
    this.role = role
    this.createdAt = createdAt
  }
}

export class Chat {
  id: number | null
  name: string
  participants: Participant[]
  messages: Message[]

  constructor(
    id: number | null = null,
    name: string = '',
    participants: Participant[] = [],
    messages: Message[] = [],
  ) {
    this.id = id
    this.name = name
    this.participants = participants
    this.messages = messages
  }
}
