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

  async addCharacter(
    name: string,
    description: string,
    settings: object,
    isPublic: boolean = false,
  ) {
    let data = await this.request('createCharacter', {
      name: name,
      description: description,
      settings: JSON.stringify(settings),
      isPublic: isPublic,
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
    isPublic: boolean | null = null,
  ) {
    await this.request('updateCharacter', {
      characterId: id,
      name: name,
      description: description,
      settings: settings != null ? JSON.stringify(settings) : null,
      isPublic: isPublic,
    })
  }

  async addPreset(name: string, description: string, settings: object, isPublic: boolean = false) {
    let data = await this.request('createPreset', {
      name: name,
      description: description,
      settings: JSON.stringify(settings),
      isPublic: isPublic,
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
    isPublic: boolean | null = null,
  ) {
    await this.request('updatePreset', {
      presetId: id,
      name: name,
      description: description,
      settings: settings != null ? JSON.stringify(settings) : null,
      isPublic: isPublic,
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
      isPublic: false,
    })
    return data.id
  }

  async deleteModel(id: number) {
    await this.request('deleteModel', {
      modelId: id,
    })
  }

  async updateModel(
    id: number,
    name: string | null = null,
    modelName: string | null = null,
    description: string | null = null,
    settings: object | null = null,
    isPublic: boolean | null = null,
  ) {
    await this.request('updateModel', {
      modelId: id,
      name: name,
      modelName: modelName,
      description: description,
      settings: settings != null ? JSON.stringify(settings) : null,
      isPublic: isPublic,
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

  async getChats() {
    let data: {
      chats: any[]
    } = await this.request('getChats', {})
    return data.chats
  }

  async getMessages(chatId: number) {
    let data: {
      messages: any[]
    } = await this.request('getMessages', {
      chatId: chatId,
    })
    return data.messages
  }

  async getTemplates() {
    let data: {
      templates: any[]
    } = await this.request('getTemplates', {})
    return data.templates
  }
  async deleteTemplate(id: number) {
    await this.request('deleteTemplate', {
      templateId: id,
    })
  }
  async updateTemplate(
    id: number,
    name: string | null = null,
    content: string | null = null,
    description: string | null = null,
    isPublic: boolean | null = null,
  ) {
    await this.request('updateTemplate', {
      templateId: id,
      name: name,
      content: content,
      description: description,
      isPublic: isPublic,
    })
  }
  async addTemplate(name: string, content: string, description: string, isPublic: boolean = false) {
    let data = await this.request('createTemplate', {
      name: name,
      content: content,
      description: description,
      isPublic: isPublic,
    })
    return data.id
  }

  async addParticipant(
    chatId: number,
    characterId: number,
    presetId: number,
    templateId: number,
    name: string,
    avatar: number,
    settings: object,
  ) {
    let data = await this.request('addParticipant', {
      chatId: chatId,
      characterId: characterId,
      presetId: presetId,
      templateId: templateId,
      name: name,
      avatar: avatar,
      settings: JSON.stringify(settings),
    })
    return data.id
  }

  async deleteParticipant(id: number) {
    await this.request('deleteParticipant', {
      participantId: id,
    })
  }
  async updateParticipant(
    id: number,
    characterId: number | null = null,
    presetId: number | null = null,
    templateId: number | null = null,
    name: string | null = null,
    avatar: number | null = null,
    settings: object | null = null,
  ) {
    await this.request('updateParticipant', {
      participantId: id,
      characterId: characterId,
      presetId: presetId,
      templateId: templateId,
      name: name,
      avatar: avatar,
      settings: settings != null ? JSON.stringify(settings) : null,
    })
  }
  async uploadFile(data: ArrayBuffer) {
    let resp = await this.request('createFile', data)
    return resp.id
  }
}

export const api = new Api()

export const generate = async (
  chatId: number,
  participantId: number,
  callback: (data: string) => void,
) => {
  const token = api.store!.user?.token
  if (!token) {
    throw new NoTokenError()
  }
  const headers = {
    Token: token,
  }
  const resp = await fetch('/api/completion', {
    method: 'POST',
    headers: headers,
    body: JSON.stringify({
      chatId: chatId,
      participantId: participantId,
    }),
  })
  let reader = resp.body?.getReader()
  let buffer = ''
  if (reader) {
    let decoder = new TextDecoder('utf-8')
    while (true) {
      // Read the next chunk of data
      const { done, value } = await reader.read()

      // If the stream is done, break the loop
      if (done) {
        break
      }

      // Decode the chunk and add it to the buffer
      buffer += decoder.decode(value, { stream: true })

      // Process lines in the buffer
      let lineEndIndex
      while ((lineEndIndex = buffer.indexOf('\n')) >= 0) {
        // Extract the line
        const line = buffer.slice(0, lineEndIndex - 1)
        buffer = buffer.slice(lineEndIndex + 1)

        // Process the line (e.g., log it or do something else)
        if (line.startsWith('data: ')) {
          const data = line.slice(6)
          if (data === '[DONE]') {
            break
          }
          callback(data)
        }
      }
    }
  }
}
