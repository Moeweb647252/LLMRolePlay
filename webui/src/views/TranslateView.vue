<script setup lang="ts">
import {
  NLayout,
  NLayoutHeader,
  NLayoutContent,
  NInput,
  NCard,
  NButton,
} from 'naive-ui'
import { useRouter } from 'vue-router'

const router = useRouter()
</script>

<template>
  <div></div>
  <NLayout>
    <NLayoutHeader
      style="
        display: flex;
        align-items: center;
        justify-content: space-between;
        height: 3.5em;
        margin-left: 0.5em;
        border-bottom: 1px solid #eaeaea;
        margin-right: 0.5em;
      "
      ><h2>Translate</h2>
      <NButton @click="router.push('/main')">聊天模式</NButton>
    </NLayoutHeader>
    <NLayoutContent>
      <div class="layout">
        <div>
          <NCard>
            <template #header> 原文 </template>
            <template #header-extra><NButton>翻译</NButton> </template>
            <NInput
              type="textarea"
              :autosize="{
                minRows: 4,
              }"
            ></NInput>
          </NCard>
        </div>
        <br />
        <div>
          <NCard>
            <NInput
              type="textarea"
              :autosize="{
                minRows: 4,
              }"
            ></NInput>
          </NCard>
        </div>
      </div>
    </NLayoutContent>
  </NLayout>
</template>

<style scoped>
.layout {
  padding-left: 1em;
  padding-right: 1em;
}

.layout > div {
  margin-top: 1em;
  width: 100%;
}
</style>
