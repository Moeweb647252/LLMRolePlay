import type { AddParticipantForm, EditParticipantForm } from './participant'

export type AddChatForm = {
  name: string
  description: string | null
  settings: object
  participants: AddParticipantForm[]
}

export type EditChatForm = {
  id: number
  name: string
  description: string | null
  settings: object
  participants: EditParticipantForm[]
}
