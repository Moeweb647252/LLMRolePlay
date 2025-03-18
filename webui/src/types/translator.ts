export class Translator {
  id: number
  name: string
  description: string | null
  modelId: number | null
  presetIds: number[]
  templateId: number | null

  constructor(
    id: number,
    name: string,
    description: string | null = null,
    modelId: number | null = null,
    presetIds: number[] = [],
    templateId: number | null = null,
  ) {
    this.id = id
    this.name = name
    this.description = description
    this.modelId = modelId
    this.presetIds = presetIds
    this.templateId = templateId
  }
}
