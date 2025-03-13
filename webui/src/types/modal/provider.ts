import type { AddModelForm, EditModelForm } from './model'

export interface AddProviderForm {
  name: string | null
  description: string | null
  type: string
  baseUrl: string | null
  apiKey: string | null
  settings: object | null
  models: AddModelForm[]
}

export interface EditProviderForm {
  id: number
  name: string
  description: string | null
  type: string
  baseUrl: string | null
  apiKey: string | null
  settings: object | null
  models: EditModelForm[]
}
