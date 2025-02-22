import { sha256 } from 'js-sha256'
import { useSettingsStore } from './stores/settings'
import axios from 'axios'

class NoTokenError extends Error {
  constructor() {
    super('No token found')
  }
}

export class Api {
  store: ReturnType<typeof useSettingsStore> | null = null
  constructor() {}

  async request(path: string, body: object, method: string = 'POST') {
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
    if (data.code != 200) {
      throw new Error(data.msg)
    } else if (data.code == 501) {
      throw new NoTokenError()
    }
    return data.data
  }

  async login(email: string, password: string) {
    const resp = await this.request('login', {
      email: email,
      password: sha256(password),
    })
    this.store!.user = {
      email: resp.data.email,
      username: resp.data.username,
      id: resp.data.id,
      group: resp.data.group,
      token: resp.data.token,
    }
  }

  async logout() {
    this.store!.user = null
  }

  async addPreset(name: string, description: string, settings: object) {
    await this.request('createPreset', {
      name: name,
      description: description,
      settings: settings,
    })
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
      settings: settings,
    })
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
