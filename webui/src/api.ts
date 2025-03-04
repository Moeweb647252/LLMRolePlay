import { sha256 } from 'js-sha256'
import { useSettingsStore } from './stores/settings'
import axios from 'axios'
import { useRouter } from 'vue-router'
import { Preset } from './types/preset'
import { Character } from './types/character'
import { Model } from './types/provider'
import { FullChat, type Message } from './types/chat'
import { User } from './types/user'
import { Chat } from './types/chat'
import { Template } from './types/template'
import { Participant } from './types/chat'
import { Provider } from './types/provider'
import { EventSourcePolyfill } from 'event-source-polyfill'

export class NoTokenError extends Error {
  constructor() {
    super('No token found')
  }
}

export class Api {
  store: ReturnType<typeof useSettingsStore> | null = null
  constructor() {}

  async request(path: string, body: object, method: string = 'POST'): Promise<any> {
    const router = useRouter()
    const token = this.store!.user?.token
    if (!token && path !== 'login') {
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
    const data = response.data
    if (data.code == 501) {
      throw new NoTokenError()
    } else if (data.code != 200) {
      throw new Error(data.msg)
    }
    return data.data
  }

  async login(email: string, password: string): Promise<void> {
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

  async logout(): Promise<void> {
    this.store!.user = null
  }

  async check(): Promise<void> {
    await this.request('check', {}, 'GET')
  }

  async addCharacter(
    name: string,
    description: string,
    content: { key: string; value: string }[],
    settings: object,
    isPublic: boolean = false,
  ): Promise<number> {
    const data = await this.request('createCharacter', {
      name: name,
      description: description,
      content: JSON.stringify(content),
      settings: JSON.stringify(settings),
      isPublic: isPublic,
    })
    return data.id
  }

  async getCharacters(): Promise<Character[]> {
    const data: {
      characters: any[]
    } = await this.request('getCharacters', {})
    return data.characters.map((character) => {
      return new Character(
        character.id,
        character.name,
        JSON.parse(character.content),
        JSON.parse(character.settings),
        character.description,
        character.isPublic,
        character.avatar,
      )
    })
  }

  async deleteCharacter(id: number): Promise<void> {
    await this.request('deleteCharacter', {
      characterId: id,
    })
  }

  async updateCharacter(
    id: number,
    options: {
      name?: string
      description?: string
      content?: { key: string; value: string }[]
      settings?: object
      isPublic?: boolean
    } = {},
  ): Promise<void> {
    await this.request('updateCharacter', {
      characterId: id,
      name: options.name ?? null,
      description: options.description ?? null,
      settings: options.settings != null ? JSON.stringify(options.settings) : null,
      content: options.content != null ? JSON.stringify(options.content) : null,
      isPublic: options.isPublic ?? null,
    })
  }

  async getPublicCharacters(): Promise<Character[]> {
    const data: {
      characters: any[]
    } = await this.request('getPublicCharacters', {})
    return data.characters.map((character) => {
      return new Character(
        character.id,
        character.name,
        JSON.parse(character.content),
        JSON.parse(character.settings),
        character.description,
        character.isPublic,
        character.avatar,
      )
    })
  }

  async addPreset(
    name: string,
    description: string,
    content: { key: string; value: string }[],
    settings: object,
    isPublic: boolean = false,
  ): Promise<number> {
    const data = await this.request('createPreset', {
      name: name,
      description: description,
      content: JSON.stringify(content),
      settings: JSON.stringify(settings),
      isPublic: isPublic,
    })
    return data.id
  }

  async getPresets(): Promise<Preset[]> {
    const data: {
      presets: any[]
    } = await this.request('getPresets', {})
    return data.presets.map((preset) => {
      return new Preset(
        preset.id,
        preset.name,
        preset.description,
        JSON.parse(preset.content),
        JSON.parse(preset.settings),
        preset.isPublic,
      )
    })
  }

  async deletePreset(id: number): Promise<void> {
    await this.request('deletePreset', {
      presetId: id,
    })
  }

  async updatePreset(
    id: number,
    options: {
      name?: string
      description?: string
      content?: { key: string; value: string }[]
      settings?: object
      isPublic?: boolean
    } = {},
  ): Promise<void> {
    await this.request('updatePreset', {
      presetId: id,
      name: options.name ?? null,
      description: options.description ?? null,
      content: options.content != null ? JSON.stringify(options.content) : null,
      settings: options.settings != null ? JSON.stringify(options.settings) : null,
      isPublic: options.isPublic ?? null,
    })
  }

  async getPublicPresets(): Promise<Preset[]> {
    const data: {
      presets: any[]
    } = await this.request('getPublicPresets', {})
    return data.presets.map((preset) => {
      return new Preset(
        preset.id,
        preset.name,
        preset.description,
        JSON.parse(preset.content),
        JSON.parse(preset.settings),
        preset.isPublic,
      )
    })
  }

  async addProvider(
    name: string,
    url: string,
    apiKey: string,
    description: string,
    settings: object,
    type: 'openai' | 'google' | 'azure',
  ): Promise<number> {
    const resp = await this.request('createProvider', {
      name: name,
      baseUrl: url,
      apiKey: apiKey,
      description: description,
      settings: JSON.stringify(settings),
      type: type,
    })
    return resp.id
  }

  async getProviders(): Promise<Provider[]> {
    const data: {
      providers: any[]
    } = await this.request('getProviders', {})
    return data.providers.map((provider) => {
      const providerObj = new Provider(
        provider.id,
        provider.name,
        provider.baseUrl,
        provider.apiKey,
        provider.description,
        JSON.parse(provider.settings),
        provider.type,
        [],
      )
      providerObj.models = provider.models.map((model: any) => {
        return new Model(
          model.id,
          model.name,
          model.modelName,
          model.description,
          model.isPublic,
          JSON.parse(model.settings),
          providerObj,
        )
      })
      return providerObj
    })
  }

  async deleteProvider(id: number): Promise<void> {
    await this.request('deleteProvider', {
      providerId: id,
    })
  }

  async updateProvider(
    id: number,
    options: {
      name?: string
      url?: string
      apiKey?: string
      description?: string
      type?: string
      settings?: object
    } = {},
  ): Promise<void> {
    await this.request('updateProvider', {
      providerId: id,
      name: options.name ?? null,
      baseUrl: options.url ?? null,
      apiKey: options.apiKey ?? null,
      description: options.description ?? null,
      type: options.type ?? null,
      settings: options.settings != null ? JSON.stringify(options.settings) : null,
    })
  }

  async addModel(
    name: string,
    modelName: string,
    description: string,
    providerId: number,
    settings: object,
  ): Promise<number> {
    const data = await this.request('createModel', {
      name: name,
      modelName: modelName,
      description: description,
      providerId: providerId,
      settings: JSON.stringify(settings),
      isPublic: false,
    })
    return data.id
  }

  async deleteModel(id: number): Promise<void> {
    await this.request('deleteModel', {
      modelId: id,
    })
  }

  async updateModel(
    id: number,
    options: {
      name?: string
      modelName?: string
      description?: string
      settings?: object
      isPublic?: boolean
    } = {},
  ): Promise<void> {
    await this.request('updateModel', {
      modelId: id,
      name: options.name ?? null,
      modelName: options.modelName ?? null,
      description: options.description ?? null,
      settings: options.settings != null ? JSON.stringify(options.settings) : null,
      isPublic: options.isPublic ?? null,
    })
  }

  async getPublicModels(): Promise<Model[]> {
    const data: {
      models: any[]
    } = await this.request('getPublicModels', {})
    return data.models.map((model) => {
      return new Model(
        model.id,
        model.name,
        model.modelName,
        model.description,
        model.isPublic,
        JSON.parse(model.settings),
        null,
      )
    })
  }

  async getUsers(): Promise<User[]> {
    const data: {
      users: any[]
    } = await this.request('getUsers', {})
    return data.users.map((user) => {
      return new User(user.id, user.username, user.email, user.token, user.group.toString())
    })
  }

  async deleteUser(id: number): Promise<void> {
    await this.request('deleteUser', {
      userId: id,
    })
  }

  async updateUser(
    id: number,
    options: {
      username?: string
      email?: string
      group?: number
    } = {},
  ): Promise<void> {
    await this.request('updateUser', {
      userId: id,
      username: options.username ?? null,
      email: options.email ?? null,
      group: options.group ?? null,
    })
  }

  async addUser(username: string, email: string, password: string, group: string): Promise<number> {
    const data = await this.request('createUser', {
      username: username,
      email: email,
      password: sha256(password),
      group: parseInt(group),
    })
    return data.id
  }

  async getChats(): Promise<Chat[]> {
    const data: {
      chats: any[]
    } = await this.request('getChats', {})
    return data.chats.map((chat) => {
      return new Chat(chat.id, chat.name, chat.participants, JSON.parse(chat.settings))
    })
  }

  async getFullChat(chatId: number): Promise<FullChat> {
    const data: {
      chat: any
      messages: any[]
      participants: any[]
      settings: any
    } = await this.request('getFullChat', {
      chatId: chatId,
    })
    return new FullChat(
      data.chat.id,
      data.chat.name,
      data.chat.description,
      data.chat.participants.map((participant: any) => {
        return new Participant(
          participant.id,
          participant.name,
          new Model(
            participant.model.id,
            participant.model.name,
            participant.model.modelName,
            participant.model.description,
            participant.model.isPublic,
            JSON.parse(participant.model.settings),
            null,
          ),
          participant.presets.map((preset: any) => {
            return new Preset(
              preset.id,
              preset.name,
              preset.description,
              JSON.parse(preset.content),
              JSON.parse(preset.settings),
              preset.isPublic,
            )
          }),
          new Character(
            participant.character.id,
            participant.character.name,
            JSON.parse(participant.character.settings),
            participant.character.description,
            participant.character.isPublic,
            participant.character.avatar,
          ),
          new Template(
            participant.template.id,
            participant.template.name,
            participant.template.content,
            participant.template.description,
            participant.template.isPublic,
          ),
        )
      }),
      JSON.parse(data.chat.settings),
    )
  }

  async updateChat(
    id: number,
    options: {
      name?: string
      description?: string
      settings?: object
    } = {},
  ): Promise<void> {
    await this.request('updateChat', {
      chatId: id,
      name: options.name ?? null,
      description: options.description ?? null,
      settings: options.settings != null ? JSON.stringify(options.settings) : null,
    })
  }

  async getMessages(chatId: number): Promise<Message[]> {
    const data: {
      messages: any[]
    } = await this.request('getMessages', {
      chatId: chatId,
    })
    return data.messages.map((message) => {
      return {
        id: message.id,
        content: message.content,
        role: message.role,
        participantId: message.participantId,
        createdAt: message.createdAt,
      }
    })
  }

  async getTemplates(): Promise<Template[]> {
    const data: {
      templates: any[]
    } = await this.request('getTemplates', {})
    return data.templates.map((template) => {
      return new Template(
        template.id,
        template.name,
        template.content,
        template.description,
        template.isPublic,
      )
    })
  }

  async deleteTemplate(id: number): Promise<void> {
    await this.request('deleteTemplate', {
      templateId: id,
    })
  }

  async updateTemplate(
    id: number,
    options: {
      name?: string
      content?: string
      description?: string
      isPublic?: boolean
    } = {},
  ): Promise<void> {
    await this.request('updateTemplate', {
      templateId: id,
      name: options.name ?? null,
      content: options.content ?? null,
      description: options.description ?? null,
      isPublic: options.isPublic ?? null,
    })
  }

  async addTemplate(
    name: string,
    content: string,
    description: string,
    isPublic: boolean = false,
  ): Promise<number> {
    const data = await this.request('createTemplate', {
      name: name,
      content: content,
      description: description,
      isPublic: isPublic,
    })
    return data.id
  }

  async getPublicTemplates(): Promise<Template[]> {
    const data: {
      templates: any[]
    } = await this.request('getPublicTemplates', {})
    return data.templates.map((template) => {
      return new Template(
        template.id,
        template.name,
        template.content,
        template.description,
        template.isPublic,
      )
    })
  }

  async addParticipant(
    chatId: number,
    characterId: number,
    presetIds: number[],
    templateId: number,
    modelId: number,
    name: string,
    settings: object,
  ): Promise<number> {
    const data = await this.request('createParticipant', {
      chatId: chatId,
      characterId: characterId,
      presetIds: presetIds,
      templateId: templateId,
      modelId: modelId,
      name: name,
      settings: JSON.stringify(settings),
    })
    return data.id
  }

  async deleteParticipant(id: number): Promise<void> {
    await this.request('deleteParticipant', {
      participantId: id,
    })
  }

  async updateParticipant(
    id: number,
    options: {
      characterId?: number
      presetIds?: number[]
      templateId?: number
      modelId?: number
      name?: string
      avatar?: number
      settings?: object
    } = {},
  ): Promise<void> {
    await this.request('updateParticipant', {
      participantId: id,
      characterId: options.characterId ?? null,
      presetIds: options.presetIds ?? null,
      templateId: options.templateId ?? null,
      name: options.name ?? null,
      modelId: options.modelId ?? null,
      avatar: options.avatar ?? null,
      settings: options.settings != null ? JSON.stringify(options.settings) : null,
    })
  }

  async uploadFile(data: ArrayBuffer): Promise<number> {
    const resp = await this.request('createFile', data)
    return resp.id
  }

  async addChat(name: string, description: string, settings: any): Promise<number> {
    const data = await this.request('createChat', {
      name: name,
      description: description,
      settings: JSON.stringify(settings),
    })
    return data.id
  }

  async deleteChat(id: number): Promise<void> {
    await this.request('deleteChat', {
      chatId: id,
    })
  }

  async addMessage(chatId: number, content: string, role: string): Promise<number> {
    const data = await this.request('createMessage', {
      chatId: chatId,
      content: content,
      role: role,
    })
    return data.id
  }

  async deleteMessage(id: number): Promise<void> {
    await this.request('deleteMessage', {
      messageId: id,
    })
  }

  async updateMessage(id: number, content: string): Promise<void> {
    await this.request('updateMessage', {
      messageId: id,
      content: content,
    })
  }
}

export const api = new Api()

export const generate = async (
  participantId: number,
  callback: (data: string) => void,
): Promise<number> => {
  const token = api.store!.user?.token
  if (!token) {
    throw new NoTokenError()
  }
  const headers = {
    Token: token,
    'Content-Type': 'application/json',
  }
  const source = new EventSourcePolyfill(`/api/completion/${participantId}`, {
    headers: headers,
  })
  let id
  source.onerror = (event) => {
    source.close()
  }
  return await new Promise((resolve) => {
    source.onmessage = (event) => {
      const data = JSON.parse(event.data)
      if (data.delta) {
        callback(data.delta)
      }
      if (data.id) {
        resolve(data.id)
      }
    }
  })
}
