<script setup lang="ts">
import { api } from '@/api'
import { Template } from '@/types/template'
import { useMessage, useModal } from 'naive-ui'
import { ref } from 'vue'
import { NButton, NList, NListItem, NSpace } from 'naive-ui'
import type { EditTemplateForm } from '@/types/modal'
import AddTemplateModal from '@/components/modals/AddTemplateModal.vue'
import EditTemplateModal from '@/components/modals/EditTemplateModal.vue'

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

const onAddConfirm = async (form: Template) => {
  templates.value.push(form)
}

let onEditConfirm = (_form: EditTemplateForm) => {}

const startAdd = () => {
  addKey.value++
  addShow.value = true
}

const startEdit = (template: Template) => {
  editing.value = {
    id: template.id!,
    name: template.name!,
    description: template.description,
    content: template.content,
    isPublic: template.isPublic,
    settings: template.settings,
  }
  editKey.value++
  editShow.value = true
  onEditConfirm = (form: EditTemplateForm) => {
    template.name = form.name
    template.description = form.description
    template.content = form.content
    template.isPublic = form.isPublic
    template.settings = form.settings
  }
}
</script>
<template>
  <div style="padding: 2em">
    <div class="header">
      <h3>模板</h3>
      <NButton type="primary" @click="startAdd"> 添加 </NButton>
    </div>
    <div>
      <NList>
        <NListItem v-for="template in templates" :key="template.id!">
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
    v-model:show="addShow"
    @confirm="onAddConfirm"
  />
  <EditTemplateModal
    v-if="editing"
    :key="editKey"
    v-model:show="editShow"
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
