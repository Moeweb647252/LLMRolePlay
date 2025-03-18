export interface AddTranslatorForm {
  name: string | null
  description: string | null
  modelId: number | null
  presetIds: number[]
  templateId: number | null
}

export interface EditTranslatorForm {
  id: number
  name: string
  description: string | null
  modelId: number
  presetIds: number[]
  templateId: number
}
