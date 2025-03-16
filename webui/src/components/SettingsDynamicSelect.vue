<script setup lang="ts">
import type { Options } from '@/types/modal'
import { MdCreate } from '@vicons/ionicons4'
import {
  NButton,
  NDynamicInput,
  NIcon,
  NList,
  NListItem,
  NSpace,
  NSelect,
} from 'naive-ui'
import { ref } from 'vue'

type Value = string | number

let editingValue = ref<Value[]>([])
let editing = ref(false)

const props = defineProps<{
  options?: Options
  value: Value[]
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
  editingValue.value = []
}
</script>

<template>
  <div v-if="editing">
    <NDynamicInput v-model:value="editingValue">
      <template #default="obj">
        <NSelect
          v-model:value="obj.value.data"
          filterable
          :options="props.options"
          @update:value="(v: any) => (obj.value.data = v)"
        />
      </template>
    </NDynamicInput>
    <NButton @click="confirm"> 确定 </NButton>
    <NButton @click="cancel"> 取消 </NButton>
  </div>
  <div v-else>
    <NSpace :wrap="false" align="center">
      <NList>
        <NListItem v-for="(item, index) in value" :key="index">
          {{ options?.find((o) => o.value == item)?.label ?? '' }}
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
