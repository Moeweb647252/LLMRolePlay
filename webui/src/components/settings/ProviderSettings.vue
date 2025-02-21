<script setup lang="ts">
import { reactive } from 'vue'

const providers = reactive([])

const providerTypeOptions = [
  { label: 'Openai-Compatible', value: 'openai' },
  {
    label: 'Google',
    value: 'google',
  },
  {
    label: 'Azure',
    value: 'azure',
  },
]

const addProviderForm = reactive({
  visible: false,
  name: '',
  url: '',
  apiKey: '',
  type: 'openai',
})

const addProvider = () => {}
</script>
<template>
  <div style="padding: 2em">
    <div class="header">
      <h3>Providers</h3>
      <n-button type="primary" @click="addProviderForm.visible = true">添加</n-button>
    </div>
    <div>
      <n-thing v-for="provider in providers"></n-thing>
    </div>
  </div>
  <n-modal
    v-model:show="addProviderForm.visible"
    preset="card"
    title="添加Provider"
    size="medium"
    style="width: fit-content; min-width: 25em"
  >
    <n-form label-placement="left">
      <n-form-item label="类型">
        <n-select :options="providerTypeOptions" v-model:value="addProviderForm.type"></n-select>
      </n-form-item>
      <n-form-item label="名称">
        <n-input v-model:value="addProviderForm.name"></n-input>
      </n-form-item>
      <n-form-item label="Base URL">
        <n-input v-model:value="addProviderForm.url"></n-input>
      </n-form-item>
      <n-form-item label="API Key">
        <n-input v-model:value="addProviderForm.apiKey"></n-input>
      </n-form-item>
    </n-form>
    <template #footer>
      <n-button type="primary" @click="addProvider">添加</n-button>
      <n-button @click="addProviderForm.visible = false">取消</n-button>
    </template>
  </n-modal>
</template>

<style scoped>
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
</style>
