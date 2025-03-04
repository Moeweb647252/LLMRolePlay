<script setup lang="ts">
import { MdCreate } from '@vicons/ionicons4'
import { ref } from 'vue'

let editingValue = ref(null as number | null)
let editing = ref(false)

const value = defineModel<number | null>('value', {
  default: null,
})

const emit = defineEmits(['confirm'])

const startEditing = () => {
  editingValue.value = value.value
  editing.value = true
}

const confirm = () => {
  editingValue.value = parseFloat(editingValue.value as any)
  value.value = editingValue.value
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
      <n-input
        v-model:value="editingValue"
        type="number"
      />
      <n-button @click="confirm">
        确定
      </n-button>
      <n-button @click="cancel">
        取消
      </n-button>
    </n-input-group>
  </div>
  <div v-else>
    <n-space
      :wrap="false"
      align="center"
    >
      {{ value }}
      <n-button
        quaternary
        circle
        @click="startEditing"
      >
        <template #icon>
          <n-icon>
            <MdCreate />
          </n-icon>
        </template>
      </n-button>
    </n-space>
  </div>
</template>
