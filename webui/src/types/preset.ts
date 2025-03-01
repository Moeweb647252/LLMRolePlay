export class Preset {
  id: number | null
  name: string | null
  description: string | null
  content: { key: string; value: string }[]
  settings: {}
  isPublic: boolean

  constructor(
    id: number | null = null,
    name: string | null = null,
    description: string | null = null,
    content: { key: string; value: string }[] = [],
    settings: {},
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
