import type { ChatSettings } from '../chat'
import type { AddParticipantForm, EditParticipantForm } from './participant'

export type AddChatForm = {
  name: string | null
  description: string | null
  settings: ChatSettings
  participants: AddParticipantForm[]
}

export type EditChatForm = {
  id: number
  name: string
  description: string | null
  settings: ChatSettings
  participants: EditParticipantForm[]
}
