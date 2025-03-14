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
        modal.create({
          title: '删除模型',
          content: `确定删除模型 ${model.name} ?`,
          preset: 'dialog',
          positiveText: '确定',
          negativeText: '取消',
          onPositiveClick: async () => {
            try {
              await api.deleteModel(model.id!)
              form.value!.models.splice(form.value!.models.indexOf(model), 1)
              message.success('删除成功')
            } catch (e) {
              console.log(e)
              message.error('删除失败')
            }
          },
          onNegativeClick: () => {
            console.log('取消删除')
          },
        })
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

const validate = () => {
  if (!form.value.name) {
    message.error('名称不能为空')
    return false
  }
  return true
}
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
        <NDynamicTags :render-tag="renderModelTags as any"> </NDynamicTags>
      </NFormItem>
    </NForm>
  </NModal>
</template>
