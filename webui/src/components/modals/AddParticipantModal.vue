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
import type { AddParticipantForm, Options } from '@/types/modal'

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

const emit = defineEmits<{
  cancel: []
  confirm: [AddParticipantForm]
}>()

const form = ref<AddParticipantForm>({
  name: null,
  settings: {},
  model: null,
  presets: [],
  character: null,
  template: null,
})

const confirm = () => {
  if (!validate()) return
  emit('confirm', form.value)
  show.value = false
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
  <NModal
    v-model:show="show"
    title="添加参与者"
    size="medium"
    preset="card"
    style="width: fit-content; min-width: 25em"
    @cancel="emit('cancel')"
  >
    <NForm label-placement="left">
      <NFormItem label="姓名">
        <NInput v-model:value="form.name" />
      </NFormItem>
      <NFormItem label="模型">
        <NSelect v-model:value="form.model" filterable :options="models" />
      </NFormItem>
      <NFormItem label="预设">
        <NSelect
          v-model:value="form.presets"
          filterable
          multiple
          :options="presets"
        />
      </NFormItem>
      <NFormItem label="角色">
        <NSelect
          v-model:value="form.character"
          filterable
          :options="characters"
        />
      </NFormItem>
      <NFormItem label="模板">
        <NSelect
          v-model:value="form.template"
          filterable
          :options="templates"
        />
      </NFormItem>
    </NForm>
    <template #footer>
      <NSpace justify="end">
        <NButton @click="emit('cancel')"> 取消 </NButton>
        <NButton type="primary" @click="confirm"> 保存 </NButton>
      </NSpace>
    </template>
  </NModal>
</template>
