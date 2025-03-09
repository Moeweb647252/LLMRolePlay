export type AddCharacterForm = {
  name: string | null
  description: string | null
  content: { key: string; value: string }[]
  settings: object | null
  isPublic: boolean
  avatar: number | null
}

export type EditCharacterForm = {
  id: number
  name: string
  description: string | null
  content: { key: string; value: string }[]
  settings: object | null
  isPublic: boolean
  avatar: number | null
}
