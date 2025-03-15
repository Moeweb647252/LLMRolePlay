<script setup lang="ts">
import { api } from '@/api'
import { Template } from '@/types/template'
import { useMessage, useModal } from 'naive-ui'
import { ref } from 'vue'
import { NButton, NList, NListItem, NSpace } from 'naive-ui'
import type { AddTemplateForm, EditTemplateForm } from '@/types/modal'
import AddTemplateModal from '@/components/modals/AddTemplate.vue'
import EditTemplateModal from '@/components/modals/EditTemplate.vue'

const templates = ref(await api.getTemplates())
const message = useMessage()
const model = useModal()

const deleteTemplate = async (template: Template) => {
  model.create({
    title: '删除模板',
    content: `确定删除模板 ${template.name} ?`,
    preset: 'dialog',
    positiveText: '确定',
    negativeText: '取消',
    onPositiveClick: async () => {
      try {
        await api.deleteTemplate(template.id!)
        templates.value.splice(templates.value.indexOf(template), 1)
        message.success('删除成功')
      } catch (e) {
        console.log(e)
        message.error('删除失败')
      }
    },
  })
}

const editShow = ref(false)
const addShow = ref(false)
const editKey = ref(0)
const addKey = ref(0)
const editing = ref<EditTemplateForm | null>(null)

const onAddConfirm = async (form: AddTemplateForm) => {}

let onEditConfirm = (_form: EditTemplateForm) => {}

const startEdit = (template: Template) => {}
</script>
<template>
  <div style="padding: 2em">
    <div class="header">
      <h3>模板</h3>
      <NButton type="primary" @click="addTemplateForm.visible = true">
        添加
      </NButton>
    </div>
    <div>
      <NList>
        <NListItem v-for="template in templates" :key="template.id">
          {{ template.name }}
          <template #suffix>
            <NSpace :wrap="false">
              <NButton type="primary" @click="startEdit(template)">
                编辑
              </NButton>
              <NButton type="error" @click="deleteTemplate(template)">
                删除
              </NButton>
            </NSpace>
          </template>
        </NListItem>
      </NList>
    </div>
  </div>
  <AddTemplateModal
    :key="addKey"
    v-model:visible="addShow"
    @confirm="onAddConfirm"
  />
  <EditTemplateModal
    v-if="editing"
    :key="editKey"
    v-model:visible="editShow"
    :value="editing"
    @confirm="onEditConfirm"
  />
</template>

<style scoped>
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1em;
}
</style>
