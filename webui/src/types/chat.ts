import { Character } from './character'
import { Preset } from './preset'
import { Model } from './provider'
import type { Template } from './template'

export class Participant {
  id: number | null
  name: string | null
  model: Model | null
  presets: Preset[]
  character: Character | null
  template: Template | null
  settings: Record<string, any>

  constructor(
    id: number | null = null,
    name: string | null = null,
    model: Model | null = null,
    presets: Preset[] = [],
    character: Character | null = null,
    template: Template | null = null,
    settings: Record<string, any> = {},
  ) {
    this.id = id
    this.model = model
    this.presets = presets
    this.character = character
    this.name = name
    this.template = template
    this.settings = settings
  }
}

export interface Message {
  id: number | null
  content: string
  role: string
  participantId: number | null
  createdAt: string
}

export class Chat {
  id: number | null
  name: string | null
  participants: {
    id: number
    name: string
    presets: {
      id: number
      name: string
    }[]
    character: {
      id: number
      name: string
    }
    template: {
      id: number
      name: string
    }
  }[]

  constructor(id: number | null = null, name: string | null = null, participants: any[] = []) {
    this.id = id
    this.name = name
    this.participants = participants
  }
}

interface ChatSettings {
  nameOfUser: string
}

export class FullChat {
  id: number | null
  name: string | null
  participants: Participant[]
  settings: ChatSettings | null
  messages: Message[]
  description: string | null

  constructor(
    id: number | null = null,
    name: string | null = null,
    description: string | null = null,
    participants: Participant[] = [],
    messages: Message[] = [],
    settings: ChatSettings | null = null,
  ) {
    this.id = id
    this.name = name
    this.participants = participants
    this.messages = messages
    this.settings = settings
    this.description = description
  }
}
