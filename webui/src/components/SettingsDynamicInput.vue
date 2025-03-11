<script setup lang="ts">
import { MdCreate } from '@vicons/ionicons4'
import { ref } from 'vue'
import {
  NButton,
  NDynamicInput,
  NIcon,
  NList,
  NListItem,
  NSpace,
} from 'naive-ui'

type Value = { key: string; value: string }[]

let editingValue = ref<Value>([])
let editing = ref(false)

const value = defineModel<Value>('value', {
  default: [],
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
  editingValue.value = []
}
</script>

<template>
  <div v-if="editing">
    <NDynamicInput
      v-model:value="editingValue"
      preset="pair"
      key-placeholder="设置名"
      value-placeholder="值"
    />
    <NButton @click="confirm"> 确定 </NButton>
    <NButton @click="cancel"> 取消 </NButton>
  </div>
  <div v-else>
    <NSpace :wrap="false" align="center">
      <NList>
        <NListItem v-for="(item, index) in value" :key="index">
          {{ item.key }}: {{ item.value }}
        </NListItem>
      </NList>
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
