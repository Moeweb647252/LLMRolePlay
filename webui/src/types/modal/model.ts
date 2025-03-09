export interface AddModelForm {
  name: string | null
  description: string | null
  modelName: string | null
  isPublic: boolean
  settings: {
    temperature: number | null
    top_p: number | null
    max_tokens: number | null
  }
}

export interface EditModelForm {
  id: number
  name: string
  description: string | null
  modelName: string | null
  isPublic: boolean
  settings: {
    temperature: number | null
    top_p: number | null
    max_tokens: number | null
  }
}
