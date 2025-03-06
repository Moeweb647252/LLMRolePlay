export type Form = {
  name: string
  description: string
  content: { key: string; value: string }[]
  settings: object
  isPublic: boolean
  avatar: ArrayBuffer | null
}
