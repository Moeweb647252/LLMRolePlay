<script setup lang="ts">
import { MdCreate } from '@vicons/ionicons4'
import { ref } from 'vue'
import { NInputGroup, NSelect, NInput, NButton, NSpace, NIcon } from 'naive-ui'
import type { Options } from '@/types/modal'

type SelectInput = string[] | number[]

type Input = string | number | SelectInput

let editingValue = ref(null as Input | null)
let editing = ref(false)
const props = defineProps<{
  value: Input | null
  type?: 'text' | 'select' | 'textarea'
  multiple?: boolean
  options?: Options
}>()

const emit = defineEmits(['confirm'])

const startEditing = () => {
  editingValue.value = props.value
  editing.value = true
}

const confirm = () => {
  emit('confirm', editingValue.value)
  editing.value = false
}

const cancel = () => {
  editing.value = false
  editingValue.value = null
}
</script>

<template>
  <div v-if="editing">
    <NInputGroup>
      <NInput
        v-if="props.type == 'text' || props.type == null"
        v-model:value="editingValue as string"
      />
      <NSelect
        v-if="props.type == 'select'"
        v-model:value="editingValue as SelectInput"
        style="min-width: 10em"
        :multiple="multiple"
        :options="props.options"
      />
      <NInput
        v-if="props.type == 'textarea'"
        v-model:value="editingValue as string"
        type="textarea"
      />
      <NButton @click="confirm"> 确定 </NButton>
      <NButton @click="cancel"> 取消 </NButton>
    </NInputGroup>
  </div>
  <div v-else>
    <NSpace :wrap="false" align="center">
      <div v-if="props.multiple">
        {{
          (value as SelectInput)
            ?.map((v: any) => options?.find((o) => o.value == v)?.label)
            .join()
        }}
      </div>
      <div v-else>
        {{ options?.find((o) => o.value == value)?.label || value }}
      </div>
      <NButton quaternary circle @click="startEditing">
        <template #icon>
          <NIcon>
            <MdCreate />
          </NIcon>
        </template>
      </NButton>
    </NSpace>
  </div>
</template>
