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
      group: resp.group.toString(),
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

  async getCharacters() {
    let data: {
      characters: any[]
    } = await this.request('getCharacters', {})
    return data.characters.map((character) => {
      return {
        id: character.id,
        name: character.name,
        description: character.description,
        settings: JSON.parse(character.settings),
      }
    })
  }

  async deleteCharacter(id: number) {
    await this.request('deleteCharacter', {
      characterId: id,
    })
  }

  async updateCharacter(
    id: number,
    name: string | null = null,
    description: string | null = null,
    settings: object | null = null,
  ) {
    await this.request('updateCharacter', {
      characterId: id,
      name: name,
      description: description,
      settings: settings != null ? JSON.stringify(settings) : null,
    })
  }

  async addPreset(name: string, description: string, settings: object) {
    let data = await this.request('createPreset', {
      name: name,
      description: description,
      settings: JSON.stringify(settings),
    })
    return data.id
  }

  async getPresets() {
    let data: {
      presets: any[]
    } = await this.request('getPresets', {})
    console.log(data)
    return data.presets.map((preset) => {
      return {
        id: preset.id,
        name: preset.name,
        description: preset.description,
        settings: JSON.parse(preset.settings),
      }
    })
  }

  async deletePreset(id: number) {
    await this.request('deletePreset', {
      presetId: id,
    })
  }

  async updatePreset(
    id: number,
    name: string | null = null,
    description: string | null = null,
    settings: object | null = null,
  ) {
    await this.request('updatePreset', {
      presetId: id,
      name: name,
      description: description,
      settings: settings != null ? JSON.stringify(settings) : null,
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
        id: provider.id,
        name: provider.name,
        url: provider.url,
        apiKey: provider.apiKey,
        description: provider.description,
        settings: provider.settings,
        type: provider.type,
        models: [],
      }
      ret.models = provider.models.map((model: any) => {
        return {
          id: model.id,
          name: model.name,
          modelName: model.modelName,
          description: model.description,
          provider: ret,
        }
      })
      return ret
    })
    return providers
  }

  async deleteProvider(id: number) {
    await this.request('deleteProvider', {
      providerId: id,
    })
  }

  async updateProvider(
    id: number,
    name: string | null = null,
    url: string | null = null,
    apiKey: string | null = null,
    description: string | null = null,
    type: string | null = null,
    settings: object | null = null,
  ) {
    await this.request('updateProvider', {
      providerId: id,
      name: name,
      baseUrl: url,
      apiKey: apiKey,
      description: description,
      type: type,
      settings: settings != null ? JSON.stringify(settings) : null,
    })
  }

  async addModel(
    name: string,
    modelName: string,
    description: string,
    providerId: number,
    settings: object,
  ) {
    let data = await this.request('createModel', {
      name: name,
      modelName: modelName,
      description: description,
      providerId: providerId,
      settings: JSON.stringify(settings),
    })
    return data.id
  }

  async deleteModel(id: number) {
    await this.request('deleteModel', {
      modelId: id,
    })
  }

  async getUsers() {
    let data: {
      users: any[]
    } = await this.request('getUsers', {})
    data.users.forEach((user) => {
      user.group = user.group.toString()
    })
    return data.users
  }

  async deleteUser(id: number) {
    await this.request('deleteUser', {
      userId: id,
    })
  }

  async updateUser(
    id: number,
    username: string | null = null,
    email: string | null = null,
    group: number | null,
  ) {
    await this.request('updateUser', {
      userId: id,
      username: username,
      email: email,
      group: group,
    })
  }

  async addUser(username: string, email: string, password: string, group: string) {
    let data = await this.request('createUser', {
      username: username,
      email: email,
      password: sha256(password),
      group: parseInt(group),
    })
    return data.id
  }
}

export const api = new Api()
