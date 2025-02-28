export class Preset {
  id: number | null
  name: string | null
  description: string | null
  settings: { key: string; value: string }[]
  isPublic: boolean

  constructor(
    id: number | null = null,
    name: string | null = null,
    description: string | null = null,
    settings: { key: string; value: string }[] = [],
    isPublic: boolean = false,
  ) {
    this.id = id
    this.name = name
    this.description = description
    this.settings = settings
    this.isPublic = isPublic
  }
}
