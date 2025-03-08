export class Character {
  id: number
  name: string
  content: { key: string; value: string }[]
  settings: object | null
  description: string | null
  isPublic: boolean
  avatar: number | null

  constructor(
    id: number,
    name: string,
    content: { key: string; value: string }[] = [],
    settings: object | null = null,
    description: string | null = null,
    isPublic: boolean = false,
    avatar: number | null = null,
  ) {
    this.id = id
    this.name = name
    this.content = content
    this.settings = settings
    this.description = description
    this.isPublic = isPublic
    this.avatar = avatar
  }
}
