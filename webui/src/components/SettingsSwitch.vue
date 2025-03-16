<script setup lang="ts">
import { MdCreate } from '@vicons/ionicons4'
import { NButton, NIcon, NSpace, NSwitch } from 'naive-ui'
import { ref } from 'vue'

let editingValue = ref(false)
let editing = ref(false)

const value = defineModel<boolean | null>('value', {
  default: null,
})

const emit = defineEmits(['confirm'])

const startEditing = () => {
  editingValue.value = value.value ?? false
  editing.value = true
}

const confirm = () => {
  value.value = editingValue.value
  emit('confirm', editingValue.value)
  editing.value = false
}

const cancel = () => {
  editing.value = false
  editingValue.value = false
}
</script>

<template>
  <div v-if="editing">
    <NSwitch v-model:value="editingValue" />
    <NButton @click="confirm"> 确定 </NButton>
    <NButton @click="cancel"> 取消 </NButton>
  </div>
  <div v-else>
    <NSpace :wrap="false" align="center">
      {{ value ? '是' : '否' }}
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
