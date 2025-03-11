<script setup lang="ts">
import { NModal, NForm, NFormItem } from 'naive-ui'
import SettingsInput from '../SettingsInput.vue'
import SettingsSwitch from '../SettingsSwitch.vue'
import { toRef } from 'vue'
import type { EditTemplateForm } from '@/types/modal'
import { api } from '@/api'

const props = defineProps<{
  template: EditTemplateForm
}>()

const form = toRef(props, 'template')
const show = defineModel('show', {
  type: Boolean,
  default: false,
})
const emit = defineEmits(['confirm'])
</script>

<template>
  <NModal
    v-model:show="show"
    title="编辑模板"
    preset="card"
    style="width: fit-content; min-width: 25em"
    size="medium"
  >
    <NForm v-if="form" label-placement="left">
      <NFormItem label="名称">
        <SettingsInput
          :value="form.name"
          @confirm="
            async (name) => {
              await api.updateTemplate(form.id, {
                name: name,
              })
              form.name = name
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
              await api.updateTemplate(form.id, {
                description: description,
              })
              form.description = description
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="公开">
        <SettingsSwitch
          :value="form.isPublic"
          @confirm="
            async (isPublic) => {
              await api.updateTemplate(form.id, {
                isPublic: isPublic,
              })
              form.isPublic = isPublic
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="内容">
        <SettingsInput
          :value="form.content"
          type="textarea"
          @confirm="
            async (content) => {
              await api.updateTemplate(form.id, {
                content: content,
              })
              form.content = content
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
    </NForm>
  </NModal>
</template>
