<script setup lang="ts">
import { MdCreate } from '@vicons/ionicons4'
import { ref } from 'vue'

let editingValue = ref(null as string | null)
let editing = ref(false)
const props = defineProps<{
  type?: 'text' | 'select'
  options?: {
    label: string
    value: string
  }[]
}>()

const value = defineModel<string | null>('value', {
  default: null,
})

const emit = defineEmits(['confirm'])

const startEditing = () => {
  editingValue.value = value.value
  editing.value = true
}

const confirm = () => {
  value.value = editingValue.value
  emit('confirm')
  editing.value = false
}

const cancel = () => {
  editing.value = false
  editingValue.value = null
}
</script>

<template>
  <div v-if="editing">
    <n-input-group>
      <n-input v-if="props.type == 'text' || props.type == null" v-model:value="editingValue" />
      <n-select v-if="props.type == 'select'" v-model:value="editingValue" :options="props.options">
      </n-select>
      <n-button @click="confirm">确定</n-button>
      <n-button @click="cancel">取消</n-button>
    </n-input-group>
  </div>
  <div v-else>
    <n-space :wrap="false" align="center">
      {{ options?.find((o) => o.value == value)?.label || value }}
      <n-button quaternary circle @click="startEditing">
        <template #icon>
          <n-icon>
            <MdCreate />
          </n-icon>
        </template>
      </n-button>
    </n-space>
  </div>
</template>
