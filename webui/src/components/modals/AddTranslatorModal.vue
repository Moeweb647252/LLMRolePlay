<script lang="ts" setup>
import { api } from '@/api'
import type { Options } from '@/types/modal'
import type { AddTranslatorForm } from '@/types/modal/translator'
import { Translator } from '@/types/translator'
import {
  NModal,
  NForm,
  NFormItem,
  NSelect,
  NInput,
  NButton,
  useMessage,
} from 'naive-ui'
import { ref } from 'vue'

const message = useMessage()

const props = defineProps<{
  models: Options
  presets: Options
  templates: Options
}>()

const form = ref<AddTranslatorForm>({
  name: null,
  description: null,
  presetIds: [],
  modelId: null,
  templateId: null,
})

const show = defineModel<boolean>('show', {
  default: false,
})

const validate = () => {
  if (!form.value.name) {
    message.error('名称不能为空')
    return false
  }
  if (!form.value.modelId) {
    message.error('模型不能为空')
    return false
  }
  if (!form.value.templateId) {
    message.error('模板不能为空')
    return false
  }
  return true
}

const confirm = async () => {
  if (!validate()) return
  let id = await api.addTranslator(
    form.value.name!,
    form.value.description,
    form.value.modelId!,
    form.value.presetIds,
    form.value.templateId!,
    {},
  )

  emit(
    'confirm',
    new Translator(
      id,
      form.value.name!,
      form.value.description,
      form.value.modelId!,
      form.value.presetIds,
      form.value.templateId!,
    ),
  )
  show.value = false
}

const emit = defineEmits<{
  cancel: []
  confirm: Translator[]
}>()

const cancel = () => {
  show.value = false
  emit('cancel')
}
</script>

<template>
  <NModal
    v-model:show="show"
    title="添加Translator"
    size="medium"
    preset="card"
    style="width: fit-content; min-width: 25em"
  >
    <template #header>添加翻译者</template>
    <template #default>
      <NForm>
        <NFormItem label="名称">
          <NInput v-model:value="form.name" />
        </NFormItem>
        <NFormItem label="描述">
          <NInput v-model:value="form.description" />
        </NFormItem>
        <NFormItem label="预设">
          <NSelect
            v-model:value="form.presetIds"
            multiple
            clearable
            placeholder="请选择预设"
            :options="props.presets"
          />
        </NFormItem>
        <NFormItem label="模型">
          <NSelect
            v-model:value="form.modelId"
            clearable
            placeholder="请选择模型"
            :options="props.models"
          />
        </NFormItem>
        <NFormItem label="模板">
          <NSelect
            v-model:value="form.templateId"
            clearable
            placeholder="请选择模板"
            :options="props.templates"
          />
        </NFormItem>
      </NForm>
    </template>
    <template #footer>
      <NButton @click="cancel">取消</NButton>
      <NButton type="primary" @click="confirm"> 确定 </NButton>
    </template>
  </NModal>
</template>
