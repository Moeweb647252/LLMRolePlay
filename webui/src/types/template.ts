export class Template {
  id: number | null
  name: string | null
  content: string | null
  description: string | null
  settings: object | null
  isPublic: boolean

  constructor(
    id: number | null = null,
    name: string | null = null,
    content: string | null = null,
    description: string | null = null,
    settings: object | null = null,
    isPublic: boolean = false,
  ) {
    this.id = id
    this.name = name
    this.content = content
    this.description = description
    this.settings = settings
    this.isPublic = isPublic
  }
}
