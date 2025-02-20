<script setup lang="ts">
import { ref } from 'vue'
import axios from 'axios'
import { useSettingsStore } from '@/stores/settings'
import { useMessage } from 'naive-ui'
import { sha256 } from 'js-sha256'
import { useRouter } from 'vue-router'

const seettings = useSettingsStore()
const message = useMessage()
const router = useRouter()

const email = ref<string>('')
const password = ref<string>('')

const login = async () => {
  try {
    const response = await axios.post('/api/login', {
      email: email.value,
      password: sha256(password.value),
    })
    console.log(response)
    if (response.status === 200) {
      // Handle successful login
      if (response.data.code === 200) {
        seettings.user = {
          email: response.data.data.email,
          username: response.data.data.username,
          id: response.data.data.id,
          group: response.data.data.group,
          token: response.data.data.token,
        }
        router.push('/')
      } else {
        message.error(response.data.msg)
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
    <n-card style="width: fit-content">
      <n-form>
        <n-form-item label="Email">
          <n-input v-model:value="email" placeholder="Enter your email" />
        </n-form-item>
        <n-form-item label="Password">
          <n-input v-model:value="password" type="password" placeholder="Enter your password" />
        </n-form-item>
        <n-form-item>
          <n-button type="primary" @click="login">Login</n-button>
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
