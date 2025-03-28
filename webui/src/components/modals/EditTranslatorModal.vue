<script setup lang="ts">
import { NModal, NForm, NFormItem } from 'naive-ui'
import SettingsInput from '../SettingsInput.vue'
import { toRef } from 'vue'
import type { EditTranslatorForm, Options } from '@/types/modal'
import { api } from '@/api'

const props = defineProps<{
  models: Options
  presets: Options
  templates: Options
  value: EditTranslatorForm
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
    title="编辑翻译器"
    preset="card"
    style="width: fit-content; min-width: 25em"
    size="medium"
  >
    <NForm v-if="form" label-placement="left">
      <NFormItem label="名称">
        <SettingsInput
          :value="form!.name"
          @confirm="
            async (name) => {
              await api.updateTranslator(form!.id, {
                name: name,
              })
              form!.name = name
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="描述">
        <SettingsInput
          :value="form!.description"
          @confirm="
            async (description) => {
              await api.updateTranslator(form!.id, {
                description: description,
              })
              form!.description = description
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="模型">
        <SettingsInput
          type="select"
          :options="models"
          :value="form!.modelId"
          @confirm="
            async (modelId) => {
              await api.updateTranslator(form!.id, {
                modelId: modelId,
              })
              form!.modelId = modelId
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
          :value="form!.presetIds"
          @confirm="
            async (_presets: number[]) => {
              await api.updateTranslator(form!.id, {
                presetIds: _presets,
              })
              form!.presetIds = _presets
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="模板">
        <SettingsInput
          :value="form!.templateId"
          type="select"
          :options="templates"
          @confirm="
            async (templateId: number) => {
              await api.updateTranslator(form!.id, {
                templateId: templateId,
              })
              form!.templateId = templateId
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
    </NForm>
  </NModal>
</template>
