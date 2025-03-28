export class Translator {
  id: number
  name: string
  description: string | null
  modelId: number
  presetIds: number[]
  templateId: number
  settings: object

  constructor(
    id: number,
    name: string,
    description: string | null = null,
    modelId: number,
    presetIds: number[] = [],
    templateId: number,
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
