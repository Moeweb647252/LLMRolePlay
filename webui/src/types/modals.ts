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

export type Options = {
  label: string
  value: number
}[]
