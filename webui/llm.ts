import { OpenAI as _OpenAI } from 'openai'

export interface Message {
  content: string
  role: 'user' | 'assistant' | 'system'
}

export interface LLM {
  completion: (messages: Message[], callback: (delta: string) => void) => Promise<number>
}

export class OpenAI implements LLM {
  openai: _OpenAI

  OpenAI(modelId: number, token: string) {
    this.openai = new _OpenAI({
      baseURL: `/api/proxy/${token}/${modelId}`,
      apiKey: '',
      dangerouslyAllowBrowser: true,
    })
  }

  async completion(messages: Message[], callback: (delta: string) => void): Promise<number> {
    return 0
  }
}
