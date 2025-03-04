<script setup lang="ts">
import { useMessage } from 'naive-ui'
import { ref } from 'vue'
import {
  NModal,
  NFormItem,
  NSelect,
  NForm,
  NInput,
  NSpace,
  NButton,
} from 'naive-ui'
import type { AddParticipantForm, Options } from '@/types/modals'

defineProps<{
  models: Options
  presets: Options
  characters: Options
  templates: Options
}>()

const show = defineModel<boolean>('show', {
  default: false,
})

const message = useMessage()

const emit = defineEmits(['cancel', 'confirm'])

const form = ref<AddParticipantForm>({
  name: '',
  settings: {},
  model: null,
  presets: [],
  character: null,
  template: null,
})

const confirm = () => {
  if (!validate()) return
  emit('confirm', form.value)
}

const validate = () => {
  if (!form.value.name) {
    message.error('姓名不能为空')
    return false
  }
  if (form.value.model == null) {
    message.error('模型不能为空')
    return false
  }
  return true
}
</script>
<template>
  <n-modal
    v-model:show="show"
    title="添加参与者"
    size="medium"
    preset="card"
    style="width: fit-content; min-width: 25em"
    @cancel="emit('cancel')"
  >
    <n-form label-placement="left">
      <n-form-item label="姓名">
        <n-input v-model:value="form.name" />
      </n-form-item>
      <n-form-item label="模型">
        <n-select v-model:value="form.model" filterable :options="models" />
      </n-form-item>
      <n-form-item label="预设">
        <n-select
          v-model:value="form.presets"
          filterable
          multiple
          :options="presets"
        />
      </n-form-item>
      <n-form-item label="角色">
        <n-select
          v-model:value="form.character"
          filterable
          :options="characters"
        />
      </n-form-item>
      <n-form-item label="模板">
        <n-select
          v-model:value="form.template"
          filterable
          :options="templates"
        />
      </n-form-item>
    </n-form>
    <template #footer>
      <n-space justify="end">
        <n-button @click="emit('cancel')"> 取消 </n-button>
        <n-button type="primary" @click="confirm"> 保存 </n-button>
      </n-space>
    </template>
  </n-modal>
</template>
