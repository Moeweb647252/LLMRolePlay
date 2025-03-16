<script setup lang="ts">
import { MdCreate } from '@vicons/ionicons4'
import { ref } from 'vue'
import { NButton, NIcon, NSpace, NInputGroup, NInputNumber } from 'naive-ui'

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
    <NInputGroup>
      <NInputNumber v-model:value="editingValue" :show-button="false" />
      <NButton @click="confirm"> 确定 </NButton>
      <NButton @click="cancel"> 取消 </NButton>
    </NInputGroup>
  </div>
  <div v-else>
    <NSpace :wrap="false" align="center">
      {{ value }}
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
