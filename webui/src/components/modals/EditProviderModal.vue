<script setup lang="ts">
import type { EditModelForm, EditProviderForm } from '@/types/modal'
import {
  NDynamicTags,
  NForm,
  NFormItem,
  NModal,
  NTag,
  useMessage,
  useModal,
} from 'naive-ui'
import { h, toRef } from 'vue'
import SettingsInput from '../SettingsInput.vue'
import { api } from '@/api'

const message = useMessage()
const modal = useModal()

const renderModelTags = (model: EditModelForm) => {
  return h(
    NTag,
    {
      closable: true,
      onClose: () => {
        form.value.models.splice(form.value.models.indexOf(model), 1)
      },
      onClick: () => {},
    },
    {
      default: () => model,
    },
  )
}

const props = defineProps<{
  form: EditProviderForm
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
              await api.updateProvider(form.id, { name })
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
              await api.updateProvider(form.id, { description })
              form.description = description
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="模型">
        <NDynamicTags></NDynamicTags>
      </NFormItem>
    </NForm>
  </NModal>
</template>
