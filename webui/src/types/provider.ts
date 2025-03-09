export class Model {
  id: number
  name: string
  modelName: string
  description: string | null
  isPublic: boolean
  settings: {
    temperature: number | null
    top_p: number | null
    max_tokens: number | null
  }
  provider: Provider | null

  constructor(
    id: number,
    name: string,
    modelName: string,
    description: string | null = null,
    isPublic: boolean = false,
    settings: {
      temperature: number | null
      top_p: number | null
      max_tokens: number | null
    } = {
      temperature: null,
      top_p: null,
      max_tokens: null,
    },
    provider: Provider | null = null,
  ) {
    this.id = id
    this.name = name
    this.modelName = modelName
    this.description = description
    this.isPublic = isPublic
    this.settings = settings
    this.provider = provider
  }
}

export class Provider {
  id: number
  name: string
  baseUrl: string
  apiKey: string
  description: string | null
  settings: object | null
  type: 'openai' | 'google' | 'azure'
  models: Model[]

  constructor(
    id: number,
    name: string,
    baseUrl: string,
    apiKey: string,
    description: string | null = null,
    settings: object | null = null,
    type: 'openai' | 'google' | 'azure',
    models: Model[] = [],
  ) {
    this.id = id
    this.name = name
    this.baseUrl = baseUrl
    this.apiKey = apiKey
    this.description = description
    this.settings = settings
    this.type = type
    this.models = models
  }
}
