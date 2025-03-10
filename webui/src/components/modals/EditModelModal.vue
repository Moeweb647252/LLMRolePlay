<script setup lang="ts">
import type { EditModelForm } from '@/types/modal'
import { ref, toRef } from 'vue'
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
  <n-modal
    v-model:show="show"
    title="编辑模型"
    size="medium"
    preset="card"
    style="width: fit-content; min-width: 25em"
    @close="cancel"
  >
    <n-form>
      <n-form-item label="名称">
        <SettingsInput
          :value="form.name"
          @confirm="
            async (name) => {
              await api.updateModel(form.id, { name })
              form.name = name
            }
          "
        ></SettingsInput>
      </n-form-item>
      <n-form-item label="描述">
        <SettingsInput
          :value="form.description"
          @confirm="
            async (description) => {
              await api.updateModel(form.id, { description })
              form.description = description
            }
          "
        ></SettingsInput>
      </n-form-item>
      <n-form-item label="模型名称">
        <SettingsInput
          :value="form.modelName"
          @confirm="
            async (modelName) => {
              await api.updateModel(form.id, { modelName })
              form.modelName = modelName
            }
          "
        ></SettingsInput>
      </n-form-item>
      <n-form-item label="是否公开">
        <SettingsSwitch
          :value="form.isPublic"
          @confirm="
            async (isPublic) => {
              await api.updateModel(form.id, { isPublic })
              form.isPublic = isPublic
            }
          "
        ></SettingsSwitch>
      </n-form-item>
    </n-form>
  </n-modal>
</template>
