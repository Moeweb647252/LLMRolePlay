<script setup lang="ts">
import type { EditModelForm } from '@/types/modal'
import { NForm, NFormItem, NModal } from 'naive-ui'
import { toRef } from 'vue'
import SettingsInput from '../SettingsInput.vue'
import { api } from '@/api'
import SettingsSwitch from '../SettingsSwitch.vue'

const props = defineProps<{
  form: EditModelForm
}>()

const form = toRef(props, 'form')

const emit = defineEmits(['confirm'])

const show = defineModel('show', {
  type: Boolean,
  default: false,
})
</script>

<template>
  <NModal v-model:show="show">
    <NForm v-if="form">
      <NFormItem label="名称">
        <SettingsInput
          :value="form.name"
          @confirm="
            async (name) => {
              await api.updateModel(form.id!, {
                name: name,
              })
              form.name = name
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="模型">
        <SettingsInput
          :value="form.modelName"
          @confirm="
            async (name) => {
              await api.updateModel(form.id!, {
                modelName: name,
              })
              form.modelName = name
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="描述">
        <SettingsInput
          :value="form.description"
          @confirm="
            async (description) => {
              await api.updateModel(form.id!, {
                description: description,
              })
              form.description = description
            }
          "
        />
      </NFormItem>
      <NFormItem label="公开">
        <SettingsSwitch
          :value="form.isPublic"
          @confirm="
            async (isPublic) => {
              await api.updateModel(form.id!, {
                isPublic: isPublic,
              })
              form.isPublic = isPublic
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="温度">
        <SettingsInput
          :value="form.settings.temperature"
          @confirm="
            async (temperature) => {
              await api.updateModel(form.id!, {
                settings: {
                  temperature: temperature,
                },
              })
              form.settings.temperature = temperature
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="Top P">
        <SettingsInput
          :value="form.settings.top_p"
          @confirm="
            async (top_p) => {
              await api.updateModel(form.id!, {
                settings: {
                  top_p: top_p,
                },
              })
              form.settings.top_p = top_p
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="Max Tokens">
        <SettingsInput
          :value="form.settings.max_tokens"
          @confirm="
            async (max_tokens) => {
              await api.updateModel(form.id!, {
                settings: {
                  max_tokens: max_tokens,
                },
              })
              form.settings.max_tokens = max_tokens
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
    </NForm>
  </NModal>
</template>
