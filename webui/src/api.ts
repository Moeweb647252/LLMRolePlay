import { sha256 } from 'js-sha256'
import { useSettingsStore } from './stores/settings'
import axios from 'axios'
import { useRouter } from 'vue-router'

export class NoTokenError extends Error {
  constructor() {
    super('No token found')
  }
}

export class Api {
  store: ReturnType<typeof useSettingsStore> | null = null
  constructor() {}

  async request(path: string, body: object, method: string = 'POST') {
    const router = useRouter()
    const token = this.store!.user?.token
    if (!token) {
      throw new NoTokenError()
    }
    const headers = {
      Token: token,
    }
    const response = await axios({
      method: method,
      url: '/api/' + path,
      data: body,
      headers: headers,
    })
    if (response.status !== 200) {
      throw new Error('Request failed')
    }
    let data = response.data
    if (data.code == 501) {
      throw new NoTokenError()
    } else if (data.code != 200) {
      throw new Error(data.msg)
    }
    return data.data
  }

  async login(email: string, password: string) {
    const resp = await this.request('login', {
      email: email,
      password: sha256(password),
    })
    this.store!.user = {
      email: resp.email,
      username: resp.username,
      id: resp.id,
      group: resp.group,
      token: resp.token,
    }
  }

  async logout() {
    this.store!.user = null
  }

  async addCharacter(name: string, description: string, settings: object) {
    let data = await this.request('createCharacter', {
      name: name,
      description: description,
      settings: JSON.stringify(settings),
    })
    return data.id
  }

  async addPreset(name: string, description: string, settings: object) {
    let data = await this.request('createPreset', {
      name: name,
      description: description,
      settings: JSON.stringify(settings),
    })
    return data.id
  }

  async addModel(
    name: string,
    modelName: string,
    description: string,
    providerId: number,
    settings: object,
  ) {
    await this.request('createModel', {
      name: name,
      modelName: modelName,
      description: description,
      providerId: providerId,
      settings: JSON.stringify(settings),
    })
  }

  async addProvider(
    name: string,
    url: string,
    apiKey: string,
    description: string,
    settings: object,
    type: 'openai' | 'google' | 'azure',
  ) {
    let resp = await this.request('createProvider', {
      name: name,
      baseUrl: url,
      apiKey: apiKey,
      description: description,
      settings: JSON.stringify(settings),
      type: type,
    })
    return resp.id
  }

  async getProviders() {
    let data: {
      providers: any[]
    } = await this.request('getProviders', {})
    let providers = data.providers.map((provider) => {
      let ret = {
        id: provider.Id,
        name: provider.Name,
        url: provider.Url,
        apiKey: provider.ApiKey,
        description: provider.Description,
        settings: provider.Settings,
        type: provider.Type,
        models: [],
      }
      ret.models = provider.Models.map((model: any) => {
        return {
          id: model.Id,
          name: model.Name,
          modelName: model.ModelName,
          description: model.Description,
          provider: ret,
        }
      })
      return ret
    })
    return providers
  }

  async getPresets() {
    let data: {
      presets: any[]
    } = await this.request('getPresets', {})
    console.log(data)
    return data.presets.map((preset) => {
      return {
        id: preset.Id,
        name: preset.Name,
        description: preset.Description,
        settings: preset.Settings,
      }
    })
  }
}

export const api = new Api()
