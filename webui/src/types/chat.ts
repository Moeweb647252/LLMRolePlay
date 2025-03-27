/* eslint-disable @typescript-eslint/no-explicit-any */
import { Character } from './character'
import { Preset } from './preset'
import { Model } from './provider'
import type { Template } from './template'

export class Participant {
  id: number
  name: string
  model: Model
  presets: Preset[]
  character: Character
  template: Template
  settings: Record<string, any>

  constructor(
    id: number,
    name: string,
    model: Model,
    presets: Preset[] = [],
    character: Character,
    template: Template,
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
  id: number
  content: string
  role: string
  participantId: number | undefined
  createdAt: string
}

export class Chat {
  id: number
  name: string
  description: string | null
  settings: ChatSettings
  participants: {
    id: number
    name: string
    model: {
      id: number
      name: string
    }
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

  constructor(
    id: number,
    name: string,
    participants: any[] = [],
    description: string | null = null,
    settings: ChatSettings = { nameOfUser: null, currentParticipantId: null },
  ) {
    this.id = id
    this.name = name
    this.description = description
    this.participants = participants
    this.settings = settings
  }
}

export interface ChatSettings {
  nameOfUser: string | null
  currentParticipantId: number | null
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
