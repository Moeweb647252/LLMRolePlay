export type AddParticipantForm = {
  name: string | null
  settings: object | null
  model: null | number
  presets: number[]
  character: null | number
  template: null | number
}

export type EditParticipantForm = {
  id: number
  name: string
  settings: object | null
  model: null | number
  presets: number[]
  character: null | number
  template: null | number
}
