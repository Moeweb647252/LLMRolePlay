<script setup lang="ts">
import { ref } from 'vue'
import { useMessage } from 'naive-ui'
import { useRouter } from 'vue-router'
import { api } from '@/api'
import { NCard, NForm, NFormItem, NInput, NButton } from 'naive-ui'

const message = useMessage()
const router = useRouter()

const email = ref<string>('')
const password = ref<string>('')

const login = async () => {
  try {
    await api.login(email.value, password.value)
    router.push('/main')
  } catch (error) {
    // Handle error
    console.log(error)
    message.error(Object(error).toString())
  }
}
</script>
<template>
  <div class="container">
    <NCard style="width: fit-content">
      <NForm>
        <NFormItem label="Email">
          <NInput v-model:value="email" placeholder="Enter your email" />
        </NFormItem>
        <NFormItem label="Password">
          <NInput
            v-model:value="password"
            type="password"
            placeholder="Enter your password"
          />
        </NFormItem>
        <NFormItem>
          <NButton type="primary" @click="login"> Login </NButton>
        </NFormItem>
      </NForm>
    </NCard>
  </div>
</template>

<style scoped>
.container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100%;
  width: 100%;
}
</style>
