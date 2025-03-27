export class Translator {
  id: number
  name: string
  description: string | null
  modelId: number | null
  presetIds: number[]
  templateId: number | null
  settings: object

  constructor(
    id: number,
    name: string,
    description: string | null = null,
    modelId: number | null = null,
    presetIds: number[] = [],
    templateId: number | null = null,
    settings: object = {},
  ) {
    this.id = id
    this.name = name
    this.description = description
    this.modelId = modelId
    this.presetIds = presetIds
    this.templateId = templateId
    this.settings = settings
  }
}
