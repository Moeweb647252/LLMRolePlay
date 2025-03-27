<script setup lang="ts">
import { api } from '@/api'
import AddTranslatorModal from '@/components/modals/AddTranslatorModal.vue'
import type { Translator } from '@/types/translator'
import {
  NLayout,
  NLayoutHeader,
  NLayoutContent,
  NInput,
  NCard,
  NButton,
} from 'naive-ui'
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const translators = ref(await api.getTranslators())

const showAddModal = ref(false)
const addModalKey = ref(0)

const onConfirmAdd = async (t: Translator) => {
  translators.value.push(t)
}
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
        padding-left: 1em;
        border-bottom: 1px solid #eaeaea;
        padding-right: 1em;
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
  <AddTranslatorModal
    :key="addModalKey"
    v-model:show="showAddModal"
    :models="translators"
    :presets="translators"
    :templates="translators"
    @confirm="onConfirmAdd"
  ></AddTranslatorModal>
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
