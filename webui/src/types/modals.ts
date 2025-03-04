import type { SelectBaseOption } from 'naive-ui/es/select/src/interface'

export type AddParticipantForm = {
  name: string
  settings: object
  model: null | number
  presets: number[]
  character: null | number
  template: null | number
}

export type AddChatForm = {
  name: string
  description: string | null
  settings: object
  participants: AddParticipantForm[]
}

export type Options = SelectBaseOption[]

export type EditParticipantForm = {
  id: number
  name: string
  settings: object
  model: null | number
  presets: number[]
  character: null | number
  template: null | number
}

export type EditChatForm = {
  id: number
  name: string
  description: string | null
  settings: object
}
