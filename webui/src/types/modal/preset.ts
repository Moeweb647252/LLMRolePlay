export interface AddPresetForm {
  name: string | null
  description: string | null
  content: { key: string; value: string }[]
  settings: object | null
  isPublic: boolean
}

export interface EditPresetForm {
  id: number
  name: string
  description: string | null
  content: { key: string; value: string }[]
  settings: object | null
  isPublic: boolean
}
