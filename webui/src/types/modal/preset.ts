export interface AddPresetForm {
  name: string | null
  description: string | null
  content: { key: string; value: string }[]
  settings: object
  isPublic: boolean
}
