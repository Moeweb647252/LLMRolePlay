<script setup lang="ts">
import { MdCreate } from '@vicons/ionicons4'
import { ref } from 'vue'

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
    <n-dynamic-input
      v-model:value="editingValue"
      preset="pair"
      key-placeholder="设置名"
      value-placeholder="值"
    />
    <n-button @click="confirm">确定</n-button>
    <n-button @click="cancel">取消</n-button>
  </div>
  <div v-else>
    <n-space :wrap="false" align="center">
      <n-list>
        <n-list-item v-for="(item, index) in value" :key="index">
          {{ item.key }}: {{ item.value }}
        </n-list-item>
      </n-list>
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
