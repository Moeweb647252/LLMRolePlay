<script setup lang="ts">
import { NModal, NForm, NFormItem, NSpace, NButton } from 'naive-ui'
import SettingsInput from '../SettingsInput.vue'
import SettingsSwitch from '../SettingsSwitch.vue'
import SettingsDynamicInput from '../SettingsDynamicInput.vue'
import { toRef } from 'vue'
import { api } from '@/api'
import type { EditPresetForm } from '@/types/modal'

const props = defineProps<{
  preset: EditPresetForm | null
}>()

const form = toRef(props, 'preset')
const show = defineModel('show', {
  type: Boolean,
  default: false,
})
const emit = defineEmits(['confirm'])
</script>

<template>
  <NModal
    v-model:show="show"
    title="编辑预设"
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
              await api.updatePreset(form!.id!, {
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
              await api.updatePreset(form!.id!, {
                description: description,
              })
              form!.description = description
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="公开">
        <SettingsSwitch
          :value="form!.isPublic"
          @confirm="
            async (isPublic) => {
              await api.updatePreset(form!.id!, {
                isPublic: isPublic,
              })
              form!.isPublic = isPublic
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="设置">
        <SettingsDynamicInput
          :value="form!.content"
          @confirm="
            async (content) => {
              await api.updatePreset(form!.id!, {
                content: content,
              })
              form!.content = content
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
    </NForm>
    <template #footer>
      <NSpace justify="end">
        <NButton @click="show = false">关闭</NButton>
      </NSpace>
    </template>
  </NModal>
</template>
