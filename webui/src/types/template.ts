export class Template {
  id: number
  name: string
  content: string
  description: string | null
  settings: object | null
  isPublic: boolean

  constructor(
    id: number,
    name: string,
    content: string,
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
