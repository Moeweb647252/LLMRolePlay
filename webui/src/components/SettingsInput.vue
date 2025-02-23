<script setup lang="ts">
import { MdCreate } from '@vicons/ionicons4'
import { ref } from 'vue'

let editingValue = ref('')
let editing = ref(false)

const value = defineModel('value', {
  type: String,
  default: '',
})

const emit = defineEmits(['onConfirm'])

const startEditing = () => {
  editingValue.value = value.value
  editing.value = true
}

const confirm = () => {
  value.value = editingValue.value
  emit('onConfirm')
  editing.value = false
}

const cancel = () => {
  editing.value = false
  editingValue.value = ''
}
</script>

<template>
  <div v-if="editing">
    <n-input-group>
      <n-input v-model:value="editingValue" />
      <n-button type="primary" @click="confirm"> 确定 </n-button>
      <n-button @click="cancel">取消</n-button>
    </n-input-group>
  </div>
  <div v-else>
    <n-space :wrap="false">
      {{ value }}
      <n-button type="primary" @click="startEditing">
        <template #icon>
          <MdCreate />
        </template>
      </n-button>
    </n-space>
  </div>
</template>
