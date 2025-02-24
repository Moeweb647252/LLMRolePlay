import type { Character } from './characters'
import type { Preset } from './presets'
import type { Model } from './providers'

export interface Participant {
  id: number
  model: Model
  preset: Preset
  character: Character
}
