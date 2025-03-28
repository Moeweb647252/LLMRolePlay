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
  NInputGroup,
  NSelect,
} from 'naive-ui'
import { computed, ref } from 'vue'
import { useRouter } from 'vue-router'

const choice = ref<number>()
const input = ref('')
const output = ref('')

const router = useRouter()
const translators = ref(await api.getTranslators())

const showAddModal = ref(false)
const addModalKey = ref(0)

const options = computed(() => {
  return translators.value.map((t) => ({
    label: t.name,
    value: t.id,
  }))
})

const onConfirmAdd = async (t: Translator) => {
  translators.value.push(t)
}

const translate = async () => {
  output.value = ''
  if (!choice.value) {
    return
  }
  const translator = translators.value.find((t) => t.id === choice.value)
  if (!translator) {
    return
  }
  for (let i = 0; i < input.value.length; i += 1000) {
    output.value += (
      await api.translate(
        choice.value,
        input.value.slice(i, i + 1000),
        '简体中文',
      )
    )
      .split('\n')
      .map((s) => {
        let id = s.slice(0, s.indexOf(': '))
        if (id === 'END') {
          return ''
        }
        return s.slice(s.indexOf(': ') + 2)
      })
      .join('')
  }
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
            <template #header-extra>
              <NInputGroup style="width: fit-content; min-width: 15em">
                <NSelect v-model:value="choice" :options="options"></NSelect>
                <NButton @click="translate">翻译</NButton>
              </NInputGroup>
            </template>
            <NInput
              v-model:value="input"
              type="textarea"
              :autosize="{
                minRows: 4,
                maxRows: 10,
              }"
            ></NInput>
          </NCard>
        </div>
        <br />
        <div>
          <NCard>
            <NInput
              v-model:value="output"
              type="textarea"
              :autosize="{
                minRows: 4,
                maxRows: 10,
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
