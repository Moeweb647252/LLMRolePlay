export class Character {
  id: number | null
  name: string | null
  settings: { key: string; value: string }[]
  description: string | null
  isPublic: boolean
  avatar: number | null

  constructor(
    id: number | null = null,
    name: string | null = null,
    settings: { key: string; value: string }[] = [],
    description: string | null = null,
    isPublic: boolean = false,
    avatar: number | null = null,
  ) {
    this.id = id
    this.name = name
    this.settings = settings
    this.description = description
    this.isPublic = isPublic
    this.avatar = avatar
  }
}
