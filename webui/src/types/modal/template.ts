export interface AddTemplateForm {
  name: string
  description: string | null
  content: string | null
  settings: object
  isPublic: boolean
}
