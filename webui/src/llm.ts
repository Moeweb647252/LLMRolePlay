import { OpenAI as _OpenAI } from 'openai'
import type { Model } from './stores/providers'

export interface Message {
  content: string
  role: 'user' | 'assistant' | 'system'
}

export interface LLM {
  completion: (messages: Message[], callback: (delta: string) => void) => Promise<number>
}

export class OpenAI implements LLM {
  private openai: _OpenAI
  private model: Model

  constructor(model: Model, token: string) {
    this.model = model
    this.openai = new _OpenAI({
      baseURL: `/api/proxy/${token}/${model.id}`,
      apiKey: '',
      dangerouslyAllowBrowser: true,
    })
  }

  async completion(messages: Message[], callback: (delta: string) => void): Promise<number> {
    const stream = await this.openai.chat.completions.create(
      {
        messages: messages,
        stream: true,
        model: this.model.modelName,
      }
    );
    return 0
  }
}
