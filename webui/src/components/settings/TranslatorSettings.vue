<script lang="ts" setup>
import { api } from '@/api'
import { Translator } from '@/types/translator'
import {
  NButton,
  NList,
  NListItem,
  NSpace,
  useMessage,
  useModal,
} from 'naive-ui'
import { computed, ref } from 'vue'
import AddTranslatorModal from '../modals/AddTranslatorModal.vue'
import type { EditTranslatorForm } from '@/types/modal'
import EditTranslatorModal from '../modals/EditTranslatorModal.vue'

const translators = ref(await api.getTranslators())
const providers = ref(await api.getProviders())
const templates = ref(await api.getTemplates())
const presets = ref(await api.getPresets())

const models = computed(() => {
  return providers.value
    .map((p) => {
      return p.models
    })
    .flat()
    .map((model) => ({
      label: model.name,
      value: model.id,
    }))
})

const message = useMessage()
const modal = useModal()

const startAdd = () => {
  addKey.value++
  addShow.value = true
}
const startEdit = (translator: Translator) => {
  editKey.value++
  editingValue.value = {
    id: translator.id!,
    name: translator.name,
    description: translator.description,
    presetIds: translator.presetIds,
    modelId: translator.modelId,
    templateId: translator.templateId,
  }
  editShow.value = true
  onEditConfirm = async (form: EditTranslatorForm) => {
    translator.name = form.name
    translator.description = form.description
    translator.presetIds = form.presetIds
    translator.modelId = form.modelId
    translator.templateId = form.templateId
  }
}

const addShow = ref(false)
const addKey = ref(0)
const editShow = ref(false)
const editKey = ref(0)
const editingValue = ref<EditTranslatorForm>()

const del = (translator: Translator) => {
  modal.create({
    title: '删除翻译器',
    content: `确定删除翻译器 ${translator.name} ?`,
    preset: 'dialog',
    positiveText: '确定',
    negativeText: '取消',
    onPositiveClick: async () => {
      try {
        await api.deleteTranslator(translator.id!)
        translators.value.splice(translators.value.indexOf(translator), 1)
        message.success('删除成功')
      } catch (e) {
        console.log(e)
        message.error('删除失败')
      }
    },
  })
}

const confirmAdd = (t: Translator) => {
  translators.value.push(t)
}

let onEditConfirm = (_form: EditTranslatorForm) => {}
</script>

<template>
  <div style="padding: 2em">
    <div class="header">
      <h3>Translator</h3>
      <NButton type="primary" @click="startAdd"> 添加 </NButton>
    </div>
    <div>
      <NList>
        <NListItem v-for="translator in translators" :key="translator.id">
          {{ translator.name }}
          <template #suffix>
            <NSpace :wrap="false">
              <NButton type="primary" @click="startEdit(translator)">
                编辑
              </NButton>
              <NButton type="error" @click="del(translator)"> 删除 </NButton>
            </NSpace>
          </template>
        </NListItem>
      </NList>
    </div>
  </div>
  <AddTranslatorModal
    :key="addKey"
    v-model:show="addShow"
    :models="models"
    :presets="
      presets.map((p) => ({
        label: p.name,
        value: p.id,
      }))
    "
    :templates="
      templates.map((t) => ({
        label: t.name,
        value: t.id,
      }))
    "
    @confirm="confirmAdd"
  ></AddTranslatorModal>
  <EditTranslatorModal
    v-if="editingValue"
    :key="editKey"
    v-model:show="editShow"
    :value="editingValue"
    :models="models"
    :presets="
      presets.map((p) => ({
        label: p.name,
        value: p.id,
      }))
    "
    :templates="
      templates.map((t) => ({
        label: t.name,
        value: t.id,
      }))
    "
    :translator="editingValue"
    @confirm="onEditConfirm"
  ></EditTranslatorModal>
</template>
