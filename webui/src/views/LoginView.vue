<script setup lang="ts">
import { ref } from 'vue'
import axios from 'axios'
import { useSettingsStore } from '@/stores/settings'
import { useMessage } from 'naive-ui'

const seettings = useSettingsStore()
const message = useMessage()

const username = ref<string>('')
const password = ref<string>('')

const login = async () => {
  try {
    const response = await axios.post('/api/login', {
      username: username.value,
      password: password.value,
    })
    if (response.status === 200) {
      // Handle successful login
      if (response.data.code === 200) {
      }
    }
  } catch (error) {
    // Handle error
    console.log(error)
    message.error('Login failed.')
  }
}
</script>
<template>
  <div class="container">
    <n-card>
      <n-form>
        <n-form-item label="Username">
          <n-input v-model="username" placeholder="Enter your username" />
        </n-form-item>
        <n-form-item label="Password">
          <n-input v-model="password" type="password" placeholder="Enter your password" />
        </n-form-item>
        <n-form-item>
          <n-button type="primary" @click="() => {}">Login</n-button>
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
