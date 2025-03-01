<script setup lang="ts">
import { MdCreate } from '@vicons/ionicons4'
import { ref } from 'vue'

let editingValue = ref(null as any | null)
let editing = ref(false)
const props = defineProps<{
  value?: any
  type?: 'text' | 'select' | 'textarea'
  multiple?: boolean
  options?: {
    label: string
    value: any
  }[]
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
    <n-input-group>
      <n-input v-if="props.type == 'text' || props.type == null" v-model:value="editingValue" />
      <n-select
        v-if="props.type == 'select'"
        style="min-width: 10em"
        :multiple="multiple"
        v-model:value="editingValue"
        :options="props.options"
      >
      </n-select>
      <n-input v-if="props.type == 'textarea'" v-model:value="editingValue" type="textarea" />
      <n-button @click="confirm">确定</n-button>
      <n-button @click="cancel">取消</n-button>
    </n-input-group>
  </div>
  <div v-else>
    <n-space :wrap="false" align="center">
      <div v-if="props.multiple">
        {{ value?.map((v: any) => options?.find((o) => o.value == v)?.label).join() }}
      </div>
      <div v-else>
        {{ options?.find((o) => o.value == value)?.label || value }}
      </div>
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
