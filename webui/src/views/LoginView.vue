<script setup lang="ts">
import { ref } from 'vue'
import { useMessage } from 'naive-ui'
import { useRouter } from 'vue-router'
import { api } from '@/api'

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
    <n-card style="width: fit-content">
      <n-form>
        <n-form-item label="Email">
          <n-input v-model:value="email" placeholder="Enter your email" />
        </n-form-item>
        <n-form-item label="Password">
          <n-input
            v-model:value="password"
            type="password"
            placeholder="Enter your password"
          />
        </n-form-item>
        <n-form-item>
          <n-button type="primary" @click="login"> Login </n-button>
        </n-form-item>
      </n-form>
    </n-card>
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
