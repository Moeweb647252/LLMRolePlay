<script setup lang="ts">
import type { EditModelForm } from '@/types/modal'
import { toRef } from 'vue'
import { NModal, NForm, NFormItem } from 'naive-ui'
import SettingsInput from '../SettingsInput.vue'
import { api } from '@/api'
import SettingsSwitch from '../SettingsSwitch.vue'

const props = defineProps<{
  form: EditModelForm
}>()

const show = defineModel<boolean>('show', {
  default: false,
})
const emit = defineEmits(['cancel', 'confirm'])

const form = toRef(props, 'form')

const cancel = () => {
  emit('cancel')
}
</script>

<template>
  <NModal
    v-model:show="show"
    title="编辑模型"
    size="medium"
    preset="card"
    style="width: fit-content; min-width: 25em"
    @close="cancel"
  >
    <NForm>
      <NFormItem label="名称">
        <SettingsInput
          :value="form.name"
          @confirm="
            async (name) => {
              await api.updateModel(form.id, { name })
              form.name = name
              emit('confirm', form)
            }
          "
        ></SettingsInput>
      </NFormItem>
      <NFormItem label="模型名称">
        <SettingsInput
          :value="form.modelName"
          @confirm="
            async (modelName) => {
              await api.updateModel(form.id, { modelName })
              form.modelName = modelName
              emit('confirm', form)
            }
          "
        ></SettingsInput>
      </NFormItem>
      <NFormItem label="描述">
        <SettingsInput
          :value="form.description"
          @confirm="
            async (description) => {
              await api.updateModel(form.id, { description })
              form.description = description
              emit('confirm', form)
            }
          "
        ></SettingsInput>
      </NFormItem>
      <NFormItem label="是否公开">
        <SettingsSwitch
          :value="form.isPublic"
          @confirm="
            async (isPublic) => {
              await api.updateModel(form.id, { isPublic })
              form.isPublic = isPublic
              emit('confirm', form)
            }
          "
        ></SettingsSwitch>
      </NFormItem>
      <NFormItem label="温度">
        <SettingsInput
          :value="form.settings?.temperature"
          @confirm="
            async (temperature) => {
              await api.updateModel(form.id, { settings: { temperature } })
              form.settings!.temperature = temperature
              emit('confirm', form)
            }
          "
        ></SettingsInput>
      </NFormItem>
      <NFormItem label="Top P">
        <SettingsInput
          :value="form.settings?.top_p"
          @confirm="
            async (top_p) => {
              await api.updateModel(form.id, { settings: { top_p } })
              form.settings!.top_p = top_p
              emit('confirm', form)
            }
          "
        ></SettingsInput>
      </NFormItem>
      <NFormItem label="最大长度">
        <SettingsInput
          :value="form.settings?.max_tokens"
          @confirm="
            async (max_tokens) => {
              await api.updateModel(form.id, { settings: { max_tokens } })
              form.settings!.max_tokens = max_tokens
              emit('confirm', form)
            }
          "
        ></SettingsInput>
      </NFormItem>
    </NForm>
  </NModal>
</template>
