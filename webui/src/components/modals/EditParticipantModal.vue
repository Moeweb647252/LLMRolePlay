<script setup lang="ts">
import { NModal, NForm, NFormItem } from 'naive-ui'
import SettingsInput from '../SettingsInput.vue'
import { toRef } from 'vue'
import type { EditParticipantForm, Options } from '@/types/modal'
import { api } from '@/api'

const props = defineProps<{
  models: Options
  presets: Options
  characters: Options
  templates: Options
  value: EditParticipantForm
}>()

const form = toRef(props, 'value')
const show = defineModel('show', {
  type: Boolean,
  default: false,
})
const emit = defineEmits(['confirm'])
</script>

<template>
  <NModal
    v-model:show="show"
    title="编辑参与者"
    preset="card"
    style="width: fit-content; min-width: 25em"
    size="medium"
  >
    <NForm v-if="form" label-placement="left">
      <NFormItem label="姓名">
        <SettingsInput
          :value="form!.name"
          @confirm="
            async (name) => {
              await api.updateParticipant(form!.id!, {
                name: form!.name!,
              })
              form!.name = name
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="模型">
        <SettingsInput
          type="select"
          :options="models"
          :value="form!.model"
          @confirm="
            async (model) => {
              await api.updateParticipant(form!.id, {
                modelId: model,
              })
              form!.model = model
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="预设">
        <SettingsInput
          type="select"
          multiple
          :options="presets"
          :value="form!.presets"
          @confirm="
            async (_presets: number[]) => {
              await api.updateParticipant(form!.id, {
                presetIds: _presets,
              })
              form!.presets = _presets
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="角色">
        <SettingsInput
          :value="form!.character"
          type="select"
          :options="characters"
          @confirm="
            async (character: number) => {
              await api.updateParticipant(form!.id, {
                characterId: character,
              })
              form!.character = character
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="模板">
        <SettingsInput
          :value="form!.template"
          type="select"
          :options="templates"
          @confirm="
            async (template: number) => {
              await api.updateParticipant(form!.id, {
                templateId: template,
              })
              form!.template = template
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
    </NForm>
  </NModal>
</template>
