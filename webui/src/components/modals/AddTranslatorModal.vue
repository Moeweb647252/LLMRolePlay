<script lang="ts" setup>
import type { Options } from '@/types/modal'
import type { AddTranslatorForm } from '@/types/modal/translator'
import { NModal, NForm, NFormItem, NSelect, NInput } from 'naive-ui'
import { ref } from 'vue'

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
</script>

<template>
  <NModal v-model:show="show">
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
  </NModal>
</template>
