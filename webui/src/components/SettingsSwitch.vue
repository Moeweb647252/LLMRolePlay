<script setup lang="ts">
import { MdCreate } from '@vicons/ionicons4'
import { ref } from 'vue'

let editingValue = ref(null as boolean | null)
let editing = ref(false)

const value = defineModel<boolean | null>('value', {
  default: null,
})

const emit = defineEmits(['confirm'])

const startEditing = () => {
  editingValue.value = value.value
  editing.value = true
}

const confirm = () => {
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
    <n-switch v-model:value="editingValue" />
    <n-button @click="confirm"> 确定 </n-button>
    <n-button @click="cancel"> 取消 </n-button>
  </div>
  <div v-else>
    <n-space :wrap="false" align="center">
      {{ value ? '是' : '否' }}
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
