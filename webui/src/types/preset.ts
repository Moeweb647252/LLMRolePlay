export class Preset {
  id: number
  name: string
  description: string | null
  content: { key: string; value: string }[]
  settings: object | null
  isPublic: boolean

  constructor(
    id: number,
    name: string,
    description: string | null = null,
    content: { key: string; value: string }[] = [],
    settings: object | null,
    isPublic: boolean = false,
  ) {
    this.id = id
    this.name = name
    this.description = description
    this.content = content
    this.settings = settings
    this.isPublic = isPublic
  }
}
