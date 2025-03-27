export interface AddTemplateForm {
  name: string | null
  description: string | null
  content: string | null
  settings: object | null
  isPublic: boolean
}

export interface EditTemplateForm {
  id: number
  name: string
  description: string | null
  content: string
  settings: object | null
  isPublic: boolean
}
