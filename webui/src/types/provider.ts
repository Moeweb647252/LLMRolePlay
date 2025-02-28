export class Model {
  id: number | null
  name: string | null
  modelName: string | null
  description: string | null
  isPublic: boolean
  settings: {
    temperature: number | null
    top_p: number | null
    max_tokens: number | null
    [key: string]: any | null
  }
  provider: Provider | null

  constructor(
    id: number | null = null,
    name: string | null = null,
    modelName: string | null = null,
    description: string | null = null,
    isPublic: boolean = false,
    settings: {
      temperature: number | null
      top_p: number | null
      max_tokens: number | null
      [key: string]: any | null
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
  id: number | null
  name: string | null
  url: string | null
  apiKey: string | null
  description: string | null
  settings: { key: string; value: string }[]
  type: 'openai' | 'google' | 'azure' | null
  models: Model[]

  constructor(
    id: number | null = null,
    name: string | null = null,
    url: string | null = null,
    apiKey: string | null = null,
    description: string | null = null,
    settings: { key: string; value: string }[] = [],
    type: 'openai' | 'google' | 'azure' | null = null,
    models: Model[] = [],
  ) {
    this.id = id
    this.name = name
    this.url = url
    this.apiKey = apiKey
    this.description = description
    this.settings = settings
    this.type = type
    this.models = models
  }
}
