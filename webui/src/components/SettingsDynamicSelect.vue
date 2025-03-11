<script setup lang="ts">
import { MdCreate } from '@vicons/ionicons4'
import { ref } from 'vue'

type Value = { data: any | null }[]

let editingValue = ref<Value>([])
let editing = ref(false)

const props = defineProps<{
  options?: {
    label: string
    value: any
  }[]
}>()

const value = defineModel<any[]>('value', {
  default: [],
})

const emit = defineEmits(['confirm'])

const startEditing = () => {
  editingValue.value = value.value.map((v) => ({ data: v }))
  editing.value = true
}

const confirm = () => {
  value.value = editingValue.value
  emit(
    'confirm',
    editingValue.value.map((v) => v.data),
  )
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
      @create="
        () => {
          let obj = { data: null }
          editingValue.push(obj)
          return obj
        }
      "
    >
      <template #default="obj">
        <n-select
          v-model:value="obj.value.data"
          filterable
          :options="props.options"
          @update:value="(v: any) => (obj.value.data = v)"
        />
      </template>
    </n-dynamic-input>
    <n-button @click="confirm"> 确定 </n-button>
    <n-button @click="cancel"> 取消 </n-button>
  </div>
  <div v-else>
    <n-space :wrap="false" align="center">
      <n-list>
        <n-list-item v-for="(item, index) in value" :key="index">
          {{ item.name ?? '' }}
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
